using System;
using System.Data.Entity;
using System.Linq;

namespace ctorx.Core.Data
{
	public interface IContextRepository<TDataContext> where TDataContext : IDataContext
	{
		/// <summary>
		/// Gets a set of the specified type
		/// </summary>
		IQueryable<TEntity> GetSet<TEntity>() where TEntity : class;

		/// <summary>
		/// Gets a set of the specified type
		/// </summary>
		IQueryable GetSet(Type type);

		/// <summary>
		/// Adds an Entity
		/// </summary>
		void Add<TEntity>(TEntity entity) where TEntity : class;

		/// <summary>
		/// Attaches an Entity
		/// </summary>
		void Attach<TEntity>(TEntity entity) where TEntity : class;

		/// <summary>
		/// Deletes the Entity
		/// </summary>
		void Delete<TEntity>(TEntity entity) where TEntity : class;
	}
}