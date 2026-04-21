using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class LineItemEntity
	{
		public string BuyersInternalProductGroupCode { get; set; }
		public DateTime? CallOffDateTime { get; set; }
		public int? CallOffNumber { get; set; }
		public decimal? CumulativeReceivedQuantity { get; set; }
		public decimal? CumulativeScheduledQuantity { get; set; }
		public string CustomersItemMaterialNumber { get; set; }
		public string DocumentNumber { get; set; }
		public string DrawingRevisionNumber { get; set; }
		public long HeaderId { get; set; }
		public int? HeaderPreviousVersion { get; set; }
		public int? HeaderVersion { get; set; }
		public long Id { get; set; }
		public DateTime? LastASNDate { get; set; }
		public DateTime? LastASNDeliveryDate { get; set; }
		public string LastASNNumber { get; set; }
		public decimal? LastReceivedQuantity { get; set; }
		public DateTime? MaterialAuthorizationDate { get; set; }
		public decimal? MaterialAuthorizationQuantity { get; set; }
		public DateTime? PlanningHorizionEnd { get; set; }
		public DateTime? PlanningHorizionStart { get; set; }
		public int PositionNumber { get; set; }
		public DateTime? PreviousCallOffDate { get; set; }
		public int? PreviousCallOffNumber { get; set; }
		public DateTime? ProductionAuthorizationDateTime { get; set; }
		public decimal? ProductionAuthorizationQuantity { get; set; }
		public string SuppliersItemMaterialNumber { get; set; }
		// - 2023-03-23
		public string UnloadingPoint { get; set; }
		public string StorageLocation { get; set; }
		public int? ArticleId { get; set; }

		public LineItemEntity() { }

		public LineItemEntity(DataRow dataRow)
		{
			BuyersInternalProductGroupCode = (dataRow["BuyersInternalProductGroupCode"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["BuyersInternalProductGroupCode"]);
			CallOffDateTime = (dataRow["CallOffDateTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CallOffDateTime"]);
			CallOffNumber = (dataRow["CallOffNumber"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CallOffNumber"]);
			CumulativeReceivedQuantity = (dataRow["CumulativeReceivedQuantity"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["CumulativeReceivedQuantity"]);
			CumulativeScheduledQuantity = (dataRow["CumulativeScheduledQuantity"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["CumulativeScheduledQuantity"]);
			CustomersItemMaterialNumber = (dataRow["CustomersItemMaterialNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomersItemMaterialNumber"]);
			DocumentNumber = Convert.ToString(dataRow["DocumentNumber"]);
			DrawingRevisionNumber = (dataRow["DrawingRevisionNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["DrawingRevisionNumber"]);
			HeaderId = Convert.ToInt64(dataRow["HeaderId"]);
			HeaderPreviousVersion = (dataRow["HeaderPreviousVersion"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["HeaderPreviousVersion"]);
			HeaderVersion = (dataRow["HeaderVersion"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["HeaderVersion"]);
			Id = Convert.ToInt64(dataRow["Id"]);
			LastASNDate = (dataRow["LastASNDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastASNDate"]);
			LastASNDeliveryDate = (dataRow["LastASNDeliveryDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastASNDeliveryDate"]);
			LastASNNumber = (dataRow["LastASNNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LastASNNumber"]);
			LastReceivedQuantity = (dataRow["LastReceivedQuantity"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["LastReceivedQuantity"]);
			MaterialAuthorizationDate = (dataRow["MaterialAuthorizationDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["MaterialAuthorizationDate"]);
			MaterialAuthorizationQuantity = (dataRow["MaterialAuthorizationQuantity"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["MaterialAuthorizationQuantity"]);
			PlanningHorizionEnd = (dataRow["PlanningHorizionEnd"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["PlanningHorizionEnd"]);
			PlanningHorizionStart = (dataRow["PlanningHorizionStart"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["PlanningHorizionStart"]);
			PositionNumber = Convert.ToInt32(dataRow["PositionNumber"]);
			PreviousCallOffDate = (dataRow["PreviousCallOffDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["PreviousCallOffDate"]);
			PreviousCallOffNumber = (dataRow["PreviousCallOffNumber"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["PreviousCallOffNumber"]);
			ProductionAuthorizationDateTime = (dataRow["ProductionAuthorizationDateTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ProductionAuthorizationDateTime"]);
			ProductionAuthorizationQuantity = (dataRow["ProductionAuthorizationQuantity"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ProductionAuthorizationQuantity"]);
			ArticleId = (dataRow["ArticleId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ArticleId"]);
			SuppliersItemMaterialNumber = (dataRow["SuppliersItemMaterialNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SuppliersItemMaterialNumber"]);
			UnloadingPoint = (dataRow["UnloadingPoint"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["UnloadingPoint"]);
			StorageLocation = (dataRow["StorageLocation"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["StorageLocation"]);
		}

		public LineItemEntity ShallowClone()
		{
			return new LineItemEntity
			{
				BuyersInternalProductGroupCode = BuyersInternalProductGroupCode,
				CallOffDateTime = CallOffDateTime,
				CallOffNumber = CallOffNumber,
				CumulativeReceivedQuantity = CumulativeReceivedQuantity,
				CumulativeScheduledQuantity = CumulativeScheduledQuantity,
				CustomersItemMaterialNumber = CustomersItemMaterialNumber,
				DocumentNumber = DocumentNumber,
				DrawingRevisionNumber = DrawingRevisionNumber,
				HeaderId = HeaderId,
				HeaderPreviousVersion = HeaderPreviousVersion,
				HeaderVersion = HeaderVersion,
				Id = Id,
				LastASNDate = LastASNDate,
				LastASNDeliveryDate = LastASNDeliveryDate,
				LastASNNumber = LastASNNumber,
				LastReceivedQuantity = LastReceivedQuantity,
				MaterialAuthorizationDate = MaterialAuthorizationDate,
				MaterialAuthorizationQuantity = MaterialAuthorizationQuantity,
				PlanningHorizionEnd = PlanningHorizionEnd,
				PlanningHorizionStart = PlanningHorizionStart,
				PositionNumber = PositionNumber,
				PreviousCallOffDate = PreviousCallOffDate,
				PreviousCallOffNumber = PreviousCallOffNumber,
				ProductionAuthorizationDateTime = ProductionAuthorizationDateTime,
				ProductionAuthorizationQuantity = ProductionAuthorizationQuantity,
				SuppliersItemMaterialNumber = SuppliersItemMaterialNumber,
				UnloadingPoint = UnloadingPoint,
				StorageLocation = StorageLocation,
				ArticleId = ArticleId
			};
		}
	}
}

