using System;
using System.Linq;

namespace ctorx.Core.Collections
{
	public class Pager
	{
		/// <summary>
		/// Gets or sets the ItemsPerPage
		/// </summary>
		public int ItemsPerPage { get; set; }

		/// <summary>
		/// Gets or sets the CurrentPage
		/// </summary>
		public int CurrentPage { get; set; }

		/// <summary>
		/// Gets the RecordCount
		/// </summary>
		public int RecordCount { get; private set; }

		/// <summary>
		/// Gets the page count
		/// </summary>
		public int PageCount { get; private set;}

		/// <summary>
		/// Gets or sets the RecordStart
		/// </summary>
		public int RecordStart { get; set; }

		/// <summary>
		/// Gets or sets the RecordEnd
		/// </summary>
		public int RecordEnd { get; set; }

		/// <summary>
		/// Determines if the pager is in range
		/// </summary>
		public bool IsInRange()
		{
			return ((this.CurrentPage * this.ItemsPerPage) - this.ItemsPerPage + 1) <= this.RecordCount;
		}

		/// <summary>
		/// Determines if the pager is on the first page
		/// </summary>
		public bool IsFirstPage()
		{
			return this.CurrentPage == 1;
		}

		/// <summary>
		/// Determines if the pager is on the last page
		/// </summary>
		public bool IsLastPage()
		{
			return this.CurrentPage == this.PageCount;
		}

		/// <summary>
		/// Handles the paging for an IQueryable
		/// </summary>
		public IPagedList<TType> Page<TType>(IQueryable<TType> query)
		{
			var pageCount = query.Count();

			var items = query
				.Skip(this.ItemsPerPage * (this.CurrentPage - 1))
				.Take(this.ItemsPerPage)
				.ToList();

			var pagedList = new PagedList<TType>();
			pagedList.AddRange(items);
			pagedList.RecordCount = pageCount;

			this.RecordCount = pagedList.RecordCount;
			this.PageCount = Convert.ToInt32(Math.Ceiling(this.RecordCount * 1.00 / this.ItemsPerPage * 1.00));

			this.RecordStart = (this.CurrentPage * this.ItemsPerPage) - this.ItemsPerPage + 1;
			this.RecordEnd = (this.RecordStart + this.ItemsPerPage - 1) > this.RecordCount ? this.RecordCount : this.RecordStart + this.ItemsPerPage - 1;

			return pagedList;
		}
	}
}