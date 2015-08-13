using System;
using Brewgr.Web.Models;
using FluentValidation;

namespace Brewgr.Web.Validators
{
	public class PasswordResetViewModelValidator : AbstractValidator<PasswordResetViewModel>
	{
		/// <summary>
		/// ctor the Mighty
		/// </summary>
		public PasswordResetViewModelValidator()
		{
			this.RuleFor(x => x.EmailAddress)
				.NotEmpty()
					.WithMessage("Please enter your email address")
				.EmailAddress()
					.WithMessage("Please enter a valid email address");
		}
	}
}