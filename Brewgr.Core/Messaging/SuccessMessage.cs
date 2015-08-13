

using System;

namespace ctorx.Core.Messaging
{
	public class SuccessMessage : AbstractSystemMessage
	{
		public SuccessMessage() : base(MessageType.Success) { }
	}
}