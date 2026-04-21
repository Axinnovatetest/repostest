using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.MGO
{
	public class CTSArticleVKMargeEntity
	{
		public string Artikelnummer { get; set; }
		public decimal? DB_I_Mit_CU { get; set; }
		public decimal? DB_I_Ohne_CU { get; set; }
		public decimal? EK_Mit_CU { get; set; }
		public decimal? EK_ohne_CU { get; set; }
		public string Freigabestatus { get; set; }
		public decimal? Kalkulatorische_kosten { get; set; }
		public decimal? Prozent_Mit_CU { get; set; }
		public decimal? Prozent_Ohne_CU { get; set; }
		public decimal? SUM_Material_Mit_CU { get; set; }
		public decimal? SUM_Material_ohne_CU { get; set; }
		public decimal? VK_PSZ { get; set; }
		public DateTime? LastSyncDate { get; set; }


		public CTSArticleVKMargeEntity() { }

		public CTSArticleVKMargeEntity(DataRow dataRow)
		{
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			DB_I_Mit_CU = (dataRow["DB_I_Mit_CU"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["DB_I_Mit_CU"]);
			DB_I_Ohne_CU = (dataRow["DB_I_Ohne_CU"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["DB_I_Ohne_CU"]);
			EK_Mit_CU = (dataRow["EK_Mit_CU"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["EK_Mit_CU"]);
			EK_ohne_CU = (dataRow["EK_ohne_CU"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["EK_ohne_CU"]);
			Freigabestatus = (dataRow["Freigabestatus"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Freigabestatus"]);
			Kalkulatorische_kosten = (dataRow["Kalkulatorische kosten"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Kalkulatorische kosten"]);
			Prozent_Mit_CU = (dataRow["Prozent_Mit_CU"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Prozent_Mit_CU"]);
			Prozent_Ohne_CU = (dataRow["Prozent_Ohne_CU"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Prozent_Ohne_CU"]);
			SUM_Material_Mit_CU = (dataRow["SUM_Material_Mit_CU"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["SUM_Material_Mit_CU"]);
			SUM_Material_ohne_CU = (dataRow["SUM_Material_ohne_CU"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["SUM_Material_ohne_CU"]);
			VK_PSZ = (dataRow["VK PSZ"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["VK PSZ"]);
			LastSyncDate = (dataRow["LastSyncDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastSyncDate"]);

		}

		public CTSArticleVKMargeEntity ShallowClone()
		{
			return new CTSArticleVKMargeEntity
			{
				Artikelnummer = Artikelnummer,
				DB_I_Mit_CU = DB_I_Mit_CU,
				DB_I_Ohne_CU = DB_I_Ohne_CU,
				EK_Mit_CU = EK_Mit_CU,
				EK_ohne_CU = EK_ohne_CU,
				Freigabestatus = Freigabestatus,
				Kalkulatorische_kosten = Kalkulatorische_kosten,
				Prozent_Mit_CU = Prozent_Mit_CU,
				Prozent_Ohne_CU = Prozent_Ohne_CU,
				SUM_Material_Mit_CU = SUM_Material_Mit_CU,
				SUM_Material_ohne_CU = SUM_Material_ohne_CU,
				VK_PSZ = VK_PSZ,
				LastSyncDate = LastSyncDate
			};
		}
	}
}
