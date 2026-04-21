using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class Preisgruppen_VorgabenEntity
	{
		//public int? Aufschlag { get; set; }
		//public int? Aufschlagsatz { get; set; }
		public double? Aufschlag { get; set; }
		public double? Aufschlagsatz { get; set; }
		public string Bemerkung { get; set; }
		public int ID { get; set; }
		public int? Preisgruppe { get; set; }

		public Preisgruppen_VorgabenEntity() { }

		public Preisgruppen_VorgabenEntity(DataRow dataRow)
		{
			//Aufschlag = (dataRow["Aufschlag"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Aufschlag"]);
			//Aufschlagsatz = (dataRow["Aufschlagsatz"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Aufschlagsatz"]);
			Aufschlag = (dataRow["Aufschlag"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Aufschlag"]);
			Aufschlagsatz = (dataRow["Aufschlagsatz"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Aufschlagsatz"]);
			Bemerkung = (dataRow["Bemerkung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			Preisgruppe = (dataRow["Preisgruppe"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Preisgruppe"]);
		}
	}
}

