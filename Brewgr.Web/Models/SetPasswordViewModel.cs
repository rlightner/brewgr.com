using System;
using System.Linq;
using ctorx.Core.Validation;
using Brewgr.Web.Validators;

namespace Brewgr.Web.Models
{
	public class SetPasswordViewModel : ValidatesWith<SetPasswordViewModelValidator>
	{
		/// <summary>
		/// Gets or sets the AuthToken
		/// </summary>
		public string AuthToken { get; set; }

		/// <summary>
		/// Gets or sets the EmailAddress
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		/// Gets or sets the ConfirmEmailAddress
		/// </summary>
		public string ConfirmPassword { get; set; }
	}
}