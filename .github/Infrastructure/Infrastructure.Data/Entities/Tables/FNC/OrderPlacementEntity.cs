using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.FNC
{
	public class OrderPlacementEntity
	{
		public int Id { get; set; }
		public int OrderId { get; set; }
		public string OrderPlacedEmailMessage { get; set; }
		public string OrderPlacedEmailTitle { get; set; }
		public int? OrderPlacedReportFileId { get; set; }
		public string OrderPlacedSendingEmail { get; set; }
		public string OrderPlacedSupplierEmail { get; set; }
		public DateTime? OrderPlacedTime { get; set; }
		public string OrderPlacedUserEmail { get; set; }
		public int? OrderPlacedUserId { get; set; }
		public string OrderPlacedUserName { get; set; }
		public string OrderPlacementCCEmail { get; set; }
		public string SupplierEmail { get; set; }
		public string SupplierNummer { get; set; }

		public OrderPlacementEntity() { }

		public OrderPlacementEntity(DataRow dataRow)
		{
			Id = Convert.ToInt32(dataRow["Id"]);
			OrderId = Convert.ToInt32(dataRow["OrderId"]);
			OrderPlacedEmailMessage = (dataRow["OrderPlacedEmailMessage"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OrderPlacedEmailMessage"]);
			OrderPlacedEmailTitle = (dataRow["OrderPlacedEmailTitle"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OrderPlacedEmailTitle"]);
			OrderPlacedReportFileId = (dataRow["OrderPlacedReportFileId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["OrderPlacedReportFileId"]);
			OrderPlacedSendingEmail = (dataRow["OrderPlacedSendingEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OrderPlacedSendingEmail"]);
			OrderPlacedSupplierEmail = (dataRow["OrderPlacedSupplierEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OrderPlacedSupplierEmail"]);
			OrderPlacedTime = (dataRow["OrderPlacedTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["OrderPlacedTime"]);
			OrderPlacedUserEmail = (dataRow["OrderPlacedUserEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OrderPlacedUserEmail"]);
			OrderPlacedUserId = (dataRow["OrderPlacedUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["OrderPlacedUserId"]);
			OrderPlacedUserName = (dataRow["OrderPlacedUserName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OrderPlacedUserName"]);
			OrderPlacementCCEmail = (dataRow["OrderPlacementCCEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OrderPlacementCCEmail"]);
			SupplierEmail = (dataRow["SupplierEmail"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SupplierEmail"]);
			SupplierNummer = (dataRow["SupplierNummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SupplierNummer"]);
		}
	}
}

