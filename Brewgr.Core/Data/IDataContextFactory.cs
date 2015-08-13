using System;

namespace ctorx.Core.Data
{
	public interface IDataContextFactory<TDataContext> where TDataContext : IDataContext
	{
		/// <summary>
		/// Makes a TDataContext
		/// </summary>
		TDataContext Make();
	}
}