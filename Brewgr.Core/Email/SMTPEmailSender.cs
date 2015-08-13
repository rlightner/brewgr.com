using System;
using System.Net;
using System.Net.Mail;
using ctorx.Core.Collections;

namespace ctorx.Core.Email
{
	public class SmtpEmailSender : IEmailSender
	{
		readonly ISmtpConfiguration SMTPConfiguration;

		public SmtpEmailSender(ISmtpConfiguration smtpConfiguration)
		{
			this.SMTPConfiguration = smtpConfiguration;
		}

		/// <summary>
		/// Sends the provided email message
		/// </summary>
		public void Send(IEmailMessage emailMessage)
		{
			if (emailMessage == null)
			{
				throw new ArgumentNullException("emailMessage");
			}

            // Fail Gracefully if SMTP Settings are not set
		    if(string.IsNullOrWhiteSpace(this.SMTPConfiguration.Host) || this.SMTPConfiguration.Port == 0)
		    {
		        return;
		    }

			var message = new MailMessage();

			// Set Sender
			message.From = new MailAddress(emailMessage.SenderAddress, emailMessage.SenderDisplayName);

			// Set Recipients
			emailMessage.ToRecipients.ForEach(x => message.To.Add(x));
			emailMessage.CcRecipients.ForEach(x => message.CC.Add(x));
			emailMessage.BccRecipients.ForEach(x => message.Bcc.Add(x));
			emailMessage.ReplyToRecipients.ForEach(x => message.ReplyToList.Add(x));

			// Set Content
			message.IsBodyHtml = emailMessage.FormatAsHtml;
			message.Body = emailMessage.BuildMessageBody();
			message.Subject = emailMessage.Subject;

			// Set Attachments
			if(emailMessage.Attachments.Count > 0)
			{
				emailMessage.Attachments.ForEach(x =>
				{
					var attachment = new Attachment(x.GetContentStream(), x.Name);
					message.Attachments.Add(attachment);
				});
			}

			// Set SMTP Settings
			var smtpClient = new SmtpClient(this.SMTPConfiguration.Host, this.SMTPConfiguration.Port);
			smtpClient.EnableSsl = this.SMTPConfiguration.EnableSSL;

			// Set Credentials
			if (!string.IsNullOrWhiteSpace(this.SMTPConfiguration.Username) || !string.IsNullOrWhiteSpace(this.SMTPConfiguration.Password))
			{
				smtpClient.UseDefaultCredentials = this.SMTPConfiguration.UseDefaultCredentials;
				smtpClient.Credentials = new NetworkCredential(this.SMTPConfiguration.Username, this.SMTPConfiguration.Password);
			}

			// Send Message
			smtpClient.Send(message);
		}
	}
}