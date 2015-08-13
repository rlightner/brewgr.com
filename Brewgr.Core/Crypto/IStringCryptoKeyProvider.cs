using System;

namespace ctorx.Core.Crypto
{
    /// <summary>
    /// Provides a key for a string crypto service
    /// </summary>
    public interface IStringCryptoKeyProvider
    {
        /// <summary>
        /// Gets the key to be used for string encryption 
        /// </summary>
        string Key { get; }

        /// <summary>
        /// Gets the Salt used for the Encryption
        /// </summary>
        string Salt { get; }
    }
}