using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.Logistics
{
	public class InventurlisteEF
	{
		public string Artikelnummer { get; set; }
		public string Bezeichnung1 { get; set; }
		public int? Bestand { get; set; }
		public int totalRows { get; set; }
		public InventurlisteEF(DataRow dataRow)
		{
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Bezeichnung1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
			Bestand = (dataRow["Bestand"] == System.DBNull.Value) ? null : Convert.ToInt32(dataRow["Bestand"]);
			totalRows = (dataRow["totalRows"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["totalRows"]);

		}
	}
}
