using System;

namespace ctorx.Core.Security
{
    /// <summary>
    /// Provides Authentication Services
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Performs the sign in process
        /// </summary>
        void SignIn(string identifier, bool persist);

        /// <summary>
        /// Performs the sign out process
        /// </summary>
        void SignOut();

        /// <summary>
        /// Determines if the user is signed in
        /// </summary>
        bool UserIsSignedIn();

        /// <summary>
        /// Gets the identifier for the user
        /// </summary>
        string GetUserIdentifier();
    }
}