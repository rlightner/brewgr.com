using System;
using System.Collections.Generic;

namespace ctorx.Core.Collections
{
	public static class Range
	{
		/// <summary>
		/// Generates a range of ints
		/// </summary>
		public static IEnumerable<int> Integer(int start, int end, int increment)
		{
			for (var i = start; i <= end; i += increment)
			{
				yield return i;
			}
		}

		/// <summary>
		/// Generates a range of doubles
		/// </summary>
		public static IEnumerable<double> Double(double start, double end, double increment)
		{
			for (var i = start; i <= end; i += increment)
			{
				yield return i;
			}
		}

	}
}