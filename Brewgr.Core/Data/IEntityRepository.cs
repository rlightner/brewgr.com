using System;
using System.Linq;

namespace ctorx.Core.Data
{
	public interface IEntityRepository<TEntity> where TEntity : class
	{
		/// <summary>
		/// Gets a queryable entity set
		/// </summary>
		IQueryable<TEntity> GetQueryable();

		/// <summary>
		/// Adds an Entity
		/// </summary>
		void Add(TEntity entity);

		/// <summary>
		/// Attaches an Entity
		/// </summary>
		void Attach(TEntity entity);

		/// <summary>
		/// Deletes the Entity
		/// </summary>
		void Delete(TEntity entity);
	}
}