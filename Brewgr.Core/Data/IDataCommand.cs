using System;
using System.Data;

namespace ctorx.Core.Data
{
	public interface IDataCommand : IDisposable
    {
		/// <summary>
		/// Sets the connection string to be used
		/// </summary>
		IDataCommand UsingConnection(string connectionString);

		/// <summary>
		/// Adds an input parameter to the command
		/// </summary>
		IDataCommand WithParam(string parameterName, object value, int? size = 0);

		/// <summary>
		/// Adds an Output parameter to the command
		/// </summary>
		IDataCommand WithOutputParam(string parameterName, object value = null, int? size = null);

		/// <summary>
		/// Sets the command timeout
		/// </summary>
		IDataCommand WithTimeout(int timeout);

		/// <summary>
		/// Exexutes a non query command
		/// </summary>
		int ExecuteNonQuery();

		/// <summary>
		/// Executes a scalar command
		/// </summary>
		object ExecuteScalar();

		/// <summary>
		/// Executes, returning a DataSet
		/// </summary>
		DataSet GetDataSet();

		/// <summary>
		/// Gets the value of a Parameter
		/// </summary>
		object GetParameterValue(string parameterName);
	}
}