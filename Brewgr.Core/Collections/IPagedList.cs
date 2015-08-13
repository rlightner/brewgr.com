using System;
using System.Collections.Generic;

namespace ctorx.Core.Collections
{
	public interface IPagedList<TType> : IList<TType>
	{
		/// <summary>
		/// Gets or sets the RecordCount
		/// </summary>
		int RecordCount { get; set; }
	}
}