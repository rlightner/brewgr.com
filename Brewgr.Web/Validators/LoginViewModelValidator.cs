using System;
using Brewgr.Web.Models;
using FluentValidation;

namespace Brewgr.Web.Validators
{
	public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
	{
		/// <summary>
		/// ctor the Mighty
		/// </summary>
		public LoginViewModelValidator()
		{
			this.RuleFor(x => x.EmailAddress)
				.NotEmpty()
				.WithMessage("Please enter an email address")
				.EmailAddress();

			this.RuleFor(x => x.Password)
				.NotEmpty()
				.WithMessage("Please enter your password");
		}
	}
}