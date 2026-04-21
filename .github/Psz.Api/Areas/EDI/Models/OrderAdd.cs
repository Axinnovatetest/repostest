using System;

namespace Psz.Api.Areas.EDI.Models
{
	public class OrderAddModel
	{
		public int? Ab_id { get; set; }
		public string ABSENDER { get; set; }
		public string Abteilung { get; set; }
		public int? Angebot_Nr { get; set; }
		public string Anrede { get; set; }
		public string Ansprechpartner { get; set; }
		public bool? Auswahl { get; set; }
		public int? Belegkreis { get; set; }
		public string Bemerkung { get; set; }
		public string Benutzer { get; set; }
		public string Bereich { get; set; }
		public string Bezug { get; set; }
		public string Briefanrede { get; set; }
		public bool? Datueber { get; set; }
		public DateTime? Datum { get; set; }
		public string Debitorennummer { get; set; }
		public string Dplatz_Sirona { get; set; }
		public string EDI_Dateiname_CSV { get; set; }
		public string EDI_Kundenbestellnummer { get; set; }
		public bool? EDI_Order_Change { get; set; }
		public bool? EDI_Order_Change_Updated { get; set; }
		public bool? EDI_Order_Neu { get; set; }
		public bool? Erledigt { get; set; }
		public DateTime? Falligkeit { get; set; }
		public string Freie_Text { get; set; }
		public string Freitext { get; set; }
		public bool? Gebucht { get; set; }
		public bool? Gedruckt { get; set; }
		public string Ihr_Zeichen { get; set; }
		public bool? In_Bearbeitung { get; set; }
		public bool? Interessent { get; set; }
		public string Konditionen { get; set; }
		public int? Kunden_Nr { get; set; }
		public string LAbteilung { get; set; }
		public string Land_PLZ_Ort { get; set; }
		public string LAnrede { get; set; }
		public string LAnsprechpartner { get; set; }
		public string LBriefanrede { get; set; }
		public string Lieferadresse { get; set; }
		public DateTime? Liefertermin { get; set; }
		public string LLand_PLZ_Ort { get; set; }
		public string LName2 { get; set; }
		public string LName3 { get; set; }
		public bool? Loschen { get; set; }
		public string LStraße_Postfach { get; set; }
		public string LVorname_NameFirma { get; set; }
		public bool? Mahnung { get; set; }
		public string Mandant { get; set; }
		public string Name2 { get; set; }
		public string Name3 { get; set; }
		public int? Neu { get; set; }
		public int Nr { get; set; }
		public int? Nr_ang { get; set; }
		public int? Nr_auf { get; set; }
		public int? Nr_BV { get; set; }
		public int? Nr_gut { get; set; }
		public int? Nr_Kanban { get; set; }
		public int? Nr_lie { get; set; }
		public int? Nr_pro { get; set; }
		public int? Nr_RA { get; set; }
		public int? Nr_rec { get; set; }
		public int? Nr_sto { get; set; }
		public bool? Offnen { get; set; }
		public int? Personal_Nr { get; set; }
		public string Projekt_Nr { get; set; }
		public int? Reparatur_nr { get; set; }
		public string Status { get; set; }
		public string Straße_Postfach { get; set; }
		public bool? Termin_eingehalten { get; set; }
		public string Typ { get; set; }
		public string Unser_Zeichen { get; set; }
		public bool? USt_Berechnen { get; set; }
		public string Versandart { get; set; }
		public string Versandarten_Auswahl { get; set; }
		public DateTime? Versanddatum_Auswahl { get; set; }
		public string Vorname_NameFirma { get; set; }
		public DateTime? Wunschtermin { get; set; }
		public string Zahlungsweise { get; set; }
		public string Zahlungsziel { get; set; }
		public bool? Neu_Order { get; set; }
	}
}
