namespace ctorx.Core.Identity
{
	public interface IUserHostAddressResolver
	{
		/// <summary>
		/// Resolves a user host address
		/// </summary>
		string Resolve();
	}
}