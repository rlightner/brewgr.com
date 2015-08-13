using System;
using System.Text.RegularExpressions;
using Brewgr.Web.Models;
using FluentValidation;

namespace Brewgr.Web.Validators
{
	public class SetUsernameViewModelValidator : AbstractValidator<SetUsernameViewModel>
	{
		/// <summary>
		/// ctor the Mighty
		/// </summary>
		public SetUsernameViewModelValidator()
		{
			this.RuleFor(x => x.Username)
				.NotEmpty().WithMessage("required")				
				.Length(3, 25).WithMessage("check length")
				.Matches(@"^\w+$").WithMessage("invalid characters");
		}
	}
}