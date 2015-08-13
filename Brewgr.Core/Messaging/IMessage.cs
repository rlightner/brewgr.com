using System;

namespace ctorx.Core.Messaging
{
	public interface IMessage
	{
		/// <summary>
		/// Gets the type of message
		/// </summary>
		MessageType Type { get; }

		/// <summary>
		/// Gets the message text
		/// </summary>
		string Text { get; }
	}
}