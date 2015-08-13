using System;

namespace ctorx.Core.Crypto
{
    public interface IHasher
    {
		/// <summary>
		/// Sets the salt legth
		/// </summary>
		void SetSaltLength(int saltLength);

    	/// <summary>
        /// Computes a Hash
        /// </summary>
        byte[] Hash(string plainText, byte[] saltBytes = null);

        /// <summary>
        /// Compares a hash
        /// </summary>
        bool Compare(string plainText, byte[] hashBytes);
    }
}