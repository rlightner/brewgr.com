using System;

namespace ctorx.Core.Data
{
	public interface IUnitOfWorkFactory<TDataContext> where TDataContext : IDataContext
	{
		/// <summary>
		/// Makes a unit of work
		/// </summary>
		IUnitOfWork<TDataContext> NewUnitOfWork();
	}
}