using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.Logistics
{
	public class ScannerRohmaterialEntity
	{
		public ScannerRohmaterialEntity(DataRow dataRow)
		{
			IdVersand = (dataRow["IdVersand"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["IdVersand"]);
			Transferlager = (dataRow["Transferlager"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Transferlager"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Lagerplatz_pos = (dataRow["Lagerplatz_pos"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Lagerplatz_pos"]);
			Menge = (dataRow["Menge"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Menge"]);
			Scanndatum = (dataRow["Scanndatum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Scanndatum"]);
			Datum = (dataRow["Datum"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Datum"]);

		}
		public int IdVersand { get; set; }
		public int Transferlager { get; set; }
		public string Artikelnummer { get; set; }
		public int Lagerplatz_pos { get; set; }
		public int Menge { get; set; }
		public DateTime? Scanndatum { get; set; }
		public string Datum { get; set; }
	}
}
