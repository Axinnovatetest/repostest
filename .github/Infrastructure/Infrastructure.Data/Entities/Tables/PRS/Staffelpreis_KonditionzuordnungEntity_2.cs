using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.PRS
{
	public class Staffelpreis_KonditionzuordnungEntity_2
	{
		public int? Artikel_Nr { get; set; }
		public decimal? Betrag { get; set; }
		public string DeliveryTime { get; set; }
		public string Kostenart { get; set; }
		public int? LotSize { get; set; }
		public int Nr_Staffel { get; set; }
		public string PackagingQuantity { get; set; }
		public string PackagingType { get; set; }
		public int? PackagingTypeId { get; set; }
		public double? ProduKtionzeit { get; set; }
		public string Staffelpreis_Typ { get; set; }
		public decimal? Stundensatz { get; set; }
		public string Type { get; set; }
		public int? TypeId { get; set; }

		public Staffelpreis_KonditionzuordnungEntity_2() { }

		public Staffelpreis_KonditionzuordnungEntity_2(DataRow dataRow)
		{
			Artikel_Nr = (dataRow["Artikel_Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel_Nr"]);
			Betrag = (dataRow["Betrag"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Betrag"]);
			DeliveryTime = (dataRow["DeliveryTime"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["DeliveryTime"]);
			Kostenart = (dataRow["Kostenart"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kostenart"]);
			LotSize = (dataRow["LotSize"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LotSize"]);
			Nr_Staffel = Convert.ToInt32(dataRow["Nr_Staffel"]);
			PackagingQuantity = (dataRow["PackagingQuantity"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PackagingQuantity"]);
			PackagingType = (dataRow["PackagingType"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["PackagingType"]);
			PackagingTypeId = (dataRow["PackagingTypeId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["PackagingTypeId"]);
			ProduKtionzeit = (dataRow["ProduKtionzeit"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["ProduKtionzeit"]);
			Staffelpreis_Typ = (dataRow["Staffelpreis_Typ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Staffelpreis_Typ"]);
			Stundensatz = (dataRow["Stundensatz"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Stundensatz"]);
			Type = (dataRow["Type"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Type"]);
			TypeId = (dataRow["TypeId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["TypeId"]);
		}
	}
}

