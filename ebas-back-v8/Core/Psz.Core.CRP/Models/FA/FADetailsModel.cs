
using Psz.Core.CustomerService.Models.InsideSalesWerksterminUpdates;

namespace Psz.Core.CRP.Models.FA
{
	public class FADetailsModel
	{
		//fa daten
		public string? Mandant { get; set; }
		public int? Fertigungsnummer { get; set; }
		public int? ID_Fertigung { get; set; }
		public string? Artikelnummer { get; set; }
		public int? Artikel_nr { get; set; }
		public string? Bezeichnung_1 { get; set; }
		public int? FA_Menge { get; set; }
		public int? Erledigt { get; set; }
		public int? Offen { get; set; }
		public string? FA_Status { get; set; }
		public int? FA_zu_AB { get; set; }
		public bool? gebucht { get; set; }
		public bool? UBG { get; set; }
		public bool? UBG_Transfer { get; set; }
		public bool? Endkontrolle { get; set; }
		public string? Urs_Artikelnummer { get; set; }
		public string? Urs_FA { get; set; }
		public DateTime? Datecreation { get; set; }
		public bool? FA_Gestartet { get; set; }
		//planung
		public string? Planungsstatus { get; set; }
		public DateTime? Termin_Ursprunglich { get; set; }
		public DateTime? Termin_Bestatigt1 { get; set; }
		public DateTime? Termin_Bestatigt2 { get; set; }
		public string? Bemerkung_II_Planung { get; set; }
		public string? Bemerkung_Planung { get; set; }
		public string? Bemerkung_zu_Prio { get; set; }
		public string? Gewerk_Teilweise_Bemerkung { get; set; }
		public string? Bemerkung_ohne_statte { get; set; }
		//Auftragsbearbeitung
		public DateTime? Termin_Fertigstellung { get; set; }
		public decimal? Preis { get; set; }
		public decimal? Zeit { get; set; }
		public string? Bemerkung { get; set; }
		//fa-info
		public DateTime? FA_Druckdatum { get; set; }
		public bool? Kommisioniert_teilweise { get; set; }
		public bool? Kommisioniert_komplett { get; set; }
		public string? Kommisioniert_text { get; set; }
		public bool? Kabel_geschnitten { get; set; }
		public DateTime? Kabel_geschnitten_Datum { get; set; }
		public bool? Prio { get; set; }
		//technik
		public bool? Erstmuster { get; set; }
		public bool IsTechnik { get; set; }
		public string? Techniker { get; set; }
		public string? Bemerkung_Technik { get; set; }
		public bool Quick_Area { get; set; }
		public int? LagerortID { get; set; }
		public string? LagerortName { get; set; }
		//gewerk details
		public bool? Check_Gewerk1_Teilweise { get; set; }
		public bool? Check_Gewerk2_Teilweise { get; set; }
		public bool? Check_Gewerk3_Teilweise { get; set; }
		public bool? Check_Gewerk1 { get; set; }
		public bool? Check_Gewerk2 { get; set; }
		public bool? Check_Gewerk3 { get; set; }
		public string? Gewerk_1 { get; set; }
		public string? Gewerk_2 { get; set; }
		public string? Gewerk_3 { get; set; }
		public bool WithVersionning { get; set; }
		//production
		public int? AB_ProjectNr { get; set; }
		public int? AB_VorfallNr { get; set; }
		//
		public int? Rucknahme_Nr { get; set; }
		public string? Rucknahme { get; set; }
		public List<FATechnikModel> Technik { get; set; }

		// -
		public string? Index_kunde { get; set; }
		// - 2022-10-26
		public int HBGFAPositionId { get; set; }
		public int HBGFaId { get; set; }
		public int HBGFaNummer { get; set; }
		// - 2024-01-25 
		public DateTime? FA_Startdatum { get; set; }
		// - 2024-06-12
		public UpdateWerksterminRequestModel? UpdateWerksterminData { get; set; } = null;

