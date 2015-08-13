namespace ctorx.Core.Primitives
{
	public static class StringExtensions
	{
		/// <summary>
		/// Returns the value, or if null, the alternate value
		/// </summary>
		public static string WhenNull(this string initial, string value)
		{
			if(string.IsNullOrWhiteSpace(initial))
			{
				return value;
			}

			return initial;
		}
	}
}