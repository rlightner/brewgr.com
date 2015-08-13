using System;
using System.Data.Entity;
using System.Linq;

namespace ctorx.Core.Data
{
	public abstract class EntityRepository<TEntity> : IEntityRepository<TEntity> where TEntity : class
	{
		readonly IDataContextResolver DataContextResolver;

		DbContext Context { get { return this.DataContextResolver.GetContext() as DbContext;  } }
		IDbSet<TEntity> Set { get { return this.Context.Set<TEntity>(); } }

		/// <summary>
		/// ctor the Mighty
		/// </summary>
		protected EntityRepository(IDataContextResolver dataContextResolver)
		{
			this.DataContextResolver = dataContextResolver;
		}

		/// <summary>
		/// Gets a queryable entity set
		/// </summary>
		public IQueryable<TEntity> GetQueryable()
		{
			return this.Set;
		}

		/// <summary>
		/// Attaches an Entity
		/// </summary>
		public void Add(TEntity entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}

			this.Set.Add(entity);
		}

		/// <summary>
		/// Attaches an Entity
		/// </summary>
		public void Attach(TEntity entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}

			this.Set.Attach(entity);
		}

		/// <summary>
		/// Deletes the Entity
		/// </summary>
		public void Delete(TEntity entity)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}

			this.Set.Remove(entity);
		}
	}
}