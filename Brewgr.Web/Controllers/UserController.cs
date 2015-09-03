using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using ctorx.Core.Data;
using ctorx.Core.Messaging;
using Brewgr.Web.Core.Data;
using Brewgr.Web.Core.Model;
using Brewgr.Web.Core.Service;
using Brewgr.Web.Mappers;
using Brewgr.Web.Models;

namespace Brewgr.Web.Controllers
{
	public class UserController : BrewgrController
	{
		readonly IUnitOfWorkFactory<BrewgrContext> UnitOfWorkFactory;
		readonly IUserService UserService;
		readonly IUserResolver UserResolver;
        readonly IRecipeService RecipeService;
		readonly INotificationService NotificationService;

		/// <summary>
		/// ctor the Mighty
		/// </summary>
        public UserController(IUnitOfWorkFactory<BrewgrContext> unitOfWorkFactory, IUserService userService, IUserResolver userResolver, 
			IRecipeService recipeService, INotificationService notificationService)
		{
			this.UnitOfWorkFactory = unitOfWorkFactory;
			this.UserService = userService;
			this.UserResolver = userResolver;
            this.RecipeService = recipeService;
			this.NotificationService = notificationService;
		}

		/// <summary>
		/// Executes the Settings View
		/// </summary>
		[ForceHttps]
		[Authorize]
		public ViewResult Settings()
		{
			var user = this.UserService.GetUserById(this.ActiveUser.UserId);
			return View(Mapper.Map(user, new UserSettingsViewModel()));	
		}

		/// <summary>
		/// Executes the Http Post View for Settings
		/// </summary>
		[HttpPost]
		[ForceHttps]
		[Authorize]
		public ActionResult Settings(UserSettingsViewModel userSettingsViewModel)
		{
			if(!this.ValidateAndAppendMessages(userSettingsViewModel))
			{
				return View(userSettingsViewModel);
			}

			// Check Email Address
			if (this.UserService.EmailAddressIsInUse(userSettingsViewModel.EmailAddress, this.ActiveUser.UserId))
			{
			    return Json(new { Success = false, Message = "The email address you entered is already in use." });
			}

			// Check Username Uniqueness
			if (this.UserService.UsernameIsInUse(this.ActiveUser.UserId, userSettingsViewModel.Username))
			{
                return Json(new { Success = false, Message = "The requested username is already in use" });
			}

			using(var unitOfWork = this.UnitOfWorkFactory.NewUnitOfWork())
			{
				try
				{
					var user = this.UserService.GetUserById(this.ActiveUser.UserId);

					var originalUsername = user.Username;

					user = Mapper.Map(userSettingsViewModel, user);

					// Cust Username Flag
					if (!user.HasCustomUsername && originalUsername != user.Username)
					{
						user.HasCustomUsername = true;
					}

					user.EmailAddress = userSettingsViewModel.EmailAddress.Trim();
					user.DateModified = DateTime.Now;

					unitOfWork.Commit();

					this.UserResolver.Update(Mapper.Map(user, new UserSummary()));

                    return Json(new { Success = true, Message = "Your settings have been saved" });
                }
				catch (Exception ex)
				{
					unitOfWork.Rollback();
					this.LogHandledException(ex);
					return this.Issue500();
				}				
			}
		}

