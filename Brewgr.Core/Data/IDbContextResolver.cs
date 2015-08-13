using System;
using System.Data.Entity;

namespace ctorx.Core.Data
{
	/// <summary>
	/// The IDbContextResolver interface defines the means for resolving instances of DbContext
	/// </summary>
	public interface IDbContextResolver : IDisposable
	{
		/// <summary>
		/// Resolves the DbContext
		/// </summary>
		DbContext GetContext();

		/// <summary>
		/// Destroys the DbContext
		/// </summary>
		void DestroyContext();
	}

	public interface IDbContextResolver<TDbContext> : IDbContextResolver where TDbContext : DbContext { }
}