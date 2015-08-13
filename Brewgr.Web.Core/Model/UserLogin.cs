using System;
using System.Linq;

namespace Brewgr.Web.Core.Model
{
	public class UserLogin
	{
		/// <summary>
		/// Gets or sets the UserId
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// Gets or sets the User
		/// </summary>
		public User User { get; set; }

		/// <summary>
		/// Gets or sets the LoginDate
		/// </summary>
		public DateTime LoginDate { get; set; }
	}
}