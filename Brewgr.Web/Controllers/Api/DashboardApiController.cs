using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace Brewgr.Web.Api.Controllers
{
    [RoutePrefix("api/dashboard")]
    public class DashboardApiController : BrewgrApiController
    {
        [Route("feed/{count?}")]
        [HttpGet]
        public IHttpActionResult Feed(int? count)
        {
            // TODO: Replace current MVC Call here with JSON result
            throw new NotImplementedException();
        }

        [Route("my/recipes/{count?}")]
        public IHttpActionResult Recipes(int? count)
        {
            // TODO: Replace current MVC Call here with JSON result
            throw new NotImplementedException();
        }

        [Route("my/sessions/{count?}")]
        public IHttpActionResult Sessions(int? count)
        {
            // TODO: Replace current MVC Call here with JSON result
            throw new NotImplementedException();
        }
    }
}