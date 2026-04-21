using Psz.Core.Common.Models;
using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.EDI.Models.OrderError
{
	public class GetRequestModel: IPaginatedRequestModel
	{
		public int CustomerId { get; set; }
		public string? SearchText { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
	}
	public class OrderErrorModel
	{
		public int Id { get; set; }
		public string Error { get; set; }
		public string FilePath { get; set; }
		public string ClientName { get; set; }
		public bool Validated { get; set; }
		public int ClientId { get; set; }

		public int? ValidationUserId { get; set; }
		public string ValidationUser { get; set; }
		public DateTime? ValidationTime { get; set; }

		public string CustomerNumber { get; set; }
		public string OrderId { get; set; }
		public string OrderNumber { get; set; }
		public DateTime CreationTime { get; set; }
		public int? AdressCustomerNumber { get; set; }
	}
}
