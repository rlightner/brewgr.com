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
    [RoutePrefix("")]
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

        [Route("features")]
        public ViewResult Features()
        {
            return View();
        }

        [Route("about")]
        public ActionResult About()
        {
            //return Content("About");
            return View();
        }

        [Route("terms")]
        public ViewResult Terms()
        {
            return View();
        }

        [Route("privacy")]
        public ViewResult Privacy()
        {
            return View();
        }

        [Route("faq")]
        public ViewResult Faq()
        {
            return View();
        }

        [Route("howitworks")]
        public ActionResult HowItWorks301()
        {
            return RedirectPermanent("~/how-it-works");
        }

        [Route("how-it-works")]
        public ViewResult HowItWorks()
        {
            return View();
        }

        [Route("open-source-homebrew-software")]
        public ViewResult OpenSourceSoftware()
        {
            return View();
        }

        public ActionResult Blank()
	    {
	        return this.View();
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
		[Route("")]
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
        /// Executes the View for RecentPhotos
        /// </summary>
        [Route("RecentPhotos")]
        public JsonResult RecentPhotos()
		{
			return Json(this.RecipeService.GetRecentRecipesCached(12)
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
        [Route("contact")]
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
        [Route("contact")]
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
        /// Executes the View for Sitemap
        /// </summary>
        [Route("SiteMap")]
        public ContentResult Sitemap()
		{
			this.Response.ContentType = "text/xml";
			return Content(this.SeoSitemap.GenerateXml(this.Url));
		}

	}
}