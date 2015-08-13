
using System;

namespace ctorx.Core.ErrorHandling
{
	public class ExceptionWrapper
	{
		/// <summary>
		/// Gets the Date of the Exception
		/// </summary>
		public DateTime ExceptionDate { get; private set; }

		/// <summary>
		/// Gets the Exception Type
		/// </summary>
		public string Type { get; private set; }

		/// <summary>
		/// Gets the Exception Source
		/// </summary>
		public string Source { get; private set; }

		/// <summary>
		/// Gets the Exception Message
		/// </summary>
		public string Message { get; private set; }

		/// <summary>
		/// Gets the Exception Details
		/// </summary>
		public string Details { get; private set; }

		/// <summary>
		/// ctor the Mighty
		/// </summary>
		private ExceptionWrapper() { }

		/// <summary>
		/// Makes an ExceptionInfo from an Exception
		/// </summary>
		public static ExceptionWrapper MakeFromException(Exception exception)
		{
			var baseException = exception.GetBaseException();

			return new ExceptionWrapper
			{
				ExceptionDate = DateTime.Now,
				Type = baseException.GetType().FullName,
				Source = baseException.Source,
				Message = baseException.Message,
				Details = exception.ToString()
			};
		}
	}
}