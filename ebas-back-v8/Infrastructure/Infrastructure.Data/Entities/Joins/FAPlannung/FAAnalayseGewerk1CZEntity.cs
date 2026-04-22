using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.FAPlannung
{
	public class FAAnalayseGewerk1CZEntity
	{
		public int? FA_Cislo { get; set; }
		public string Freigabestatus { get; set; }
		public Decimal? FA_Mnostvi { get; set; }
		public string Cislo_Zbozi { get; set; }
		public string Cislo_Material { get; set; }
		public Decimal? Bedarf { get; set; }
		public Decimal? P3000 { get; set; }
		public Decimal? Im_Lager { get; set; }
		public Decimal? In_derProduktion { get; set; }
		public DateTime? Termin_Rezarna { get; set; }
		public string Material { get; set; }
		public string Typ_Material { get; set; }
		public string Gewerk_1 { get; set; }
		public string Gewerk_2 { get; set; }
		public string Gewerk_3 { get; set; }
		public DateTime? FA_begonnen { get; set; }
		public Decimal RestBestand { get; set; }

		public FAAnalayseGewerk1CZEntity(DataRow dataRow)
		{
			FA_Cislo = (dataRow["FA_Cislo"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["FA_Cislo"]);
			Freigabestatus = (dataRow["Freigabestatus"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Freigabestatus"]);
			FA_Mnostvi = (dataRow["FA_Mnostvi"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["FA_Mnostvi"]);
			Cislo_Zbozi = (dataRow["Cislo_Zbozi"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Cislo_Zbozi"]);
			Cislo_Material = (dataRow["Cislo_Material"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Cislo_Material"]);
			Bedarf = (dataRow["Bedarf"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["Bedarf"]);
			P3000 = (dataRow["P3000"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["P3000"]);
			Im_Lager = (dataRow["Im Lager"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["Im Lager"]);
			In_derProduktion = (dataRow["In derProduktion"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["In derProduktion"]);
			Termin_Rezarna = (dataRow["Termin_Rezarna"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Termin_Rezarna"]);
			Material = (dataRow["Material"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Material"]);
			Typ_Material = (dataRow["Typ_Material"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Typ_Material"]);
			Gewerk_1 = (dataRow["Gewerk 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Gewerk 1"]);
			Gewerk_2 = (dataRow["Gewerk 2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Gewerk 2"]);
			Gewerk_3 = (dataRow["Gewerk 3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Gewerk 3"]);
			FA_begonnen = (dataRow["FA_begonnen"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["FA_begonnen"]);
		}

	}
}
