using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class OrderProcesssing_LogEntity
	{
		public int? AngebotNr { get; set; }
		public DateTime? DateTime { get; set; }
		public int Id { get; set; }
		public string LogObject { get; set; }
		public string LogText { get; set; }
		public string LogType { get; set; }
		public int? Nr { get; set; }
		public string Origin { get; set; }
		public int? PositionNr { get; set; }
		public int? ProjektNr { get; set; }
		public int? UserId { get; set; }
		public string Username { get; set; }

		public OrderProcesssing_LogEntity() { }

		public OrderProcesssing_LogEntity(DataRow dataRow)
		{
			AngebotNr = (dataRow["AngebotNr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["AngebotNr"]);
			DateTime = (dataRow["DateTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["DateTime"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			LogObject = (dataRow["LogObject"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LogObject"]);
			LogText = (dataRow["LogText"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LogText"]);
			LogType = (dataRow["LogType"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LogType"]);
			Nr = (dataRow["Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Nr"]);
			Origin = (dataRow["Origin"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Origin"]);
			PositionNr = (dataRow["PositionNr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["PositionNr"]);
			ProjektNr = (dataRow["ProjektNr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ProjektNr"]);
			UserId = (dataRow["UserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["UserId"]);
			Username = (dataRow["Username"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Username"]);
		}

		public OrderProcesssing_LogEntity ShallowClone()
		{
			return new OrderProcesssing_LogEntity
			{
				AngebotNr = AngebotNr,
				DateTime = DateTime,
				Id = Id,
				LogObject = LogObject,
				LogText = LogText,
				LogType = LogType,
				Nr = Nr,
				Origin = Origin,
				PositionNr = PositionNr,
				ProjektNr = ProjektNr,
				UserId = UserId,
				Username = Username
			};
		}
	}
}