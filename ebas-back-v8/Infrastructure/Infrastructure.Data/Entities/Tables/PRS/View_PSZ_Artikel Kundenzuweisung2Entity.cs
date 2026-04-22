using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.PRS
{
	public class View_PSZ_Artikel_Kundenzuweisung2Entity
	{
		public string Artikelnummer { get; set; }
		public string CS_Kontakt { get; set; }
		public string Kunde { get; set; }
		public string PSZ_Artikel3 { get; set; }
		public int System_Artikel_Nr { get; set; }

		public View_PSZ_Artikel_Kundenzuweisung2Entity() { }

		public View_PSZ_Artikel_Kundenzuweisung2Entity(DataRow dataRow)
		{
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			CS_Kontakt = (dataRow["CS Kontakt"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CS Kontakt"]);
			Kunde = (dataRow["Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kunde"]);
			PSZ_Artikel3 = (dataRow["PSZ_Artikel3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PSZ_Artikel3"]);
			System_Artikel_Nr = Convert.ToInt32(dataRow["System_Artikel-Nr"]);
		}
	}
}

