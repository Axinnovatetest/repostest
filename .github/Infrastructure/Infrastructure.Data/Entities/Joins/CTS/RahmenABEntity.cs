using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.CTS
{
	public class RahmenABEntity
	{
		public int? Nr { get; set; }
		public int? ProjektNr { get; set; }
		public int? Vorfallnr { get; set; }
		public DateTime? Falligkeit { get; set; }
		public string Name { get; set; }
		public int? NrRA { get; set; }
		public string Bezug { get; set; }
		public DateTime? Datum { get; set; }

		public RahmenABEntity(DataRow dataRow)
		{
			Nr = (dataRow["Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Nr"]);
			ProjektNr = (dataRow["Projekt-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Projekt-Nr"]);
			Vorfallnr = (dataRow["Angebot-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Angebot-Nr"]);
			Falligkeit = (dataRow["Fälligkeit"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Fälligkeit"]);
			Name = (dataRow["Vorname/NameFirma"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Vorname/NameFirma"]);
			NrRA = (dataRow["nr_RA"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["nr_RA"]);
			Bezug = (dataRow["Bezug"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezug"]);
			Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
		}
	}
}
