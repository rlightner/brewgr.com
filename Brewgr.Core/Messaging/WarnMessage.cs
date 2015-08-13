

using System;

namespace ctorx.Core.Messaging
{
	public class WarnMessage : AbstractSystemMessage
	{
		public WarnMessage() : base(MessageType.Warn) { }
	}
}