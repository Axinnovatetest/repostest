using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.MTM.Order
{
	public class NeededQuantityEntity
	{

		public decimal? NeededQuantity { get; set; }
		public string Week { get; set; }
		public NeededQuantityEntity(DataRow datarow)
		{
			NeededQuantity = (datarow["NeedQuantity"] == System.DBNull.Value) ? -1 : Convert.ToDecimal(datarow["NeedQuantity"]);
			Week = (datarow["Week"] == System.DBNull.Value) ? "" : datarow["Week"].ToString();
		}
	}
	public class OrderedQuantityEntity
	{
		public decimal? orderedQuantity { get; set; }
		public string Week { get; set; }
		public OrderedQuantityEntity(DataRow datarow)
		{
			orderedQuantity = (datarow["OrderedQuantity"] == System.DBNull.Value) ? -1 : Convert.ToDecimal(datarow["OrderedQuantity"]);
			Week = (datarow["WeekPO"] == System.DBNull.Value) ? "" : datarow["WeekPO"].ToString();
		}
	}
	public class FaultyOrdersCountEntity
	{

		public int? faulty_Orders_Count { get; set; }

		public FaultyOrdersCountEntity() { }
		public FaultyOrdersCountEntity(System.Data.DataRow datarow)
		{

			faulty_Orders_Count = (datarow["faulty_Orders"] == System.DBNull.Value) ? -1 : Convert.ToInt32(datarow["faulty_Orders"]);
		}
	}

	#region Need Analyse
	public class CTSNeedEntity
	{
		public string Artikelnummer { get; set; }
		public string Bestell_Nr { get; set; }
		public decimal? DiffPrice { get; set; }
		public decimal? DiffQuantity { get; set; }
		public decimal? Einkaufspreis { get; set; }
		public decimal? Gesamtpreis { get; set; }
		public string Name1 { get; set; }
		public decimal? ROH_Bestand { get; set; }
		public decimal? ROH_Quantity { get; set; }
		public decimal? Wert_LagerBestandBedarf { get; set; }

		public CTSNeedEntity() { }

		public CTSNeedEntity(DataRow dataRow)
		{
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
			Bestell_Nr = (dataRow["Bestell-Nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bestell-Nr"]);
			DiffPrice = (dataRow["DiffPrice"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["DiffPrice"]);
			DiffQuantity = (dataRow["DiffQuantity"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["DiffQuantity"]);
			Einkaufspreis = (dataRow["Einkaufspreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Einkaufspreis"]);
			Gesamtpreis = (dataRow["Gesamtpreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Gesamtpreis"]);
			Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"]);
			ROH_Bestand = (dataRow["ROH_Bestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ROH_Bestand"]);
			ROH_Quantity = (dataRow["ROH_Quantity"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ROH_Quantity"]);
			Wert_LagerBestandBedarf = (dataRow["Wert_LagerBestandBedarf"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Wert_LagerBestandBedarf"]);
		}

		public CTSNeedEntity ShallowClone()
		{
			return new CTSNeedEntity
			{
				Artikelnummer = Artikelnummer,
				Bestell_Nr = Bestell_Nr,
				DiffPrice = DiffPrice,
				DiffQuantity = DiffQuantity,
				Einkaufspreis = Einkaufspreis,
				Gesamtpreis = Gesamtpreis,
				Name1 = Name1,
				ROH_Bestand = ROH_Bestand,
				ROH_Quantity = ROH_Quantity,
				Wert_LagerBestandBedarf = Wert_LagerBestandBedarf
			};
		}
	}
	public class CTSNeedSummaryEntity
	{
		public decimal? TotalAmount { get; set; }
		public decimal? PeriodAmount { get; set; }
		public int KW { get; set; }
		public int Year { get; set; }

		public CTSNeedSummaryEntity() { }

		public CTSNeedSummaryEntity(DataRow dataRow)
		{
			TotalAmount = (dataRow["TotalAmount"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["TotalAmount"]);
			PeriodAmount = (dataRow["PeriodAmount"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["PeriodAmount"]);
			KW = Convert.ToInt32(dataRow["KW"]);
			Year = Convert.ToInt32(dataRow["Year"]);
		}

		public CTSNeedSummaryEntity ShallowClone()
		{
			return new CTSNeedSummaryEntity
			{
				TotalAmount = TotalAmount,
				PeriodAmount = PeriodAmount,
			};
		}
	}
	#endregion Need Analyse
}
