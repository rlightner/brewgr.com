using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ctorx.Core.Messaging;

namespace ctorx.Core.Web
{
	public class TempDataMessageStore : IMessageStore
	{
		const string PASSED_MESSAGES_KEY = "passed-messaged";
		readonly TempDataDictionary TempData;

		/// <summary>
		/// ctor the Mighty
		/// </summary>
		public TempDataMessageStore(TempDataDictionary tempData)
		{
			this.TempData = tempData;
		}

		/// <summary>
		/// Pushes a message
		/// </summary>
		public void AddMessage(IMessage message)
		{
			if (message == null)
			{
				throw new ArgumentNullException("message");
			}

			var messages = this.TempData[PASSED_MESSAGES_KEY] as List<IMessage> ?? new List<IMessage>();

			messages.Add(message);
			this.TempData[PASSED_MESSAGES_KEY] = messages;
		}

		/// <summary>
		/// Gets any pushed messages
		/// </summary>
		public IList<IMessage> GetMessages()
		{
			return this.TempData[PASSED_MESSAGES_KEY] as List<IMessage>;
		}

	}
}