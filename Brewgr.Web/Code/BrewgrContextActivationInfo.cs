using System;
using System.Configuration;
using ctorx.Core.Data;
using Brewgr.Web.Core.Data;

namespace Brewgr.Web
{
	public class BrewgrContextActivationInfo : IDataContextActivationInfo<BrewgrContext>
	{
		public string ConnectionString
		{
		    get
		    {
		        return Environment.GetEnvironmentVariable("Brewgr_ConnectionString");
		    }
		}
	}
}