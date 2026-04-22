using Psz.Core.Common.Models;
using System;
using static Psz.Core.CustomerService.Models.Delfor.AnalysisResponseModel;

namespace Psz.Core.CustomerService.Models.Delfor
{
	public class AnalysisRequestModel: IPaginatedRequestModel
	{
		public bool? IsManual { get; set; } = null;
		public bool? OnlyLastVersion { get; set; } = null;
		public bool? OnlyOpen { get; set; } = true;
		public bool? SplitCustomers { get; set; } = false;
		public string CustomerNumber { get; set; }
		public string DocumentNumber { get; set; }
		public DateOnly? DateFrom { get; set; }
		public DateOnly? DateTo { get; set; }
	}
	public class AnalysisResponseModel: IPaginatedResponseModel<AnalysisItem>
	{
		public class AnalysisItem
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
			public string Lieferadresse { get; set; }
			public AnalysisItem(Infrastructure.Data.Entities.Joins.CTS.DLF_AnalysisEntity entity)
			{
				if(entity == null)
				{
					return;
				}

				Bezeichnung1 = entity.Bezeichnung1;
				BuyerPartyName = entity.BuyerPartyName;
				DocumentNumber = entity.DocumentNumber;
				LastASNDate = entity.LastASNDate;
				LastASNNumber = entity.LastASNNumber;
				LastReceivedQuantity = entity.LastReceivedQuantity;
				PlanningQuantityCumulativeQuantity = entity.PlanningQuantityCumulativeQuantity;
				PlanningQuantityQuantity = entity.PlanningQuantityQuantity;
				PlanningQuantityRequestedShipmentDate = entity.PlanningQuantityRequestedShipmentDate;
				ReceivingDate = entity.ReceivingDate;
				PositionNumber = entity.PositionNumber;
				RSDWeek = entity.RSDWeek;
				RSDYear = entity.RSDYear;
				SuppliersItemMaterialNumber = entity.SuppliersItemMaterialNumber;
				TotalPrice = entity.TotalPrice;
				UnitPrice = entity.UnitPrice;
				AbTotalQty = entity.AbTotalQty;
				ConsigneePartyName = entity.ConsigneePartyName;
				Lieferadresse = entity.Lieferadresse;
			}
		}
	}
}
