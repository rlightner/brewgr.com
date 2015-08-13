using System;
using System.Collections.Generic;
using System.Linq;

namespace ctorx.Core.Collections
{
	public class PagedList<TType> : List<TType>, IPagedList<TType>
	{
		/// <summary>
		/// Gets or sets the PageCount
		/// </summary>
		public int RecordCount { get; set; }
	}
}