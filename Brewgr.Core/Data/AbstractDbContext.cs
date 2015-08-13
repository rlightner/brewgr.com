using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ctorx.Core.Data
{
	public abstract class AbstractDbContext : DbContext
	{
		/// <summary>
		/// ctor the Mighty
		/// </summary>
		protected AbstractDbContext(string connectionString) : base(connectionString) { }

		/// <summary>
		/// Fires when the model is created
		/// </summary>
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
		}
	}
}