using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Models.OrderDetails
{
	public class GetResponseModel
	{
		public decimal? Anzahl { get; set; }
		public decimal? Start_Anzahl { get; set; }
		public int? Artikel_Nr { get; set; }
		public string Bemerkung_Pos { get; set; }
		public string Bestellnummer { get; set; }
		public int? Bestellung_Nr { get; set; }
		public string Bezeichnung_1 { get; set; }
		public string Bezeichnung_2 { get; set; }
		public decimal? CUPreis { get; set; }
		public string Einheit { get; set; }
		public decimal? Einzelpreis { get; set; }
		public decimal? Erhalten { get; set; }
		public bool? erledigt_pos { get; set; }
		public decimal? Gesamtpreis { get; set; }
		public string InfoRahmennummer { get; set; }
		public bool? Kanban { get; set; }
		public int? Lagerort_id { get; set; }
		public DateTime? Liefertermin { get; set; }
		public DateTime? Bestatigter_Termin { get; set; }
		public int Nr { get; set; }
		public bool? Position_erledigt { get; set; }
		public decimal? Preiseinheit { get; set; }
		public int? Preisgruppe { get; set; }
		public Single? Rabatt { get; set; }
		public Single? Rabatt1 { get; set; }
		public string schriftart { get; set; }
		public string sortierung { get; set; }
		public Single? Umsatzsteuer { get; set; }


		public bool? MHD { get; set; }
		public bool? COF_Pflichtig { get; set; }
		public string Zeitraum_MHD { get; set; }
		public string Artikelnummer { get; set; }
		public bool ESD_Schutz { get; set; }

		public string LagerortName { get; set; }
		public List<DropDownMenu> ListLagerort { get; set; }

		public int Position { get; set; }
		public int WECount { get; set; }
		public string ABNummer { get; set; }
		public bool? StandardSupplierViolation { get; set; }
		public GetResponseModel() { }
		public GetResponseModel(Infrastructure.Data.Entities.Tables.MTM.Bestellte_ArtikelEntity bestellte_ArtikelEntity,
								Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity artikelEntity,
								List<Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity> lagerorteEntities,
								int wECount)
		{
			Preiseinheit = bestellte_ArtikelEntity.Preiseinheit;
			Anzahl = bestellte_ArtikelEntity.Anzahl;
			Artikel_Nr = bestellte_ArtikelEntity.Artikel_Nr;
			Bemerkung_Pos = bestellte_ArtikelEntity.Bemerkung_Pos;
			Bestellnummer = bestellte_ArtikelEntity.Bestellnummer;
			Bestellung_Nr = bestellte_ArtikelEntity.Bestellung_Nr;
			Bezeichnung_1 = bestellte_ArtikelEntity.Bezeichnung_1;
			Bezeichnung_2 = bestellte_ArtikelEntity.Bezeichnung_2;
			CUPreis = bestellte_ArtikelEntity.CUPreis;
			Einheit = bestellte_ArtikelEntity.Einheit;
			Einzelpreis = bestellte_ArtikelEntity.Einzelpreis;
			Erhalten = bestellte_ArtikelEntity.Erhalten;
			erledigt_pos = bestellte_ArtikelEntity.erledigt_pos;
			Gesamtpreis = bestellte_ArtikelEntity.Gesamtpreis;
			InfoRahmennummer = bestellte_ArtikelEntity.InfoRahmennummer;
			Kanban = bestellte_ArtikelEntity.Kanban;
			Lagerort_id = bestellte_ArtikelEntity.Lagerort_id;
			Liefertermin = bestellte_ArtikelEntity.Liefertermin;
			Bestatigter_Termin = bestellte_ArtikelEntity.Bestatigter_Termin;
			Nr = bestellte_ArtikelEntity.Nr;
			Position_erledigt = bestellte_ArtikelEntity.Position_erledigt;
			Preisgruppe = bestellte_ArtikelEntity.Preisgruppe;
			Rabatt = bestellte_ArtikelEntity.Rabatt;
			Rabatt1 = bestellte_ArtikelEntity.Rabatt1;
			schriftart = bestellte_ArtikelEntity.schriftart;
			sortierung = bestellte_ArtikelEntity.sortierung;
			Umsatzsteuer = bestellte_ArtikelEntity.Umsatzsteuer;
			Start_Anzahl = bestellte_ArtikelEntity.Start_Anzahl;

			//Artikel Entity Data
			MHD = artikelEntity.MHD;
			COF_Pflichtig = artikelEntity.COF_Pflichtig;
			Zeitraum_MHD = artikelEntity.Zeitraum_MHD;
			Artikelnummer = artikelEntity.ArtikelNummer;
			ESD_Schutz = artikelEntity.ESD_Schutz ?? false;


			ListLagerort = DropDownMenu.GetDropDownMenu<Infrastructure.Data.Entities.Tables.MTM.LagerorteEntity>(lagerorteEntities, "Lagerort_id", "Lagerort");
			LagerortName = lagerorteEntities?.FirstOrDefault(l => l.Lagerort_id == Lagerort_id)?.Lagerort;

			if(string.IsNullOrWhiteSpace(LagerortName) && Lagerort_id.HasValue && Lagerort_id.Value > 0)
			{
				var lager = Infrastructure.Data.Access.Tables.MTM.LagerorteAccess.Get(Lagerort_id.Value);
				LagerortName = lager.Lagerort;
			}

			Position = bestellte_ArtikelEntity?.Position ?? 0;


			WECount = wECount;
			ABNummer = bestellte_ArtikelEntity.AB_Nr_Lieferant;
			StandardSupplierViolation = bestellte_ArtikelEntity.StandardSupplierViolation;
		}
	}
	public class GetRequestModel: IPaginatedRequestModel
	{
		public int Id { get; set; }

	}
}
