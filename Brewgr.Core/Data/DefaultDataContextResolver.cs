using System;

namespace ctorx.Core.Data
{
	public class DefaultDataContextResolver<TDataContext> : IDataContextResolver<TDataContext> where TDataContext : class, IDataContext
	{
		IDataContextFactory<TDataContext> DataContextFactory;
		TDataContext Context;
		
		bool IsDisposed = false;

		/// <summary>
		/// ctor the Mighty
		/// </summary>
		public DefaultDataContextResolver(IDataContextFactory<TDataContext> dataContextFactory)
		{
			this.DataContextFactory = dataContextFactory;
		}

		/// <summary>
		/// Resolves the Data Context
		/// </summary>
		public IDataContext GetContext()
		{
			if(this.Context == null)
			{
				this.Context = this.DataContextFactory.Make();
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

				this.IsDisposed = true;
			}
		}
	}
}