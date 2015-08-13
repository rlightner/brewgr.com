using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Brewgr.Web.Core.Data;
using Brewgr.Web.Core.Model;
using ctorx.Core.Identity;

namespace Brewgr.Web.Core.Service
{
	public class DefaultMarketingService : IMarketingService
	{
		readonly IBrewgrRepository Repository;
		readonly IUserResolver UserResolver;
		readonly IUserHostAddressResolver UserHostAddressResolver;

		/// <summary>
		/// ctor the Mighty
		/// </summary>
		public DefaultMarketingService(IBrewgrRepository repository, IUserResolver userResolver, IUserHostAddressResolver userHostAddressResolver)
		{
			this.Repository = repository;
			this.UserResolver = userResolver;
			this.UserHostAddressResolver = userHostAddressResolver;
		}

		/// <summary>
		/// Adds a Newsletter Signup
		/// </summary>
		public void AddNewsletterSignup(NewsletterSignup newsletterSignup)
		{
			if(newsletterSignup == null)
			{
				throw new ArgumentNullException("newsletterSignup");
			}

			// Ignore the Request if Email Already Exists
			if(this.Repository.GetSet<NewsletterSignup>().Any(x => x.EmailAddress == newsletterSignup.EmailAddress))
			{
				return;
			}

			this.Repository.Add(newsletterSignup);
		}

		/// <summary>
		/// Logs user feedback
		/// </summary>
		public void LogUserFeedback(string feedback)
		{
			var currentUser = this.UserResolver.Resolve();

			var userFeedback = new UserFeedback
			{
				UserId = currentUser.UserId > 0 ? (int?)currentUser.UserId : null,
				Feedback = feedback,
				UserHostAddress = this.UserHostAddressResolver.Resolve(),
				DateCreated = DateTime.Now				
			};

			this.Repository.Add(userFeedback);
		}

		/// <summary>
		/// Gets a list of recent user feedback
		/// </summary>
		public IList<UserFeedback> GetFeedback()
		{
			return this.Repository.GetSet<UserFeedback>()
				.Include(x => x.User)
				.Where(x => x.DateResponded == null && x.RespondedBy == null)
				.OrderByDescending(x => x.DateCreated)
				.ToList();
		}

		/// <summary>
		/// Determines if a user can send feedback
		/// </summary>
		public bool UserCanSendFeedback()
		{
			var startDate = DateTime.Now.AddMinutes(-5);
			var ipaddress = this.UserHostAddressResolver.Resolve();

			var count = this.Repository.GetSet<UserFeedback>()
				.Where(x => x.UserHostAddress == ipaddress)
				.Where(x => x.DateCreated >= startDate)
				.Count();

			return count < 3;
		}
	}
}