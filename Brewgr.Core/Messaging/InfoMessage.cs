
using System;

namespace ctorx.Core.Messaging
{
	public class InfoMessage : AbstractSystemMessage
	{
		public InfoMessage() : base(MessageType.Info) { }
	}
}