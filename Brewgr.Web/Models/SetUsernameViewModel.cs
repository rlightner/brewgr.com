using System;
using System.Linq;
using ctorx.Core.Validation;
using Brewgr.Web.Validators;

namespace Brewgr.Web.Models
{
	public class SetUsernameViewModel : ValidatesWith<SetUsernameViewModelValidator>
	{
		/// <summary>
		/// Gets or sets the Username
		/// </summary>
		public string Username { get; set; }
	}
}