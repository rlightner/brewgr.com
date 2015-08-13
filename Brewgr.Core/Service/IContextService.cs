using System.Data.Entity;
using ctorx.Core.Data;

namespace ctorx.Core.Service
{
	public interface IContextService<TContext> where TContext : IDataContext
	{
		/// <summary>
		/// Adds an Entity
		/// </summary>
		void Add<TEntity>(TEntity entity) where TEntity : class;

		/// <summary>
		/// Attaches an Entity
		/// </summary>
		void Attach<TEntity>(TEntity entity) where TEntity : class;

		/// <summary>
		/// Deletes an Entity
		/// </summary>
		void Delete<TEntity>(TEntity entity) where TEntity : class;
	}
}