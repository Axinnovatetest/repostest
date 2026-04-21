using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class PSZ_BH_Kontenrahmen_LogEntity
	{
		public DateTime? DateTime { get; set; }
		public int Id { get; set; }
		public int TotalCount { get; set; }
		public string LogObject { get; set; }
		public string LogText { get; set; }
		public string LogType { get; set; }
		public int? UserId { get; set; }
		public string Username { get; set; }

		public PSZ_BH_Kontenrahmen_LogEntity() { }
		public PSZ_BH_Kontenrahmen_LogEntity(DataRow dataRow)
		{
			DateTime = (dataRow["DateTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["DateTime"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			TotalCount = Convert.ToInt32(dataRow["TotalCount"]);
			LogObject = (dataRow["LogObject"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LogObject"]);
			LogText = (dataRow["LogText"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LogText"]);
			LogType = (dataRow["LogType"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LogType"]);
			UserId = (dataRow["UserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["UserId"]);
			Username = (dataRow["Username"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Username"]);
		}
	}
}
