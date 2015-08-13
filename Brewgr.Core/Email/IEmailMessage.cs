using System;
using System.Collections.Generic;

namespace ctorx.Core.Email
{
	public interface IEmailMessage
	{
		/// <summary>
		/// Gets or sets the email sender address
		/// </summary>
		string SenderAddress { get; }

		/// <summary>
		/// Gets or sets the email sender display name
		/// </summary>
		string SenderDisplayName { get; }

		/// <summary>
		/// Gets or sets the To Recipients
		/// </summary>
		IList<string> ToRecipients { get; }

		/// <summary>
		/// Gets or sets the CC Recipients
		/// </summary>
		IList<string> CcRecipients { get; }

		/// <summary>
		/// GEts or sets the Bcc Recipients
		/// </summary>
		IList<string> BccRecipients { get; }

		/// <summary>
		/// Gets or sets the ReplyToRecipients
		/// </summary>
		IList<string> ReplyToRecipients { get; set; }

		/// <summary>
		/// Gets a list of attachments
		/// </summary>
		IList<IEmailAttachment> Attachments { get; }

		/// <summary>
		/// Gets or sets the message subject
		/// </summary>
		string Subject { get; }

		/// <summary>
		/// Gets or sets a value specifying whether or not
		/// the message should be formatted as HTML
		/// </summary>
		bool FormatAsHtml { get; }

		/// <summary>
		/// Builds the message body
		/// </summary>
		string BuildMessageBody();
	}
}