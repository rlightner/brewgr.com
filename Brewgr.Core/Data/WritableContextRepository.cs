using System;
using System.Data.Entity;

namespace ctorx.Core.Data
{
	public abstract class WritableContextRepository<TDataContext> : ContextRepository<TDataContext> where TDataContext : IDataContext
	{
		readonly IDataContextResolver DataContextResolver;

		/// <summary>
		/// Gets the Context
		/// </summary>
		protected override DbContext Context
		{
			get { return this.DataContextResolver.GetContext() as DbContext; }
		}

		/// <summary>
		/// ctor the Mighty
		/// </summary>
		protected WritableContextRepository(IDataContextResolver dataContextResolver)
		{
			this.DataContextResolver = dataContextResolver;
		}
	}
}