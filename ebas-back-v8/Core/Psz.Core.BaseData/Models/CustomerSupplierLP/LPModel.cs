using System;
using System.ComponentModel.DataAnnotations;

namespace Psz.Core.BaseData.Models.CustomerSupplierLP
{
	using Microsoft.AspNetCore.Http;
	using Psz.Core.BaseData.Tools;
	using System.Collections.Generic;

	public class LPModel
	{
		public int ArticleId { get; set; }
		public string Articlenumber { get; set; }
		public string Name1 { get; set; }
		public bool? Standard_supplier { get; set; }
		public string Article_Designation { get; set; }
		public string Article_Designation_2 { get; set; }
		public string Order_number { get; set; }
		public decimal? purchasing_price { get; set; }
		public string Offer { get; set; }
		public DateTime? Offer_date { get; set; }
		public string Phone { get; set; }
		public string Fax { get; set; }
		public string Email { get; set; }
		public int? Minimum_Order_Quantity { get; set; }
		public int? Replacement_period { get; set; }

		public string StandardLiefrentenName { get; set; }
		public LPModel()
		{

		}

		public LPModel(Infrastructure.Data.Entities.Joins.Kunden_Lieferanten_LPEntities LPEntity)
		{
			StandardLiefrentenName = LPEntity.StandardLiefrentenName;
			ArticleId = LPEntity.ArtikelNr;
			Articlenumber = LPEntity.Artikelnummer;
			Name1 = LPEntity.Name1;
			Standard_supplier = LPEntity.Standardlieferant;
			Article_Designation = LPEntity.Artikelbezeichnung;
			Article_Designation_2 = LPEntity.Artikelbezeichnung2;
			Order_number = LPEntity.Bestell_Nr;
			purchasing_price = LPEntity.Einkaufspreis;
			Offer = LPEntity.Angebot;
			Offer_date = LPEntity.Angebot_Datum;
			Phone = LPEntity.Telefon;
			Fax = LPEntity.Fax;
			Email = LPEntity.eMail;
			Minimum_Order_Quantity = LPEntity.Mindestbestellmenge;
			Replacement_period = LPEntity.Wiederbeschaffungszeitraum;
		}

	}
	public class LPExtendedModel
	{
		public int ArticleId { get; set; }
		public string Articlenumber { get; set; }
		public string Name1 { get; set; }
		public bool? Standard_supplier { get; set; }
		public string Article_Designation { get; set; }
		public string Article_Designation_2 { get; set; }
		public string Order_number { get; set; }
		public decimal? purchasing_price { get; set; }
		public string Offer { get; set; }
		public DateTime? Offer_date { get; set; }
		public string Phone { get; set; }
		public string Fax { get; set; }
		public string Email { get; set; }
		public int? Minimum_Order_Quantity { get; set; }
		public int? Replacement_period { get; set; }
		public decimal? PackagingUnit { get; set; }
		public bool? ActiveSince24Months { get; set; }
		public decimal? OpenQuantityNext360Days { get; set; }

		public string StandardLiefrentenName { get; set; }
		public LPExtendedModel()
		{

		}

		public LPExtendedModel(Infrastructure.Data.Entities.Joins.Kunden_Lieferanten_LPExtendedEntity LPEntity)
		{
			StandardLiefrentenName = LPEntity.StandardLiefrentenName;
			ArticleId = LPEntity.ArtikelNr;
			Articlenumber = LPEntity.Artikelnummer;
			Name1 = LPEntity.Name1;
			Standard_supplier = LPEntity.Standardlieferant;
			Article_Designation = LPEntity.Artikelbezeichnung;
			Article_Designation_2 = LPEntity.Artikelbezeichnung2;
			Order_number = LPEntity.Bestell_Nr;
			purchasing_price = LPEntity.Einkaufspreis;
			Offer = LPEntity.Angebot;
			Offer_date = LPEntity.Angebot_Datum;
			Phone = LPEntity.Telefon;
			Fax = LPEntity.Fax;
			Email = LPEntity.eMail;
			Minimum_Order_Quantity = LPEntity.Mindestbestellmenge;
			Replacement_period = LPEntity.Wiederbeschaffungszeitraum;
			PackagingUnit = LPEntity.Verpackungseinheit;
			ActiveSince24Months = LPEntity.Last2YearsOrderQuantity > 0;
			OpenQuantityNext360Days = LPEntity.BedarfPO;
		}

	}
	public class LPMinimalModel: IEquatable<LPMinimalModel>
	{
		public static List<LPMinimalModel> InvalidArticles { get; set; } = new List<LPMinimalModel>();
		public static List<LPMinimalModel> HasContetnWillBeErrased { get; set; } = new List<LPMinimalModel>();
		public bool EinkaufspreisChanged { get; private set; } = default;
		public bool AngebotChanged { get; private set; } = default;
		public bool MindestbestellmengeChanged { get; private set; } = default;
		public bool WiederbeschaffungszeitraumChanged { get; private set; } = default;
		public bool Angebot_DatumChanged { get; private set; } = default;
		public bool Bestell_NrChanged { get; private set; } = default;

