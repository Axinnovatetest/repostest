using Infrastructure.Data.Entities.Joins.CTS;
using Psz.Core.Common.Models;
using System;

namespace Psz.Core.CustomerService.Models.InsideSalesCustomerSummary
{
	public class InsideSalesCustomerSummaryModel
	{
		public class CustomerSummaryResponseModel
		{
			public string CustomerName { get; set; }
			public string DocumentType { get; set; }
			public string DocumentNumber { get; set; }
			public int DocumentAngebotNr { get; set; }
			public string ArticleNumber { get; set; }
			public string ArticleDesignation { get; set; }
			public int OpenQuantity { get; set; }
			public int FANumber { get; set; }
			public DateTime? Date { get; set; }
			public int Week { get; set; }
			public int Year { get; set; }
			public decimal? UnitPrice { get; set; }
			public decimal? TotalPrice { get; set; }
			public int? ArticleId { get; set; }
			public int? AngeboteNR { get; set; }
			public int? CustomerId { get; set; }

			public CustomerSummaryResponseModel(CustomerSummaryEntity entity)
			{
				if(entity == null)
				{
					return;
				}
				CustomerName = entity.CustomerName;
				DocumentType = entity.DocumentType;
				DocumentNumber = entity.DocumentNumber;
				DocumentAngebotNr = entity.DocumentAngebotNr;
				ArticleNumber = entity.ArticleNumber;
				ArticleDesignation = entity.ArticleDesignation;
				OpenQuantity = entity.OpenQuantity;
				Date = entity.Date;
				FANumber = entity.FANumber;
				Week = entity.Week;
				Year = entity.Year;
				UnitPrice = entity.UnitPrice;
				TotalPrice = entity.TotalPrice;
				ArticleId = entity.ArticleId;
				AngeboteNR = entity.AngeboteNR;
				CustomerId = entity.CustomerId;
			}

		}
		public class GetCustomerSummaryRequestModel: IPaginatedRequestModel
		{
			public string InputFilter { get; set; }
			public string DocumentType { get; set; }
			public int Week { get; set; }
			public int Year { get; set; }
		}

		public class GetCustomerSummaryResponseModel: IPaginatedResponseModel<CustomerSummaryResponseModel>
		{

		}
	}
}
