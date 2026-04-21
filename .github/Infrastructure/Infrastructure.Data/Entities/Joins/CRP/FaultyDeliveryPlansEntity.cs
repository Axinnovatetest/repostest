using System;
using System.Data;


namespace Infrastructure.Data.Entities.Joins.CRP
{
	public class FaultyDeliveryPlansEntity
	{
		public string DocumentNumber { get; set; }
		public DateTime? PlanningQuantityRequestedShipmentDate { get; set; }
		public decimal? PlanningQuantityQuantity { get; set; }
		public FaultyDeliveryPlansEntity(DataRow dataRow)
		{
			DocumentNumber = (dataRow[columnName: "DocumentNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["DocumentNumber"]);
			PlanningQuantityRequestedShipmentDate = (dataRow[columnName: "PlanningQuantityRequestedShipmentDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["PlanningQuantityRequestedShipmentDate"]);
			PlanningQuantityQuantity = (dataRow[columnName: "PlanningQuantityQuantity"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["PlanningQuantityQuantity"]);
		}
	}
}