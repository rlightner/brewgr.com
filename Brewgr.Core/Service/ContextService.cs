using System;
using ctorx.Core.Data;

namespace ctorx.Core.Service
{
	public abstract class ContextService<TContext> : IContextService<TContext> where TContext : IDataContext
	{
		readonly IContextRepository<TContext> Repository;

		/// <summary>
		/// ctor the Mighty
		/// </summary>
		public ContextService(IContextRepository<TContext> repository)
		{
			this.Repository = repository;
		}

		/// <summary>
		/// Adds an Entity
		/// </summary>
		public void Add<TEntity>(TEntity entity) where TEntity : class
		{
			if(entity == null)
			{
				throw new ArgumentNullException("entity");
			}

			this.Repository.Add(entity);
		}

		/// <summary>
		/// Attaches an Entity
		/// </summary>
		public void Attach<TEntity>(TEntity entity) where TEntity : class
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}

			this.Repository.Attach(entity);
		}

		/// <summary>
		/// Deletes an Entity
		/// </summary>
		public void Delete<TEntity>(TEntity entity) where TEntity : class
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}

			this.Repository.Delete(entity);
		}
	}
}