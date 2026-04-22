using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Joins.CTS
{
	public class Fehler_AuswertungABEntity
	{
		public string Kunde { get; set; }
		public string DokumentNr { get; set; }
		public int? Position { get; set; }
		public int? VorfallNr { get; set; }
		public string? Artikelnummer { get; set; }
		public string Bezeichnung1 { get; set; }
		public int? Mengeoffen { get; set; }
		public DateTime? liefertermin { get; set; }
		public string? Auslieferlager { get; set; }
		public string? Hauptlager { get; set; }
		public Fehler_AuswertungABEntity()
		{

		}
		public Fehler_AuswertungABEntity(DataRow dataRow)
		{
			Kunde = (dataRow["Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kunde"]);
			DokumentNr = (dataRow["DokumentNr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["DokumentNr"]);
			Position = (dataRow["Position"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Position"]);
			VorfallNr = (dataRow["VorfallNr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["VorfallNr"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Bezeichnung1 = (dataRow["Bezeichnung1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung1"]);
			Mengeoffen = (dataRow["Mengeoffen"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Mengeoffen"]);
			liefertermin = (dataRow["Liefertermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Liefertermin"]);
			Auslieferlager = (dataRow["Auslieferlager"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Auslieferlager"]);

		}
	}
}

