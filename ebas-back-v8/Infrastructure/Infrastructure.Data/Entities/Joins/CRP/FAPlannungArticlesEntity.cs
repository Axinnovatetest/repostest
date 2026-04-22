using System;
using System.Data;


namespace Infrastructure.Data.Entities.Joins.CRP
{
	public class FAPlannungArticlesEntity
	{
		public int? ArtikelNr { get; set; }
		public string Artikelnummer { get; set; }
		public decimal? SumFA { get; set; }
		public decimal? SumAB { get; set; }
		public decimal? SumLP { get; set; }
		public decimal? SumFC { get; set; }
		public FAPlannungArticlesEntity(DataRow dataRow)
		{
			ArtikelNr = (dataRow[columnName: "Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr"]);
			Artikelnummer = (dataRow[columnName: "Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			SumFA = (dataRow[columnName: "SumFA"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["SumFA"]);
			SumAB = (dataRow[columnName: "SumAB"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["SumAB"]);
			SumLP = (dataRow[columnName: "SumLP"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["SumLP"]);
			SumFC = (dataRow[columnName: "SumFC"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["SumFC"]);
		}
	}
	public class FAPlannungArticlesKwEntity
	{
		public int? ArtikelNr { get; set; }
		public string Artikelnummer { get; set; }
		public int? KW { get; set; }
		public decimal? SumFA { get; set; }
		public decimal? SumAB { get; set; }
		public decimal? SumLP { get; set; }
		public decimal? SumFC { get; set; }
		public FAPlannungArticlesKwEntity(DataRow dataRow)
		{
			ArtikelNr = (dataRow[columnName: "Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr"]);
			Artikelnummer = (dataRow[columnName: "Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			KW = (dataRow[columnName: "KW"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["KW"]);
			SumFA = (dataRow[columnName: "SumFA"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["SumFA"]);
			SumAB = (dataRow[columnName: "SumAB"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["SumAB"]);
			SumLP = (dataRow[columnName: "SumLP"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["SumLP"]);
			SumFC = (dataRow[columnName: "SumFC"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["SumFC"]);
		}
	}
	public class FAPlannungArticlesKwDataEntity
	{
		public int? ArticleId { get; set; }
		public int? Id { get; set; }
		public string? Number { get; set; }
		public decimal? Quantity { get; set; }
		public bool? Manual { get; set; }
		public int? CustomerNumber { get; set; }
		public FAPlannungArticlesKwDataEntity(DataRow dataRow)
		{
			ArticleId = (dataRow[columnName: "ArticleId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ArticleId"]);
			Id = (dataRow[columnName: "Id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Id"]);
			Number = (dataRow[columnName: "Number"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Number"]);
			Quantity = (dataRow[columnName: "Quantity"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Quantity"]);
			Manual = (dataRow[columnName: "Manual"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Manual"]);
			CustomerNumber = (dataRow[columnName: "CustomerNumber"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CustomerNumber"]);
		}
	}
	public class FAPlannungArticlesEntity_2
	{
		public int? ArtikelNr { get; set; }
		public string Artikelnummer { get; set; }
		public decimal? SumProd { get; set; }
		public decimal? SumNeeds { get; set; }
		public int? Year { get; set; }
		public int? Kw { get; set; }
		public FAPlannungArticlesEntity_2(DataRow dataRow)
		{
			ArtikelNr = (dataRow[columnName: "Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr"]);
			Artikelnummer = (dataRow[columnName: "Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			SumProd = (dataRow[columnName: "SumProd"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["SumProd"]);
			SumNeeds = (dataRow[columnName: "SumNeeds"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["SumNeeds"]);
			Year = (dataRow[columnName: "Year"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Year"]);
			Kw = (dataRow[columnName: "Kw"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Year"]);
		}
	}
	public class FAPlannungArticlesEntity_3
	{
		public int? ArtikelNr { get; set; }
		public int? Prio { get; set; }
		public string Artikelnummer { get; set; }
		public bool UBG { get; set; }
		public FAPlannungArticlesEntity_3(DataRow dataRow)
		{
			ArtikelNr = (dataRow[columnName: "Artikel-Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel-Nr"]);
			Prio = (dataRow[columnName: "Prio"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Prio"]);
			Artikelnummer = (dataRow[columnName: "Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			UBG = (dataRow[columnName: "Artikelnummer"] == System.DBNull.Value) ? false : Convert.ToBoolean(dataRow["UBG"]);
		}
	}
	public class FaPlanningComputeLogsEntity
	{
		public DateTime? ExecDate { get; set; }
		public int? ExecUserId { get; set; }
		public int Id { get; set; }
		public bool? UBG { get; set; }

		public FaPlanningComputeLogsEntity() { }

		public FaPlanningComputeLogsEntity(DataRow dataRow)
		{
			ExecDate = (dataRow["ExecDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ExecDate"]);
			ExecUserId = (dataRow["ExecUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ExecUserId"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			UBG = (dataRow["UBG"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["UBG"]);
		}
	}
}
