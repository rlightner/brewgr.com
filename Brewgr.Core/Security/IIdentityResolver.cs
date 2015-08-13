namespace ctorx.Core.Security
{
	public interface IIdentityResolver
	{
		/// <summary>
		/// Resolves the current Identity
		/// </summary>
		IIdentity Resolve();
	}
}