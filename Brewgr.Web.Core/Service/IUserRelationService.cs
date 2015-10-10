using System.Collections.Generic;
using Brewgr.Web.Core.Model;

namespace Brewgr.Web.Core.Service
{
    public interface IUserRelationService
    {
        /// <summary>
        /// Adds a user relation
        /// </summary>
        void AddRelation(int initiatingUserId, int targetUserId);

        /// <summary>
        /// Removes a user relation
        /// </summary>
        void RemoveRelation(int initiatingUserId, int targetUserId);

        /// <summary>
        /// Gets a list of followed users
        /// </summary>
        IList<int> GetFollowedUsers(int userId);

        /// <summary>
        /// Determines if a user is followed by another user
        /// </summary>
        bool UserIsFollowedBy(int userId, int followerId);

        /// <summary>
        /// Gets a user's followers
        /// </summary>
        IList<MiniUserSummary> GetFollowersOf(int userId);

        /// <summary>
        /// Gets people followed by a user
        /// </summary>
        IList<MiniUserSummary> GetFollowedBy(int userId);

        /// <summary>
        /// Gets people followed by a user, returning count
        /// </summary>
        IList<MiniUserSummary> GetFollowedBy(int userId, int count);

        /// <summary>
        /// Gets people followed by a user count
        /// </summary>
        int GetFollowedByCount(int userId);
    }
}