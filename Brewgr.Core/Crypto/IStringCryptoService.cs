using System;

namespace ctorx.Core.Crypto
{
    /// <summary>
    /// Provides string crypto services
    /// </summary>
    public interface IStringCryptoService
    {
        /// <summary>
        /// /Encrypts a string
        /// </summary>
        string Encrypt(string plainText);

        /// <summary>
        /// Decrypts a string
        /// </summary>
        string Decrypt(string encryptedText);
    }
}