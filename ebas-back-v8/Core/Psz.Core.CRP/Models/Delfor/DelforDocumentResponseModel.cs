
namespace Psz.Core.CRP.Models.Delfor
{
	public class DelforCustomerResponseModel
	{
		public int CustomerId { get; set; }
		public int CustomerNumber { get; set; }
		public string CustomerName { get; set; }
		public string ConsigneeName { get; set; }
		public string CustomerContact { get; set; }
		public string CustomerAdress { get; set; }
		public int DocumentCount { get; set; }
	}
	public class DelforDocumentRequestModel
	{
		public bool IsManual { get; set; }
		public int CustomerId { get; set; }
	}
	public class DelforDocumentResponseModel
	{
		public string Customer { get; set; }
		public string Consignee { get; set; }
		public string DocumentNumber { get; set; }
		public string DocumentNumber_Archived { get; set; }
		public string CustomerContact { get; set; }
		public int? CustomerNumber { get; set; }
		public string CustomerAdress { get; set; }
		public int NumberOfVersions { get; set; }
		public List<KeyValuePair<int, DateTime?>> Versions { get; set; }
		public KeyValuePair<int, DateTime?> LastVersion { get; set; }

	}

	public class DeflorItemsResponseModel
	{
		public int PositionNumber { get; set; }
		public string Artikelnummer { get; set; }
		public int? ArticleNr { get; set; }
		public decimal? RecivedQuantity { get; set; }
		public decimal? ScheduledQuantity { get; set; }
		public decimal? LastReceivedQuantity { get; set; }
		public string LastDeliveryNumer { get; set; }
		public DateTime? LastDeliverydate { get; set; }
		public string kundenIndex { get; set; }
		public int ABNumber { get; set; }
		public int? Id { get; set; }
		public int? HeaderId { get; set; }
		public int? Customernumber { get; set; }
		public bool? Done { get; set; }
		public decimal? FirstLineItemPlanWithOpenQuantity { get; set; }
		public DateTime? FirstLineItemPlanWithOpenQuantityRSD { get; set; }

	}

	public class DelforVersionsRequsetModel
	{
		public string DocumentNumber { get; set; }
		public int CustomerNumber { get; set; }
	}
	public class DelforItemsRequsetModel: DelforVersionsRequsetModel
	{
		public int Version { get; set; }
	}

	public class DelforItemsResponseModel: DelforVersionsRequsetModel
	{
		public int Version { get; set; }
		public DateTime? CreatedOn { get; set; }
		public DateTime? ValidFrom { get; set; }
		public DateTime? ValidTill { get; set; }
		public int PositionsCount { get; set; }
		public int HeaderId { get; set; }
	}

	public class SearchManualDelforRequestModel
	{
		public string Customer { get; set; }
		public string DocumentNummer { get; set; }
	}
}
