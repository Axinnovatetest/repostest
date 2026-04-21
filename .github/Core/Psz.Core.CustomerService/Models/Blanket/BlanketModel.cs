using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Models.Blanket
{
	public class BlanketModel
	{
		public string Auftraggeber { get; set; }

		public string Warenemfanger { get; set; }
		public decimal? Gesamtpreis { get; set; }
		public decimal? GesamtpreisDefault { get; set; }

		public int? BlanketTypeId { get; set; }
		public string BlanketTypeName { get; set; }
		public int CustomerId { get; set; }
		public int SupplierId { get; set; }
		public int? StatusId { get; set; }
		public string StatusName { get; set; }
		public int CreationUserId { get; set; }
		public string CreationUserName { get; set; }
		public string LStrasse_Postfach { get; set; }
		public string LType { get; set; }

		#region >>>>>>>>>> Angebote <<<<<<<<<<
		public DateTime? Datum { get; set; } // Date
		public DateTime? Falligkeit { get; set; } // DueDate
		public DateTime? Liefertermin { get; set; } //DeliveryDate
		public DateTime? Versanddatum_Auswahl { get; set; } // ShippingDate
		public DateTime? Wunschtermin { get; set; }
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
		public string Debitorennummer { get; set; }
		public string Dplatz_Sirona { get; set; }
		public string EDI_Dateiname_CSV { get; set; }
		public string EDI_Kundenbestellnummer { get; set; }
		public bool? EDI_Order_Change { get; set; }
		public bool? EDI_Order_Change_Updated { get; set; }
		public bool? EDI_Order_Neu { get; set; }
		public bool? Erledigt { get; set; }
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
		public string Vorname_NameFirma { get; set; }
		public string Zahlungsweise { get; set; }
		public string Zahlungsziel { get; set; }
		public bool? Neu_Order { get; set; }


		public List<BlanketAttachementsModel> FileIds { get; set; }
		public BlanketModel()
		{

		}
		#endregion

		public BlanketModel(Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity entity,
			Infrastructure.Data.Entities.Tables.CTS.AngeboteBlanketExtensionEntity extensionEntity,
			Infrastructure.Data.Entities.Tables.COR.UserEntity userEntity)
		{
			if(entity == null)
				return;

			Auftraggeber = extensionEntity.Auftraggeber;//supplier
			Warenemfanger = extensionEntity.Warenemfanger;//customer
			Gesamtpreis = Helpers.SpecialHelper.GetGesamtPries(extensionEntity.AngeboteNr);
			GesamtpreisDefault = Helpers.SpecialHelper.GetGesamtPriesDefault(extensionEntity.AngeboteNr);
			BlanketTypeId = extensionEntity.BlanketTypeId;
			BlanketTypeName = extensionEntity.BlanketTypeName;
			CustomerId = extensionEntity.CustomerId ?? -1;
			SupplierId = extensionEntity.SupplierId ?? -1;
			StatusId = extensionEntity.StatusId;
			StatusName = extensionEntity.StatusName;        // -
			CreationUserId = extensionEntity.CreateUserId;
			CreationUserName = userEntity?.Username;
			Datum = entity.Datum; // Date
			Falligkeit = entity.Falligkeit; // DueDate
			Liefertermin = entity.Liefertermin; //DeliveryDate
			Versanddatum_Auswahl = entity.Versanddatum_Auswahl; // ShippingDate
			Wunschtermin = entity.Wunschtermin; // DesiredDate
			Ab_id = entity.Ab_id;
			ABSENDER = entity.ABSENDER;
			Abteilung = entity.Abteilung;
			Angebot_Nr = entity.Angebot_Nr;
			Anrede = entity.Anrede;
			Ansprechpartner = entity.Ansprechpartner;
			Auswahl = entity.Auswahl;
			Belegkreis = entity.Belegkreis;
			Bemerkung = entity.Bemerkung;
			Benutzer = entity.Benutzer;
			Bereich = entity.Bereich;
			Bezug = entity.Bezug;
			Briefanrede = entity.Briefanrede;
			Datueber = entity.Datueber;
			Debitorennummer = entity.Debitorennummer;
			Dplatz_Sirona = entity.Dplatz_Sirona;
			EDI_Dateiname_CSV = entity.EDI_Dateiname_CSV;
			EDI_Kundenbestellnummer = entity.EDI_Kundenbestellnummer;
			EDI_Order_Change = entity.EDI_Order_Change;
			EDI_Order_Change_Updated = entity.EDI_Order_Change_Updated;
			EDI_Order_Neu = entity.EDI_Order_Neu;
			Erledigt = entity.Erledigt;
			Freie_Text = entity.Freie_Text;
			Freitext = entity.Freitext;
			Gebucht = entity.Gebucht;
			Gedruckt = entity.Gedruckt;
			Ihr_Zeichen = entity.Ihr_Zeichen;
			In_Bearbeitung = entity.In_Bearbeitung;
			Interessent = entity.Interessent;
			Konditionen = entity.Konditionen;
			Kunden_Nr = entity.Kunden_Nr;
			LAbteilung = entity.LAbteilung;
			Land_PLZ_Ort = entity.Land_PLZ_Ort;
			LAnrede = entity.LAnrede;
			LAnsprechpartner = entity.LAnsprechpartner;
			LBriefanrede = entity.LBriefanrede;
			Lieferadresse = entity.Lieferadresse;
			LLand_PLZ_Ort = entity.LLand_PLZ_Ort;
			LName2 = entity.LName2;
			LName3 = entity.LName3;
			Loschen = entity.Loschen;
			LStraße_Postfach = entity.LStraße_Postfach;
			LStrasse_Postfach = entity.LStraße_Postfach;
			LVorname_NameFirma = entity.LVorname_NameFirma;
			Mahnung = entity.Mahnung;
			Mandant = entity.Mandant;
			Name2 = entity.Name2;
			Name3 = entity.Name3;
			Neu = entity.Neu;
			Nr = entity.Nr;
			Nr_ang = entity.Nr_ang;
			Nr_auf = entity.Nr_auf;
			Nr_BV = entity.Nr_BV;
			Nr_gut = entity.Nr_gut;
			Nr_Kanban = entity.Nr_Kanban;
			Nr_lie = entity.Nr_lie;
			Nr_pro = entity.Nr_pro;
			Nr_RA = entity.Nr_RA;
			Nr_rec = entity.Nr_rec;
			Nr_sto = entity.Nr_sto;
			Offnen = entity.Offnen;
			Personal_Nr = entity.Personal_Nr;
			Projekt_Nr = entity.Projekt_Nr;
			Reparatur_nr = entity.Reparatur_nr;
			Status = entity.Status;
			Straße_Postfach = entity.Straße_Postfach;
			Termin_eingehalten = entity.Termin_eingehalten;
			Typ = entity.Typ;
			Unser_Zeichen = entity.Unser_Zeichen;
			USt_Berechnen = entity.USt_Berechnen;
			Versandart = entity.Versandart;
			Versandarten_Auswahl = entity.Versandarten_Auswahl;
			Vorname_NameFirma = entity.Vorname_NameFirma;
			Zahlungsweise = entity.Zahlungsweise;
			Zahlungsziel = entity.Zahlungsziel;
			Neu_Order = entity.Neu_Order;
		}
	}
	public class BlanketItem
	{
		public int Nr { get; set; }
		public int AngeboteArtiklNr { get; set; }
		public int AngeboteVorfallNr { get; set; }
		public string AngeboteProjektNr { get; set; }
		public int? Position { get; set; }
		public string Material { get; set; }
		public decimal? Zielmenge { get; set; }
		public string Bezeichnung { get; set; }
		public string KundenMatNummer { get; set; }
		public decimal? Preis { get; set; }
		public decimal? PreisDefault { get; set; }
		public string ME { get; set; }
		public DateTime? ValidFrom { get; set; }
		public DateTime? DateOfExpiry { get; set; }
		public DateTime? ExtensionDate { get; set; }
		public decimal? Gesamtpreis { get; set; }
		public decimal? GesamtpreisDefault { get; set; }
		public string WahrungName { get; set; }
		public string WahrungSymbole { get; set; }
		public int? WahrungId { get; set; }
		public int? AngebotNr { get; set; }
		public int? MaterialNr { get; set; }
		public decimal? DelivredQuantity { get; set; }
		public decimal? RestQuantity { get; set; }
		public bool? Done { get; set; }
		public bool? DateExpired { get; set; }
		public int LinkedToAB { get; set; }
		public decimal BasePrice { get; set; }
		public int? Lagerort { get; set; }
		public string Reason { get; set; }
		public string Comment { get; set; }
		public string ABNummer { get; set; }

		public BlanketItem()
		{

		}
		public BlanketItem(Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity entity,
			Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity extensionEntity)
		{
			if(entity == null)
				return;
			var linkedAB = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetAbByRahmenPosition(entity.Nr);
			var linkedBS = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.GetbyRahmenPositions(new List<int> { entity.Nr });
			var raEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(entity.AngebotNr ?? -1);
			var raExtensionEntity = Infrastructure.Data.Access.Tables.CTS.AngeboteBlanketExtensionAccess.GetByAngeboteNr(raEntity.Nr);
			Nr = extensionEntity.Id;
			AngeboteArtiklNr = entity.Nr;
			AngeboteVorfallNr = raEntity?.Angebot_Nr ?? -1;
			AngeboteProjektNr = raEntity?.Projekt_Nr;
			Position = entity.Position;
			Zielmenge = extensionEntity?.Zielmenge;
			Bezeichnung = entity?.Bezeichnung1;
			KundenMatNummer = entity?.Bezeichnung2;
			BasePrice = extensionEntity?.BasePrice ?? 0;
			Preis = extensionEntity?.Preis;
			PreisDefault = extensionEntity.PreisDefault;
			ME = extensionEntity?.ME;
			ValidFrom = extensionEntity?.GultigAb;
			DateOfExpiry = extensionEntity?.GultigBis;
			Gesamtpreis = extensionEntity?.Gesamtpreis;
			GesamtpreisDefault = extensionEntity.GesamtpreisDefault;
			MaterialNr = entity?.ArtikelNr;
			WahrungName = extensionEntity?.WahrungName;
			WahrungSymbole = extensionEntity?.WahrungSymbole;
			WahrungId = extensionEntity?.WahrungId;
			Material = extensionEntity?.Material;
			AngebotNr = entity?.AngebotNr;
			DelivredQuantity = entity.Geliefert;
			RestQuantity = entity.Anzahl;
			Done = entity.erledigt_pos;
			DateExpired = DateTime.Now > extensionEntity.GultigBis ? true : false;
			ExtensionDate = extensionEntity.GultigBis;
			LinkedToAB = raExtensionEntity.BlanketTypeId == (int)Enums.BlanketEnums.Types.sale ?
				linkedAB?.Select(x => x.AngebotNr).Distinct().Count() ?? 0 :
				linkedBS?.Select(x => x.Bestellung_Nr).Distinct().Count() ?? 0;
			Reason = extensionEntity.ReasonNewPosition;
			Comment = extensionEntity.Comment;
			ABNummer = extensionEntity.AB_nummer;
		}
	}
	public class BlanketAttachementsModel
	{
		public int AttachementID { get; set; }
		public string AttachementName { get; set; }
	}
}
