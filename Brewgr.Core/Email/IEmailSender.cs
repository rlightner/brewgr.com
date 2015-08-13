using System;

namespace ctorx.Core.Email
{
	public interface IEmailSender
	{
		/// <summary>
		/// Sends an Email Message
		/// </summary>
		void Send(IEmailMessage emailMessage);
	}
}