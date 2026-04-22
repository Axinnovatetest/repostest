using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.MTM
{
	public class ArticleProductionEntity
	{
		public int? Angebot_Artikel_Nr { get; set; }
		public int? Angebot_nr { get; set; }
		public int? Artikel_Nr { get; set; }
		public string Bemerkung { get; set; }
		public DateTime? CreationDate { get; set; }
		public bool? Erstmuster { get; set; }
		public int? Fertigungsnummer { get; set; }
		public int Id { get; set; }
		public int? Lagerort_id { get; set; }
		public DateTime? ProductionDeadlineDate { get; set; }
		public decimal? Quantity { get; set; }
		public double WSAmount { get; set; }
		public string WSComment { get; set; }
		public int WSCountryId { get; set; }
		public DateTime WSCreationDate { get; set; }
		public int WSCreationUserId { get; set; }
		public int WSDepartmentId { get; set; }
		public string WSFromToolInsert { get; set; }
		public string WSFromToolInsert2 { get; set; }
		public int WSHallId { get; set; }
		public DateTime? WSLastEditDate { get; set; }
		public int? WSLastEditUserId { get; set; }
		public int WSLotSizeSTD { get; set; }
		public int? WSOperationDescriptionId { get; set; }
		public int WSOperationNumber { get; set; }
		public double WSOperationTimeSeconds { get; set; }
		public double WSOperationTimeValueAdding { get; set; }
		public bool? WSOperationValueAdding { get; set; }
		public int WSPredecessorOperation { get; set; }
		public int WSPredecessorSubOperation { get; set; }
		public double WSRelationOperationTime { get; set; }
		public double WSSetupTimeMinutes { get; set; }
		public double WSStandardOccupancy { get; set; }
		public int WSStandardOperationId { get; set; }
		public int WSSubOperationNumber { get; set; }
		public double WSTotalTimeOperation { get; set; }
		public int WSWorkAreaId { get; set; }
		public int WSWorkScheduleId { get; set; }
		public int? WSWorkStationMachineId { get; set; }

		public ArticleProductionEntity() { }

		public ArticleProductionEntity(DataRow dataRow)
		{
			Angebot_Artikel_Nr = (dataRow["Angebot_Artikel_Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Angebot_Artikel_Nr"]);
			Angebot_nr = (dataRow["Angebot_nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Angebot_nr"]);
			Artikel_Nr = (dataRow["Artikel_Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel_Nr"]);
			Bemerkung = (dataRow["Bemerkung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung"]);
			CreationDate = (dataRow["CreationDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationDate"]);
			Erstmuster = (dataRow["Erstmuster"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Erstmuster"]);
			Fertigungsnummer = (dataRow["Fertigungsnummer"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Fertigungsnummer"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			Lagerort_id = (dataRow["Lagerort_id"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Lagerort_id"]);
			ProductionDeadlineDate = (dataRow["ProductionDeadlineDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["ProductionDeadlineDate"]);
			Quantity = (dataRow["Quantity"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Quantity"]);
			WSAmount = float.Parse(Convert.ToString(dataRow["WSAmount"]));
			WSComment = (dataRow["WSComment"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["WSComment"]);
			WSCountryId = Convert.ToInt32(dataRow["WSCountryId"]);
			WSCreationDate = Convert.ToDateTime(dataRow["WSCreationDate"]);
			WSCreationUserId = Convert.ToInt32(dataRow["WSCreationUserId"]);
			WSDepartmentId = Convert.ToInt32(dataRow["WSDepartmentId"]);
			WSFromToolInsert = (dataRow["WSFromToolInsert"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["WSFromToolInsert"]);
			WSFromToolInsert2 = (dataRow["WSFromToolInsert2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["WSFromToolInsert2"]);
			WSHallId = Convert.ToInt32(dataRow["WSHallId"]);
			WSLastEditDate = (dataRow["WSLastEditDate"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["WSLastEditDate"]);
			WSLastEditUserId = (dataRow["WSLastEditUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["WSLastEditUserId"]);
			WSLotSizeSTD = Convert.ToInt32(dataRow["WSLotSizeSTD"]);
			WSOperationDescriptionId = (dataRow["WSOperationDescriptionId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["WSOperationDescriptionId"]);
			WSOperationNumber = Convert.ToInt32(dataRow["WSOperationNumber"]);
			WSOperationTimeSeconds = float.Parse(Convert.ToString(dataRow["WSOperationTimeSeconds"]));
			WSOperationTimeValueAdding = float.Parse(Convert.ToString(dataRow["WSOperationTimeValueAdding"]));
			WSOperationValueAdding = (dataRow["WSOperationValueAdding"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["WSOperationValueAdding"]);
			WSPredecessorOperation = Convert.ToInt32(dataRow["WSPredecessorOperation"]);
			WSPredecessorSubOperation = Convert.ToInt32(dataRow["WSPredecessorSubOperation"]);
			WSRelationOperationTime = float.Parse(Convert.ToString(dataRow["WSRelationOperationTime"]));
			WSSetupTimeMinutes = float.Parse(Convert.ToString(dataRow["WSSetupTimeMinutes"]));
			WSStandardOccupancy = float.Parse(Convert.ToString(dataRow["WSStandardOccupancy"]));
			WSStandardOperationId = Convert.ToInt32(dataRow["WSStandardOperationId"]);
			WSSubOperationNumber = Convert.ToInt32(dataRow["WSSubOperationNumber"]);
			WSTotalTimeOperation = float.Parse(Convert.ToString(dataRow["WSTotalTimeOperation"]));
			WSWorkAreaId = Convert.ToInt32(dataRow["WSWorkAreaId"]);
			WSWorkScheduleId = Convert.ToInt32(dataRow["WSWorkScheduleId"]);
			WSWorkStationMachineId = (dataRow["WSWorkStationMachineId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["WSWorkStationMachineId"]);
		}
	}
}

