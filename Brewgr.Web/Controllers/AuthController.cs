using System;
using System.ComponentModel;
using System.Linq;
using System.Security;
using System.Web.Mvc;
using AutoMapper;
using Brewgr.Web.Email;
using Brewgr.Web.Mappers;
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

namespace Brewgr.Web.Controllers
{
	[ForceHttps]
	public class AuthController : BrewgrController
	{
		readonly IUnitOfWorkFactory<BrewgrContext> UnitOfWorkFactory;
		readonly IUserLoginService UserLoginService;
		readonly IAuthenticationService AuthenticationService;
		readonly IUserResolver UserResolver;
		readonly IOAuthService OAuthService;
		readonly IFacebookConnectSettings FacebookConnectSettings;
		readonly IEmailSender EmailSender;
		readonly IEmailMessageFactory EmailMessageFactory;
		readonly IUserService UserService;

		/// <summary>
		/// ctor the Mighty
		/// </summary>
		public AuthController(IUnitOfWorkFactory<BrewgrContext> unitOfWorkFactory, IUserLoginService userLoginService, 
			IAuthenticationService authService, IUserResolver userResolver, IOAuthService oAuthService, IUserService userService,
			IFacebookConnectSettings facebookConnectSettings, IEmailSender emailSender,
			IEmailMessageFactory emailMessageFactory)
		{
			this.UnitOfWorkFactory = unitOfWorkFactory;
			this.UserLoginService = userLoginService;
			this.AuthenticationService = authService;
			this.UserResolver = userResolver;
			this.OAuthService = oAuthService;
			this.UserService = userService;
			this.FacebookConnectSettings = facebookConnectSettings;
			this.EmailSender = emailSender;
			this.EmailMessageFactory = emailMessageFactory;
		}

		#region SIGN UP

		/// <summary>
		/// Executes the Http Post View for SignUp
		/// </summary>
		[HttpPost]
		public ActionResult SignUp(SignUpViewModel signUpViewModel)
		{
			this.InitializeFacebookAuth(Url.Action("OAuthLogin", "Auth", null, "https"));

			if(!this.ValidateAndAppendMessages(signUpViewModel))
			{
				return View("Login");
			}

			if(this.UserService.EmailAddressIsInUse(signUpViewModel.NewUserEmailAddress))
			{
				this.AppendMessage(new ErrorMessage { Text = "The email address you entered is already registered" });
				return View("Login");
			}

			try
			{
				var user = this.CreateAccount(signUpViewModel);
				this.ForwardMessage(new SuccessMessage { Text = "Your account has been created.  Welcome to Brewgr!" });
				
				return this.SignInAndRedirect(user);
			}
			catch (Exception ex)
			{
				this.LogHandledException(ex);
				this.AppendMessage(new ErrorMessage { Text = GenericMessages.ErrorMessage });
				return View("Login");
			}
		}

		/// <summary>
		/// Executes the View for SignUpViaDialog
		/// </summary>
		[ForceHttps]
		[HttpPost]
		public ViewResult SignUpViaDialog(SignUpViewModel signUpViewModel)
		{
			this.InitializeFacebookAuth(Url.Action("OAuthLogin", "Auth", null, "https"));

			if (!this.ValidateAndAppendMessages(signUpViewModel))
			{
				ViewBag.LoginViaDialogSuccess = false;
				return View("LoginViaDialog");
			}

			if (this.UserService.EmailAddressIsInUse(signUpViewModel.NewUserEmailAddress))
			{
				ViewBag.LoginViaDialogSuccess = false;
				this.AppendMessage(new ErrorMessage { Text = "The email address you entered is already registered" });
				return View("LoginViaDialog");
			}

			try
			{
				var userSummary = this.CreateAccount(signUpViewModel);
				ViewBag.LoginViaDialogSuccess = true;
				this.SignIn(userSummary, false);

				this.AppendLoginViaDialogSuccessMessage(userSummary, !string.IsNullOrWhiteSpace(Request["editMode"]));
			}
			catch (Exception ex)
			{
				this.LogHandledException(ex);
				this.AppendMessage(new ErrorMessage { Text = GenericMessages.ErrorMessage });
			}

			return View("LoginViaDialog");
		}

		/// <summary>
		/// Creates a new Account
		/// </summary>
		UserSummary CreateAccount(SignUpViewModel signUpViewModel)
		{
			using (var unitOfWork = this.UnitOfWorkFactory.NewUnitOfWork())
			{
				var user = this.UserService.RegisterNewUser(signUpViewModel.NewUserFullName, signUpViewModel.NewUserEmailAddress, signUpViewModel.NewUserPassword);
				unitOfWork.Commit();

                // Send the Email Message
                var newAccountEmailMessage = (NewAccountEmailMessage)this.EmailMessageFactory.Make(EmailMessageType.NewAccount);

                newAccountEmailMessage.ToRecipients.Add(signUpViewModel.NewUserEmailAddress);
                this.EmailSender.Send(newAccountEmailMessage);

                return Mapper.Map(user, new UserSummary());				
			}
		}

