using System;

namespace ctorx.Core.Conversion
{
    public interface IStringEncoder
    {
        /// <summary>
        /// Encodes a string
        /// </summary>
        string Encode(string value);

        /// <summary>
        /// Decodes a string
        /// </summary>
        string Decode(string value);
    }
}