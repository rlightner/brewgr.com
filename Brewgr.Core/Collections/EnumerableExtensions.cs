using System;
using System.Collections.Generic;

namespace ctorx.Core.Collections
{
	public static class EnumerableExtensions
	{
		/// <summary>
		/// Create the ForEach construct on ienumerables
		/// </summary>
		public static void ForEach<TEnumerable>(this IEnumerable<TEnumerable> enumerable, Action<TEnumerable> action)
		{
			foreach (var x in enumerable)
			{
				action(x);
			}
		}
	}
}