using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.Logistics.CustomsEntity;

public static class ConversionHelpers
{
	/// <summary>
	/// It takes one string and parse it to a long
	/// </summary>
	/// <param name="item"></param>
	/// <param name="dataRow"></param>
	/// <returns></returns>
	public static long ConvertToLong(string item, ref DataRow dataRow)
	{
		return
			((string.IsNullOrEmpty(dataRow[$"{item}"].ToString())) ||
			(string.IsNullOrWhiteSpace(dataRow[$"{item}"].ToString())) ||
			dataRow[$"{item}"] == System.DBNull.Value) ? 0 :
			Convert.ToInt64(dataRow[$"{item}"].ToString());
	}
	/// <summary>
	///  It takes one string and parse it to an int
	/// </summary>
	/// <param name="item"></param>
	/// <param name="dataRow"></param>
	/// <returns></returns>
	public static int ConvertToInt32(string item, ref DataRow dataRow)
	{
		return
			((string.IsNullOrEmpty(dataRow[$"{item}"].ToString())) ||
			(string.IsNullOrWhiteSpace(dataRow[$"{item}"].ToString())) ||
			dataRow[$"{item}"] == System.DBNull.Value) ? 0 :
			Convert.ToInt32(dataRow[$"{item}"].ToString());
	}
	/// <summary>
	/// It takes one string and parse it to a double
	/// </summary>
	/// <param name="item"></param>
	/// <param name="dataRow"></param>
	/// <returns></returns>
	public static double ConvertToDouble(string item, ref DataRow dataRow)
	{
		return
			((string.IsNullOrEmpty(dataRow[$"{item}"].ToString())) ||
			(string.IsNullOrWhiteSpace(dataRow[$"{item}"].ToString())) ||
			dataRow[$"{item}"] == System.DBNull.Value) ? 0 :
			Convert.ToDouble(dataRow[$"{item}"].ToString());
	}
	public static double? ConvertToDoubleNullable(string item, ref DataRow dataRow)
	{
		return
			((string.IsNullOrEmpty(dataRow[$"{item}"].ToString())) ||
			(string.IsNullOrWhiteSpace(dataRow[$"{item}"].ToString())) ||
			dataRow[$"{item}"] == System.DBNull.Value) ? null :
			Convert.ToDouble(dataRow[$"{item}"].ToString());
	}
	/// <summary>
	///  It takes one string and parse it to a nullable DateTime
	/// </summary>
	/// <param name="item"></param>
	/// <param name="dataRow"></param>
	/// <returns></returns>
	public static DateTime? ConvertToDateTime(string item, ref DataRow dataRow)
	{
		return
			((string.IsNullOrEmpty(dataRow[$"{item}"].ToString())) ||
			(string.IsNullOrWhiteSpace(dataRow[$"{item}"].ToString())) ||
			dataRow[$"{item}"] == System.DBNull.Value) ? (DateTime?)null :
			Convert.ToDateTime(dataRow[$"{item}"].ToString());
	}
	/// <summary>
	/// It takes one string and parse it to a string
	/// </summary>
	/// <param name="item"></param>
	/// <param name="dataRow"></param>
	/// <returns></returns>
	public static string ConvertToString(string item, ref DataRow dataRow)
	{
		return
			(dataRow[$"{item}"] == System.DBNull.Value) ? "" :
			Convert.ToString(dataRow[$"{item}"].ToString());
	}
}

