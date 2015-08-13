using Brewgr.Web.Core.Model;

namespace Brewgr.Web.Core.Service
{
	public interface IAdminService
	{
		/// <summary>
		/// Gets Site Stats
		/// </summary>
		SiteStats GetSiteStats();

		/// <summary>
		/// Resolves Feedback
		/// </summary>
		void ResolveFeedback(int userFeedbackId, int userId);
	}
}