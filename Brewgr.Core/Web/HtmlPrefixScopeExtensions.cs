using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace ctorx.Core.Web
{
	/// <summary>
	/// Taken from: http://blog.stevensanderson.com/2010/01/28/editing-a-variable-length-list-aspnet-mvc-2-style/
	/// TODO: Spend time dissecting this to understand it better...possibly write a replacement
	/// </summary>
	public static class HtmlPrefixScopeExtensions
	{
		public static IDisposable BeginCollectionItem(this HtmlHelper html, string collectionName)
		{
			var itemIndex = Guid.NewGuid().ToString();

			html.ViewData.Add("CurrentIndex", itemIndex);
			return new HtmlFieldPrefixScope(html.ViewData.TemplateInfo, string.Format("{0}[{1}]", collectionName, itemIndex));
		}

		private class HtmlFieldPrefixScope : IDisposable
		{
			private readonly TemplateInfo templateInfo;
			private readonly string previousHtmlFieldPrefix;

			public HtmlFieldPrefixScope(TemplateInfo templateInfo, string htmlFieldPrefix)
			{
				this.templateInfo = templateInfo;

				this.previousHtmlFieldPrefix = templateInfo.HtmlFieldPrefix;
				templateInfo.HtmlFieldPrefix = htmlFieldPrefix;
			}

			public void Dispose()
			{
				this.templateInfo.HtmlFieldPrefix = this.previousHtmlFieldPrefix;
			}
		}
	}
}