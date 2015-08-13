using System;
using System.Linq;

namespace Brewgr.Web.Core.Model
{
	public class NewsletterSignup
	{
		/// <summary>
		/// Gets or sets the NewsletterSignupId
		/// </summary>
		public int NewsletterSignupId { get; set; }

		/// <summary>
		/// Gets or sets the EmailAddress
		/// </summary>
		public string EmailAddress { get; set; }

		/// <summary>
		/// Gets or sets the IPAddress
		/// </summary>
		public string IPAddress { get; set; }

		/// <summary>
		/// Gets or sets the Source
		/// </summary>
		public string Source { get; set; }

		/// <summary>
		/// Gets or sets the DateCreated
		/// </summary>
		public DateTime DateCreated { get; set; }
	}
}