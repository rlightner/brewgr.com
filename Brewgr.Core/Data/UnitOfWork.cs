using System;

namespace ctorx.Core.Data
{
	public class UnitOfWork<TDataContext> : IUnitOfWork<TDataContext> where TDataContext : IDataContext
	{
		IDataContextResolver<TDataContext> DbContextResolver;

		bool IsDisposed = false;

		IDataContext Context
		{
			get { return this.DbContextResolver.GetContext(); }
		}

		/// <summary>
		/// ctor the Mighty
		/// </summary>
		public UnitOfWork(IDataContextResolver<TDataContext> dbContextResolver)
		{
			this.DbContextResolver = dbContextResolver;

			// Multiple units of work for the same context cannot exist
			this.DbContextResolver.DestroyContext();
		}

		/// <summary>
		/// Commits the changes
		/// </summary>
		public void Commit()
		{
			this.Context.SaveChanges();
		}

		/// <summary>
		/// Rollsback the Changes
		/// </summary>
		public void Rollback()
		{
			this.Dispose();
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
					this.DbContextResolver.DestroyContext();;
				}

				this.IsDisposed = true;
			}
		}
	}
}