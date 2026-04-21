using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.FAPlannung
{
	public class FAAnalayseGewerk3ALEntity
	{
		public int? FA_Nr { get; set; }
		public string Freigabestatus { get; set; }
		public Decimal? FA_Sasia { get; set; }
		public string Numeri_i_artikullit { get; set; }
		public string Numeri_i_materialit { get; set; }
		public Decimal? Nevoje { get; set; }
		public Decimal? Ne_magazine { get; set; }
		public Decimal? Ne_prodhim { get; set; }
		public DateTime? Afati_i_prestarise { get; set; }
		public string Material { get; set; }
		public string Typ_Material { get; set; }
		public string Gewerk_1 { get; set; }
		public string Gewerk_2 { get; set; }
		public string Gewerk_3 { get; set; }
		public DateTime? FA_begonnen { get; set; }

		public FAAnalayseGewerk3ALEntity(DataRow dataRow)
		{
			FA_Nr = (dataRow["FA_Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["FA_Nr"]);
			Freigabestatus = (dataRow["Freigabestatus"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Freigabestatus"]);
			FA_Sasia = (dataRow["FA_Sasia"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["FA_Sasia"]);
			Numeri_i_artikullit = (dataRow["Numeri i artikullit"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Numeri i artikullit"]);
			Numeri_i_materialit = (dataRow["Numeri i materialit"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Numeri i materialit"]);
			Nevoje = (dataRow["Nevoje"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["Nevoje"]);
			Ne_magazine = (dataRow["Ne magazine"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["Ne magazine"]);
			Ne_prodhim = (dataRow["Ne prodhim"] == System.DBNull.Value) ? (Decimal?)null : Convert.ToDecimal(dataRow["Ne prodhim"]);
			Afati_i_prestarise = (dataRow["Afati i prestarise"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Afati i prestarise"]);
			Material = (dataRow["Material"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Material"]);
			Typ_Material = (dataRow["Typ_Material"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Typ_Material"]);
			Gewerk_1 = (dataRow["Gewerk 1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Gewerk 1"]);
			Gewerk_2 = (dataRow["Gewerk 2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Gewerk 2"]);
			Gewerk_3 = (dataRow["Gewerk 3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Gewerk 3"]);
			FA_begonnen = (dataRow["FA_begonnen"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["FA_begonnen"]);
		}
	}
}
