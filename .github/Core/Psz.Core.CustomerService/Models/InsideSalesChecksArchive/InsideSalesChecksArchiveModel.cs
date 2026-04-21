using Psz.Core.Common.Models;
using System;


namespace Psz.Core.CustomerService.Models.InsideSalesChecksArchive
{
	public class InsideSalesArchiveResponseModel
	{
		public int Id { get; set; }
		public int? OrderId { get; set; }
		public string CustomerOrderNumber { get; set; }
		public string CustomerName { get; set; }
		public string ArticleNumber { get; set; }
		public int? OrderNumber { get; set; }
		public int? ArticleId { get; set; }
		public bool? CheckCRP { get; set; }
		public string CheckCRPComments { get; set; }
		public DateTime? CheckCRPDate { get; set; }
		public string CheckCRPUserName { get; set; }
		public bool? CheckFST { get; set; }
		public string CheckFSTComments { get; set; }
		public DateTime? CheckFSTDate { get; set; }
		public string CheckFSTUserName { get; set; }
		public bool? CheckINS { get; set; }
		public string CheckINSComments { get; set; }
		public string CheckINSUserName { get; set; }
		public DateTime? CheckINSDate { get; set; }
		public bool? CheckPRS { get; set; }
		public string CheckPRSComments { get; set; }
		public DateTime? CheckPRSDate { get; set; }
		public string CheckPRSUserName { get; set; }
		public bool? CheckStock { get; set; }
		public string CheckStockComments { get; set; }
		public DateTime? CheckStockDate { get; set; }
		public string CheckStockUserName { get; set; }
		public decimal OpenQuantity { get; set; }
		public DateTime? DeliveryDate { get; set; }
		public bool? CheckFa { get; set; }
		public string CheckFaComments { get; set; }
		public DateTime? CheckFaDate { get; set; }
		public int? CheckFaUserId { get; set; }
		public string CheckFaUserName { get; set; }


	}

	public class GetInsideSalesHistoryRequestModel: IPaginatedRequestModel
	{
		public string SearchValue { get; set; }
	}

	public class GetInsideSalesHistoryResponseModel: IPaginatedResponseModel<InsideSalesArchiveResponseModel>
	{
	}

}
