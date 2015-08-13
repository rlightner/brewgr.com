using System;
using System.Linq;
using ctorx.Core.Data;
using Brewgr.Web.Core.Model;

namespace Brewgr.Web.Core.Data
{
	public interface IBrewgrRepository : IContextRepository<BrewgrContext> { }
}