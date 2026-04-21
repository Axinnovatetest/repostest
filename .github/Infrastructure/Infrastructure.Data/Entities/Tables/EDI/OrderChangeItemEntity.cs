using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.PRS
{
	public class OrderChangeItemEntity
	{
		public DateTime? ActionTime { get; set; }
		public int? ActionUserId { get; set; }
		public string ActionUsername { get; set; }
		public DateTime CreationTime { get; set; }
		public decimal CurrentItemPriceCalculationNet { get; set; }
		public string CustomerItemNumber { get; set; }
		public DateTime DesiredDate { get; set; }
		public int Id { get; set; }
		public string ItemDescription { get; set; }
		public string ItemNumber { get; set; }
		public string MeasureUnitQualifier { get; set; }
		public string Notes { get; set; }
		public int OrderChangeId { get; set; }
		public decimal OrderedQuantity { get; set; }
		public int OrderId { get; set; }
		public string OrderReference { get; set; }
		public int PositionNumber { get; set; }
		public int Status { get; set; }
		public int Type { get; set; }
		public decimal UnitPriceBasis { get; set; }
		public decimal LineItemAmount { get; set; }

		public OrderChangeItemEntity() { }
		public OrderChangeItemEntity(DataRow dataRow)
		{
			ActionTime = (dataRow["ActionTime"] is System.DBNull) ? (DateTime?)null : Convert.ToDateTime(dataRow["ActionTime"]);
			ActionUserId = (dataRow["ActionUserId"] is System.DBNull) ? (int?)null : Convert.ToInt32(dataRow["ActionUserId"]);
			ActionUsername = (dataRow["ActionUsername"] == System.DBNull.Value) ? null : Convert.ToString(dataRow["ActionUsername"]);
			CreationTime = Convert.ToDateTime(dataRow["CreationTime"]);
			CurrentItemPriceCalculationNet = Convert.ToDecimal(dataRow["CurrentItemPriceCalculationNet"]);
			CustomerItemNumber = (dataRow["CustomerItemNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerItemNumber"]);
			DesiredDate = Convert.ToDateTime(dataRow["DesiredDate"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			ItemDescription = (dataRow["ItemDescription"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ItemDescription"]);
			ItemNumber = (dataRow["ItemNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ItemNumber"]);
			MeasureUnitQualifier = Convert.ToString(dataRow["MeasureUnitQualifier"]);
			Notes = (dataRow["Notes"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Notes"]);
			OrderChangeId = Convert.ToInt32(dataRow["OrderChangeId"]);
			OrderedQuantity = Convert.ToDecimal(dataRow["OrderedQuantity"] is System.DBNull ? 0 : dataRow["OrderedQuantity"]);
			OrderId = Convert.ToInt32(dataRow["OrderId"]);
			OrderReference = Convert.ToString(dataRow["OrderReference"]);
			PositionNumber = Convert.ToInt32(dataRow["PositionNumber"]);
			Status = Convert.ToInt32(dataRow["Status"]);
			Type = Convert.ToInt32(dataRow["Type"]);
			UnitPriceBasis = Convert.ToDecimal(dataRow["UnitPriceBasis"] is System.DBNull ? 0 : dataRow["UnitPriceBasis"]);
			LineItemAmount = Convert.ToDecimal(dataRow["LineItemAmount"] is System.DBNull ? 0 : dataRow["LineItemAmount"]);
		}
	}
}

