namespace Psz.Core.Logistics.Models.InverntoryStockModels
{
	public class InventoryFaStatsModel
	{
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

		public InventoryFaStatsModel(Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.InventoryStatsEntity entity)
		{
			if(entity is null)
			{
				return;
			}

			HqLastRejectionNotes = entity.HqLastRejectionNotes;
			HqLastRejectionTime = entity.HqLastRejectionTime;
			HqNotesHL = entity.HqNotesHL;
			HqNotesPL = entity.HqNotesPL;
			HqValidatorId = entity.HqValidatorId;
			HqValidatorName = entity.HqValidatorName;
			HqValidatorValidateTime = entity.HqValidatorValidateTime;
			Id = entity.Id;
			OpenFaCount = entity.OpenFaCount;
			RohSurplusCount = entity.RohSurplusCount;
			RohWihtoutNeedCount = entity.RohWihtoutNeedCount;
			StartedFaCount = entity.StartedFaCount;
			StartTime = entity.StartTime;
			StartUsername = entity.StartUsername;
			StopTime = entity.StopTime;
			StopUsername = entity.StopUsername;
			WarehouseId = entity.WarehouseId;
			WarehouseNotesHL = entity.WarehouseNotesHL;
			WarehouseNotesPL = entity.WarehouseNotesPL;
			WarehouseValidatorId = entity.WarehouseValidatorId;
			WarehouseValidatorName = entity.WarehouseValidatorName;
			WarehouseValidatorValidateTime = entity.WarehouseValidatorValidateTime;
		}
	}
}
