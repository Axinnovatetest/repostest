using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Models.Article.Statistics.Basics
{
	public class BedarfRequestModel
	{
		public string ArticleNumber { get; set; }
		public int ArticleId { get; set; }
		public int Land { get; set; }
	}

	public class BedarfResponseModel
	{
		public string Title { get; set; }
		public List<NeedsModel> BedarfPositive { get; set; }
		public List<NeedsModel> BedarfNegative { get; set; }
		public List<SupplierModel> Suppliers { get; set; }
		public List<BestellungModel> SubItems { get; set; }

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
			public DateTime? CreateDateFA { get; set; }
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
				CreateDateFA = entity.CreateDateFA;
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
			public bool ProjectPurchase { get; set; }
			public DateTime? CreateDatePO { get; set; }

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
				ProjectPurchase = entity.ProjectPurchase ?? false;
				CreateDatePO = entity.CreateDatePO;
			}
		}
	}
}
