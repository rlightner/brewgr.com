using System;

namespace Brewgr.Web.Core.Data
{
    public class DefaultBrewgrBlogConnection : IBrewgrBlogConnection
    {
        /// <summary>
        /// Gets the connection string
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return Environment.GetEnvironmentVariable("BrewgrBlog_ConnectionString");
            }
        }
    }
}