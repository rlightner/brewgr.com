using System;
using System.IO;

namespace ctorx.Core.Email
{
	public class EmailAttachment : IEmailAttachment
	{
		/// <summary>
		/// Gets the attachment Name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets the attachment content bytes
		/// </summary>
		public byte[] Content { get; set; }

		/// <summary>
		/// Gets a content stream for the attachment
		/// </summary>
		public Stream GetContentStream()
		{
			return new MemoryStream(this.Content);
		}
	}
}