using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Brewgr.Web.Core.Data;
using Brewgr.Web.Core.Model;

namespace Brewgr.Web.Core.Service
{
    public class DefaultUserRelationService : IUserRelationService
    {
        readonly IBrewgrRepository Repository;
        readonly INotificationService NotificationService;

        /// <summary>
        /// ctor the Mighty
        /// </summary>
        public DefaultUserRelationService(IBrewgrRepository repository, INotificationService notificationService)
        {
            this.Repository = repository;
            this.NotificationService = notificationService;
        }

        /// <summary>
        /// Adds a user relation
        /// </summary>
        public void AddRelation(int initiatingUserId, int targetUserId)
        {
            if (initiatingUserId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(initiatingUserId));
            }
            if (targetUserId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(targetUserId));
            }

            var currentConnection = this.Repository.GetSet<UserConnection>()
                .FirstOrDefault(x => x.UserId == targetUserId && x.FollowedById == initiatingUserId);

            if(currentConnection != null)
            {
                if(!currentConnection.IsActive)
                {
                    currentConnection.IsActive = true;
                    currentConnection.DateModified = DateTime.Now;
                }
                return;
            }

            // Not Connected, Connect (and message)
            var userConnection = new UserConnection
            {
                UserId = targetUserId,
                FollowedById = initiatingUserId,
                IsActive = true,
                DateCreated = DateTime.Now
            };

            this.Repository.Add(userConnection);

            // Queue the Notification (we only do this for followed the first time)
            this.NotificationService.QueueNotification(NotificationType.BrewerFollowed, new[] { targetUserId, initiatingUserId });
        }

        /// <summary>
        /// Removes a user relation
        /// </summary>
        public void RemoveRelation(int initiatingUserId, int targetUserId)
        {
            if (initiatingUserId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(initiatingUserId));
            }

            if (targetUserId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(targetUserId));
            }

            var currentConnection = this.Repository.GetSet<UserConnection>()
                .FirstOrDefault(x => x.UserId == targetUserId && x.FollowedById == initiatingUserId);

            if (currentConnection != null)
            {
                if (currentConnection.IsActive)
                {
                    currentConnection.IsActive = false;
                    currentConnection.DateModified = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// Gets a list of followed users
        /// </summary>
        public IList<int> GetFollowedUsers(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException("userId");
            }

            return this.Repository.GetSet<UserConnection>()
                .Where(x => x.UserId == userId)
                .Where(x => x.IsActive)
                .Select(x => x.FollowedById)
                .ToList();
        }

        /// <summary>
        /// Determines if a user is followed by another user
        /// </summary>
        public bool UserIsFollowedBy(int userId, int followerId)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException("userId");
            }

            if (followerId <= 0)
            {
                throw new ArgumentOutOfRangeException("followerId");
            }

            return this.Repository.GetSet<UserConnection>()
                .Any(x => x.UserId == userId && x.FollowedById == followerId && x.IsActive);
        }

        /// <summary>
        /// Toggles a User Follow
        /// </summary>
        public void ToggleUserFollow(int userId, int followedById)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException("userId");
            }

            if (followedById <= 0)
            {
                throw new ArgumentOutOfRangeException("followedById");
            }

            var currentConnection = this.Repository.GetSet<UserConnection>()
                .FirstOrDefault(x => x.UserId == userId && x.FollowedById == followedById);

            // Not Connected, Connect (and message)
            if (currentConnection == null)
            {
                var userConnection = new UserConnection
                {
                    UserId = userId,
                    FollowedById = followedById,
                    IsActive = true,
                    DateCreated = DateTime.Now
                };

                this.Repository.Add(userConnection);

                // Queue the Notification (we only do this for followed the first time)
                this.NotificationService.QueueNotification(NotificationType.BrewerFollowed, new[] { userId, followedById });

                return;
            }

            // Toggle Existing
            currentConnection.IsActive = !currentConnection.IsActive;
            currentConnection.DateModified = DateTime.Now;
        }

        /// <summary>
        /// Gets a user's followers
        /// </summary>
        public IList<MiniUserSummary> GetFollowersOf(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException("userId");
            }

            return this.Repository.GetSet<UserConnection>()
                .Where(x => x.UserId == userId)
                .Where(x => x.IsActive)
                .Join(this.Repository.GetSet<MiniUserSummary>(), x => x.FollowedById, y => y.UserId, (x, y) => new { MiniUserSummary = y })
                .Select(x => x.MiniUserSummary)
                .OrderBy(x => x.Username.StartsWith("Brewer") ? "zzz" + x.Username : x.Username)
                .ToList();
        }

        /// <summary>
        /// Gets people followed by a user
        /// </summary>
        public IList<MiniUserSummary> GetFollowedBy(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException("userId");
            }

            return this.Repository.GetSet<UserConnection>()
                .Where(x => x.FollowedById == userId)
                .Where(x => x.IsActive)
                .Join(this.Repository.GetSet<MiniUserSummary>(), x => x.UserId, y => y.UserId, (x, y) => new { MiniUserSummary = y })
                .Select(x => x.MiniUserSummary)
                .OrderBy(x => x.Username.StartsWith("Brewer") ? "zzz" + x.Username : x.Username)
                .ToList();
        }

        /// <summary>
        /// Gets people followed by a user with a count
        /// </summary>
        public IList<MiniUserSummary> GetFollowedBy(int userId, int count)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException("userId");
            }

            return this.Repository.GetSet<UserConnection>()
                .Where(x => x.FollowedById == userId)
                .Where(x => x.IsActive)
                .Join(this.Repository.GetSet<MiniUserSummary>(), x => x.UserId, y => y.UserId, (x, y) => new { MiniUserSummary = y })
                .Select(x => x.MiniUserSummary)
                .OrderBy(x => x.Username.StartsWith("Brewer") ? "zzz" + x.Username : x.Username)
                .Take(count)
                .ToList();
        }

        /// <summary>
        /// Gets people followed by a user
        /// </summary>
        public int GetFollowedByCount(int userId)
        {
            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException("userId");
            }

            return this.Repository.GetSet<UserConnection>()
                .Where(x => x.FollowedById == userId)
                .Where(x => x.IsActive)
                .Join(this.Repository.GetSet<MiniUserSummary>(), x => x.UserId, y => y.UserId, (x, y) => new { MiniUserSummary = y })
                .Select(x => x.MiniUserSummary)
                .Count();
        }
    }
}