using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ctorx.Core.Crypto
{
    public class SHA512Hasher : IHasher
    {
		int SaltLength = 16;
        readonly SHA512Managed Hasher = new SHA512Managed();
        int HashByteLength = 32;

        /// <summary>
        /// ctor the Mighty
        /// </summary>
        public SHA512Hasher()
        {
            var settingHashSaltLength = Environment.GetEnvironmentVariable("Setting_HashSaltLength");
            var settingHashByteLength = Environment.GetEnvironmentVariable("Setting_HashByteLength");

            if(!string.IsNullOrWhiteSpace(settingHashSaltLength))
            {
                var saltLength = 0;
                if(int.TryParse(settingHashSaltLength, out saltLength))
                {
                    this.SaltLength = saltLength;
                }

                var byteLength = 0;
                if (int.TryParse(settingHashByteLength, out byteLength))
                {
                    this.HashByteLength = byteLength;
                }
            }

        }

		/// <summary>
		/// Sets the Salt Length
		/// </summary>
		public void SetSaltLength(int saltLength)
		{
			this.SaltLength = saltLength;
		}

        /// <summary>
        /// Computes a Hash
        /// </summary>
        public byte[] Hash(string plainText, byte[] saltBytes = null)
        {
            if(string.IsNullOrWhiteSpace(plainText))
            {
                throw new ArgumentNullException("plainText");
            }

            if (saltBytes == null)
            {
                saltBytes = this.GenerateRandomSaltBytes();
            }

            // Append Salt Bytes to Plain Text Bytes
            var bytesToHash = Encoding.UTF8.GetBytes(plainText).Concat(saltBytes).ToArray();

            // Compute the Hash
            var hashedBytes = this.Hasher.ComputeHash(bytesToHash);

            // Return Hashed Bytes with Appended Salt
            return saltBytes.Concat(hashedBytes).ToArray();
        }


        /// <summary>
        /// Compares a Hash
        /// </summary>
        public bool Compare(string plainText, byte[] hashBytes)
        {
            if(string.IsNullOrWhiteSpace(plainText))
            {
                throw new ArgumentNullException("plainText");
            }

            if(hashBytes == null || hashBytes.Length == 0)
            {
                throw new ArgumentNullException("hashBytes");
            }

            // Check if Hash is Invalid
            if(hashBytes.Length < HashByteLength)
            {
                return false;
            }

            var saltBytes = hashBytes.Take(this.SaltLength).ToArray();

            var hash = this.Hash(plainText, saltBytes);
            return hash.SequenceEqual(hashBytes);
        }       


        /// <summary>
        /// Generates random salt bytes
        /// </summary>
        private byte[] GenerateRandomSaltBytes()
        {
            var saltBytes = new byte[this.SaltLength];

            var rng = new RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(saltBytes);

            return saltBytes;
        }
    }
}