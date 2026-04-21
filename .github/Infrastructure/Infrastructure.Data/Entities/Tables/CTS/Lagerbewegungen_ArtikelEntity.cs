using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class Lagerbewegungen_ArtikelEntity
	{
		public decimal? AnfangLagerBestand { get; set; }
		public decimal? Anzahl { get; set; }
		public decimal? Anzahl_nach { get; set; }
		public int? Artikel_nr { get; set; }
		public int? Artikel_nr_nach { get; set; }
		public string Bemerkung { get; set; }
		public string Bezeichnung_1 { get; set; }
		public string Bezeichnung_1_nach { get; set; }
		public string Einheit { get; set; }
		public decimal? EndeLagerBestand { get; set; }
		public int? Fertigungsnummer { get; set; }
		public string Gebucht_von { get; set; }
		public string Grund { get; set; }
		public int ID { get; set; }
		public long? ID_Schneiderei { get; set; }
		public int? Karton_ID { get; set; }
		public int? Lager_nach { get; set; }
		public int? Lager_von { get; set; }
		public int? Lagerbewegungen_id { get; set; }
		public bool? Loschen { get; set; }
		public decimal? Preiseinheit { get; set; }
		public decimal? receivedQuantity { get; set; }
		public long? Rollennummer { get; set; }
		public string STRG_SCAN { get; set; }
		public int? Umlaufartikel { get; set; }
		public int? WereingangId { get; set; }
		public Lagerbewegungen_ArtikelEntity() { }

		public Lagerbewegungen_ArtikelEntity(DataRow dataRow)
		{
			AnfangLagerBestand = (dataRow["AnfangLagerBestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["AnfangLagerBestand"]);
			Anzahl = (dataRow["Anzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Anzahl"]);
			Anzahl_nach = (dataRow["Anzahl_nach"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Anzahl_nach"]);
			Artikel_nr = (dataRow["Artikel-nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-nr"]);
			Artikel_nr_nach = (dataRow["Artikel-nr_nach"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-nr_nach"]);
			Bemerkung = (dataRow["Bemerkung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung"]);
			Bezeichnung_1 = (dataRow["Bezeichnung 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1"]);
			Bezeichnung_1_nach = (dataRow["Bezeichnung 1_nach"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung 1_nach"]);
			Einheit = (dataRow["Einheit"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Einheit"]);
			EndeLagerBestand = (dataRow["EndeLagerBestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["EndeLagerBestand"]);
			Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
			Gebucht_von = (dataRow["Gebucht von"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Gebucht von"]);
			Grund = (dataRow["Grund"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Grund"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			ID_Schneiderei = (dataRow["ID_Schneiderei"] == System.DBNull.Value) ? (long?)null : Convert.ToInt64(dataRow["ID_Schneiderei"]);
			Karton_ID = (dataRow["Karton_ID"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Karton_ID"]);
			Lager_nach = (dataRow["Lager_nach"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lager_nach"]);
			Lager_von = (dataRow["Lager_von"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lager_von"]);
			Lagerbewegungen_id = (dataRow["Lagerbewegungen_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerbewegungen_id"]);
			Loschen = (dataRow["Löschen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Löschen"]);
			Preiseinheit = (dataRow["Preiseinheit"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Preiseinheit"]);
			receivedQuantity = (dataRow["receivedQuantity"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["receivedQuantity"]);
			Rollennummer = (dataRow["Rollennummer"] == System.DBNull.Value) ? (long?)null : Convert.ToInt64(dataRow["Rollennummer"]);
			STRG_SCAN = (dataRow["STRG_SCAN"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["STRG_SCAN"]);
			Umlaufartikel = (dataRow["Umlaufartikel"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Umlaufartikel"]);
			WereingangId = (dataRow["WereingangId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["WereingangId"]);
		}

		public Lagerbewegungen_ArtikelEntity ShallowClone()
		{
			return new Lagerbewegungen_ArtikelEntity
			{
				AnfangLagerBestand = AnfangLagerBestand,
				Anzahl = Anzahl,
				Anzahl_nach = Anzahl_nach,
				Artikel_nr = Artikel_nr,
				Artikel_nr_nach = Artikel_nr_nach,
				Bemerkung = Bemerkung,
				Bezeichnung_1 = Bezeichnung_1,
				Bezeichnung_1_nach = Bezeichnung_1_nach,
				Einheit = Einheit,
				EndeLagerBestand = EndeLagerBestand,
				Fertigungsnummer = Fertigungsnummer,
				Gebucht_von = Gebucht_von,
				Grund = Grund,
				ID = ID,
				ID_Schneiderei = ID_Schneiderei,
				Karton_ID = Karton_ID,
				Lager_nach = Lager_nach,
				Lager_von = Lager_von,
				Lagerbewegungen_id = Lagerbewegungen_id,
				Loschen = Loschen,
				Preiseinheit = Preiseinheit,
				receivedQuantity = receivedQuantity,
				Rollennummer = Rollennummer,
				STRG_SCAN = STRG_SCAN,
				Umlaufartikel = Umlaufartikel,
				WereingangId = WereingangId
			};
		}
	}
}