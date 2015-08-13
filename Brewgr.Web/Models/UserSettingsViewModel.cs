using System;
using System.Linq;
using ctorx.Core.Validation;
using Brewgr.Web.Validators;

namespace Brewgr.Web.Models
{
	public class UserSettingsViewModel : ValidatesWith<UserSettingsViewModelValidator>
	{
		/// <summary>
		/// Gets or sets the UserId
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// Gets or sets the Username
		/// </summary>
		public string Username { get; set; }

		/// <summary>
		/// Gets or sets the HasCustomUsername
		/// </summary>
		public bool HasCustomUsername { get; set; }

		/// <summary>
		/// Gets or sets the EmailAddress
		/// </summary>
		public string EmailAddress { get; set; }

		/// <summary>
		/// Gets or sets the FirstName
		/// </summary>
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets the LastName
		/// </summary>
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets the Bio
		/// </summary>
		public string Bio { get; set; }

		/// <summary>
		/// Gets or sets the RecipeCommentNotifications
		/// </summary>
		public bool RecipeCommentNotifications { get; set; }

		/// <summary>
		/// Gets or sets the BrewSessionCommentNotifications
		/// </summary>
		public bool BrewSessionCommentNotifications { get; set; }

		/// <summary>
		/// Gets or sets the BrewerFollowNotifications
		/// </summary>
		public bool BrewerFollowNotifications { get; set; }

		/// <summary>
		/// Gets or sets the SiteFeatureNotifications
		/// </summary>
		public bool SiteFeatureNotifications { get; set; }

		/// <summary>
		/// Gets or sets the SiteOutageNotifications
		/// </summary>
		public bool SiteOutageNotifications { get; set; }
	}
}