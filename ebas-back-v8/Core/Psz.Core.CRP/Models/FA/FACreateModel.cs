using System;

namespace Psz.Core.CRP.Models.FA
{
	public class FACreateModel
	{
		public string? Mandant { get; set; }
		public int? ArticleId { get; set; }
		public int Menge { get; set; }
		public bool? UBG { get; set; }
		public bool? UBGTransfer { get; set; }
		public bool Technikauftrage { get; set; }
		public int? Produktionsort { get; set; }
		public int? Hauptlager { get; set; }
		public string? Kunde { get; set; }
		public string? Kontakt { get; set; }
		public string? Benutzer { get; set; }
		public bool? Erstmusterauftrag { get; set; }
		public Decimal? Erstmusterpreis { get; set; }
		public string? Techniker { get; set; }
		public DateTime? Produktionstermin { get; set; }
		public string? Typ { get; set; }
		public int? Ursp_Artikel { get; set; }
		// -  2022-10-24 
		public int HBGFaPositionId { get; set; }
		// - 05-03-2025
		public bool CreateUBGFas { get; set; }
		public List<UbgFaItem> UbgFaItems { get; set; }
		public decimal? Surcharge { get; set; }

		public FACreateModel()
		{

		}
	}

	public class PotentialHBGFaRequestModel
	{
		public int ArticleId { get; set; }
		public int LagerId { get; set; }
	}
	public class PotentialHBGFaResponseModel
	{
		public int FaId { get; set; }
		public int FaNummer { get; set; }
		public int FaPositionId { get; set; }
		public double FaPositionQuantity { get; set; }
		public string? ArticleNumber { get; set; }
		public PotentialHBGFaResponseModel(
			Infrastructure.Data.Entities.Tables.PRS.FertigungEntity fertigungEntity,
			Infrastructure.Data.Entities.Tables.PRS.FertigungPositionenEntity fertigungPositionenEntity,
			Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity artikelEntity)
		{
			FaId = fertigungEntity?.ID ?? -1;
			FaNummer = fertigungEntity?.Fertigungsnummer ?? -1;
			FaPositionId = fertigungPositionenEntity?.ID ?? -1;
			FaPositionQuantity = fertigungPositionenEntity?.Anzahl ?? 0;
			ArticleNumber = artikelEntity?.ArtikelNummer;
		}
	}
	public class LinkFaPositionHBGRequestModel
	{
		public int HBGFaPositionId { get; set; }
		public int UBGFaId { get; set; }
	}
	public class PotentialUBGFaResponseModel
	{
		public int FaId { get; set; }
		public int FaNummer { get; set; }
		public double FaQuantity { get; set; }
		public PotentialUBGFaResponseModel(
			Infrastructure.Data.Entities.Tables.PRS.FertigungEntity fertigungEntity)
		{
			FaId = fertigungEntity?.ID ?? -1;
			FaNummer = fertigungEntity?.Fertigungsnummer ?? -1;
			FaQuantity = fertigungEntity?.Anzahl ?? 0;
		}
	}
	public class UBGProductionRequestModel
	{
		public int ArticleId { get; set; }
		public decimal? FaQuantity { get; set; }
	}
	public class UBGProductionResponseModel
	{
		public int Key { get; set; }
		public string? Value { get; set; }
		// - 
		public int ArticleId { get; set; }
		public string? ArticleDesignation { get; set; }
		public DateTime? ProdDate { get; set; }
		public int ProdLager { get; set; }
		public decimal ProdQuantity { get; set; }
		public bool ProdUBG { get; set; }
		public UBGProductionResponseModel(int k, string v)
		{
			Key = k;
			Value = v;
		}
		public UBGProductionResponseModel(
			Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity stucklistenEntity,
			Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity artikelEntity,
			Infrastructure.Data.Entities.Tables.BSD.ArtikelProductionExtensionEntity extensionEntity)
		{
			if(stucklistenEntity == null)
			{
				return;
			}

			Key = stucklistenEntity.Artikel_Nr_des_Bauteils ?? -1;
			Value = stucklistenEntity.Artikelnummer;
			// -
			ArticleId = stucklistenEntity.Artikel_Nr_des_Bauteils ?? -1;
			ArticleDesignation = artikelEntity?.Bezeichnung1;
			ProdDate = DateTime.Now.AddDays(-7);
			ProdLager = extensionEntity?.ProductionPlace1_Id ?? -1;
			ProdQuantity = (decimal?)stucklistenEntity.Anzahl ?? 0;
			ProdUBG = true;
		}
		public UBGProductionResponseModel(decimal faQuantity,
			Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity stucklistenEntity,
			Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity artikelEntity,
			Infrastructure.Data.Entities.Tables.BSD.ArtikelProductionExtensionEntity extensionEntity)
			: this(stucklistenEntity, artikelEntity, extensionEntity)
		{
			ProdQuantity = ProdQuantity * faQuantity;
		}
	}
	public class UbgFaItem
	{
		public int ArticleId { get; set; }
		public DateTime ProdDate { get; set; }
		public int? ProdLager { get; set; }
		public int DestLager { get; set; }
		public decimal ProdQuantity { get; set; }
		public bool ProdUBG { get; set; }
		public bool Checked { get; set; } = false;

	}
}
