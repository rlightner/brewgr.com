using System;
using System.IO;

namespace ctorx.Core.Email
{
	public interface IEmailAttachment
	{
		/// <summary>
		/// Gets the attachment Name
		/// </summary>
		string Name { get; }
		
		/// <summary>
		/// Gets the attachment content bytes
		/// </summary>
		byte[] Content { get; }

		/// <summary>
		/// Gets a content stream for the attachment
		/// </summary>
		Stream GetContentStream();
	}
}