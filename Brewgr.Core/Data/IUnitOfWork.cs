using System;

namespace ctorx.Core.Data
{
	public interface IUnitOfWork<TDataContext> : IDisposable where TDataContext : IDataContext
	{
		/// <summary>
		/// Commits the changes
		/// </summary>
		void Commit();

		/// <summary>
		/// Rollsback the Changes
		/// </summary>
		void Rollback();
	}
}