using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security;
using System.Web.Mvc;
using Brewgr.Web.Email;
using ctorx.Core.Crypto;
using ctorx.Core.Data;
using ctorx.Core.Email;
using ctorx.Core.Messaging;
using ctorx.Core.Security;
using Brewgr.Web.Core.Configuration;
using Brewgr.Web.Core.Data;
using Brewgr.Web.Core.Model;
using Brewgr.Web.Core.Service;
using Brewgr.Web.Models;
using ctorx.Core.Web;
using AutoMapper;

namespace Brewgr.Web.Controllers
{
	public class RootController : BrewgrController
	{
		readonly IUnitOfWorkFactory<BrewgrContext> UnitOfWorkFactory;
		readonly IUserLoginService UserLoginService;
		readonly IAuthenticationService AuthService;
		readonly IUserResolver UserResolver;
		readonly IOAuthService OAuthService;
		readonly IFacebookConnectSettings FacebookConnectSettings;
		readonly IMarketingService MarketingService;
		readonly IRecipeService RecipeService;
		readonly IEmailMessageFactory EmailMessageFactory;
		readonly IEmailSender EmailSender;
		readonly ISeoSitemap SeoSitemap;
		readonly IUserService UserService;
		readonly ISearchService SearchService;

		/// <summary>
		/// ctor the Mighty
		/// </summary>
		public RootController(IUnitOfWorkFactory<BrewgrContext> unitOfWorkFactory, IUserLoginService userLoginService, 
			IAuthenticationService authService, IUserResolver userResolver, IOAuthService oAuthService, IUserService userService,
			ISearchService searchService, IFacebookConnectSettings facebookConnectSettings, IMarketingService marketingService,
			IRecipeService recipeService, IEmailMessageFactory emailMessageFactory, IEmailSender emailSender, ISeoSitemap seoSitemap)
		{
			this.UnitOfWorkFactory = unitOfWorkFactory;
			this.UserLoginService = userLoginService;
			this.AuthService = authService;
			this.UserResolver = userResolver;
			this.OAuthService = oAuthService;
			this.UserService = userService;
			this.SearchService = searchService;
			this.FacebookConnectSettings = facebookConnectSettings;
			this.MarketingService = marketingService;
			this.RecipeService = recipeService;
			this.EmailMessageFactory = emailMessageFactory;
			this.EmailSender = emailSender;
			this.SeoSitemap = seoSitemap;
		}

		#region LOGIN CHECK

		/// <summary>
		/// Executes the User Is Logged In View
		/// </summary>
		public ContentResult UserIsLoggedIn()
		{
			return this.Content(this.ActiveUser != null ? "1" : "0");
		}

		#endregion

		/// <summary>
		/// Executes the View for Index
		/// </summary>
		public ActionResult Index()
		{
            if (this.ActiveUser != null)
            {
                return RedirectToAction("Dashboard", "Dashboard");
            }

			var newRecipes = this.RecipeService.GetNewestRecipes(4);
			var popularRecipes = this.RecipeService.GetPopularRecipes(4);
			var topContrinutors = this.UserService.GetWeeklyTopContributors(4);

			return View(new HomePageViewModel { NewRecipes = newRecipes, TopContributors = topContrinutors, PopularRecipes = popularRecipes});
		}

		/// <summary>
		/// Executes the View for Featires
		/// </summary>
		public ViewResult Features()
		{
			return View();
		}
		
		/// <summary>
		/// Executes the View for About
		/// </summary>
		public ViewResult About()
		{
			return View();
		}

		/// <summary>
		/// Executes the View for Terms
		/// </summary>
		public ViewResult Terms()
		{
			return View();
		}

		/// <summary>
		/// Executes the View for Privacy
		/// </summary>
		public ViewResult Privacy()
		{
			return View();
		}

		/// <summary>
		/// Executes the View for Faq
		/// </summary>
		public ViewResult Faq()
		{
			return View();
		}

		/// <summary>
		/// Executes the View for RecentPhotos
		/// </summary>
		public JsonResult RecentPhotos()
		{
			return Json(this.RecipeService.GetRecentRecipesCached(3)
				.Select(x => new
				{
					ImageUrl = Url.RecipeThumbnailUrl(x.ImageUrlRoot, x.Srm),
					Url = Url.RecipeDetailUrl(x.RecipeId, x.RecipeName, x.BJCPStyleName),
					Name = x.RecipeName
				}), JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// Executes the View for Contact
		/// </summary>
		public ViewResult Contact()
		{
			if(this.ActiveUser != null)
			{
				return View(new ContactViewModel { Name = this.ActiveUser.FullName, EmailAddress = this.ActiveUser.EmailAddress });	
			}

			return View();
		}

		/// <summary>
		/// Executes the Http Post View for Contact
		/// </summary>
		[HttpPost]
		public ActionResult Contact(ContactViewModel contactViewModel)
		{
			if(!this.ValidateAndAppendMessages(contactViewModel))
			{
				return View(contactViewModel);
			}

			var contactMessage = (ContactFormEmailMessage)this.EmailMessageFactory.Make(EmailMessageType.ContactForm);
			contactMessage.SetContactViewModel(contactViewModel);
			this.EmailSender.Send(contactMessage);

			this.ForwardMessage(new SuccessMessage { Text = "Thank You.  Your message has been sent" });

			return RedirectToAction("contact");
		}

        /// <summary>
        /// Executes the View for HowItWorks
        /// </summary>
        public ViewResult HowItWorks()
        {
            return View();
        }

        [Route("open-source-homebrew-software")]
	    public ViewResult OpenSourceSoftware()
        {
            return View();
        }

		/// <summary>
		/// Executes the View for Sitemap
		/// </summary>
		public ContentResult Sitemap()
		{
			this.Response.ContentType = "text/xml";
			return Content(this.SeoSitemap.GenerateXml(this.Url));
		}

	}
}