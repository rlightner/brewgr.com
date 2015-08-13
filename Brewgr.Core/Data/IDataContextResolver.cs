using System;

namespace ctorx.Core.Data
{
	public interface IDataContextResolver : IDisposable
	{
		/// <summary>
		/// Resolves the Data Context
		/// </summary>
		IDataContext GetContext();

		/// <summary>
		/// Destroys the Context
		/// </summary>
		void DestroyContext();
	}

	public interface IDataContextResolver<TDataContext> : IDataContextResolver where TDataContext : IDataContext { }
}