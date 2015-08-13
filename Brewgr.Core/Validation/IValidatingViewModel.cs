using System;
using FluentValidation;
using FluentValidation.Results;

namespace ctorx.Core.Validation
{
	public interface IValidatingViewModel
	{
		/// <summary>
		/// Validates the Model
		/// </summary>
		ValidationResult Validate();
	}
}