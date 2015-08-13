using System;
using System.Collections.Generic;

namespace ctorx.Core.Messaging
{
	public interface IMessageStore
	{
		/// <summary>
		/// Adds a message to the message store
		/// </summary>
		/// <param name="message"></param>
		void AddMessage(IMessage message);

		/// <summary>
		/// Gets messages in the message store
		/// </summary>
		IList<IMessage> GetMessages();
	}
}