using Infrastructure.Data.Entities.Joins.MGO;
using Infrastructure.Data.Entities.Tables.MGO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.ManagementOverview.ProductionWorkload.Models.Data
{
	public class WorkloadResponseModel
	{
		public DateTime LastEditTime { get; set; }
		public List<WorkloadItem> Items { get; set; }
		public class WorkloadItem
		{
			public int Year { get; set; }
			public int Week { get; set; }
			public int TotalHours { get; set; }
			public int FaCount { get; set; }
			public int FaTotalQuantity { get; set; }
			public int ArticleCount { get; set; }
			public int Index { get; set; }
			public string WeekName { get; set; }
			public decimal MaxCapacity { get; set; }
		}
	}
	public class WorkloadHistoryRequestModel
	{
		public int Year { get; set; }
		public int Week { get; set; }
		public int WarehouseId { get; set; }
		public bool? IsBacklog { get; set; } = false;
	}
	public class WorkloadHistoryResponseModel
	{
		public int Index { get; set; }
		public DateTime SyncDate { get; set; }
		public int SyncHourValue { get; set; }
		public decimal MaxCapacity { get; set; }
		public WorkloadHistoryResponseModel(ProductionWorkloadEntity x)
		{
			if(x is null)
			{
				return;
			}
			// -
			Index = 0;
			SyncDate = x.RecordSyncTime ?? DateTime.MinValue;
			SyncHourValue = x.FaTime ?? 0;
			MaxCapacity = x.WarehouseMaxCapacity ?? 0;
		}
	}
	public class WeekFaResponseModel
	{
		public WeekFaResponseModel(ProductionWorkload_WeekFa x)
		{
			if(x == null)
			{
				return;
			}
			// -
			FaId = x.FaId ;
			FaNumber = x.FaNumber ?? 0;
			FaCreationTime = x.FaCreationTime ?? DateTime.MinValue;
			FaProductionTime = x.FaProductionTime ?? DateTime.MinValue;
			ArticleId = x.ArticleId ?? 0;
			ArticleNumber = x.ArticleNumber ?? "";
			OrderId = x.OrderId ?? 0;
			OrderNumber = x.OrderNumber ?? 0;
		}

		public int FaId { get; set; }
		public int FaNumber { get; set; }
		public DateTime FaCreationTime{ get; set; }
		public DateTime FaProductionTime { get; set; }
		public int ArticleId { get; set; }
		public string ArticleNumber { get; set; }
		public int OrderId { get; set; }
		public int OrderNumber { get; set; }

	}
	public class WorkloadBacklogResponseModel
	{
		public int Year { get; set; }
		public int Week { get; set; }
		public int Index { get; set; }
	}
}
