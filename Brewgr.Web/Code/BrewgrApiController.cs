using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Web.Http;
using System.Web.Mvc;
using Brewgr.Web.Core.Model;
using Brewgr.Web.Core.Service;
using ctorx.Core.Ninject;
using StackExchange.Exceptional;

namespace Brewgr.Web
{
    public abstract class BrewgrApiController : ApiController
    {
        protected UserSummary ActiveUser;
        readonly IUserResolver UserResolver;

        /// <summary>
        /// ctor the Mighty
        /// </summary>
        protected BrewgrApiController()
        {
            // Dependencies (manual injection to avoid ctor pollution)
            var kernel = KernelPersister.Get();
            this.UserResolver = kernel.GetService(typeof(IUserResolver)) as IUserResolver;
        }

        /// <summary>
        /// Logs a handled exception
        /// </summary>
        public void LogHandledException(Exception exception)
        {
            ErrorStore.LogException(exception, System.Web.HttpContext.Current);
        }

        /// <summary>
        /// Issues a 400 Response
        /// </summary>
        public IHttpActionResult Issue400()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            };

            return this.ResponseMessage(response);
        }

        /// <summary>
        /// Issues a 404 Response
        /// </summary>
        public IHttpActionResult Issue404()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound
            };

            return this.ResponseMessage(response);
        }

        /// <summary>
        /// Issues a 500 Response
        /// </summary>
        public IHttpActionResult Issue500()
        {
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError
            };

            return this.ResponseMessage(response);
        }

        /// <summary>
        /// Overrides Initialize
        /// </summary>
        protected override void Initialize(System.Web.Http.Controllers.HttpControllerContext controllerContext)
        {
            // Resolve the Active User
            if (this.User.Identity.IsAuthenticated)
            {
                this.ActiveUser = this.UserResolver?.Resolve();
            }

            base.Initialize(controllerContext);
        }
    }
}