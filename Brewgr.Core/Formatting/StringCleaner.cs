using System;
using System.Linq;

namespace ctorx.Core.Formatting
{
	public static class StringCleaner
	{
		/// <summary>
		/// Cleans a string to be used for Preview Text
		/// </summary>
		public static string CleanForPreviewText(string value, int maxLength)
		{
			if(string.IsNullOrWhiteSpace(value))
			{
				throw new ArgumentNullException("value");
			}

			if(maxLength <= 0)
			{
				throw new ArgumentOutOfRangeException("maxLength");
			}

			var cleanedString = HtmlStripper.Strip(value);
			cleanedString = cleanedString.Replace(Environment.NewLine, string.Empty);
			cleanedString = cleanedString.Replace("\n", " ");
			//cleanedString = cleanedString.Replace("  ", "");
			cleanedString = cleanedString.Replace("\t", string.Empty);

			return StringShortener.Shorten(cleanedString, maxLength);
		}

		/// <summary>
		/// Cleans Text for a Url
		/// </summary>
		public static string CleanForUrl(string sourceText)
		{
			return sourceText
				.Replace("&", "And")
				.Replace("(", " ")
				.Replace(")", " ")
				.Replace(" ", "-")
				.Replace("/", "-")
				.Replace("+", "-")
				.Replace("_", "-")
				.Replace("--", "-")
				.Replace("--", "-")
				.Replace(".", "")
				.TrimStart(new[]{'-'})
				.TrimEnd(new[]{'-'})
				.ToLower();
		}
	}
}