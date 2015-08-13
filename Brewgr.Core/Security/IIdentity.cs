using System;

namespace ctorx.Core.Security
{
	public interface IIdentity
	{
		/// <summary>
		/// Gets the username of the Identity
		/// </summary>
		string UserName { get; }
	}
}