using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.Logistics
{
	public class Logistics_LogEntity
	{
		public int Id { get; set; }
		public int? AngebotNr { get; set; }
		public int? ProjektNr { get; set; }
		public DateTime? DateTime { get; set; }
		public string LogType { get; set; }
		public string Origin { get; set; }
		public string LogObject { get; set; }
		public string LogText { get; set; }
		public int? UserId { get; set; }
		public string Username { get; set; }
		public Logistics_LogEntity() { }

		public Logistics_LogEntity(DataRow dataRow)
		{
			AngebotNr = (dataRow["AngebotNr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["AngebotNr"]);
			DateTime = (dataRow["DateTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["DateTime"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			LogObject = (dataRow["LogObject"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LogObject"]);
			LogText = (dataRow["LogText"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LogText"]);
			LogType = (dataRow["LogType"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LogType"]);
			Origin = (dataRow["Origin"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Origin"]);
			ProjektNr = (dataRow["ProjektNr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ProjektNr"]);
			UserId = (dataRow["UserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["UserId"]);
			Username = (dataRow["Username"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Username"]);
		}
	}
}
