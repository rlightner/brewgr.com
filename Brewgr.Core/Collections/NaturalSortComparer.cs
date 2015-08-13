using System;
using System.Collections.Generic;
using System.Linq;

namespace ctorx.Core.Collections
{
	public class NaturalSortComparer : IComparer<string>
	{
		//use a buffer for performance since we expect
		//the Compare method to be called a lot
		private readonly char[] _splitBuffer = new char[256];

		public int Compare(string x, string y)
		{
			//first split each string into segments
			//of non-numbers and numbers
			var a = SplitByNumbers(x);
			var b = SplitByNumbers(y);

			int aInt, bInt;
			var numToCompare = (a.Count < b.Count) ? a.Count : b.Count;
			for (var i = 0; i < numToCompare; i++)
			{
				if (a[i].Equals(b[i]))
					continue;

				var aIsNumber = Int32.TryParse(a[i], out aInt);
				var bIsNumber = Int32.TryParse(b[i], out bInt);
				var bothNumbers = aIsNumber && bIsNumber;
				var bothNotNumbers = !aIsNumber && !bIsNumber;
				//do an integer compare
				if (bothNumbers) return aInt.CompareTo(bInt);
				//do a string compare
				if (bothNotNumbers) return a[i].CompareTo(b[i]);
				//only one is a number, which are
				//by definition less than non-numbers
				if (aIsNumber) return -1;
				return 1;
			}
			//only get here if one string is empty
			return a.Count.CompareTo(b.Count);
		}

		private IList<string> SplitByNumbers(string val)
		{
			System.Diagnostics.Debug.Assert(val.Length <= 256);
			var list = new List<string>();
			var current = 0;
			var dest = 0;
			while (current < val.Length)
			{
				//accumulate non-numbers
				while (current < val.Length &&
					   !char.IsDigit(val[current]))
				{
					_splitBuffer[dest++] = val[current++];
				}
				if (dest > 0)
				{
					list.Add(new string(_splitBuffer, 0, dest));
					dest = 0;
				}
				//accumulate numbers
				while (current < val.Length &&
					   char.IsDigit(val[current]))
				{
					_splitBuffer[dest++] = val[current++];
				}
				if (dest > 0)
				{
					list.Add(new string(_splitBuffer, 0, dest));
					dest = 0;
				}
			}
			return list;
		}
	}
}