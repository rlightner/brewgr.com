using System;

namespace ctorx.Core.Email
{
	public interface ISmtpConfiguration
	{
		/// <summary>
		/// Gets the SMTP Host Name
		/// </summary>
		string Host { get; }

		/// <summary>
		/// Gets the SMTP Port
		/// </summary>
		int Port { get; }

		/// <summary>
		/// Gets a value specifying if the
		/// SMTP server should use SSL
		/// </summary>
		bool EnableSSL { get; }

		/// <summary>
		/// Gets a value specifying whether or not default credentials should
		/// be used.
		/// </summary>
		bool UseDefaultCredentials { get; }

		/// <summary>
		/// Gets the username
		/// </summary>
		string Username { get; }

		/// <summary>
		/// Gets the password
		/// </summary>
		string Password { get; }
	}
}