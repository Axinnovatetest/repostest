using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class ArtikelSalesExtensionEntity
	{
		//public int ArticleNr { get; set; }
		//public string ArticleSalesType { get; set; }
		//public int? ArticleSalesTypeId { get; set; }
		//public int Id { get; set; }
		//public string Lieferzeit { get; set; }
		//public int? Losgroesse { get; set; }
		//public decimal? MOQ { get; set; }
		//public int? Preisgruppe { get; set; }
		//public string Produktionskosten { get; set; }
		//public decimal? Profuktionszeit { get; set; }
		//public decimal? Stundensatz { get; set; }
		//public decimal? Verkaufspreis { get; set; }
		//public string Verpackungsart { get; set; }
		//public int? VerpackungsmengeId { get; set; }
		//      public decimal? Einkaufspreis { get; set; }
		//      public bool? brutto { get; set; }
		//     // public decimal? kalk_kosten { get; set; }
		//      public string Bemerkung { get; set; }
		//public decimal? Aufschlag { get; set; }
		//public decimal? Aufschlagsatz { get; set; }

		/// <summary>
		public int ArticleNr { get; set; }
		public string ArticleSalesType { get; set; }
		public int? ArticleSalesTypeId { get; set; }
		public decimal? Aufschlag { get; set; }
		public double? Aufschlagsatz { get; set; }
		public string Bemerkung { get; set; }
		public bool? brutto { get; set; }
		public decimal? DBwoCU { get; set; }
		public decimal? Einkaufspreis { get; set; }
		public int Id { get; set; }
		public decimal? Lieferzeit { get; set; }
		public int? Losgroesse { get; set; }
		public decimal? MOQ { get; set; }
		public int? Preisgruppe { get; set; }
		public decimal? Produktionskosten { get; set; }
		public decimal? Profuktionszeit { get; set; }
		public decimal? Stundensatz { get; set; }
		public decimal? Verkaufspreis { get; set; }
		public string Verpackungsart { get; set; }
		public int? VerpackungsartId { get; set; }
		public decimal? Verpackungsmenge { get; set; }
		/// </summary>

		public ArtikelSalesExtensionEntity() { }

		public ArtikelSalesExtensionEntity(DataRow dataRow)
		{
			ArticleNr = Convert.ToInt32(dataRow["ArticleNr"]);
			ArticleSalesType = (dataRow["ArticleSalesType"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArticleSalesType"]);
			ArticleSalesTypeId = (dataRow["ArticleSalesTypeId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ArticleSalesTypeId"]);
			Aufschlag = (dataRow["Aufschlag"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Aufschlag"]);
			Aufschlagsatz = (dataRow["Aufschlagsatz"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Aufschlagsatz"]);
			Bemerkung = (dataRow["Bemerkung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung"]);
			brutto = (dataRow["brutto"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["brutto"]);
			DBwoCU = (dataRow["DBwoCU"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["DBwoCU"]);
			Einkaufspreis = (dataRow["Einkaufspreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Einkaufspreis"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			Lieferzeit = (dataRow["Lieferzeit"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Lieferzeit"]);
			Losgroesse = (dataRow["Losgroesse"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Losgroesse"]);
			MOQ = (dataRow["MOQ"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["MOQ"]);
			Preisgruppe = (dataRow["Preisgruppe"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Preisgruppe"]);
			Produktionskosten = (dataRow["Produktionskosten"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Produktionskosten"]);
			Profuktionszeit = (dataRow["Profuktionszeit"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Profuktionszeit"]);
			Stundensatz = (dataRow["Stundensatz"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Stundensatz"]);
			Verkaufspreis = (dataRow["Verkaufspreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Verkaufspreis"]);
			Verpackungsart = (dataRow["Verpackungsart"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Verpackungsart"]);
			VerpackungsartId = (dataRow["VerpackungsartId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["VerpackungsartId"]);
			Verpackungsmenge = (dataRow["Verpackungsmenge"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Verpackungsmenge"]);
		}
	}
}

