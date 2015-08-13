using System;
using System.Linq;
using System.Text;

namespace ctorx.Core.Formatting
{
	public class HumanReadableFormatter
	{
		/// <summary>
		/// Adds spaces to a Pascal Case string, making it more human readable. Assume two
		/// uppercase characters in a row would be an abbreviation.
		/// </summary>
		public static string AddSpacesToPascalCaseString(string pascalCaseString)
		{
			if (string.IsNullOrWhiteSpace(pascalCaseString))
			{
				throw new ArgumentNullException("pascalCaseString");
			}

			if (pascalCaseString.Length == 1)
			{
				return pascalCaseString;
			}

			if (pascalCaseString.Length == 1)
			{
				return pascalCaseString;
			}

			var output = new StringBuilder();
			output.Append(pascalCaseString[0]);

			for (var i = 1; i < pascalCaseString.Length; i++)
			{
				var current = pascalCaseString[i];

				if (!char.IsUpper(current))
				{
					output.Append(current);
					continue;
				}

				if (!char.IsUpper(pascalCaseString[i - 1]) || ((i < pascalCaseString.Length - 1) && !char.IsUpper(pascalCaseString[i + 1])))
				{
					output.Append(" " + current);
					continue;
				}
				else
				{
					output.Append(current);
					continue;
				}
			}

			return output.ToString();
		}
	}
}