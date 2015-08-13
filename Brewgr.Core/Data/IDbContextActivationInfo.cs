using System;
using System.Data.Entity;

namespace ctorx.Core.Data
{
	/// <summary>
	/// The DbContextActivationInfo interface provides information used to activate instances of specific DbContexts.
	/// </summary>
	public interface IDbContextActivationInfo<TDbContext> where TDbContext : DbContext
	{
		/// <summary>
		/// Gets the Connection String
		/// </summary>
		string ConnectionString { get; }
	}
}