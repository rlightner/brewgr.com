using System;
using FluentValidation;
using FluentValidation.Results;

namespace ctorx.Core.Validation
{
	public abstract class ValidatesWith<TValidatorType> : AbstractValidator<TValidatorType>, IValidatingViewModel where TValidatorType : IValidator, new()
	{
		/// <summary>
		/// Validates the Model
		/// </summary>
		public ValidationResult Validate()
		{
			return new TValidatorType().Validate(this);
		}
	}
}