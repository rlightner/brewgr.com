using System;
using System.Text;

namespace ctorx.Core.Conversion
{
    public class Base64StringEncoder : IStringEncoder
    {
        /// <summary>
        /// Encodes a string
        /// </summary>
        public string Encode(string value)
        {
            var encodedBytes = ASCIIEncoding.ASCII.GetBytes(value);
            return Convert.ToBase64String(encodedBytes);
        }


        /// <summary>
        /// Decodes a string
        /// </summary>
        public string Decode(string value)
        {
            var decodedBytes = Convert.FromBase64String(value);
            return ASCIIEncoding.ASCII.GetString(decodedBytes);
        }
    }
}