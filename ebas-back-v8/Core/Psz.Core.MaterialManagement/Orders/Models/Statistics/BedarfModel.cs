using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Models.Statistics
{
	public class BedarfResponseModel
	{
		public Header DataHeader { get; set; }
		public List<NeedsModel> BedarfPositive { get; set; }
		public List<NeedsModel> BedarfNegative { get; set; }
		public List<SupplierModel> Suppliers { get; set; }
		public List<BestellungModel> SubItems { get; set; }
		public class Header
		{
			public string Artikelnummer { get; set; }
			public string Bezeichung { get; set; }
			public decimal Bestand { get; set; }
			public decimal Reserviert { get; set; }
			public string Title { get; set; }
			public string Date { get; set; }
		}

		public class NeedsModel
		{
			public decimal? Anzahl { get; set; }
			public string ArtikelNummer { get; set; }
			public decimal? Bedarf_FA { get; set; }
			public decimal? Bedarf_Summiert { get; set; }
			public string Bezeichnung { get; set; }
			public string Bezeichnung_des_Bauteils { get; set; }
			public decimal? FA_Offen { get; set; }
			public string Fertigung { get; set; }
			public bool? Gestart { get; set; }
			public bool? Kabel_geschnitten { get; set; }
			public bool? Kommisioniert_komplett { get; set; }
			public bool? Kommisioniert_teilweise { get; set; }
			public decimal? Reserviert_Menge { get; set; }
			public string S_Intern { get; set; }
			public string S_Extetrn { get; set; }
			public string Stucklisten_Artikelnummer { get; set; }
			public DateTime? Termin_Bestatigen { get; set; }
			public DateTime? Termin_MA { get; set; }
			public decimal? Verfug_Ini { get; set; }
			public decimal? Verfugbar { get; set; }
			public NeedsModel(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.Disposition_Bedarf entity)
			{
				if(entity == null)
					return;

				Anzahl = entity.Anzahl;
				ArtikelNummer = entity.ArtikelNummer;
				Bedarf_FA = entity.Bedarf_FA;
				Bedarf_Summiert = entity.Bedarf_Summiert;
				Bezeichnung = entity.Bezeichnung;
				Bezeichnung_des_Bauteils = entity.Bezeichnung_des_Bauteils;
				FA_Offen = entity.FA_Offen;
				Fertigung = entity.Fertigung;
				Gestart = entity.Gestart;
				Kabel_geschnitten = entity.Kabel_geschnitten;
				Kommisioniert_komplett = entity.Kommisioniert_komplett;
				Kommisioniert_teilweise = entity.Kommisioniert_teilweise;
				Reserviert_Menge = entity.Reserviert_Menge;
				S_Intern = entity.S_Intern;
				S_Extetrn = entity.S_Extetrn;
				Stucklisten_Artikelnummer = entity.Stücklisten_Artikelnummer;
				Termin_Bestatigen = entity.Termin_Bestatigen;
				Termin_MA = entity.Termin_MA;
				Verfug_Ini = entity.Verfug_Ini;
				Verfugbar = entity.Verfügbar;
			}
		}
		public class SupplierModel
		{
			public int? Artikel_Nr { get; set; }
			public string Bestell_Nr { get; set; }
			public string Fax { get; set; }
			public int? Lief_Nr { get; set; }
			public string Lieferant { get; set; }
			public int? LT { get; set; }
			public decimal? MQO { get; set; }
			public decimal? Peis { get; set; }
			public bool? Standar_Liferent { get; set; }
			public string Telefon { get; set; }
			public SupplierModel(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.Disposition_Liferant entity)
			{
				if(entity == null)
					return;

				Artikel_Nr = entity.Artikel_Nr;
				Bestell_Nr = entity.Bestell_Nr;
				Fax = entity.Fax;
				Lief_Nr = entity.Lief_Nr;
				Lieferant = entity.Lieferant;
				LT = entity.LT;
				MQO = entity.MQO;
				Peis = entity.Peis;
				Standar_Liferent = entity.Standar_Liferent;
				Telefon = entity.Telefon;
			}
		}
		public class BestellungModel
		{
			public string AB { get; set; }
			public DateTime? ABtermin { get; set; }
			public decimal? Anzhal { get; set; }
			public string Bemerkung { get; set; }
			public int? Lief_Nr { get; set; }
			public DateTime? Liefertermin { get; set; }
			public int? PO { get; set; }
			public string VornameFirma { get; set; }

			public BestellungModel(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_Bedarf.Disposition_Bestellung entity)
			{
				if(entity == null)
					return;

				AB = entity.AB;
				ABtermin = entity.ABtermin;
				Anzhal = entity.Anzhal;
				Bemerkung = entity.Bemerkung;
				Lief_Nr = entity.Lief_Nr;
				Liefertermin = entity.Liefertermin;
				PO = entity.PO;
				VornameFirma = entity.VornameFirma;
			}
		}

		public BedarfPDFResponseModel ToPDFModel()
		{
			return new BedarfPDFResponseModel
			{
				DataHeader = new BedarfPDFResponseModel.Header
				{
					Artikelnummer = DataHeader.Artikelnummer,
					Bestand = DataHeader.Bestand,
					Bezeichung = DataHeader.Bezeichung,
					Date = DataHeader.Date,
					Reserviert = DataHeader.Reserviert,
					Title = DataHeader.Title,
				},
				Suppliers = Suppliers?.Select(x => new BedarfPDFResponseModel.SupplierModel
				{
					Artikel_Nr = x.Artikel_Nr ?? -0,
					Bestell_Nr = x.Bestell_Nr.Length > 11 ? x.Bestell_Nr.Substring(0, 11) : x.Bestell_Nr,
					Fax = x.Fax,
					Lieferant = x.Lieferant.Length > 22 ? x.Lieferant.Substring(0, 22) : x.Lieferant,
					Lief_Nr = x.Lief_Nr ?? -1,
					LT = x.LT ?? -1,
					MQO = x.MQO ?? -1,
					Peis = x.Peis ?? -1,
					Standar_Liferent = x.Standar_Liferent.HasValue && x.Standar_Liferent.Value ? "Ja" : "Nein",
					Telefon = x.Telefon,
				}).ToList(),
				SubItems = SubItems?.Select(x => new BedarfPDFResponseModel.BestellungModel
				{
					AB = x.AB,
					ABtermin = x.ABtermin.HasValue ? x.ABtermin.Value.ToString("dd.MM.yyyy") : "",
					Anzhal = x.Anzhal ?? 0m,
					Bemerkung = x.Bemerkung,
					Liefertermin = x.Liefertermin.HasValue ? x.Liefertermin.Value.ToString("dd.MM.yyyy") : "",
					Lief_Nr = x.Lief_Nr ?? -1,
					PO = x.PO ?? -1,
					VornameFirma = x.VornameFirma,
				}).ToList(),
				BedarfNegative = BedarfNegative?.Select(x => new BedarfPDFResponseModel.NeedsModel
				{
					Anzahl = x.Anzahl ?? 0m,
					ArtikelNummer = x.ArtikelNummer,
					Bedarf_FA = x.Bedarf_FA ?? 0m,
					Bedarf_Summiert = x.Bedarf_Summiert ?? 0m,
					Bezeichnung = x.Bezeichnung,
					Bezeichnung_des_Bauteils = x.Bezeichnung_des_Bauteils,
					FA_Offen = x.FA_Offen ?? 0m,
					Fertigung = x.Fertigung,
					Gestart = x.Gestart ?? false,
					Kabel_geschnitten = x.Kabel_geschnitten.HasValue && x.Kabel_geschnitten.Value ? "✔" : "",
					Kommisioniert_komplett = x.Kommisioniert_komplett.HasValue && x.Kommisioniert_komplett.Value ? "✔" : "",
					Kommisioniert_teilweise = x.Kommisioniert_teilweise.HasValue && x.Kommisioniert_teilweise.Value ? "✔" : "",
					Reserviert_Menge = x.Reserviert_Menge ?? 0m,
					S_Intern = x.S_Intern,
					S_Extetrn = x.S_Extetrn,
					Stucklisten_Artikelnummer = x.Stucklisten_Artikelnummer,
					Termin_Bestatigen = x.Termin_Bestatigen.HasValue ? x.Termin_Bestatigen.Value.ToString("dd.MM.yyyy") : "",
					Termin_MA = x.Termin_MA.HasValue ? x.Termin_MA.Value.ToString("dd.MM.yyyy") : "",
					Verfug_Ini = x.Verfug_Ini ?? 0m,
					Verfugbar = x.Verfugbar ?? 0m,
				}).ToList(),
				BedarfPositive = BedarfPositive?.Select(x => new BedarfPDFResponseModel.NeedsModel
				{
					Anzahl = x.Anzahl ?? 0m,
					ArtikelNummer = x.ArtikelNummer,
					Bedarf_FA = x.Bedarf_FA ?? 0m,
					Bedarf_Summiert = x.Bedarf_Summiert ?? 0m,
					Bezeichnung = x.Bezeichnung,
					Bezeichnung_des_Bauteils = x.Bezeichnung_des_Bauteils,
					FA_Offen = x.FA_Offen ?? 0m,
					Fertigung = x.Fertigung,
					Gestart = x.Gestart ?? false,
					Kabel_geschnitten = x.Kabel_geschnitten.HasValue && x.Kabel_geschnitten.Value ? "✔" : "",
					Kommisioniert_komplett = x.Kommisioniert_komplett.HasValue && x.Kommisioniert_komplett.Value ? "✔" : "",
					Kommisioniert_teilweise = x.Kommisioniert_teilweise.HasValue && x.Kommisioniert_teilweise.Value ? "✔" : "",
					Reserviert_Menge = x.Reserviert_Menge ?? 0m,
					S_Intern = x.S_Intern,
					S_Extetrn = x.S_Extetrn,
					Stucklisten_Artikelnummer = x.Stucklisten_Artikelnummer,
					Termin_Bestatigen = x.Termin_Bestatigen.HasValue ? x.Termin_Bestatigen.Value.ToString("dd.MM.yyyy") : "",
					Termin_MA = x.Termin_MA.HasValue ? x.Termin_MA.Value.ToString("dd.MM.yyyy") : "",
					Verfug_Ini = x.Verfug_Ini ?? 0m,
					Verfugbar = x.Verfugbar ?? 0m,
				}).ToList(),
			};
		}
	}

}
