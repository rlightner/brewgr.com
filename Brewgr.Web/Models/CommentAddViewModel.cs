using System;
using Brewgr.Web.Validators;
using ctorx.Core.Validation;
using Brewgr.Web.Core.Model;

namespace Brewgr.Web.Models
{
	public class CommentAddViewModel : ValidatesWith<CommentAddViewModelValidator>
	{
        /// <summary>
		/// Gets or sets the GenericId
		/// </summary>
		public int GenericId { get; set; }

		/// <summary>
		/// Gets or sets the CommentText
		/// </summary>
		public string CommentText { get; set; }

        /// <summary>
        /// Gets the Avatar
        /// </summary>
        public string GetAvatar(int size, string email)
        {
            return UserAvatar.GetAvatar(size, email);
        }

        /// <summary>
        /// Gets or sets the CommentType
        /// </summary>
        public CommentType CommentType { get; set; }
		
	}
}