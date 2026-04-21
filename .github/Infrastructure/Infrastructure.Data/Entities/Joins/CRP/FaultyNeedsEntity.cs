using System;
using System.Data;


namespace Infrastructure.Data.Entities.Joins.CRP
{
	public class FaultyNeedsEntity
	{
		public int Id { get; set; }
		public int? vorfallNr { get; set; }
		public string DocumentNumber { get; set; }
		public string Type { get; set; }
		public int? CustomerNumber { get; set; }
		public DateTime? DeliveryDate { get; set; }
		public decimal? Quantity { get; set; }
		public bool? IsManual { get; set; }
		public FaultyNeedsEntity(DataRow dataRow)
		{
			Id = Convert.ToInt32(dataRow["Id"]);
			vorfallNr = (dataRow[columnName: "vorfallNr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["vorfallNr"]);
			DocumentNumber = (dataRow[columnName: "DocumentNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["DocumentNumber"]);
			DeliveryDate = (dataRow[columnName: "DeliveryDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["DeliveryDate"]);
			Quantity = (dataRow[columnName: "Quantity"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Quantity"]);
			CustomerNumber = (dataRow[columnName: "CustomerNumber"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CustomerNumber"]);
			Type = (dataRow[columnName: "Type"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Type"]);
			IsManual = (dataRow[columnName: "IsManual"] == System.DBNull.Value) ? null : Convert.ToBoolean(dataRow["IsManual"]);
		}
	}
}