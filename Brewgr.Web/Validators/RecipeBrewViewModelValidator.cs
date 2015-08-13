using System;
using System.Linq;
using Brewgr.Web.Models;
using FluentValidation;

namespace Brewgr.Web.Validators
{
	[Obsolete]
	public class BrewSessionViewModelValidator : AbstractValidator<BrewSessionViewModel>
	{
		/// <summary>
		/// ctor the Mighty
		/// </summary>
		public BrewSessionViewModelValidator()
		{
			this.RuleFor(x => x.BrewDate)
				.NotEmpty();
		}
	}
}