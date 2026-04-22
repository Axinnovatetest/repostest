using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class Protokolierung_Stücklisten_Bei_AktionEntity
	{
		public DateTime? Aenderungsdatum { get; set; }
		public string Alter_menge { get; set; }
		public string Bearbeiter { get; set; }
		public string FG_Artikelnummer { get; set; }
		public int ID { get; set; }
		public string Neuer_menge { get; set; }
		public string Status { get; set; }
		public string Stück_Artikelnummer_Aktuell { get; set; }
		public string Stück_Artikelnummer_Voränderung { get; set; }

		public Protokolierung_Stücklisten_Bei_AktionEntity() { }

		public Protokolierung_Stücklisten_Bei_AktionEntity(DateTime? aenderungsdatum, string alter_menge, string bearbeiter, string fG_Artikelnummer, int iD, string neuer_menge, string status, string stück_Artikelnummer_Aktuell, string stück_Artikelnummer_Voränderung)
		{
			Aenderungsdatum = aenderungsdatum;
			Alter_menge = alter_menge;
			Bearbeiter = bearbeiter;
			FG_Artikelnummer = fG_Artikelnummer;
			ID = iD;
			Neuer_menge = neuer_menge;
			Status = status;
			Stück_Artikelnummer_Aktuell = stück_Artikelnummer_Aktuell;
			Stück_Artikelnummer_Voränderung = stück_Artikelnummer_Voränderung;
		}

		public Protokolierung_Stücklisten_Bei_AktionEntity(DataRow dataRow)
		{
			Aenderungsdatum = (dataRow["Aenderungsdatum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Aenderungsdatum"]);
			Alter_menge = (dataRow["Alter_menge"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Alter_menge"]);
			Bearbeiter = (dataRow["Bearbeiter"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bearbeiter"]);
			FG_Artikelnummer = (dataRow["FG_Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["FG_Artikelnummer"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			Neuer_menge = (dataRow["Neuer_menge"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Neuer_menge"]);
			Status = (dataRow["Status"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Status"]);
			Stück_Artikelnummer_Aktuell = (dataRow["Stück_Artikelnummer_Aktuell"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Stück_Artikelnummer_Aktuell"]);
			Stück_Artikelnummer_Voränderung = (dataRow["Stück_Artikelnummer_Voränderung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Stück_Artikelnummer_Voränderung"]);
		}
	}
}