		// Logs
		public string EinkaufspreisChangesLog { get; private set; } = default;
		public string AngebotChangesLog { get; private set; } = default;
		public string MindestbestellmengeChangesLog { get; private set; } = default;
		public string WiederbeschaffungszeitraumChangesLog { get; private set; } = default;
		public string Angebot_DatumChangesLog { get; private set; } = default;
		public string Bestell_NrChangesLog { get; private set; } = default;
		public decimal? Einkaufspreis1 { get; set; }
		public decimal? Einkaufspreis2 { get; set; }
		public DateTime? Einkaufspreis1_gultig_bis { get; set; }
		public DateTime? Einkaufspreis2_gultig_bis { get; set; }
		public int ArtikelNr { get; set; }
		public int Nr { get; set; }
		[Column(1)]
		[Required]
		public string Artikelnummer { get; set; }
		public string Name1 { get; set; }
		public string errors { get; set; } = string.Empty;
		public bool? Standardlieferant { get; set; }
		public string Artikelbezeichnung { get; set; }
		public string Artikelbezeichnung2 { get; set; }
		[Column(2)]
		[Required]
		public string Bestell_Nr { get; set; }
		[Column(3)]
		[Required]
		public decimal? Einkaufspreis { get; set; }
		[Column(4)]
		[Required]
		public string Angebot { get; set; }
		[Column(5)]
		[Required]
		public DateTime? Angebot_Datum { get; set; }
		public string Telefon { get; set; }
		public string Fax { get; set; }
		public string eMail { get; set; }
		[Column(6)]
		[Required]
		public int? Mindestbestellmenge { get; set; }
		[Column(7)]
		[Required]
		public int? Wiederbeschaffungszeitraum { get; set; }
		public LPMinimalModel()
		{

		}
		public LPMinimalModel(Infrastructure.Data.Entities.Joins.Kunden_Lieferanten_LPEntities LPEntity)
		{
			Nr = LPEntity.Nr;
			ArtikelNr = LPEntity.ArtikelNr;
			Artikelnummer = LPEntity.Artikelnummer;
			Name1 = LPEntity.Name1;
			Standardlieferant = LPEntity.Standardlieferant;
			Artikelbezeichnung = LPEntity.Artikelbezeichnung;
			Artikelbezeichnung2 = LPEntity.Artikelbezeichnung2;
			Bestell_Nr = LPEntity.Bestell_Nr;
			Einkaufspreis = LPEntity.Einkaufspreis;
			Angebot = LPEntity.Angebot;
			Angebot_Datum = LPEntity.Angebot_Datum;
			Telefon = LPEntity.Telefon;
			Fax = LPEntity.Fax;
			eMail = LPEntity.eMail;
			Mindestbestellmenge = LPEntity.Mindestbestellmenge;
			Wiederbeschaffungszeitraum = LPEntity.Wiederbeschaffungszeitraum;
			Einkaufspreis1 = LPEntity.Einkaufspreis1;
			Einkaufspreis2 = LPEntity.Einkaufspreis2;
			Einkaufspreis1_gultig_bis = LPEntity.Einkaufspreis1_gultig_bis;
			Einkaufspreis2_gultig_bis = LPEntity.Einkaufspreis2_gultig_bis;
		}
		public bool Equals(LPMinimalModel data)
		{

			if((Artikelnummer ?? "_") == (data.Artikelnummer ?? string.Empty))
			{
				AngebotChanged = CompareElement(Angebot, data.Angebot, data);
				Bestell_NrChanged = CompareElement(Bestell_Nr, data.Bestell_Nr, data);

				if(!(Mindestbestellmenge == data.Mindestbestellmenge))
					MindestbestellmengeChanged = true;
				if(!(Einkaufspreis == data.Einkaufspreis))
					EinkaufspreisChanged = true;
				if(!(Wiederbeschaffungszeitraum == data.Wiederbeschaffungszeitraum))
					WiederbeschaffungszeitraumChanged = true;

				Angebot_DatumChanged = CompareDates(Angebot_Datum, data.Angebot_Datum, data);
			}
			return
			   AngebotChanged
			   || MindestbestellmengeChanged
			   || EinkaufspreisChanged
			   || WiederbeschaffungszeitraumChanged
			   || Bestell_NrChanged
			   || Angebot_DatumChanged;
		}
		public static bool CompareDates(DateTime? Angebot_Datum, DateTime? data_Angebot_Datum, LPMinimalModel data)
		{
			if(Angebot_Datum.HasValue && data_Angebot_Datum.HasValue)
			{
				return !(Angebot_Datum.Value == data_Angebot_Datum.Value);
			}

			if(!Angebot_Datum.HasValue && !data_Angebot_Datum.HasValue)
			{
				return false;
			}

			if(Angebot_Datum.HasValue && !data_Angebot_Datum.HasValue)
			{
				HasContetnWillBeErrased.Add(data);
				return true;
			}
			if(!Angebot_Datum.HasValue && data_Angebot_Datum.HasValue)
			{
				InvalidArticles.Add(data);
			}
			return false;
		}
		private static bool CompareElement(string str1, string str2, LPMinimalModel data)
		{
			if(string.IsNullOrEmpty(str1) && string.IsNullOrEmpty(str2))
			{
				return false;
			}
			if(string.IsNullOrEmpty(str1) && !string.IsNullOrEmpty(str2))
			{
				HasContetnWillBeErrased.Add(data);
				return true;
			}
			if(!string.IsNullOrEmpty(str1) && string.IsNullOrEmpty(str2))
			{
				return true;
			}
			if(!string.IsNullOrEmpty(str1) && !string.IsNullOrEmpty(str2) && str1 != str2)
			{
				return true;
			}
			return false;
		}
		public void UpdateChangesLogs(LPMinimalModel data)
		{
			Nr = data.Nr;
			ArtikelNr = data.ArtikelNr;
			if(EinkaufspreisChanged)
			{
				EinkaufspreisChangesLog = $"[ExcelBSD] The [Einkaufspreis] was Updated   From [{data.Einkaufspreis}] to [{Einkaufspreis}] ";
				Einkaufspreis1 = data.Einkaufspreis;
				Einkaufspreis2 = data.Einkaufspreis1;
				Einkaufspreis1_gultig_bis = data.Angebot_Datum;
				Einkaufspreis2_gultig_bis = data.Einkaufspreis1_gultig_bis;
			}

			if(AngebotChanged)
				AngebotChangesLog = $"[ExcelBSD] The [Angebot] was Updated   From [{data.Angebot}] to [{Angebot}] ";
			if(MindestbestellmengeChanged)
				MindestbestellmengeChangesLog = $"[ExcelBSD] The [Mindestbestellmenge] was Updated   From [{data.Mindestbestellmenge}] to [{Mindestbestellmenge}] ";
			if(WiederbeschaffungszeitraumChanged)
				WiederbeschaffungszeitraumChangesLog = $"[ExcelBSD] The [Wiederbeschaffungszeitraum] was Updated   From [ {data.Wiederbeschaffungszeitraum} ] to [ {Wiederbeschaffungszeitraum} ] ";
			if(Angebot_DatumChanged)
				Angebot_DatumChangesLog = $"[ExcelBSD] The [Angebot_Datum] was Updated From  From [ {data.Angebot_Datum?.ToString()} ] to [ {Angebot_Datum?.ToString()} ] ";
			if(Bestell_NrChanged)
				Bestell_NrChangesLog = $"[ExcelBSD] The [Bestell_Nr] was Updated From  From [ {data.Bestell_Nr} ] to [ {Bestell_Nr} ] ";
		}

	}
	public class LPRequestModel
	{
		public int nr { get; set; }
		public bool UseSixMonthsFilter { get; set; }
	}
	public class LPMinimalRequestModel
	{
		public IFormFile ExcelFile { get; set; }
		public int nr { get; set; }
	}
	public class SuppliersExcelBulkUpdateReponseModel
	{
		public byte[] file { get; set; }
		public int nr { get; set; }
		public List<UpdateLogSummaryMinimalModel> data { get; set; } = new List<UpdateLogSummaryMinimalModel>();
	}
	public class UpdateLogSummary
	{
		public int Id { get; set; }
		public int ArtikleNr { get; set; }
		public string changeLog { get; set; }
		public string Artikelnummer { get; set; }
	}
	public class UpdateLogSummaryMinimalModel
	{
		public int ArtikleNr { get; set; }
		public string changeLog { get; set; }
		public string Artikelnummer { get; set; }
		public UpdateLogSummaryMinimalModel(UpdateLogSummary data)
		{
			ArtikleNr = data.ArtikleNr;
			changeLog = data.changeLog;
			Artikelnummer = data.Artikelnummer;
		}
	}

	public class GetOutdatedArticlesExcel
	{
		public int nr { get; set; }
		public int mnths { get; set; }
	}
}
