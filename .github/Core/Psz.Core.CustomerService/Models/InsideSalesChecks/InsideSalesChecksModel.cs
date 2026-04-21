using Infrastructure.Services.Files.Parsing;
using Psz.Core.Common.Models;
using System;
using static Psz.Core.CustomerService.Models.InsideSalesChecks.InsideSalesChecksUpdateLogResponseModel;

namespace Psz.Core.CustomerService.Models.InsideSalesChecks
{

	public class InsideSalesChecksSearchRequestModel: IPaginatedRequestModel
	{
		public string SearchValue { get; set; }
	}

	public class InsideSalesChecksUpdateRequestModel
	{
		public int Id { get; set; }
		public Enums.InsideSalesEnums.CheckTypes CheckedType { get; set; } // - needs an Enum for the 5 checks
		public bool? CheckedTypeValue { get; set; }
		public string? Comment { get; set; }
		public bool? CheckCRPDateAdjusted { get; set; }
		public int? CheckCRPWeek { get; set; }
		public bool? CheckCRPWTCompliedOk { get; set; }
		public bool? CheckFa { get; set; }
		public bool? CheckFaAvaialable { get; set; }
		public string? CheckFaDate { get; set; }
		public bool? CheckFaDateOk { get; set; }
		public int? CheckFaWeek { get; set; }
		public string? CheckFSTDate { get; set; }
		public bool? CheckFSTKapaOk { get; set; }
		public string? CheckFSTKapaReason { get; set; }
		public int? CheckFSTKapaWeek { get; set; }
		public DateTime? CheckPRSMaterialLastDeliveryDate { get; set; }
		public string? CheckPRSMaterialMissing { get; set; }
		public bool? CheckPRSMaterialOk { get; set; }
		public bool? CheckINSAbConfirmed { get; set; }
		public bool? IsCheckedStock { get; set; }
		public string? CheckFaComments { get; set; }
	}
	public class InsideSalesChecksUpdateLogRequestModel:IPaginatedRequestModel
	{
		public string SearchTerms { get; set; }
	}
	public class InsideSalesChecksUpdateLogResponseModel:IPaginatedResponseModel<UpdateLogItem>
	{
		public class UpdateLogItem
		{
			public int Id { get; set; }
			public int? NewRecordCount { get; set; }
			public DateTime? RecordTime { get; set; }
			public UpdateLogItem(Infrastructure.Data.Entities.Tables.CTS.InsideSalesChecksLogsEntity entity)
			{
				if(entity is null)
				{
					return;
				}
				// 
				Id = entity.Id;
				NewRecordCount = entity.NewRecordCount;
				RecordTime = entity.RecordTime;
			}
		}
	}
	
	public class UpdateCommentRequestModel
	{
		public string Comment { get; set; }
		public int Id { get; set; }
		public Enums.InsideSalesEnums.CheckTypes Type { get; set; }
	}


	public class InsideSalesResponseModel
	{
		public int Id { get; set; }
		public string CustomerName { get; set; }
		public string CustomerOrderNumber { get; set; }
		public string ArticleNumber { get; set; }
		public int? OrderNumber { get; set; }
		public int? OrderId { get; set; }
		public int? ArticleId { get; set; }
		public bool? CheckStock { get; set; }
		public bool? CheckFST { get; set; }
		public bool? CheckPRS { get; set; }
		public bool? CheckCRP { get; set; }
		public bool? CheckINS { get; set; }
		public string CheckStockComments { get; set; }
		public string CheckFSTComments { get; set; }
		public string CheckPRSComments { get; set; }
		public string CheckCRPComments { get; set; }
		public string CheckINSComments { get; set; }
		public string CheckFaComments { get; set; }

		public decimal OpenQuantity { get; set; }
		public DateTime? DeliveryDate { get; set; }
		public bool? CheckCRPDateAdjusted { get; set; }
		public int? CheckCRPWeek { get; set; }
		public bool? CheckCRPWTCompliedOk { get; set; }
		public bool? CheckFa { get; set; }
		public bool? CheckFaAvaialable { get; set; }
		public DateTime? CheckFaDate { get; set; }
		public bool? CheckFaDateOk { get; set; }
		public int? CheckFaWeek { get; set; }
		public DateTime? CheckFSTDate { get; set; }
		public bool? CheckFSTKapaOk { get; set; }
		public string CheckFSTKapaReason { get; set; }
		public int CheckFSTKapaWeek { get; set; }
		public DateTime? CheckPRSMaterialLastDeliveryDate { get; set; }
		public DateTime? CheckStockDate { get; set; }
		public DateTime? CheckPRSDate { get; set; }
		public DateTime? CheckCRPDate { get; set; }
		public DateTime? CheckINSDate { get; set; }
		public string CheckPRSMaterialMissing { get; set; }
		public string CheckStockUserName { get; set; }
		public string CheckFSTUserName { get; set; }
		public string CheckPRSUserName { get; set; }
		public string CheckCRPUserName { get; set; }
		public string CheckINSUserName { get; set; }
		public string CheckFaUserName { get; set; }
		public bool? CheckPRSMaterialOk { get; set; }
		public bool? CheckINSAbConfirmed { get; set; }
		public string? Department { get; set; }
		public string? Status { get; set; }
		public bool? IsCheckedStock { get; set; }

	}

	public class SearchInsideSaleResponseModel: IPaginatedResponseModel<InsideSalesResponseModel>
	{
	}
}
