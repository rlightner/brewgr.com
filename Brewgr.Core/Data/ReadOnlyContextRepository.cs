using System;
using System.Data.Entity;

namespace ctorx.Core.Data
{
	public abstract class ReadOnlyContextRepository<TDataContext> : ContextRepository<TDataContext> where TDataContext : IDataContext
	{
		readonly IDataContextFactory<TDataContext> DataContextFactory;

		/// <summary>
		/// ctor the Mighty
		/// </summary>
		public ReadOnlyContextRepository(IDataContextFactory<TDataContext> dataContextFactory )
		{
			this.DataContextFactory = dataContextFactory;
		}

		DbContext _Context;

		/// <summary>
		/// Gets the Context
		/// </summary>
		protected override DbContext Context
		{
			get
			{
				return this._Context = this.DataContextFactory.Make() as DbContext;
			}
		}
	}
}