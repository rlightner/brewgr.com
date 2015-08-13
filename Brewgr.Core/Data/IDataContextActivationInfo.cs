using System;

namespace ctorx.Core.Data
{
	public interface IDataContextActivationInfo<TDataContext> where TDataContext : IDataContext
	{
		/// <summary>
		/// Gets the Connection String
		/// </summary>
		string ConnectionString { get; }
	}
}