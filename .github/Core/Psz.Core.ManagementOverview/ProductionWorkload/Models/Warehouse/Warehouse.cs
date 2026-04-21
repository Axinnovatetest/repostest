using System;
using System.Collections.Generic;

namespace Psz.Core.ManagementOverview.ProductionWorkload.Models.Warehouse
{
	public class GetWarehouseMaxCapacityRequestModel
	{
		public int WarehouseId { get; set; }
		public int Week { get; set; }
	}
	public class GetWarehouseMaxCapacityResponseModel
	{
		public int WarehouseId { get; set; }
		public string Warehouse { get; set; }
		public int Week { get; set; }
		public int Id { get; set; }
		public int MaxCapacity { get; set; }
		public GetWarehouseMaxCapacityResponseModel(Infrastructure.Data.Entities.Tables.MGO.WarehouseWeekMaxCapacityEntity capacityEntity)
		{
			if(capacityEntity == null)
				return;

			//  -
			WarehouseId = capacityEntity.WarehouseId ?? 0;
			Warehouse = capacityEntity.Warehouse;
			Week = capacityEntity.Week ?? 0;
			Id = capacityEntity.Id;
			MaxCapacity = capacityEntity.WeekMaxCapacity ?? 0;
		}
	}
	public class AddWarehouseMaxCapacityRequestModel
	{
		public int Id { get; set; }
		public int WarehouseId { get; set; }
		public int Week { get; set; }
		public int MaxCapacity { get; set; }
		public Infrastructure.Data.Entities.Tables.MGO.WarehouseWeekMaxCapacityEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.MGO.WarehouseWeekMaxCapacityEntity
			{
				Id = Id,
				Week = Week,
				WeekMaxCapacity = MaxCapacity,
				WarehouseId = WarehouseId
			};
		}
		public Infrastructure.Data.Entities.Tables.MGO.WarehouseWeekMaxCapacityEntity ToEntity(
			Infrastructure.Data.Entities.Tables.BSD.LagerorteEntity entity)
		{
			return new Infrastructure.Data.Entities.Tables.MGO.WarehouseWeekMaxCapacityEntity
			{
				Id = Id,	
				Week = Week,
				WeekMaxCapacity = MaxCapacity,
				WarehouseId = WarehouseId,
				Warehouse = entity?.Lagerort
			};
		}
	}
}
