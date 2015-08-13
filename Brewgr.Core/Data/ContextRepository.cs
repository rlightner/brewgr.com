using System;
using System.Data.Entity;
using System.Linq;

namespace ctorx.Core.Data
{
	public abstract class ContextRepository<TDataContext> : IContextRepository<TDataContext> where TDataContext : IDataContext
	{
		/// <summary>
		/// Gets the Context
		/// </summary>
		protected abstract DbContext Context { get; }

		/// <summary>
		/// Gets a set of the specified type
		/// </summary>
		public IQueryable<TEntity> GetSet<TEntity>() where TEntity : class
		{
			return this.Context.Set<TEntity>();
		}

		/// <summary>
		/// Gets a set of the specified type
		/// </summary>
		public IQueryable GetSet(Type type)
		{
			return this.Context.Set(type);
		}

		/// <summary>
		/// Adds an Entity
		/// </summary>
		public void Add<TEntity>(TEntity entity) where TEntity : class
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}

			this.Context.Set<TEntity>().Add(entity);
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

			this.Context.Set<TEntity>().Attach(entity);
		}

		/// <summary>
		/// Deletes the Entity
		/// </summary>
		public void Delete<TEntity>(TEntity entity) where TEntity : class
		{
			if(entity == null)
			{
				throw new ArgumentNullException("entity");
			}

			this.Context.Set<TEntity>().Remove(entity);
		}
	}
}