		#endregion

		#region LOGIN / LOGOUT

		/// <summary>
		/// Executes the View for Login
		/// </summary>
		public ViewResult Login()
		{
			this.InitializeFacebookAuth(Url.Action("OAuthLogin", "Auth", null, "https"));
			return View();
		}

		/// <summary>
		/// Executes the Http Post View for Login
		/// </summary>
		[HttpPost]
		public ActionResult Login(LoginViewModel loginViewModel)
		{
			this.InitializeFacebookAuth(Url.Action("OAuthLogin", "Auth", null, "https"));

			if (!this.ValidateAndAppendMessages(loginViewModel))
			{
				return View();
			}

			var userSummary = this.AuthenticateLogin(loginViewModel);

			if(userSummary == null)
			{
				this.AppendMessage(new ErrorMessage { Text = "Your credentials could not be validated" });
				return View();
			}

			return SignInAndRedirect(userSummary, loginViewModel.KeepMeLoggedIn);
		}

		/// <summary>
		/// Executes the View for LoginViaDialog
		/// </summary>
		public ViewResult LoginViaDialog()
		{
			this.InitializeFacebookAuth(string.Concat(this.WebSettings.RootPathSecure, "/OAuthLoginViaDialog"));

			ViewBag.LoginViaDialogSuccess = false;

			if (string.IsNullOrWhiteSpace(Request["LoginViaDialog"]))
			{
				this.AppendMessage(new SuccessMessage {Text = "We'll save your recipe after you login or create an account"});
			}

			return View();
		}

		/// <summary>
		/// Executes the Http Post View for LoginViaDialog
		/// </summary>
		[HttpPost]
		public ViewResult LoginViaDialog(LoginViewModel loginViewModel)
		{
			this.InitializeFacebookAuth(string.Concat(this.WebSettings.RootPathSecure, "/OAuthLoginViaDialog"));

			var userSummary = this.AuthenticateLogin(loginViewModel);

			if (userSummary == null)
			{
				ViewBag.LoginViaDialogSuccess = false;
				this.AppendMessage(new ErrorMessage { Text = "Your credentials could not be validated" });
				return View();
			}

			this.SignIn(userSummary, loginViewModel.KeepMeLoggedIn);

			this.AppendLoginViaDialogSuccessMessage(userSummary, !string.IsNullOrWhiteSpace(Request["editMode"]));

			ViewBag.LoginViaDialogSuccess = true;
			return View();
		}

		/// <summary>
		/// Executes the Logout View
		/// </summary>
		public ActionResult Logout()
		{
			this.AuthenticationService.SignOut();
			Session.Abandon();

			if (!string.IsNullOrWhiteSpace(Request["ReturnUrl"]))
			{
				return Redirect(string.Concat(this.WebSettings.RootPath, Server.UrlDecode(Request["ReturnUrl"])));
			}

			return RedirectToAction("Login");
		}

		/// <summary>
		/// Authenticates a Login
		/// </summary>
		UserSummary AuthenticateLogin(LoginViewModel loginViewModel)
		{
			UserSummary userSummary = null;

			using (var unitOfWork = this.UnitOfWorkFactory.NewUnitOfWork())
			{
				// Perform Login
				if (!this.UserLoginService.Login(loginViewModel.EmailAddress, loginViewModel.Password, out userSummary))
				{
					return null;
				}

				unitOfWork.Commit();
			}

			return userSummary;
		}

		#endregion

		#region PASSWORD RESET

