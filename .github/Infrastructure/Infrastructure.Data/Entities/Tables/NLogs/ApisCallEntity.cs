

namespace Infrastructure.Data.Entities.Tables.NLogs
{
	public class ApisCallEntity
	{
		public string Api { get; set; }
		public string Url { get; set; }
		public int? CallCountAll { get; set; }
		public int? Calls_Last_3_Months { get; set; }
		public int? Calls_Last_6_Months { get; set; }
		public int? Calls_Last_12_Months { get; set; }
		public ApisCallEntity()
		{

		}
		public ApisCallEntity(DataRow dataRow)
		{
			Api = (dataRow[columnName: "Api"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["Api"]);
			Url = (dataRow["Url"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["Url"]);
			CallCountAll = (dataRow["calls_all"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["calls_all"]);
			Calls_Last_3_Months = (dataRow["calls_last_3_months"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["calls_last_3_months"]);
			Calls_Last_6_Months = (dataRow["calls_last_6_months"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["calls_last_6_months"]);
			Calls_Last_12_Months = (dataRow["calls_last_12_months"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["calls_last_12_months"]);
		}
		public ApisCallEntity(IDataRecord r)
		{
			Api = r["Api"].ToString();
			Url = r["Url"].ToString();
			CallCountAll = Convert.ToInt32(r["CallCountAll"]);
			Calls_Last_3_Months = Convert.ToInt32(r["Calls_Last_3_Months"]);
			Calls_Last_6_Months = Convert.ToInt32(r["Calls_Last_6_Months"]);
			Calls_Last_12_Months = Convert.ToInt32(r["Calls_Last_12_Months"]);
		}
	}
	public class ApiCallsSixMonthsPriorEntity
	{
		public string MonthYear { get; set; }
		public int? CallCount { get; set; }
		public ApiCallsSixMonthsPriorEntity()
		{

		}
		public ApiCallsSixMonthsPriorEntity(DataRow dataRow)
		{
			MonthYear = (dataRow[columnName: "MonthYear"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["MonthYear"]);
			CallCount = (dataRow[columnName: "CallCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CallCount"]);
		}
	}
}