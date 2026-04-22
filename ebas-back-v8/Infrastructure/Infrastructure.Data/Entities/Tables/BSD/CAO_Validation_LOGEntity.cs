using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class CAO_Validation_LOGEntity
	{
		public int? artikel_nr { get; set; }
		public string artikelnummer { get; set; }
		public DateTime? date_time { get; set; }
		public int ID { get; set; }
		public string kunden_index { get; set; }
		public string username { get; set; }
		public string val_status { get; set; }

		public CAO_Validation_LOGEntity() { }

		public CAO_Validation_LOGEntity(DataRow dataRow)
		{
			artikel_nr = (dataRow["artikel_nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["artikel_nr"]);
			artikelnummer = (dataRow["artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["artikelnummer"]);
			date_time = (dataRow["date_time"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["date_time"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			kunden_index = (dataRow["kunden_index"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["kunden_index"]);
			username = (dataRow["username"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["username"]);
			val_status = (dataRow["val_status"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["val_status"]);
		}
		public CAO_Validation_LOGEntity ShallowClone()
		{
			return new CAO_Validation_LOGEntity
			{
				artikel_nr = artikel_nr,
				artikelnummer = artikelnummer,
				date_time = date_time,
				ID = ID,
				kunden_index = kunden_index,
				username = username,
				val_status = val_status
			};
		}
	}
}

