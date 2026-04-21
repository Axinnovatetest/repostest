using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.CTS
{
	public class DLF_AnalysisEntity
	{
		public string Bezeichnung1 { get; set; }
		public string BuyerPartyName { get; set; }
		public string DocumentNumber { get; set; }
		public DateTime? LastASNDate { get; set; }
		public string LastASNNumber { get; set; }
		public int LastReceivedQuantity { get; set; }
		public decimal? PlanningQuantityCumulativeQuantity { get; set; }
		public decimal? PlanningQuantityQuantity { get; set; }
		public DateTime? PlanningQuantityRequestedShipmentDate { get; set; }
		public DateTime? ReceivingDate { get; set; }
		public int PositionNumber { get; set; }
		public int? RSDWeek { get; set; }
		public int? RSDYear { get; set; }
		public string SuppliersItemMaterialNumber { get; set; }
		public decimal TotalPrice { get; set; }
		public decimal UnitPrice { get; set; }
		public decimal AbTotalQty { get; set; }
		public string ConsigneePartyName { get; set; }

		public DLF_AnalysisEntity() { }

		public DLF_AnalysisEntity(DataRow dataRow)
		{
			Bezeichnung1 = (dataRow["Bezeichnung1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung1"]);
			BuyerPartyName = Convert.ToString(dataRow["BuyerPartyName"]);
			DocumentNumber = Convert.ToString(dataRow["DocumentNumber"]);
			LastASNDate = (dataRow["LastASNDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastASNDate"]);
			LastASNNumber = (dataRow["LastASNNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LastASNNumber"]);
			LastReceivedQuantity = Convert.ToInt32(dataRow["LastReceivedQuantity"]);
			PlanningQuantityCumulativeQuantity = (dataRow["PlanningQuantityCumulativeQuantity"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["PlanningQuantityCumulativeQuantity"]);
			PlanningQuantityQuantity = (dataRow["PlanningQuantityQuantity"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["PlanningQuantityQuantity"]);
			PlanningQuantityRequestedShipmentDate = (dataRow["PlanningQuantityRequestedShipmentDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["PlanningQuantityRequestedShipmentDate"]);
			ReceivingDate = (dataRow["ReceivingDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ReceivingDate"]);
			PositionNumber = Convert.ToInt32(dataRow["PositionNumber"]);
			RSDWeek = (dataRow["RSDWeek"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["RSDWeek"]);
			RSDYear = (dataRow["RSDYear"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["RSDYear"]);
			SuppliersItemMaterialNumber = (dataRow["SuppliersItemMaterialNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SuppliersItemMaterialNumber"]);
			TotalPrice = (dataRow["TotalPrice"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["TotalPrice"]);
			UnitPrice = (dataRow["UnitPrice"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["UnitPrice"]);
			AbTotalQty = (dataRow["AbTotalQty"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["AbTotalQty"]);
			ConsigneePartyName = (dataRow["ConsigneePartyName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ConsigneePartyName"]);
		}
		public DLF_AnalysisEntity ShallowClone()
		{
			return new DLF_AnalysisEntity
			{
				Bezeichnung1 = Bezeichnung1,
				BuyerPartyName = BuyerPartyName,
				DocumentNumber = DocumentNumber,
				LastASNDate = LastASNDate,
				LastASNNumber = LastASNNumber,
				LastReceivedQuantity = LastReceivedQuantity,
				PlanningQuantityCumulativeQuantity = PlanningQuantityCumulativeQuantity,
				PlanningQuantityQuantity = PlanningQuantityQuantity,
				PlanningQuantityRequestedShipmentDate = PlanningQuantityRequestedShipmentDate,
				ReceivingDate = ReceivingDate,
				PositionNumber = PositionNumber,
				RSDWeek = RSDWeek,
				RSDYear = RSDYear,
				SuppliersItemMaterialNumber = SuppliersItemMaterialNumber,
				TotalPrice = TotalPrice,
				UnitPrice = UnitPrice,
				ConsigneePartyName = ConsigneePartyName
			};
		}
	}

}
