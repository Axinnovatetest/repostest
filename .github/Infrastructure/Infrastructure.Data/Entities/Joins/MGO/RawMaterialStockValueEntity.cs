using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.MGO
{
	public class RawMaterialStockValueEntity
	{
		public decimal Bestandswert__Summe_EUR_ { get; set; }
		public string Lagerort { get; set; }
		public string Warengruppe { get; set; }

		public RawMaterialStockValueEntity() { }

		public RawMaterialStockValueEntity(DataRow dataRow)
		{
			Bestandswert__Summe_EUR_ = (dataRow["Bestandswert (Summe EUR)"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Bestandswert (Summe EUR)"]);
			Lagerort = (dataRow["Lagerort"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lagerort"]);
			Warengruppe = (dataRow["Warengruppe"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Warengruppe"]);
		}

		public RawMaterialStockValueEntity ShallowClone()
		{
			return new RawMaterialStockValueEntity
			{
				Bestandswert__Summe_EUR_ = Bestandswert__Summe_EUR_,
				Lagerort = Lagerort,
				Warengruppe = Warengruppe
			};
		}
	}
}
