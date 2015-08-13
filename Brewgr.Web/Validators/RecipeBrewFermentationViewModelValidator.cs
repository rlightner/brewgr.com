using System;
using FluentValidation;

namespace Brewgr.Web.Models
{
	public class BrewSessionFermentationViewModelValidator : AbstractValidator<BrewSessionFermentationViewModel>
	{
		/// <summary>
		/// ctor the Mighty
		/// </summary>
		public BrewSessionFermentationViewModelValidator()
		{
			this.RuleFor(x => x.PitchingTemp)
				.NotNull();
		}
	}
}