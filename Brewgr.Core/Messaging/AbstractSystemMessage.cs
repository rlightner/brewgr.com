
using System;

namespace ctorx.Core.Messaging
{
	public abstract class AbstractSystemMessage : IMessage
	{
		/// <summary>
		/// Gets or sets the message severity
		/// </summary>
		public MessageType Type { get; private set; }

		/// <summary>
		/// Gets or sets the message text
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// ctor the Mighty
		/// </summary>
		protected AbstractSystemMessage(MessageType type)
		{
			this.Type = type;
		}
	}
}