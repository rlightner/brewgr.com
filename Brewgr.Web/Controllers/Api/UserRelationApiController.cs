using System;
using System.Web.Http;
using Brewgr.Web.Core.Data;
using Brewgr.Web.Core.Service;
using ctorx.Core.Data;
using FluentValidation.Results;

namespace Brewgr.Web.Api.Controllers
{
    [RoutePrefix("api/user/relation")]
    public class UserRelationApiController : BrewgrApiController
    {
        readonly IUnitOfWorkFactory<BrewgrContext> UnitOfWorkFactory;
        readonly IUserRelationService UserRelationService;

        /// <summary>
        /// ctor the Mighty
        /// </summary>
        public UserRelationApiController(IUnitOfWorkFactory<BrewgrContext> unitOfWorkFactory, IUserRelationService userRelationService)
        {
            this.UnitOfWorkFactory = unitOfWorkFactory;
            this.UserRelationService = userRelationService;
        }

        [HttpPost]
        [Route("{targetUserId}")]
        public IHttpActionResult Add(int targetUserId)
        {
            if(targetUserId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(targetUserId));
            }

            if(this.ActiveUser == null)
            {
                throw new InvalidOperationException("Not Logged In");
            }

            using(var unitOfWork = this.UnitOfWorkFactory.NewUnitOfWork())
            {
                this.UserRelationService.AddRelation(this.ActiveUser.UserId, targetUserId);
                unitOfWork.Commit();
            }

            return this.Ok();
        }

        [HttpDelete]
        [Route("{targetUserId}")]
        public IHttpActionResult Remove(int targetUserId)
        {
            if (targetUserId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(targetUserId));
            }

            if (this.ActiveUser == null)
            {
                throw new InvalidOperationException("Not Logged In");
            }

            using (var unitOfWork = this.UnitOfWorkFactory.NewUnitOfWork())
            {
                this.UserRelationService.RemoveRelation(this.ActiveUser.UserId, targetUserId);
                unitOfWork.Commit();
            }

            return this.Ok();
        }
    }
}