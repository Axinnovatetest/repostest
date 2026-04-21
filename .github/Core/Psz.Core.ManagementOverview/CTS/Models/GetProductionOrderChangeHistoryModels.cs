using Infrastructure.Data.Entities.Joins.MGO;
using Psz.Core.Common.Helpers;
using System;

namespace Psz.Core.ManagementOverview.CTS.Models
{
	public class GetProductionOrderChangeHistoryRequestModel
	{
		public DateTime DateFrom { get; set; }
		public DateTime DateTo { get; set; }
	}
	public class GetProductionOrderChangeHistoryResponseModel
	{
		public int ProductionOrderWarehouseId { get; set; }
		public decimal ProductionOrderCount_DoneIn { get; set; }
		public decimal ProductionOrderCount_CancelledIn { get; set; }
		public decimal ProductionOrderCount_OpenIn { get; set; }
		public decimal ProductionOrderCount_DoneOut { get; set; }
		public decimal ProductionOrderCount_CancelledOut { get; set; }
		public decimal ProductionOrderCount_OpenOut { get; set; }
		public GetProductionOrderChangeHistoryResponseModel()
		{
		}
	}
	public class GetProductionOrderChangeHistoryChartRequestModel
	{
		public int ProductionOrderWarehouseId { get; set; }
		public bool InFrozenZone { get; set; }
		public bool OutFrozenZone { get; set; }
	}
	public class GetProductionOrderChangeHistoryChartResponseModel
	{
		public int ProductionOrderWarehouseId { get; set; }
		public int ProductionOrderCountIn { get; set; }
		public int ProductionOrderCountOut { get; set; }
		public decimal ProductionOrderTimeIn { get; set; }
		public decimal ProductionOrderTimeOut { get; set; }
		public int ChangeDateYear { get; set; }
		public int ChangeDateWeek { get; set; }
		public GetProductionOrderChangeHistoryChartResponseModel(ProductionOrderChangeHistoryWarehouseYearWeekFullEntity x)
		{
			if(x is null)
				return;

			// -
			ProductionOrderWarehouseId = x.ProductionOrderWarehouseId ?? 0;
			ProductionOrderCountIn = x.ProductionOrderCountIn ?? 0;
			ProductionOrderCountOut = x.ProductionOrderCountOut ?? 0;
			ProductionOrderTimeIn = x.ProductionOrderTimeIn ?? 0;
			ProductionOrderTimeOut = x.ProductionOrderTimeOut ?? 0;
			ChangeDateYear = x.ChangeDateYear ?? 0;
			ChangeDateWeek = x.ChangeDateWeek ?? 0;
		}		
	}
	public class FaChangesWeekYearHoursLeftResponseModel
	{
		public int? AffectedWeek { get; set; }
		public int? AffectedYear { get; set; }
		public int? Lager { get; set; }
		public decimal? HoursLeft { get; set; }
		public FaChangesWeekYearHoursLeftResponseModel(FaChangesWeekYearHoursLeftEntity x)
		{
			if(x is null)
				return;

			// -
			AffectedWeek = x.AffectedWeek ?? 0;
			AffectedYear = x.AffectedYear ?? 0;
			HoursLeft = MathHelper.RoundDecimal(x.HoursLeft ?? 0 , 2);
			Lager = x.Lager ?? 0;
		}
	}
	public class GetProductionOrderChangeHistoryDetailRequestModel
	{
		public int ProductionOrderWarehouseId { get; set; }
		public DateTime DateFrom { get; set; }
		public DateTime DateTo { get; set; }
		public string Status { get; set; }
		public bool? InFrozenZone { get; set; }
		public bool? OutFrozenZone { get; set; }
	}
	public class GetProductionOrderChangeHistoryDetailResponseModel
	{
		public int ProductionOrderChangeId { get; set; }
		public DateTime? ChangeDate { get; set; }
		public DateTime? ConfirmedDeadline { get; set; }
		public DateTime? PreviousDeadline { get; set; }
		public int? ProductionOrderNumber { get; set; }
		public string ProductionOrderStatus { get; set; }
		public Single? ProductionOrderTime { get; set; }
		public int? ProductionOrderWarehouseId { get; set; }
		public int? FertigungId { get; set; }
		public GetProductionOrderChangeHistoryDetailResponseModel(ProductionOrderChangeHistoryEntity x)
		{
			ChangeDate = x.ChangeDate ?? new DateTime(1900, 1, 1);
			ConfirmedDeadline = x.ConfirmedDeadline ?? new DateTime(1900, 1, 1);
			PreviousDeadline = x.PreviousDeadline ?? new DateTime(1900, 1, 1);
			ProductionOrderStatus = x.ProductionOrderStatus ?? "";
			ProductionOrderTime = x.ProductionOrderTime ?? 0;
			ProductionOrderWarehouseId = x.ProductionOrderWarehouseId ?? 0;
			ProductionOrderNumber = x.ProductionOrderNumber;
			FertigungId = x.ID;
		}
	}
}
