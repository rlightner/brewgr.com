using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace ctorx.Core.Data
{
	public class DefaultDbContextFactory<TDbContext> : IDbContextFactory<TDbContext> where TDbContext : DbContext
	{
		readonly IDbContextActivationInfo<TDbContext> DataContextActivationInfo;

		/// <summary>
		/// ctor the Mighty
		/// </summary>
		public DefaultDbContextFactory(IDbContextActivationInfo<TDbContext> dataContextActivationInfo)
		{
			this.DataContextActivationInfo = dataContextActivationInfo;
		}

		/// <summary>
		/// Creates a new instance of a derived <see cref="T:System.Data.Entity.DbContext"/> type.
		/// </summary>
		public TDbContext Create()
		{
			// Use reflection to instantiate TDataContext
			var contextType = typeof(TDbContext);
			var constructorInfo = contextType.GetConstructor(new[] { typeof(string) });

			if (constructorInfo == null)
			{
				throw new InvalidOperationException("The specified type does not have a constructor with a single argument of type string");
			}

			return (TDbContext)constructorInfo.Invoke(new object[] { this.DataContextActivationInfo.ConnectionString });
		}
	}
}