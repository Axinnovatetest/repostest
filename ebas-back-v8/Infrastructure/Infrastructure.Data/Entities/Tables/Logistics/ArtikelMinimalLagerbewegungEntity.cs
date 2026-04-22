using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.Logistics
{
	public class ArtikelMinimalLagerbewegungEntity
	{
		public int artikelNr { get; set; }
		public string artikelnummer { get; set; }
		public string bezeichnung1 { get; set; }
		public string einheit { get; set; }
		public ArtikelMinimalLagerbewegungEntity() { }
		public ArtikelMinimalLagerbewegungEntity(DataRow dataRow)
		{
			artikelNr = Convert.ToInt32(dataRow["ArtikelNr"]);
			artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			bezeichnung1 = (dataRow["Bezeichnung1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung1"]);
			einheit = (dataRow["Einheit"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Einheit"]);

		}

	}
}
