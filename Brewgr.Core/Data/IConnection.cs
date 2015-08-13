using System;

namespace ctorx.Core.Data
{
	public interface IConnection
	{
		/// <summary>
		/// Gets the Connection String
		/// </summary>
		string ConnectionString { get; }
	}
}