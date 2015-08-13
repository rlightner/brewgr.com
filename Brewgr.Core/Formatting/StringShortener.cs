using System;
using System.Linq;

namespace ctorx.Core.Formatting
{
	public static class StringShortener
	{
		/// <summary>
		/// Shortens a string in a pretty way
		/// </summary>
		public static string Shorten(string value, int maxLength)
		{
			if(value.Length < maxLength)
			{
				return value;
			}

			var shortened = value.Substring(0, maxLength);
			var splitBySpaces = shortened.Split(' ');

			return string.Join(" ", splitBySpaces.Take(splitBySpaces.Length - 1));
		}
	}
}