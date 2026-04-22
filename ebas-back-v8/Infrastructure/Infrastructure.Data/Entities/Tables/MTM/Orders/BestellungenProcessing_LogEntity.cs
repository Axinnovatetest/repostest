using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.MTM
{
	public class BestellungenProcessing_LogEntity
	{
		public int? BestellungenNr { get; set; }
		public DateTime? DateTime { get; set; }
		public int Id { get; set; }
		public string LogObject { get; set; }
		public string LogText { get; set; }
		public string LogType { get; set; }
		public int? Nr { get; set; }
		public string Origin { get; set; }
		public int? ProjektNr { get; set; }
		public int? UserId { get; set; }
		public string Username { get; set; }

		public BestellungenProcessing_LogEntity() { }

		public BestellungenProcessing_LogEntity(DataRow dataRow)
		{
			BestellungenNr = (dataRow["BestellungenNr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["BestellungenNr"]);
			DateTime = (dataRow["DateTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["DateTime"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			LogObject = (dataRow["LogObject"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LogObject"]);
			LogText = (dataRow["LogText"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LogText"]);
			LogType = (dataRow["LogType"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LogType"]);
			Nr = (dataRow["Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Nr"]);
			Origin = (dataRow["Origin"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Origin"]);
			ProjektNr = (dataRow["ProjektNr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ProjektNr"]);
			UserId = (dataRow["UserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["UserId"]);
			Username = (dataRow["Username"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Username"]);
		}

		public BestellungenProcessing_LogEntity ShallowClone()
		{
			return new BestellungenProcessing_LogEntity
			{
				BestellungenNr = BestellungenNr,
				DateTime = DateTime,
				Id = Id,
				LogObject = LogObject,
				LogText = LogText,
				LogType = LogType,
				Nr = Nr,
				Origin = Origin,
				ProjektNr = ProjektNr,
				UserId = UserId,
				Username = Username
			};
		}
	}
}