		/// <summary>
		/// Executes the Http Post View for SetNotifications
		/// </summary>
		[HttpPost]
		public ActionResult SetNotifications(UserSettingsViewModel userSettingsViewModel)
		{
			if (userSettingsViewModel.UserId != this.ActiveUser.UserId)
			{
				return this.Issue404();
			}

			using (var unitOfWork = this.UnitOfWorkFactory.NewUnitOfWork())
			{
				try
				{
					var user = this.UserService.GetUserById(userSettingsViewModel.UserId);

					#region Sub/Un-SUB Types

					// RecipeComments: Handle Unsubscribe
					if (!userSettingsViewModel.RecipeCommentNotifications &&
					    user.UserNotificationTypes.Any(x => x.NotificationTypeId == (int) NotificationType.RecipeComment))
					{
						this.UserService.UnsubscribeUserFromNotificationType(user, NotificationType.RecipeComment);
					}

					// RecipeComments: Handle Subscribe
					if (userSettingsViewModel.RecipeCommentNotifications &&
					    !user.UserNotificationTypes.Any(x => x.NotificationTypeId == (int) NotificationType.RecipeComment))
					{
						this.UserService.SubscribeUserToNotificationType(user, NotificationType.RecipeComment);
					}

					// BrewSessionComments: Handle Unsubscribe
					if (!userSettingsViewModel.BrewSessionCommentNotifications &&
						user.UserNotificationTypes.Any(x => x.NotificationTypeId == (int)NotificationType.BrewSessionComment))
					{
						this.UserService.UnsubscribeUserFromNotificationType(user, NotificationType.BrewSessionComment);
					}

					// BrewSessionComments: Handle Subscribe
					if (userSettingsViewModel.BrewSessionCommentNotifications &&
						!user.UserNotificationTypes.Any(x => x.NotificationTypeId == (int)NotificationType.BrewSessionComment))
					{
						this.UserService.SubscribeUserToNotificationType(user, NotificationType.BrewSessionComment);
					}

					// BrewerFollow: Handle Unsubscribe
					if (!userSettingsViewModel.BrewerFollowNotifications &&
						user.UserNotificationTypes.Any(x => x.NotificationTypeId == (int)NotificationType.BrewerFollowed))
					{
						this.UserService.UnsubscribeUserFromNotificationType(user, NotificationType.BrewerFollowed);
					}

					// BrewerFollow: Handle Subscribe
					if (userSettingsViewModel.BrewerFollowNotifications &&
						!user.UserNotificationTypes.Any(x => x.NotificationTypeId == (int)NotificationType.BrewerFollowed))
					{
						this.UserService.SubscribeUserToNotificationType(user, NotificationType.BrewerFollowed);
					}

					// SiteFeatures: Handle Unsubscribe
					if (!userSettingsViewModel.SiteFeatureNotifications &&
						user.UserNotificationTypes.Any(x => x.NotificationTypeId == (int)NotificationType.SiteFeatures))
					{
						this.UserService.UnsubscribeUserFromNotificationType(user, NotificationType.SiteFeatures);
					}

					// SiteFeatures: Handle Subscribe
					if (userSettingsViewModel.SiteFeatureNotifications &&
						!user.UserNotificationTypes.Any(x => x.NotificationTypeId == (int)NotificationType.SiteFeatures))
					{
						this.UserService.SubscribeUserToNotificationType(user, NotificationType.SiteFeatures);
					}

					// SiteOutages: Handle Unsubscribe
					if (!userSettingsViewModel.SiteOutageNotifications &&
						user.UserNotificationTypes.Any(x => x.NotificationTypeId == (int)NotificationType.SiteOutages))
					{
						this.UserService.UnsubscribeUserFromNotificationType(user, NotificationType.SiteOutages);
					}

					// SiteOutages: Handle Subscribe
					if (userSettingsViewModel.SiteOutageNotifications &&
						!user.UserNotificationTypes.Any(x => x.NotificationTypeId == (int)NotificationType.SiteOutages))
					{
						this.UserService.SubscribeUserToNotificationType(user, NotificationType.SiteOutages);
					}

					#endregion

					unitOfWork.Commit();

				    return Json(new { Success = true, Message = "Your notification settings have been saved" });
				}
				catch (Exception ex)
				{
					unitOfWork.Rollback();
					this.LogHandledException(ex);
					return this.Issue500();
				}
			}
		}

		/// <summary>
		/// Executes the UsernameExists view
		/// </summary>
		[Authorize]
		public ContentResult UsernameExists(string username)
		{
			if(string.IsNullOrWhiteSpace(username))
			{
				throw new ArgumentNullException("username");
			}

			return Content(this.UserService.UsernameIsInUse(this.ActiveUser.UserId, username) ? "1" : "0");
		}

		/// <summary>
		/// Executes the View for EmailAddressInUse
		/// </summary>
		[Authorize]
		public ContentResult EmailAddressInUse()
		{
			var emailAddress = Request["email"];

			if (string.IsNullOrWhiteSpace(emailAddress))
			{
				throw new ArgumentNullException("emailAddress");
			}

			return Content(this.UserService.EmailAddressIsInUse(Server.UrlDecode(emailAddress), this.ActiveUser.UserId) ? "1" : "0");
		}

		/// <summary>
		/// Executes the View for UserProfile
		/// </summary>
		public ActionResult UserProfile(string userName)
		{            
            var user = this.UserService.GetUserByUserName(userName);
			
			if(user == null)
			{
				return this.Issue404();
			}
			
			var userProfileViewModel = new UserProfileViewModel();
			userProfileViewModel.UserSummary = Mapper.Map(user, new UserSummary());

			// Check if active user follows this user
			if (this.ActiveUser != null)
			{
				ViewBag.UserIsFollowed = this.UserService.UserIsFollowedBy(user.UserId, this.ActiveUser.UserId);
			}

			// Recipes
            userProfileViewModel.Recipes = this.RecipeService.GetUserRecipes(user.UserId)
				.Where(x => x.IsPublic)
				.ToList();

			// Brew Summaries
			userProfileViewModel.BrewSessionSummaries = this.RecipeService.GetUserBrewSessions(user.UserId)
				.OrderByDescending(x => x.BrewDate)
				.ToList();

			// Followers
			userProfileViewModel.Followers = this.UserService.GetFollowersOf(user.UserId);

			// Followed
			userProfileViewModel.Follows = this.UserService.GetFollowedBy(user.UserId);

			return View(userProfileViewModel);	
		}

		/// <summary>
		/// Executes the View for ToggleView
		/// </summary>
		[HttpPost]
		public ActionResult ToggleBrewerFollow(int userId)
		{
			using (var unitOfWork = this.UnitOfWorkFactory.NewUnitOfWork())
			{
				try
				{
					this.UserService.ToggleUserFollow(userId, this.ActiveUser.UserId);
					unitOfWork.Commit();
					return new EmptyResult();
				}
				catch (Exception ex)
				{
					this.LogHandledException(ex);
					unitOfWork.Rollback();
					return this.Issue500();
				}
			}
		}

		/// <summary>
		/// Gets the reputation score for a user
		/// </summary>
		public int UserRep(int userId)
		{
			return this.UserService.GetUserReputationScore(userId);
		}

	}
}