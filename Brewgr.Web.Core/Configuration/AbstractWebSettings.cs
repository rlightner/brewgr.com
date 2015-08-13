using System;
using System.Collections.Generic;

namespace Brewgr.Web.Core.Configuration
{
	public abstract class AbstractWebSettings : IWebSettings
	{
		/// <summary>
		/// Gets the RootPath
		/// </summary>
		public abstract string RootPath { get; }

		/// <summary>
		/// Gets the RootPathSecure
		/// </summary>
		public abstract string RootPathSecure { get;  }

		/// <summary>
		/// Gets the static root path
		/// </summary>
		public abstract string StaticRootPath { get;  }

		/// <summary>
		/// Gets the secure static root path
		/// </summary>
		public abstract string StaticRootPathSecure { get;  }

		/// <summary>
		/// Gets a value specifying whether or not https is disabled
		/// </summary>
		public abstract bool DisableHttps { get; }

		/// <summary>
		/// Gets the MediaPhysicalRoot
		/// </summary>
		public abstract string MediaPhysicalRoot { get;  }

		/// <summary>
		/// Gets the MediaUrlRoot
		/// </summary>
		public string MediaUrlRoot 
		{ 
			get { return this.RootPath + "/Media"; }
		}

		/// <summary>
		/// Gets the MediaUrlRoot Secure
		/// </summary>
		public string MediaUrlRootSecure
		{
			get { return this.RootPathSecure + "/Media"; }
		}

		/// <summary>
		/// Gets or sets the SenderName
		/// </summary>
		public virtual string SenderDisplayName
		{
			get { return "Brewgr"; }
		}

		/// <summary>
		/// Gets or sets the SenderAddress
		/// </summary>
		public virtual string SenderAddress
		{
			get { return "no-reply@brewgr.com"; }
		}

		/// <summary>
		/// Gets the contact form Email Address
		/// </summary>
		public virtual IList<string> ContactFormEmailAddress
		{
			get { return new[] { "brewgr@brewgr.com" }; }
		}

		/// <summary>
		/// Gets the default number of Recipes per page
		/// </summary>
		public int DefaultRecipesPerPage
		{
			get { return 10; }
		}

		/// <summary>
		/// Gets the default image root
		/// </summary>
		public string DefaultRecipeImageRoot
		{
			get { return "/img/mug/"; }
		}
	}
}