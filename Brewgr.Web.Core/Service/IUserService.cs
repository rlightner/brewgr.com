using System;
using System.Collections.Generic;
using Brewgr.Web.Core.Data;
using Brewgr.Web.Core.Model;
using ctorx.Core.Data;

namespace Brewgr.Web.Core.Service
{
	public interface IUserService
	{
        /// <summary>
        /// Gets a user by Id
        /// </summary>
        User GetUserById(int userId);

        /// <summary>
        /// Gets a user by UserName
        /// </summary>
        User GetUserByUserName(string userName);

        /// <summary>
		/// Adds a User
		/// </summary>
		void AddUser(User user);

		/// <summary>
		/// Gets a User Summary
		/// </summary>
		UserSummary GetUserSummaryById(int userId);

		/// <summary>
		/// Determines if the provided email address is already in use
		/// </summary>
		bool EmailAddressIsInUse(string emailAddress, int? userId = null);

		/// <summary>
		/// Registers a new User
		/// </summary>
		User RegisterNewUser(string fullName, string emailAddress, string password);

		/// <summary>
		/// Determines if a username is in use
		/// </summary>
		bool UsernameIsInUse(int userId, string username);

		/// <summary>
		/// Sets a username on a User
		/// </summary>
		void SetUserUsername(int userId, string username);

		/// <summary>
		/// Gets the reputation score for a user
		/// </summary>
		int GetUserReputationScore(int userId);

		/// <summary>
		/// Awards Reputation to a User
		/// </summary>
		void AwardReputation(int userId, ReputationAwardType awardType, ReputationObjectType objectType, int objectId);

		/// <summary>
		/// Gets all users.  Use with Caution.
		/// </summary>
		/// <returns></returns>
		IList<User> GetAllUsers();

		/// <summary>
		/// Determines if a user is an Admin
		/// </summary>
		bool UserIsAdmin(int userId);

		/// <summary>
		/// Gets a list of user notification types
		/// </summary>
		IList<UserNotificationType> GetUserNotificationTypes(int userId);

		/// <summary>
		/// Subscribes a user to default notifications
		/// </summary>
		void SubscribeUserToDefaultNotifications(User user);

		/// <summary>
		/// Subscribes a user to a notification type
		/// </summary>
		void SubscribeUserToNotificationType(User user, NotificationType notificationType);

		/// <summary>
		/// Unsiubscribes a user from a notification type
		/// </summary>
		void UnsubscribeUserFromNotificationType(User user, NotificationType notificationType);
        
        /// <summary>
		/// Gets a list of the top contributors
		/// </summary>
		IList<UserSummary> GetWeeklyTopContributors(int count);

        /// <summary>
		/// Gets a list of the top contributors
		/// </summary>
        UserStat GetUserStats(int userId, DateTime howFarBackToSearch);

		/// <summary>
		/// Gets a list of countries
		/// </summary>
		IList<Country> GetCountryList();
	}
}