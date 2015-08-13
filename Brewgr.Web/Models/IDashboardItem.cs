using System;

namespace Brewgr.Web.Models
{
    public interface IDashboardItem
    {
        /// <summary>
        /// The Item for the dashboard
        /// </summary>
        Object Item { get; set; }

        /// <summary>
        /// The Item for the dashboard
        /// </summary>
        DateTime DateCreated { get; set; }
    }
}