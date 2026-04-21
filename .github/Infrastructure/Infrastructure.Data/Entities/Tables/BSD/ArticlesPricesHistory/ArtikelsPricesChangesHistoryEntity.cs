using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD.ArticlesPricesHistory
{
	public class ArtikelsPricesChangesHistoryEntity
	{
		public string ArtikelnummerOriginal { get; set; }
		public int? SumMaterialNewMinusPreviousState { get; set; }
		public int? KalkulatorischeKostenNewMinusPreviousState { get; set; }
		public int? EKNewMinusPreviousState { get; set; }
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

		public ArtikelsPricesChangesHistoryEntity() { }

		public ArtikelsPricesChangesHistoryEntity(DataRow dataRow)
		{
			ArtikelnummerOriginal = (dataRow["ArtikelnummerOriginal"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArtikelnummerOriginal"]);
			DB = (dataRow["DB"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["DB"]);
			DB_wo = (dataRow["DB_wo"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["DB_wo"]);
			EK = (dataRow["EK"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["EK"]);
			EK_wo = (dataRow["EK_wo"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["EK_wo"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			KalkulatorischeKosten = (dataRow["KalkulatorischeKosten"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["KalkulatorischeKosten"]);
			KalkulatorischeKosten_wo = (dataRow["KalkulatorischeKosten_wo"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["KalkulatorischeKosten_wo"]);
			lastupdated = (dataRow["lastupdated"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["lastupdated"]);
			Logs = (dataRow["Logs"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Logs"]);
			precent = (dataRow["precent"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["precent"]);
			precent_wo = (dataRow["precent_wo"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["precent_wo"]);
			SumMaterial = (dataRow["SumMaterial"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["SumMaterial"]);
			SumMaterial_wo = (dataRow["SumMaterial_wo"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["SumMaterial_wo"]);
			VK = (dataRow["VK"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["VK"]);
			VK_wo = (dataRow["VK_wo"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["VK_wo"]);
		}
		public ArtikelsPricesChangesHistoryEntity(DataRow dataRow, bool WithCount)
		{
			TotalCount = (dataRow["TotalCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["TotalCount"]);
			SumMaterialNewMinusPreviousState = (dataRow["SumMaterialNewMinusPreviousState"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["SumMaterialNewMinusPreviousState"]);
			KalkulatorischeKostenNewMinusPreviousState = (dataRow["KalkulatorischeKostenNewMinusPreviousState"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["KalkulatorischeKostenNewMinusPreviousState"]);
			EKNewMinusPreviousState = (dataRow["EKNewMinusPreviousState"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["EKNewMinusPreviousState"]);
			ArtikelnummerOriginal = (dataRow["ArtikelnummerOriginal"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArtikelnummerOriginal"]);
			DB = (dataRow["DB"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["DB"]);
			DB_wo = (dataRow["DB_wo"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["DB_wo"]);
			EK = (dataRow["EK"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["EK"]);
			EK_wo = (dataRow["EK_wo"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["EK_wo"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			KalkulatorischeKosten = (dataRow["KalkulatorischeKosten"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["KalkulatorischeKosten"]);
			KalkulatorischeKosten_wo = (dataRow["KalkulatorischeKosten_wo"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["KalkulatorischeKosten_wo"]);
			lastupdated = (dataRow["lastupdated"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["lastupdated"]);
			Logs = (dataRow["Logs"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Logs"]);
			precent = (dataRow["precent"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["precent"]);
			precent_wo = (dataRow["precent_wo"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["precent_wo"]);
			SumMaterial = (dataRow["SumMaterial"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["SumMaterial"]);
			SumMaterial_wo = (dataRow["SumMaterial_wo"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["SumMaterial_wo"]);
			VK = (dataRow["VK"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["VK"]);
			VK_wo = (dataRow["VK_wo"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["VK_wo"]);
		}
		public ArtikelsPricesChangesHistoryEntity(DataRow dataRow, int WithCount)
		{
			TotalCount = (dataRow["TotalCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["TotalCount"]);
			ArtikelnummerOriginal = (dataRow["ArtikelnummerOriginal"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArtikelnummerOriginal"]);
			DB = (dataRow["DB"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["DB"]);
			DB_wo = (dataRow["DB_wo"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["DB_wo"]);
			EK = (dataRow["EK"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["EK"]);
			EK_wo = (dataRow["EK_wo"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["EK_wo"]);
			ID = Convert.ToInt32(dataRow["ID"]);
			KalkulatorischeKosten = (dataRow["KalkulatorischeKosten"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["KalkulatorischeKosten"]);
			KalkulatorischeKosten_wo = (dataRow["KalkulatorischeKosten_wo"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["KalkulatorischeKosten_wo"]);
			lastupdated = (dataRow["lastupdated"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["lastupdated"]);
			Logs = (dataRow["Logs"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Logs"]);
			precent = (dataRow["precent"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["precent"]);
			precent_wo = (dataRow["precent_wo"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["precent_wo"]);
			SumMaterial = (dataRow["SumMaterial"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["SumMaterial"]);
			SumMaterial_wo = (dataRow["SumMaterial_wo"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["SumMaterial_wo"]);
			VK = (dataRow["VK"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["VK"]);
			VK_wo = (dataRow["VK_wo"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["VK_wo"]);

			SubtractedSumMaterial = (dataRow["SubtractedSumMaterialres"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["SubtractedSumMaterialres"]);
			SubtractedKalkulatorischeKosten = (dataRow["SubtractedKalkulatorischeKostenres"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["SubtractedKalkulatorischeKostenres"]);
			SubtractedVK = (dataRow["SubtractedVKres"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["SubtractedVKres"]);
			SubtractedEK = (dataRow["SubtractedEKres"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["SubtractedEKres"]);
			SubtractedDB = (dataRow["SubtractedDBres"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["SubtractedDBres"]);
			Subtractedprecent = (dataRow["Subtractedprecentres"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Subtractedprecentres"]);
		}
		public ArtikelsPricesChangesHistoryEntity ShallowClone()
		{
			return new ArtikelsPricesChangesHistoryEntity
			{
				ArtikelnummerOriginal = ArtikelnummerOriginal,
				DB = DB,
				DB_wo = DB_wo,
				EK = EK,
				EK_wo = EK_wo,
				ID = ID,
				KalkulatorischeKosten = KalkulatorischeKosten,
				KalkulatorischeKosten_wo = KalkulatorischeKosten_wo,
				lastupdated = lastupdated,
				Logs = Logs,
				precent = precent,
				precent_wo = precent_wo,
				SumMaterial = SumMaterial,
				SumMaterial_wo = SumMaterial_wo,
				VK = VK,
				VK_wo = VK_wo
			};
		}
	}

}
