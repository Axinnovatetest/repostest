using Psz.Core.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Models.Article.ArticlesPricesHistory
{
	public class ArtikelsPricesChangesHistoryModel
	{
		public int? SumMaterialNewMinusPreviousState { get; set; }
		public int? KalkulatorischeKostenNewMinusPreviousState { get; set; }
		public int? EKNewMinusPreviousState { get; set; }
		public string ArtikelnummerOriginal { get; set; }
		public decimal? DB { get; set; }
		public decimal? DB_wo { get; set; }
		public decimal? EK { get; set; }
		public decimal? EK_wo { get; set; }
		public int ID { get; set; }
		public int? TotalCount { get; set; }
		public decimal? KalkulatorischeKosten { get; set; }
		public decimal? KalkulatorischeKosten_wo { get; set; }
		public DateTime? lastupdated { get; set; }
		public string Logs { get; set; }
		public decimal? precent { get; set; }
		public decimal? precent_wo { get; set; }
		public decimal? SumMaterial { get; set; }
		public decimal? SumMaterial_wo { get; set; }
		public decimal? VK { get; set; }
		public decimal? VK_wo { get; set; }

		public decimal? SubtractedSumMaterial { get; set; }
		public decimal? SubtractedKalkulatorischeKosten { get; set; }
		public decimal? SubtractedVK { get; set; }
		public decimal? SubtractedEK { get; set; }
		public decimal? SubtractedDB { get; set; }
		public decimal? Subtractedprecent { get; set; }

		public ArtikelsPricesChangesHistoryModel(Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory.ArtikelsPricesChangesHistoryEntity data)
		{
			if(data is null)
				return;
			ArtikelnummerOriginal = data.ArtikelnummerOriginal;
			DB = data.DB;
			DB_wo = data.DB_wo;
			EK = data.EK;
			EK_wo = data.EK_wo;
			ID = data.ID;
			TotalCount = data.TotalCount;
			KalkulatorischeKosten = data.KalkulatorischeKosten;
			KalkulatorischeKosten_wo = data.KalkulatorischeKosten_wo;
			lastupdated = data.lastupdated;
			Logs = data.Logs;
			precent = data.precent;
			precent_wo = data.precent_wo;
			SumMaterial = data.SumMaterial;
			SumMaterial_wo = data.SumMaterial_wo;
			VK = data.VK;
			VK_wo = data.VK_wo;
			SumMaterialNewMinusPreviousState = data.SumMaterialNewMinusPreviousState;
			KalkulatorischeKostenNewMinusPreviousState = data.KalkulatorischeKostenNewMinusPreviousState;
			EKNewMinusPreviousState = data.EKNewMinusPreviousState;

			// --> 
			SubtractedSumMaterial = data.SubtractedSumMaterial;
			SubtractedKalkulatorischeKosten = data.SubtractedKalkulatorischeKosten;
			SubtractedVK = data.SubtractedVK;
			SubtractedEK = data.SubtractedEK;
			SubtractedDB = data.SubtractedDB;
			Subtractedprecent = data.Subtractedprecent;
		}

		//IPaginatedRequestModel
	}
	public class ArtikelsPricesChangesHistoryRequestModel: IPaginatedRequestModel
	{
		public DateTime afterDate { get; set; }
	}
	public class ArtikelsPricesHistoryRequestModel: IPaginatedRequestModel
	{
		public int changes { get; set; } = 15;
		public string Artikelnummer { get; set; }
		public int ArtikelNr { get; set; }
	}
}
