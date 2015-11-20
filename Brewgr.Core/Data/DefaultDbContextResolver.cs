using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace ctorx.Core.Data
{
	public class DefaultDbContextResolver<TDbContext> : IDbContextResolver<TDbContext> where TDbContext : DbContext
	{
		IDbContextFactory<TDbContext> DbContextFactory;
		TDbContext Context;
		
		bool IsDisposed;

		/// <summary>
		/// ctor the Mighty
		/// </summary>
		public DefaultDbContextResolver(IDbContextFactory<TDbContext> dataContextFactory)
		{
			this.DbContextFactory = dataContextFactory;
		}

		/// <summary>
		/// Resolves the Data Context
		/// </summary>
		public DbContext GetContext()
		{
			if(this.Context == null)
			{
				this.Context = this.DbContextFactory.Create();
			}

			return this.Context;
		}

		/// <summary>
		/// Destroys the Context
		/// </summary>
		public void DestroyContext()
		{
			if(this.Context == null)
			{
				return;
			}

			this.Context.Dispose();
			this.Context = null;
		}

		/// <summary>
		/// Disposes the object
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Disposes the object
		/// </summary>
		protected virtual void Dispose(bool disposing)
		{
			if (!IsDisposed)
			{
				if (disposing)
				{
					this.DestroyContext();
				}

				this.DbContextFactory = null;
				this.IsDisposed = true;
			}
		}
	}
}