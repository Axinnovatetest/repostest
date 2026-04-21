
using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.Statistics.MGO
{
	public class PSZFGTNEntity
	{
		public decimal? Arbeitskosten_PSZ_TN { get; set; }
		public string Artikelnummer { get; set; }
		public decimal? Marge_mit_CU { get; set; }
		public decimal? Marge_ohne_CU { get; set; }
		public Single? Produktionszeit { get; set; }
		public decimal? Produktivitat__FA_Zeit_ { get; set; }
		public decimal? Produktivitat_Artikelzeit_ { get; set; }
		public decimal? Stundensatz { get; set; }
		public decimal? Umsatz_PSZ_TN { get; set; }
		public PSZFGTNEntity() { }
		public PSZFGTNEntity(DataRow dataRow)
		{
			Arbeitskosten_PSZ_TN = (dataRow["Arbeitskosten PSZ_TN"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Arbeitskosten PSZ_TN"]);
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Marge_mit_CU = (dataRow["Marge mit CU"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Marge mit CU"]);
			Marge_ohne_CU = (dataRow["Marge ohne CU"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Marge ohne CU"]);
			Produktionszeit = (dataRow["Produktionszeit"] == System.DBNull.Value) ? (Single?)null : Convert.ToSingle(dataRow["Produktionszeit"]);
			Produktivitat__FA_Zeit_ = (dataRow["Produktivität (FA Zeit)"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Produktivität (FA Zeit)"]);
			Produktivitat_Artikelzeit_ = (dataRow["Produktivität(Artikelzeit)"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Produktivität(Artikelzeit)"]);
			Stundensatz = (dataRow["Stundensatz"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Stundensatz"]);
			Umsatz_PSZ_TN = (dataRow["Umsatz PSZ_TN"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Umsatz PSZ_TN"]);
		}
	}
}