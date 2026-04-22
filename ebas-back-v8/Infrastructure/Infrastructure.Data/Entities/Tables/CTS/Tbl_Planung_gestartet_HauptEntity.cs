using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class Tbl_Planung_gestartet_HauptEntity
	{
		public int? Artikel_Nr { get; set; }
		public string Artikelnummer { get; set; }
		public string Benutzer_Komm { get; set; }
		public string Bezeichnung { get; set; }
		public DateTime? Datum_Bereit_G2 { get; set; }
		public DateTime? Datum_Gestart_CAO { get; set; }
		public DateTime? Datum_Planung { get; set; }
		public bool? FA_Gestart_CAO { get; set; }
		public int? Fertigungsnummer { get; set; }
		public bool? Gedruckt_Bereit_G2 { get; set; }
		public int ID { get; set; }
		public int? ID_Fertigung { get; set; }
		public int? Id_User { get; set; }
		public int? Lagerort_ID { get; set; }
		public decimal? Menge { get; set; }
		public int? N_Position_Ajouter { get; set; }
		public bool? Offen_Komm { get; set; }
		public int? Status { get; set; }
		public DateTime? Termin_Bestatigt1 { get; set; }
		public bool Vergleich_Stuckliste { get; set; }
		public bool Vergleich_Stuckliste_Automatique { get; set; }

		public Tbl_Planung_gestartet_HauptEntity() { }

		public Tbl_Planung_gestartet_HauptEntity(DataRow dataRow)
		{
			Artikel_Nr = (dataRow["Artikel_Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel_Nr"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Benutzer_Komm = (dataRow["Benutzer_Komm"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Benutzer_Komm"]);
			Bezeichnung = (dataRow["Bezeichnung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung"]);
			Datum_Bereit_G2 = (dataRow["Datum_Bereit_G2"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum_Bereit_G2"]);
			Datum_Gestart_CAO = (dataRow["Datum_Gestart_CAO"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum_Gestart_CAO"]);
			Datum_Planung = (dataRow["Datum_Planung"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum_Planung"]);
			FA_Gestart_CAO = (dataRow["FA_Gestart_CAO"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["FA_Gestart_CAO"]);
			Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
			Gedruckt_Bereit_G2 = (dataRow["Gedruckt_Bereit_G2"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Gedruckt_Bereit_G2"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			ID_Fertigung = (dataRow["ID_Fertigung"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ID_Fertigung"]);
			Id_User = (dataRow["Id_User"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id_User"]);
			Lagerort_ID = (dataRow["Lagerort_ID"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_ID"]);
			Menge = (dataRow["Menge"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Menge"]);
			N_Position_Ajouter = (dataRow["N_Position_Ajouter"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["N_Position_Ajouter"]);
			Offen_Komm = (dataRow["Offen_Komm"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Offen_Komm"]);
			Status = (dataRow["Status"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Status"]);
			Termin_Bestatigt1 = (dataRow["Termin_Bestatigt1"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Bestatigt1"]);
			Vergleich_Stuckliste = Convert.ToBoolean(dataRow["Vergleich_Stuckliste"]);
			Vergleich_Stuckliste_Automatique = Convert.ToBoolean(dataRow["Vergleich_Stuckliste_Automatique"]);
		}

		public Tbl_Planung_gestartet_HauptEntity ShallowClone()
		{
			return new Tbl_Planung_gestartet_HauptEntity
			{
				Artikel_Nr = Artikel_Nr,
				Artikelnummer = Artikelnummer,
				Benutzer_Komm = Benutzer_Komm,
				Bezeichnung = Bezeichnung,
				Datum_Bereit_G2 = Datum_Bereit_G2,
				Datum_Gestart_CAO = Datum_Gestart_CAO,
				Datum_Planung = Datum_Planung,
				FA_Gestart_CAO = FA_Gestart_CAO,
				Fertigungsnummer = Fertigungsnummer,
				Gedruckt_Bereit_G2 = Gedruckt_Bereit_G2,
				ID = ID,
				ID_Fertigung = ID_Fertigung,
				Id_User = Id_User,
				Lagerort_ID = Lagerort_ID,
				Menge = Menge,
				N_Position_Ajouter = N_Position_Ajouter,
				Offen_Komm = Offen_Komm,
				Status = Status,
				Termin_Bestatigt1 = Termin_Bestatigt1,
				Vergleich_Stuckliste = Vergleich_Stuckliste,
				Vergleich_Stuckliste_Automatique = Vergleich_Stuckliste_Automatique
			};
		}
	}
}

