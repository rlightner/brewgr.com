using System;
using System.Linq;
using Brewgr.Web.Models;
using FluentValidation;

namespace Brewgr.Web.Validators
{
	public class CommentAddViewModelValidator : AbstractValidator<CommentAddViewModel>
	{
		/// <summary>
		/// ctor the Mighty
		/// </summary>
		public CommentAddViewModelValidator()
		{
			this.RuleFor(m => m.CommentText)
				.NotEmpty()
					.WithMessage("Please enter a comment")
				.NotEqual("Write a comment...")
					.WithMessage("Please enter a comment");
		}
	}
}