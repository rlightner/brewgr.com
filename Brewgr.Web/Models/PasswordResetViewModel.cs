using System;
using System.Linq;
using ctorx.Core.Validation;
using Brewgr.Web.Validators;

namespace Brewgr.Web.Models
{
	public class PasswordResetViewModel : ValidatesWith<PasswordResetViewModelValidator>
	{
		/// <summary>
		/// Gets or sets the EmailAddress
		/// </summary>
		public string EmailAddress { get; set; }
	}
}