using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using ctorx.Core.Conversion;

namespace ctorx.Core.Crypto
{
    public class AESStringCryptoService : IStringCryptoService
    {
        readonly IStringCryptoKeyProvider KeyProvider;

        enum EncryptionAction : short
        {
            Encrypt = 1,
            Decrypt = 2
        }

        public AESStringCryptoService(IStringCryptoKeyProvider stringEncryptionKeyProvider)
        {
            this.KeyProvider = stringEncryptionKeyProvider;
        }

        /// <summary>
        /// /Encrypts a string
        /// </summary>
        public string Encrypt(string value)
        {
            var encryptor = this.GetCryptoTransformer(EncryptionAction.Encrypt);

            using (var memoryStream = new MemoryStream())
            {
                var plainText = System.Text.Encoding.Unicode.GetBytes(value);

                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainText, 0, plainText.Length);
                    cryptoStream.FlushFinalBlock();

                    return ByteConverter.ToHexString(memoryStream.ToArray());
                }
            }
        }

        /// <summary>
        /// Decrypts a string
        /// </summary>
        public string Decrypt(string encryptedText)
        {
            var bytes = ByteConverter.FromHexString(encryptedText);

            var decryptor = this.GetCryptoTransformer(EncryptionAction.Decrypt);

            using (var memoryStream = new MemoryStream(bytes))
            {
                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    var plainText = new byte[bytes.Length];
                    var decryptedCount = cryptoStream.Read(plainText, 0, plainText.Length);

                    return Encoding.Unicode.GetString(plainText, 0, decryptedCount);
                }
            }
        }

        /// <summary>
        /// Gets the Secret Key 
        /// </summary>
        private Rfc2898DeriveBytes GetSecretKey()
        {
            var salt = Encoding.ASCII.GetBytes(this.KeyProvider.Salt);
            return new Rfc2898DeriveBytes(this.KeyProvider.Key, salt);
        }

        /// <summary>
        /// Gets a RijndaelManaged CryptoTransformer
        /// </summary>
        private ICryptoTransform GetCryptoTransformer(EncryptionAction encryptionAction)
        {
            var cipher = new RijndaelManaged();
            cipher.Padding = PaddingMode.ISO10126;

            var secretKey = this.GetSecretKey();

            ICryptoTransform transform = null;

            switch (encryptionAction)
            {
                case EncryptionAction.Decrypt:
                    transform = cipher.CreateDecryptor(secretKey.GetBytes(32), secretKey.GetBytes(16));
                    break;
                case EncryptionAction.Encrypt:
                    transform = cipher.CreateEncryptor(secretKey.GetBytes(32), secretKey.GetBytes(16));
                    break;
            }

            return transform;
        }
    }
}