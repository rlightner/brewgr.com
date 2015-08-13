using System;

namespace ctorx.Core.Data
{
	public interface IDataContext : IDisposable
	{
		// This is a Marker Interface
		int SaveChanges();
	}
}