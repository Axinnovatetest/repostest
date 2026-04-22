using System;

namespace Infrastructure.Data.Entities.Joins.MTM.Order
{
	public class GetOrdersInTimeSpanEntity
	{
		public decimal? OrderedQuantity { get; set; }
		public string SupplierName { get; set; }
		public int? BestellungenNr { get; set; }
		public int? BestellungenNummer { get; set; }
		public DateTime? Bestätigter_Termin { get; set; }
		public DateTime? Liefertermin { get; set; }
		public GetOrdersInTimeSpanEntity(System.Data.DataRow datarow)
		{
			BestellungenNr = (datarow["Nr"] == System.DBNull.Value) ? -1 : Convert.ToInt32(datarow["Nr"]);
			BestellungenNummer = (datarow["Bestellung-Nr"] == System.DBNull.Value) ? -1 : Convert.ToInt32(datarow["Bestellung-Nr"]);
			OrderedQuantity = (datarow["orderedQuantity"] == System.DBNull.Value) ? -1 : Convert.ToDecimal(datarow["orderedQuantity"]);
			Bestätigter_Termin = (datarow["Bestätigter_Termin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(datarow["Bestätigter_Termin"]);
			SupplierName = (datarow["SupplierName"] == System.DBNull.Value) ? "" : Convert.ToString(datarow["SupplierName"]);
			Liefertermin = (datarow["Liefertermin"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(datarow["Liefertermin"]);
		}
	}
}
