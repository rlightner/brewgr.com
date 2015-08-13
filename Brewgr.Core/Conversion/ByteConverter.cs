using System;
using System.Text;
using ctorx.Core.Collections;

namespace ctorx.Core.Conversion
{
	public static class ByteConverter
	{
		/// <summary>
		/// Converts a byte array as a hex string
		/// </summary>
		public static string ToHexString(byte[] bytes)
		{
			var hex = new StringBuilder(bytes.Length * 2);
			bytes.ForEach(x => hex.AppendFormat("{0:x2}", (object) x));
			return hex.ToString();
		}

		/// <summary>
		/// Comverts a hex string to a byte array
		/// </summary>
		public static byte[] FromHexString(string hexFormattedString)
		{
			var numberChars = hexFormattedString.Length;
			var bytes = new byte[numberChars / 2];

			for (var i = 0; i < numberChars; i += 2)
			{
				bytes[i / 2] = Convert.ToByte(hexFormattedString.Substring(i, 2), 16);
			}
			return bytes;
		}
	}
}