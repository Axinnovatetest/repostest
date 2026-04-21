using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Models.Article
{
	public class CreateRequestModel
	{
		public string ArticleNumber { get; set; }
		public string ArticleNumberCustom { get; set; }
		public bool IsArticleNumberCustom { get; set; } = false;
		public int GoodsGroupId { get; set; }
		public string GoodsGroupName { get; set; }
		public int? GoodsTypeId { get; set; }
		public string GoodsTypeName { get; set; }
		public int ProductionCountryId { get; set; }
		public int ProductionSiteId { get; set; }
		// -
		public int CopyFromId { get; set; }
		// -
		public int CustomerId { get; set; }
		public int CustomerNumber { get; set; }
		public string CustomerName { get; set; }
		public string CustomerItemNumber { get; set; }
		public string ProductionCountryCode { get; set; }
		public string ProductionCountryName { get; set; }
		public string ProductionSiteCode { get; set; }
		public string ProductionSiteName { get; set; }
		public string CustomerItemNumberSequence { get; set; }
		public string Designation { get; set; }
		public string CustomerItemIndex { get; set; }
		public DateTime? CustomerItemIndexDate { get; set; }
		public string CustomerItemIndexSequence { get; set; }
		public string CustomerPrefix { get; set; }
		public bool IsArticleNumberSpecial { get; set; } = false;
		// - 2023-01-22 - EDI
		public bool IsEdiDefault { get; set; } = false;
		// - 2023-07-06  - Required fileds
		public string CustomsNumber { get; set; }
		public string OriginCountry { get; set; }
		public string ProjectClassification { get; set; }

		// - 2023-08-24 - CoC
		public bool? CocActive { get; set; }
		public string CocVersion { get; set; }
		// - 2024-02-28 - Capital // E-Drawing
		public bool IsEDrawing { get; set; } = false;

		// - 2024-03-06 Task :00024 PM - FG1(back)
		public bool DELFixiert { get; set; }
		public bool VKFestpreis { get; set; }
		public string ProjektartFG { get; set; }
		public string Artikelbezeichnung { get; set; }
		public string OrderNumber { get; set; }
		public string Consumption12Months { get; set; }
		public int Losgroesse { get; set; }
		public string Projektname { get; set; }
		// - souilmi 21/05/2024 for roh artikelnummer
		public string Manufacturer { get; set; }
		public string ManufacturerNumber { get; set; }
		public string Unit { get; set; }
		public decimal? CopperWeight { get; set; }
		public int? IdLevelOne { get; set; }
	}
	public class CustomerResponseModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Kreis { get; set; }
		public int Number { get; set; }
		public CustomerResponseModel(Infrastructure.Data.Entities.Tables.CTS.PSZ_Nummerschlüssel_KundeEntity entity)
		{
			if(entity == null)
				return;

			Id = entity.ID;
			Name = entity.Analyse == true ? $"{entity.Kunde} // {entity.AnalyseName}" : entity.Kunde;
			Kreis = entity.Nummerschlüssel;
			Number = entity.Kundennummer ?? -1;
		}
	}
	public class CountryResponseModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Prefix { get; set; }
		public int Sequence { get; set; }
		public CountryResponseModel(Infrastructure.Data.Entities.Tables.WPL.CountryEntity entity)
		{
			if(entity == null)
				return;

			Id = entity.Id;
			Name = entity.Name;
			Prefix = entity.Designation;
			Sequence = entity.MtdArticleSequence ?? 0;
		}
	}
	public class CountryFullResponseModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Prefix { get; set; }
		public int Sequence { get; set; }
		public List<HallResponseModel> Sites { get; set; }
		public CountryFullResponseModel(Infrastructure.Data.Entities.Tables.WPL.CountryEntity entity, List<Infrastructure.Data.Entities.Tables.WPL.HallEntity> halls)
		{
			if(entity == null)
				return;

			Id = entity.Id;
			Name = entity.Name;
			Prefix = entity.Designation;
			Sequence = entity.MtdArticleSequence ?? 0;
			Sites = halls?.Select(x => new HallResponseModel(x))?.ToList();
		}
	}
	public class HallResponseModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int CountryId { get; set; }
		public int? LagerortId { get; set; }
		public HallResponseModel(Infrastructure.Data.Entities.Tables.WPL.HallEntity entity)
		{
			if(entity == null)
				return;

			Id = entity.Id;
			Name = entity.Name;
			CountryId = entity.CountryId;
			LagerortId = entity.LagerortId;
		}
	}
	public class CustomerItemNumbersRequestModel
	{
		public int CustomerId { get; set; }
		public int CustomerNumber { get; set; }
		public string CustomerPrefix { get; set; }
	}
	public class CustomerIndexesRequestModel: CustomerItemNumbersRequestModel
	{
		public string CustomerItemNumber { get; set; }
	}
	public class CustomerIndexesResponseModel
	{
		public int Key { get; set; }
		public string Value { get; set; }
		public bool IsArticleNumberSpecial { get; set; }
	}
	public class CustomerItemNumbersResponseModel
	{
		public int Key { get; set; }
		public string Value { get; set; }
		public string CustomerItemNumberKreis { get; set; }
		public CustomerItemNumbersResponseModel(Tuple<int, string, string> artikelEntity)
		{
			if(artikelEntity == null)
				return;

			Key = artikelEntity.Item1;
			Value = artikelEntity.Item2;
			var splits = artikelEntity.Item3?.Split('-');
			CustomerItemNumberKreis = splits.Count() > 1 ? splits[1] : "";
		}
	}
	public class CreateFromCopyRequestModel
	{
		public int ArticleNr { get; set; }
		public string NewArticleNumber { get; set; }
		public string NewArticleDesignation { get; set; }
		public bool WithBOM { get; set; } = true;
	}
	public class CreateFromCopyXLSRequestModel
	{
		public int ArticleNr { get; set; }
		public string XLSPath { get; set; }
	}
}