using System;
using System.Linq;

namespace ctorx.Core.Conversion
{
	public static class Converter
	{
		/// <summary>
		/// Converts to a Nullable Int
		/// </summary>
		public static int? ToNullableInt(object value, int? defaultValue = null)
		{
			if(value == null)
			{
				return defaultValue;
			}

			var intValue = 0;
			if(Int32.TryParse(value as string, out intValue))
			{
				return intValue;
			}

			return defaultValue;
		}

		/// <summary>
		/// Converts a string to a primitive
		/// </summary>
		public static TPrimitive Convert<TPrimitive>(object value) where TPrimitive : struct
		{
			if(value == null)
			{
				throw new ArgumentNullException("value");
			}

			if(string.IsNullOrWhiteSpace(value.ToString()))
			{
				throw new ArgumentNullException("value");
			}

			return (TPrimitive)System.Convert.ChangeType(value, typeof(TPrimitive));
		}
	}
}