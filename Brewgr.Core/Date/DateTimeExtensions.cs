using System;
using System.Linq;

namespace ctorx.Core.Date
{
	public static class DateTimeExtensions
	{
		/// <summary>
		/// Determines if the DateTime is between two dates
		/// </summary>
		public static bool Between(this DateTime dateTime, DateTime rangeStart, DateTime rangeEnd, bool inclusive = true)
		{
			if (inclusive)
			{
				return dateTime >= rangeStart && dateTime <= rangeEnd;
			}
			else
			{
				return dateTime > rangeStart && dateTime < rangeEnd;
			}
		}
	}
}