		public FADetailsModel(Infrastructure.Data.Entities.Tables.PRS.FertigungEntity FAEntity,
			List<Infrastructure.Data.Entities.Tables.CTS.Fertigung_PlanungsdetailsEntity> technikEntities,
			Infrastructure.Data.Entities.Joins.FAUpdate.FAErlidigtEntity faerlidegtEntity,
			Infrastructure.Data.Entities.Tables.PRS.FertigungEntity HBGFaEntity,
			Infrastructure.Data.Entities.Tables.CTS.Tbl_Planung_gestartet_HauptEntity plannungEntity)
		{
			if(FAEntity != null)
			{
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(FAEntity.Artikel_Nr.HasValue ? FAEntity.Artikel_Nr.Value : -1);
				var AngebotEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(FAEntity.Angebot_nr ?? -1);
				var lagerEntity = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.GetForFAStcklist(new List<int> { FAEntity.Lagerort_id ?? -1 });
				var LagerWithVersionning = Module.LagersWithVersionning;
				Mandant = FAEntity.Mandant;
				Fertigungsnummer = FAEntity.Fertigungsnummer;
				ID_Fertigung = FAEntity.ID;
				Artikelnummer = articleEntity?.ArtikelNummer;
				Artikel_nr = FAEntity.Artikel_Nr;
				Bezeichnung_1 = articleEntity?.Bezeichnung1;
				FA_Menge = FAEntity.Originalanzahl;
				Erledigt = FAEntity.Anzahl_erledigt;
				Offen = FAEntity.Anzahl;
				FA_Status = FAEntity.Kennzeichen;
				//production
				FA_zu_AB = FAEntity.Angebot_nr;
				AB_ProjectNr = AngebotEntity != null ? int.TryParse(AngebotEntity?.Projekt_Nr?.ToString(), out var val) ? val : 0 : 0;
				AB_VorfallNr = AngebotEntity != null ? AngebotEntity?.Angebot_Nr : 0;
				//
				gebucht = FAEntity.Gebucht;
				UBG = FAEntity.UBG;
				UBG_Transfer = FAEntity.UBGTransfer;
				Endkontrolle = FAEntity.Endkontrolle;
				Urs_Artikelnummer = FAEntity.Urs_Artikelnummer;
				Urs_FA = FAEntity.Urs_Fa;
				Planungsstatus = FAEntity.Planungsstatus;
				Termin_Ursprunglich = FAEntity.Termin_Ursprunglich;
				Termin_Bestatigt1 = FAEntity.Termin_Bestatigt1;
				Termin_Bestatigt2 = FAEntity.Termin_Bestatigt2;
				Bemerkung_II_Planung = FAEntity.Bemerkung_II_Planung;
				Bemerkung_Planung = FAEntity.Bemerkung_Planung;
				Bemerkung_zu_Prio = FAEntity.Bemerkung_zu_Prio;
				Bemerkung_ohne_statte = FAEntity.Bemerkung_ohne_statte;
				Gewerk_Teilweise_Bemerkung = FAEntity.Gewerk_Teilweise_Bemerkung;
				Datecreation = FAEntity.Datum;
				FA_Gestartet = FAEntity.FA_Gestartet.HasValue ? FAEntity.FA_Gestartet.Value : false;
				//Auftragsbearbeitung
				Termin_Fertigstellung = FAEntity.Termin_Fertigstellung;
				Preis = FAEntity.Preis;
				Zeit = FAEntity.Zeit;
				Bemerkung = FAEntity.Bemerkung;
				//fa-info
				FA_Druckdatum = FAEntity.FA_Druckdatum;
				Kommisioniert_teilweise = FAEntity.Kommisioniert_teilweise;
				Kommisioniert_komplett = FAEntity.Kommisioniert_komplett;
				Kabel_geschnitten = FAEntity.Kabel_geschnitten;
				Kabel_geschnitten_Datum = FAEntity.Kabel_geschnitten_Datum;
				Prio = FAEntity.Prio;
				Index_kunde = FAEntity.KundenIndex;

				if(Kommisioniert_komplett.HasValue && Kommisioniert_komplett.Value)
				{
					Kommisioniert_text = "Kommisioniert";
				}
				else
				{
					if(Kommisioniert_teilweise.HasValue && Kommisioniert_teilweise.Value)
					{
						Kommisioniert_text = "Teilweise Kommisioniert";
					}
				}
				Erstmuster = FAEntity.Erstmuster;
				IsTechnik = FAEntity.Technik ?? false;
				Techniker = FAEntity.Techniker;
				Bemerkung_Technik = FAEntity.Bemerkung_Technik;
				Technik = new List<FATechnikModel>();
				Quick_Area = FAEntity.Quick_Area ?? false;
				LagerortID = FAEntity.Lagerort_id;
				LagerortName = lagerEntity != null && lagerEntity.Count > 0 ? lagerEntity[0]?.Lagerort : "";
				//gewerk details
				Check_Gewerk1_Teilweise = FAEntity.Check_Gewerk1_Teilweise;
				Check_Gewerk2_Teilweise = FAEntity.Check_Gewerk2_Teilweise;
				Check_Gewerk3_Teilweise = FAEntity.Check_Gewerk3_Teilweise;

				Check_Gewerk1 = FAEntity.Check_Gewerk1;
				Check_Gewerk2 = FAEntity.Check_Gewerk2;
				Check_Gewerk3 = FAEntity.Check_Gewerk3;

				Gewerk_1 = FAEntity.Gewerk_1;
				Gewerk_2 = FAEntity.Gewerk_2;
				Gewerk_3 = FAEntity.Gewerk_3;
				WithVersionning = (LagerWithVersionning?.Contains((int)FAEntity.Lagerort_id) == true) ? true : false;
				if(technikEntities != null && technikEntities.Count > 0)
				{
					foreach(var technikEntity in technikEntities)
					{
						Technik.Add(new FATechnikModel(technikEntity));
					}
				}
				//
				Rucknahme_Nr = faerlidegtEntity.Lagerort_id_Entnahme;
				Rucknahme = faerlidegtEntity.Lagerort_Entnahme;

				// - 2022-10-26
				HBGFaId = HBGFaEntity?.ID ?? -1;
				HBGFaNummer = HBGFaEntity?.Fertigungsnummer ?? -1;
				HBGFAPositionId = FAEntity.HBGFAPositionId ?? -1;

				// - 2024-01-25
				FA_Startdatum = plannungEntity?.Datum_Planung;
			}
		}
	}
}