using System;
using ctorx.Core.Email;

namespace Brewgr.Web.Email
{
	public interface IEmailMessageFactory
	{
		/// <summary>
		/// Makes an Email Message
		/// </summary>
		IEmailMessage Make(EmailMessageType emailMessageType);
	}
}