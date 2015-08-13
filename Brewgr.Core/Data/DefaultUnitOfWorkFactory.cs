using System;

namespace ctorx.Core.Data
{
	public class DefaultUnitOfWorkFactory<TDataContext> : IUnitOfWorkFactory<TDataContext> where TDataContext : IDataContext
	{
		readonly IDataContextResolver<TDataContext> DataContextResolver;

		/// <summary>
		/// ctor the Mighty
		/// </summary>
		public DefaultUnitOfWorkFactory(IDataContextResolver<TDataContext> dataContextResolver)
		{
			this.DataContextResolver = dataContextResolver;
		}

		/// <summary>
		/// Makes a new unit of work
		/// </summary>
		public IUnitOfWork<TDataContext> NewUnitOfWork()
		{
			return new UnitOfWork<TDataContext>(this.DataContextResolver);
		}
	}
}