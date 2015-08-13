using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ctorx.Core.Formatting
{
	public static class HtmlStripper
	{
		static readonly Regex HtmlRegex = new Regex("<.*?>", RegexOptions.Compiled);

		/// <summary>
		/// Strips Html from a string
		/// </summary>
		public static string Strip(string input)
		{
			if(string.IsNullOrWhiteSpace(input))
			{
				throw new ArgumentNullException("input");
			}

			return HtmlRegex.Replace(input, string.Empty);
		}
	}
}