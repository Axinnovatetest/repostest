using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities
{
	public class InventoryStatsEntity
	{
		public int? HqLastRejectionId { get; set; }
		public string HqLastRejectionName { get; set; }
		public string HqLastRejectionNotes { get; set; }
		public DateTime? HqLastRejectionTime { get; set; }
		public string HqNotesHL { get; set; }
		public string HqNotesPL { get; set; }
		public int? HqValidatorId { get; set; }
		public string HqValidatorName { get; set; }
		public DateTime? HqValidatorValidateTime { get; set; }
		public int Id { get; set; }
		public int? OpenFaCount { get; set; }
		public int? RohSurplusCount { get; set; }
		public int? RohWihtoutNeedCount { get; set; }
		public int? StartedFaCount { get; set; }
		public DateTime? StartTime { get; set; }
		public string StartUsername { get; set; }
		public DateTime? StopTime { get; set; }
		public string StopUsername { get; set; }
		public int? WarehouseId { get; set; }
		public string WarehouseNotesHL { get; set; }
		public string WarehouseNotesPL { get; set; }
		public int? WarehouseValidatorId { get; set; }
		public string WarehouseValidatorName { get; set; }
		public DateTime? WarehouseValidatorValidateTime { get; set; }

		public InventoryStatsEntity() { }

		public InventoryStatsEntity(DataRow dataRow)
		{
			HqLastRejectionId = (dataRow["HqLastRejectionId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["HqLastRejectionId"]);
			HqLastRejectionName = (dataRow["HqLastRejectionName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["HqLastRejectionName"]);
			HqLastRejectionNotes = (dataRow["HqLastRejectionNotes"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["HqLastRejectionNotes"]);
			HqLastRejectionTime = (dataRow["HqLastRejectionTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["HqLastRejectionTime"]);
			HqNotesHL = (dataRow["HqNotesHL"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["HqNotesHL"]);
			HqNotesPL = (dataRow["HqNotesPL"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["HqNotesPL"]);
			HqValidatorId = (dataRow["HqValidatorId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["HqValidatorId"]);
			HqValidatorName = (dataRow["HqValidatorName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["HqValidatorName"]);
			HqValidatorValidateTime = (dataRow["HqValidatorValidateTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["HqValidatorValidateTime"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			OpenFaCount = (dataRow["OpenFaCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["OpenFaCount"]);
			RohSurplusCount = (dataRow["RohSurplusCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["RohSurplusCount"]);
			RohWihtoutNeedCount = (dataRow["RohWihtoutNeedCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["RohWihtoutNeedCount"]);
			StartedFaCount = (dataRow["StartedFaCount"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["StartedFaCount"]);
			StartTime = (dataRow["StartTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["StartTime"]);
			StartUsername = (dataRow["StartUsername"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["StartUsername"]);
			StopTime = (dataRow["StopTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["StopTime"]);
			StopUsername = (dataRow["StopUsername"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["StopUsername"]);
			WarehouseId = (dataRow["WarehouseId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["WarehouseId"]);
			WarehouseNotesHL = (dataRow["WarehouseNotesHL"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["WarehouseNotesHL"]);
			WarehouseNotesPL = (dataRow["WarehouseNotesPL"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["WarehouseNotesPL"]);
			WarehouseValidatorId = (dataRow["WarehouseValidatorId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["WarehouseValidatorId"]);
			WarehouseValidatorName = (dataRow["WarehouseValidatorName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["WarehouseValidatorName"]);
			WarehouseValidatorValidateTime = (dataRow["WarehouseValidatorValidateTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["WarehouseValidatorValidateTime"]);
		}

		public InventoryStatsEntity ShallowClone()
		{
			return new InventoryStatsEntity
			{
				HqLastRejectionId = HqLastRejectionId,
				HqLastRejectionName = HqLastRejectionName,
				HqLastRejectionNotes = HqLastRejectionNotes,
				HqLastRejectionTime = HqLastRejectionTime,
				HqNotesHL = HqNotesHL,
				HqNotesPL = HqNotesPL,
				HqValidatorId = HqValidatorId,
				HqValidatorName = HqValidatorName,
				HqValidatorValidateTime = HqValidatorValidateTime,
				Id = Id,
				OpenFaCount = OpenFaCount,
				RohSurplusCount = RohSurplusCount,
				RohWihtoutNeedCount = RohWihtoutNeedCount,
				StartedFaCount = StartedFaCount,
				StartTime = StartTime,
				StartUsername = StartUsername,
				StopTime = StopTime,
				StopUsername = StopUsername,
				WarehouseId = WarehouseId,
				WarehouseNotesHL = WarehouseNotesHL,
				WarehouseNotesPL = WarehouseNotesPL,
				WarehouseValidatorId = WarehouseValidatorId,
				WarehouseValidatorName = WarehouseValidatorName,
				WarehouseValidatorValidateTime = WarehouseValidatorValidateTime
			};
		}
	}
}
