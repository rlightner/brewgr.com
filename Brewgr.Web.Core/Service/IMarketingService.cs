using System;
using System.Collections.Generic;
using Brewgr.Web.Core.Model;

namespace Brewgr.Web.Core.Service
{
	public interface IMarketingService
	{
		/// <summary>
		/// Adds a Newsletter Signup
		/// </summary>
		void AddNewsletterSignup(NewsletterSignup newsletterSignup);

		/// <summary>
		/// Logs user feedback
		/// </summary>
		void LogUserFeedback(string suggestion);

		/// <summary>
		/// Gets a list of feedback that hasn't been responded to
		/// </summary>
		IList<UserFeedback> GetFeedback();

		/// <summary>
		/// Determines if a user can send feedback
		/// </summary>
		bool UserCanSendFeedback();
	}
}