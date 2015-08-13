using System;
using System.Linq;
using ctorx.Core.Configuration;
using ctorx.Core.Email;

namespace Brewgr.Web.Core.Configuration
{
	public class BrewgrSmtpConfiguration : ISmtpConfiguration
	{
	    /// <summary>
	    /// Gets the SMTP Host Name
	    /// </summary>
	    public string Host
	    {
	        get { return ConfigReader.AppSettings.Read("SmtpHost"); }
	    } 

	    /// <summary>
		/// Gets the SMTP Port
		/// </summary>
		public int Port
		{
			get { return int.Parse(ConfigReader.AppSettings.Read("SmtpPort")); }
		}

		/// <summary>
		/// Gets a value specifying if the
		/// SMTP server should use SSL
		/// </summary>
		public bool EnableSSL
		{
			get { return true; }
		}

		/// <summary>
		/// Gets a value specifying whether or not default credentials should
		/// be used.
		/// </summary>
		public bool UseDefaultCredentials
		{
			get { return false; }
		}

		/// <summary>
		/// Gets the username
		/// </summary>
		public string Username
		{
			get { return ConfigReader.AppSettings.Read("SmtpUserName"); }
		}

		/// <summary>
		/// Gets the password
		/// </summary>
		public string Password
		{
			get { return ConfigReader.AppSettings.Read("SmtpPassword"); }
		}
	}
}