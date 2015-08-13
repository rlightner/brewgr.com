using System;
using System.Linq;
using ctorx.Core.Validation;
using Brewgr.Web.Validators;

namespace Brewgr.Web.Models
{
	public class LoginViewModel : ValidatesWith<LoginViewModelValidator>
	{
		/// <summary>
		/// Gets or sets the EmailAddress
		/// </summary>
		public string EmailAddress { get; set; }

		/// <summary>
		/// Gets or sets the Password
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		/// Gets or sets the KeepMeLoggedIn
		/// </summary>
		public bool KeepMeLoggedIn {
			get { return true; }
		}
	}
}