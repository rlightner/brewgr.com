

using System;

namespace ctorx.Core.Messaging
{
	public class ErrorMessage : AbstractSystemMessage
	{
		public ErrorMessage() : base(MessageType.Error) { }
	}
}