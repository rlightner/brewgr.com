using System;
using ctorx.Core.Conversion;

namespace ctorx.Core.Primitives
{
	public static class FloatingPointExtensions
	{
		/// <summary>
		/// Determines if a float equals another numeric object using an epsilon
		/// </summary>
		public static bool IsEqualTo(this float value, object comparison, float epsilon)
		{
			var comparisonAsFloat = Converter.Convert<float>(comparison);
			return Math.Abs(value - comparisonAsFloat) < epsilon;
		}

		/// <summary>
		/// Determines if a double equals another numeric object using an epsilon
		/// </summary>
		public static bool IsEqualTo(this double value, object comparison, double epsilon)
		{
			var comparisonAsDouble = Converter.Convert<double>(comparison);
			return Math.Abs(value - comparisonAsDouble) < epsilon;
		}

		/// <summary>
		/// Determines if a decimal equals another numeric object using an epsilon
		/// </summary>
		public static bool IsEqualTo(this decimal value, object comparison, decimal epsilon)
		{
			var comparisonAsDecimal = Converter.Convert<decimal>(comparison);
			return Math.Abs(value - comparisonAsDecimal) < epsilon;
		}
	}
}