		/// <summary>
		/// Executes the view for ResetPassword
		/// </summary>
		[ActionName("reset-password")]
		public ViewResult ResetPassword()
		{
			return View();
		}
		/// <summary>
		/// Executes the post view for ResetPassword
		/// </summary>
		[HttpPost]
		[ActionName("reset-password")]
		public ActionResult ResetPassword(PasswordResetViewModel passwordResetViewModel)
		{
			if (!this.ValidateAndAppendMessages(passwordResetViewModel))
			{
				return View();
			}
			
			string token = null;
			using(var unitOfWork = this.UnitOfWorkFactory.NewUnitOfWork())
			{
				// Generate Token
				token = this.UserLoginService.CreateUserAuthToken(passwordResetViewModel.EmailAddress);

				if(token == null)
				{
					this.AppendMessage(new ErrorMessage { Text = "The email address you entered could not be found" });
					return View(); 
				}

				unitOfWork.Commit();
			}

			// Send the Email Message
			var passwordResetEmailMessage = (PasswordResetEmailMessage)this.EmailMessageFactory.Make(EmailMessageType.PasswordReset);
			passwordResetEmailMessage.SetAuthToken(token);
			
			passwordResetEmailMessage.ToRecipients.Add(passwordResetViewModel.EmailAddress);
			this.EmailSender.Send(passwordResetEmailMessage);

			if (!string.IsNullOrWhiteSpace(Request.Form["LoginViaDialog"]))
			{
				this.ForwardMessage(new SuccessMessage { Text = "Please check your email for instructions on how to reset your password" });
				return RedirectToAction("LoginViaDialog", new { LoginViaDialog = true });
			}

			// Append Message and Redirect
			this.AppendMessage(new SuccessMessage { Text = "Please check your email for instructions on how to reset your password" });

			return View();
		}

		/// <summary>
		/// Executes the SetPassword view
		/// </summary>
		[ActionName("set-password")]
		public ActionResult SetPassword(string authToken)
		{
			if(string.IsNullOrWhiteSpace(authToken))
			{
				return this.Issue404();
			}

			// Check if Auth Token is Expired
			if(this.UserLoginService.AuthTokenIsExired(authToken))
			{
				return this.Issue404();
			}

			return View(new SetPasswordViewModel { AuthToken = authToken });
		}

		/// <summary>
		/// Executes the Http Post View for SetPassword
		/// </summary>
		[HttpPost]
		[ActionName("set-password")]
		public ActionResult SetPassword(SetPasswordViewModel setPasswordViewModel)
		{
			if(!this.ValidateAndAppendMessages(setPasswordViewModel))
			{
				return View(new SetPasswordViewModel { AuthToken = setPasswordViewModel.AuthToken });
			}

			// Check if Auth Token is Expired
			if (this.UserLoginService.AuthTokenIsExired(setPasswordViewModel.AuthToken))
			{
				return this.Issue404();
			}

			using (var unitOfWork = this.UnitOfWorkFactory.NewUnitOfWork())
			{
				try
				{
					this.UserLoginService.SetPasswordUsingAuthToken(setPasswordViewModel.AuthToken, setPasswordViewModel.Password);
					unitOfWork.Commit();

					this.ForwardMessage(new SuccessMessage { Text = "Your password has been reset.  You may now login using your new password." });

					return RedirectToAction("login");
				}
				catch (Exception ex)
				{
					this.LogHandledException(ex);
					unitOfWork.Rollback();

					this.AppendMessage(new ErrorMessage { Text = GenericMessages.ErrorMessage });
				}
			}

			return View();
		}

		#endregion

		#region CHANGE PASSWORD 

		/// <summary>
		/// Executes the Http Post View for ChangePassword
		/// </summary>
		[HttpPost]
		public ActionResult ChangePassword(ChangePasswordViewModel changePasswordViewModel)
		{
			if(!this.ValidateAndForwardMessages(changePasswordViewModel))
			{
				return RedirectToAction("settings", "user");
			}

			using(var unitOfWork = this.UnitOfWorkFactory.NewUnitOfWork())
			{
				try
				{
					if(!this.UserLoginService.VerifyUserPassword(this.ActiveUser.UserId, changePasswordViewModel.CurrentPassword))
					{
                        return Json(new { Success = false, Message = "The current password you provided is not correct" });
					}

					this.UserLoginService.SetUserPassword(this.ActiveUser.UserId, changePasswordViewModel.NewPassword);
					unitOfWork.Commit();
				}
				catch (Exception ex)
				{
					this.LogHandledException(ex);
					unitOfWork.Rollback();
           
                    return Json(new { Success = false, Message = GenericMessages.ErrorMessage });
				}
			}

		    return Json(new { Success = true, Message = "Your password has been changed" });
		}

		#endregion

		# region OAUTH

		/// <summary>
		/// Executes the View for fbOpenGraph
		/// </summary>
		public ContentResult FbOpenGraph()
		{
			return Content("");
		}

		/// <summary>
		/// Executes the Http Post View for FacebookLogin
		/// </summary>
		public ActionResult OAuthLogin()
		{
			var userSummary = this.ProcessOAuthResponse(Url.Action("OAuthLogin", "Auth", null, "https"));
			return SignInAndRedirect(userSummary, true); // always persist login when oauth is used
		}

