namespace Psz.Core.MaterialManagement.Orders.Models.Wareneingang
{
	public class GetRequestModel
	{
		public int Bestellung_Nr { get; set; }
	}
	public class GetResponseModel
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
		public string Eingangslieferscheinn { get; set; }
		public bool MHD { get; set; }
		public bool COF_Pflichtig { get; set; }
		public int Zeitraum_MHD { get; set; }
		public string MhdDatumArtikel { get; set; }
		public bool COC_bestatigung { get; set; }
		public decimal? Grosse { get; set; }
		public bool EMPB { get; set; }
		public bool EMPB_Freigegeben { get; set; }
		public bool EMPB_Bestatigun { get; set; }
		public string Erstell { get; set; }
		public string Id_Alt_Ligne { get; set; }

		public string Original_IDOr { get; set; }
		public bool ESD_Schutz { get; set; }
		public string Username { get; set; }
		// - 2023-08-25 - CoC
		public string CocVersion { get; set; }
		public GetResponseModel()
		{

		}
		public GetResponseModel(Infrastructure.Data.Entities.Joins.MTM.Order.WareneingangEntity wareneingangEntity)
		{
			Position = wareneingangEntity.Position;
			Liefertermin = wareneingangEntity.Liefertermin;
			Bestatigter_Termin = wareneingangEntity.Bestatigter_Termin;
			BestellungNr = wareneingangEntity.BestellungNr;
			Nr = wareneingangEntity.Nr;
			WE_Pos_zu_Bestellposition = wareneingangEntity.WE_Pos_zu_Bestellposition;
			Typ = wareneingangEntity.Typ;
			erledigt_pos = wareneingangEntity.erledigt_pos;
			Vorname_NameFirma = wareneingangEntity.Vorname_NameFirma;
			ArtikelNr = wareneingangEntity.ArtikelNr;
			Artikelnummer = wareneingangEntity.Artikelnummer;
			Bezeichnung1 = wareneingangEntity.Bezeichnung1;
			Einheit = wareneingangEntity.Einheit;
			AnfangLagerBestand = wareneingangEntity.AnfangLagerBestand;
			Anzahl = wareneingangEntity.Anzahl;
			Start_Anzahl = wareneingangEntity.Start_Anzahl;
			Erhalten = wareneingangEntity.Erhalten;
			Aktuelle_Anzahl = wareneingangEntity.Aktuelle_Anzahl;
			EndeLagerBestand = wareneingangEntity.EndeLagerBestand;
			Bestellnummer = wareneingangEntity.Bestellnummer;
			Lagerort_id = wareneingangEntity.Lagerort_id;
			Lagerort = wareneingangEntity.Lagerort;
			Mandant = wareneingangEntity.Mandant;
			Eingangslieferscheinn = wareneingangEntity.Eingangslieferscheinnr;
			MHD = wareneingangEntity.MHD;
			COF_Pflichtig = wareneingangEntity.COF_Pflichtig;
			Zeitraum_MHD = wareneingangEntity.Zeitraum_MHD;
			MhdDatumArtikel = wareneingangEntity.MhdDatumArtikel;
			COC_bestatigung = wareneingangEntity.COC_bestatigung;
			Grosse = wareneingangEntity.Grosse;
			EMPB = wareneingangEntity.EMPB;
			EMPB_Freigegeben = wareneingangEntity.EMPB_Freigegeben;
			EMPB_Bestatigun = wareneingangEntity.EMPB_Bestatigung;
			Erstell = wareneingangEntity.Erstellt;
			Id_Alt_Ligne = wareneingangEntity.Id_Alt_Ligne;
			Original_IDOr = wareneingangEntity.Original_IDOrt;
			ESD_Schutz = wareneingangEntity.ESD_Schutz;
			CocVersion = wareneingangEntity.CocVersion;
			Username = !string.IsNullOrEmpty(wareneingangEntity.Username) ? wareneingangEntity.Username.Substring(0, wareneingangEntity.Username.Length - 20) : ""; // remove date from the bentuzer field
		}
	}
}
