using System;
using System.Linq;
using System.Web.Mvc;
using Brewgr.Web.Models;
using ctorx.Core.Data;
using Brewgr.Web.Core.Data;
using Brewgr.Web.Core.Model;
using Brewgr.Web.Core.Service;

namespace Brewgr.Web.Controllers
{
	public class MarketingController : BrewgrController
	{
		readonly IUnitOfWorkFactory<BrewgrContext> UnitOfWorkFactory;
		readonly IMarketingService MarketingService;

		/// <summary>
		/// ctor the Mighty
		/// </summary>
		public MarketingController(IUnitOfWorkFactory<BrewgrContext> unitOfWorkFactory, IMarketingService marketingService)
		{
			this.UnitOfWorkFactory = unitOfWorkFactory;
			this.MarketingService = marketingService;
		}

		/// <summary>
		/// Executes the EmailSignUp view
		/// </summary>
		[ForceHttps]
		public EmptyResult EmailSignUp()
		{
			var emailAddress = Request["emailAddress"];

			if(string.IsNullOrWhiteSpace(emailAddress))
			{
				throw new ArgumentNullException("emailAddress");
			}

			// Create It
			var newsletterSignup = new NewsletterSignup
			{
				EmailAddress = emailAddress,
				Source = "HomePage",
				IPAddress = Request.UserHostAddress,
				DateCreated = DateTime.Now
			};

			// Save It
			using(var unitOfWork = this.UnitOfWorkFactory.NewUnitOfWork())
			{
				this.MarketingService.AddNewsletterSignup(newsletterSignup);
				unitOfWork.Commit();
			}

			return new EmptyResult();
		}

		/// <summary>
		/// Executes the View for Feedback
		/// </summary>
		public ViewResult Feedback()
		{
			return View();
		}

		/// <summary>
		/// Executes the Http Post View for UserSuggestion
		/// </summary>
		[HttpPost]
		public ActionResult Feedback(FeedbackViewModel feedbackViewModel)
		{
			// Simple Validation (one property model)
			if (feedbackViewModel == null || string.IsNullOrWhiteSpace(feedbackViewModel.Feedback) || feedbackViewModel.Feedback.Length > 1000)
			{
				return this.Issue404();
			}

			// Only 3 submissions allowed in 5 minutes
			if (!this.MarketingService.UserCanSendFeedback())
			{
				return View("FeedbackReceived", false);
			}

			using (var unitOfWork = this.UnitOfWorkFactory.NewUnitOfWork())
			{
				try
				{
					this.MarketingService.LogUserFeedback(feedbackViewModel.Feedback);
					unitOfWork.Commit();
				}
				catch (Exception ex)
				{
					this.LogHandledException(ex);
					unitOfWork.Rollback();
				}
			}

			return View("FeedbackReceived", true);
		}

		/// <summary>
		/// Executes the View for FeedbackReceived
		/// </summary>
		public ViewResult FeedbackReceived()
		{
			return View();
		}
	}
}