		/// <summary>
		/// Executes the OAuthLoginViaDialog view
		/// </summary>
		public ActionResult OAuthLoginViaDialog()
		{
			var userSummary = this.ProcessOAuthResponse(Url.Action("OAuthLoginViaDialog", "Auth", null, "https"));
			ViewBag.LoginViaDialogSuccess = true;
			this.SignIn(userSummary, true); // always persist login when oauth is used

			this.AppendLoginViaDialogSuccessMessage(userSummary);

			return View("LoginViaDialog");
		}

		/// <summary>
		/// Initializes the Facebook Auth
		/// </summary>
		void InitializeFacebookAuth(string fbRedirectUrl)
		{
			// Keep the ReturnUrl for When they come back from FB
			if (!string.IsNullOrWhiteSpace(Request["ReturnUrl"]))
			{
				Session["OAuthReturnUrl"] = Request["ReturnUrl"];
			}

			ViewBag.FacebookAuthRedirectUrl = fbRedirectUrl.ToLower();

			// Set Facebook Auth State Token
			var oauthStateToken = Guid.NewGuid().ToString().Replace("-", "");
			Session["OAuthStateToken"] = "fb-" + oauthStateToken;
		}

		/// <summary>
		/// Processes the OAuthResponse
		/// </summary>
		UserSummary ProcessOAuthResponse(string loginUrl)
		{
			var state = Request["state"];
			var code = Request["code"];

			if (state != Session["OAuthStateToken"] as string)
			{
				throw new SecurityException("OAuth State does not match last generated state - [" + state + "] VS. [" + (Session["OAuthStateToken"] as string) + "]");
			}

			var oAuthUserInfo = this.OAuthService.GetUserInfoFromAuthCode(state, code, loginUrl);

			var userId = this.OAuthService.GetLocalUserIdFromOAuthUserInfo(oAuthUserInfo);

			if (userId == null)
			{
				// LOCATE Existing Users
				userId = this.OAuthService.GetLocalUserIdFromEmailAddress(oAuthUserInfo.EmailAddress);

				// CONNECT Existing Users
				if (userId != null)
				{
					using (var unitOfWork = this.UnitOfWorkFactory.NewUnitOfWork())
					{
						this.OAuthService.ConnectLocalUserToOAuthProvider(userId.Value, oAuthUserInfo);
						unitOfWork.Commit();
					}
				}

				// REGISTER New Users
				if (userId == null)
				{
					User newUser;
					using (var unitOfWork = this.UnitOfWorkFactory.NewUnitOfWork())
					{
						// Register the User
						newUser = this.OAuthService.RegisterNewUser(oAuthUserInfo);
						unitOfWork.Commit();
					}

					userId = newUser.UserId;

                    // Send the Email Message
                    var newAccountEmailMessage = (NewAccountEmailMessage)this.EmailMessageFactory.Make(EmailMessageType.NewAccount);

                    newAccountEmailMessage.ToRecipients.Add(oAuthUserInfo.EmailAddress);
                    this.EmailSender.Send(newAccountEmailMessage);

                    // Track the Login
                    using (var unitOfWork = this.UnitOfWorkFactory.NewUnitOfWork())
					{
						this.UserLoginService.TrackLogin(userId.Value);
						unitOfWork.Commit();
					}
				}
			}

			// Get the User Summary
			var userSummary = this.UserService.GetUserSummaryById(userId.Value);

			Session.Remove("OAuthStateToken");

			return userSummary;
		}

		#endregion

		/// <summary>
		/// Appends a success message for LoginViaDialog
		/// </summary>
		void AppendLoginViaDialogSuccessMessage(UserSummary userSummary, bool editMode = false)
		{
			this.AppendMessage(new SuccessMessage { Text = "Thank you " + userSummary.FirstName + ", " + (editMode ? "You have been logged in" : "Your recipe is being saved") });
		}

		/// <summary>
		/// Signs the User In
		/// </summary>
		void SignIn(UserSummary userSummary, bool persistLogin)
		{
			// Sign User In
			this.AuthenticationService.SignIn(userSummary.UserId.ToString(), persistLogin);
			this.UserResolver.Persist(userSummary);
		}

		/// <summary>
		/// Signs the user in and performs redirection
		/// </summary>
		ActionResult SignInAndRedirect(UserSummary userSummary, bool persistLogin = false)
		{
			// Sign User In
			this.SignIn(userSummary, persistLogin);

			// Redirect
			var redirectUrl = (Session["OAuthReturnUrl"] ?? Request["ReturnUrl"]) != null ? string.Format("{0}{1}", this.WebSettings.RootPath, Session["OAuthReturnUrl"].ToString()) 
				: this.WebSettings.RootPath; 

			Session.Remove("OAuthReturnUrl");

			return RedirectPermanent(redirectUrl);
		}
	}
}