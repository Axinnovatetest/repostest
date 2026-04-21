using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.MTM.Order
{
	public class WareneingangEntity
	{
		public string Position { get; set; }
		public DateTime? Liefertermin { get; set; }
		public DateTime? Bestatigter_Termin { get; set; }
		public string BestellungNr { get; set; }
		public int Nr { get; set; }
		public string WE_Pos_zu_Bestellposition { get; set; }
		public string Typ { get; set; }
		public bool erledigt_pos { get; set; }
		public string Vorname_NameFirma { get; set; }
		public string ArtikelNr { get; set; }
		public string Artikelnummer { get; set; }
		public string Bezeichnung1 { get; set; }
		public string Einheit { get; set; }
		public string AnfangLagerBestand { get; set; }
		public string Anzahl { get; set; }
		public string Start_Anzahl { get; set; }
		public string Erhalten { get; set; }
		public string Aktuelle_Anzahl { get; set; }
		public string EndeLagerBestand { get; set; }
		public string Bestellnummer { get; set; }
		public string Lagerort_id { get; set; }
		public string Lagerort { get; set; }
		public string Mandant { get; set; }
		public string Eingangslieferscheinnr { get; set; }
		public bool MHD { get; set; }
		public bool COF_Pflichtig { get; set; }
		public int Zeitraum_MHD { get; set; }
		public string MhdDatumArtikel { get; set; }
		public bool COC_bestatigung { get; set; }
		public decimal? Grosse { get; set; }
		public bool EMPB { get; set; }
		public bool EMPB_Freigegeben { get; set; }
		public bool EMPB_Bestatigung { get; set; }
		public string Erstellt { get; set; }
		public string Id_Alt_Ligne { get; set; }

		public string Original_IDOrt { get; set; }
		public bool ESD_Schutz { get; set; }
		public string Username { get; set; }
		// - 2023-08-25 - CoC
		public string CocVersion { get; set; }
		public WareneingangEntity(DataRow dataRow)
		{
			Position = dataRow["Position"].ToString();
			Liefertermin = (dataRow["Liefertermin"] == System.DBNull.Value) ? null : Convert.ToDateTime(dataRow["Liefertermin"].ToString());
			Bestatigter_Termin = (dataRow["Bestätigter_Termin"] == System.DBNull.Value) ? null : Convert.ToDateTime(dataRow["Bestätigter_Termin"].ToString());
			BestellungNr = dataRow["Bestellung-Nr"].ToString();
			Nr = (dataRow["Nr"] == System.DBNull.Value) ? -1 : Convert.ToInt32(dataRow["Nr"].ToString());
			WE_Pos_zu_Bestellposition = dataRow["WE Pos zu Bestellposition"].ToString();
			Typ = dataRow["Typ"].ToString();
			erledigt_pos = (dataRow["erledigt_pos"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["erledigt_pos"].ToString());
			Vorname_NameFirma = dataRow["Vorname/NameFirma"].ToString();
			ArtikelNr = dataRow["Artikel-Nr"].ToString();
			Artikelnummer = dataRow["Artikelnummer"].ToString();
			Bezeichnung1 = dataRow["Bezeichnung 1"].ToString();
			Einheit = dataRow["Einheit"].ToString();
			AnfangLagerBestand = dataRow["AnfangLagerBestand"].ToString();
			Anzahl = dataRow["Anzahl"].ToString();
			Start_Anzahl = dataRow["Start Anzahl"].ToString();
			Erhalten = dataRow["Erhalten"].ToString();
			Aktuelle_Anzahl = dataRow["Aktuelle Anzahl"].ToString();
			EndeLagerBestand = dataRow["EndeLagerBestand"].ToString();
			Bestellnummer = dataRow["Bestellnummer"].ToString();
			Lagerort_id = dataRow["Lagerort_id"].ToString();
			Lagerort = dataRow["Lagerort"].ToString();
			Mandant = dataRow["Mandant"].ToString();
			Eingangslieferscheinnr = dataRow["Eingangslieferscheinnr"].ToString();
			MHD = (dataRow["MHD"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["MHD"].ToString());
			COF_Pflichtig = (dataRow["COF_Pflichtig"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["COF_Pflichtig"].ToString());
			Zeitraum_MHD = (dataRow["Zeitraum_MHD"] == System.DBNull.Value || String.IsNullOrWhiteSpace(dataRow["Zeitraum_MHD"].ToString())) ? 0 : Convert.ToInt32(dataRow["Zeitraum_MHD"].ToString());
			MhdDatumArtikel = dataRow["MhdDatumArtikel"].ToString();
			COC_bestatigung = (dataRow["COC_bestätigung"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["COC_bestätigung"].ToString());
			Grosse = (dataRow["Größe"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Größe"].ToString());
			EMPB = (dataRow["EMPB"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["EMPB"].ToString());
			EMPB_Freigegeben = (dataRow["EMPB_Freigegeben"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["EMPB_Freigegeben"].ToString());
			EMPB_Bestatigung = (dataRow["EMPB_Bestätigung"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["EMPB_Bestätigung"].ToString());
			Erstellt = dataRow["Erstellt"].ToString();
			Id_Alt_Ligne = dataRow["Id_Alt_Ligne"].ToString();
			Original_IDOrt = dataRow["Original_IDOrt"].ToString();
			ESD_Schutz = (dataRow["ESD_Schutz"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["ESD_Schutz"].ToString());
			Username = (dataRow["Username"] == System.DBNull.Value) ? "" : dataRow["Username"].ToString();
			CocVersion = (dataRow["CocVersion"] == System.DBNull.Value) ? "" : dataRow["CocVersion"].ToString();

		}
	}
}
