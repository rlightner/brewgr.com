using System;
using System.Collections.Generic;

namespace ctorx.Core.Email
{
	public abstract class AbstractEmailMessage : IEmailMessage
	{
		/// <summary>
		/// Gets or sets the email sender address
		/// </summary>
		public string SenderAddress { get; set; }

		/// <summary>
		/// Gets or sets the email sender display name
		/// </summary>
		public string SenderDisplayName { get; set; }

		/// <summary>
		/// Gets or sets the To Recipients
		/// </summary>
		public IList<string> ToRecipients { get; private set; }

		/// <summary>
		/// Gets or sets the CC Recipients
		/// </summary>
		public IList<string> CcRecipients { get; private set; }

		/// <summary>
		/// GEts or sets the Bcc Recipients
		/// </summary>
		public IList<string> BccRecipients { get; private set; }

		/// <summary>
		/// Gets or sets the ReplyToRecipients
		/// </summary>
		public IList<string> ReplyToRecipients { get; set; }

		/// <summary>
		/// Gets a list of attachments
		/// </summary>
		public IList<IEmailAttachment> Attachments { get; private set; }

		/// <summary>
		/// Gets or sets the message subject
		/// </summary>
		public string Subject { get; set; }

		/// <summary>
		/// Gets or sets a value specifying whether or not
		/// the message should be formatted as HTML
		/// </summary>
		public bool FormatAsHtml { get; set; }

		/// <summary>
		/// ctor the Mighty
		/// </summary>
		protected AbstractEmailMessage()
		{
			this.ToRecipients = new List<string>();
			this.CcRecipients = new List<string>();
			this.BccRecipients = new List<string>();
			this.ReplyToRecipients = new List<string>();
			this.Attachments = new List<IEmailAttachment>();
		}

		/// <summary>
		/// Builds the message body
		/// </summary>
		public abstract string BuildMessageBody();
	}
}