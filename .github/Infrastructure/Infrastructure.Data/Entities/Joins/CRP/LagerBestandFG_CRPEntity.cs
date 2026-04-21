using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Joins.CRP
{
	public class LagerBestandFG_CRPEntity
	{
		public string Artikelnummer { get; set; }
		public string Kunde { get; set; }
		public string Bezeichnung_1 { get; set; }
		public string Bezeichnung_2 { get; set; }
		public string Freigabestatus { get; set; }
		public string CS_Kontakt { get; set; }
		public string Lagerort { get; set; }
		public Decimal? Bestand { get; set; }
		public Decimal? VK_Gesamt { get; set; }
		public Decimal? Kosten_gesamt { get; set; }
		public Decimal? Kosten_gesamt_ohne_cu { get; set; }
		public Decimal? VKE { get; set; }
		public bool UBG { get; set; }
		public bool EdiDefault { get; set; }
		public int Mindestbestand { get; set; }
		public string BemerkungCRP { get; set; }

		public LagerBestandFG_CRPEntity(DataRow dataRow)
		{
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Kunde = (dataRow["Kunde"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kunde"]);
			Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
			Bezeichnung_2 = (dataRow["Bezeichnung 2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 2"]);
			Freigabestatus = (dataRow["Freigabestatus"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Freigabestatus"]);
			CS_Kontakt = (dataRow["CS Kontakt"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CS Kontakt"]);
			Lagerort = (dataRow["Lagerort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lagerort"]);
			Bestand = (dataRow["Bestand"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Bestand"]);
			VK_Gesamt = (dataRow["VK Gesamt"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["VK Gesamt"]);
			Kosten_gesamt = (dataRow["Kosten gesamt"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Kosten gesamt"]);
			Kosten_gesamt_ohne_cu = (dataRow["Kosten gesamt ohne CU"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Kosten gesamt ohne CU"]);
			VKE = (dataRow["VKE"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["VKE"]);
			UBG = Convert.ToBoolean(dataRow["UBG"]);
			EdiDefault = Convert.ToBoolean(dataRow["EdiDefault"]);
			Mindestbestand = (dataRow["Mindestbestand"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Mindestbestand"]);
			BemerkungCRP = (dataRow["BemerkungCRP"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["BemerkungCRP"]);
		}
	}
}
