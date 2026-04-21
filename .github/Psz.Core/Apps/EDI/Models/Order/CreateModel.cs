using System.Collections.Generic;

namespace Psz.Core.Apps.EDI.Models.Order
{
	public class CreateModel
	{
		public bool IsManualCreation { get; set; } = false;
		public int ManualCreationCustomerId { get; set; }

		public string DocumentNumber { get; set; }
		public string DocumentName { get; set; }
		public string FreeText { get; set; }

		public string SenderId { get; set; }
		public string RecipientId { get; set; }

		public string SenderDuns { get; set; }

		// > Buyer
		public string BuyerDuns { get; set; }
		public string BuyerPartyIdentification { get; set; }
		public string BuyerPartyIdentificationCodeListQualifier { get; set; }
		public string BuyerName { get; set; }
		public string BuyerName2 { get; set; }
		public string BuyerName3 { get; set; }
		public string BuyerStreet { get; set; }
		public string BuyerPostalCode { get; set; }
		public string BuyerCity { get; set; }
		public string BuyerCountryName { get; set; }
		public string BuyerPurchasingDepartment { get; set; }
		// > Buyer >> Contact
		public string BuyerContactName { get; set; }
		public string BuyerContactTelephone { get; set; }
		public string BuyerContactFax { get; set; }

		// > Consignee
		public string ConsigneeIdentification { get; set; }
		public string ConsigneeIdentificationCodeListQualifier { get; set; }
		public string ConsigneeDUNS { get; set; }
		public string ConsigneeName { get; set; }
		public string ConsigneeName2 { get; set; }
		public string ConsigneeName3 { get; set; }
		public string ConsigneeStreet { get; set; }
		public string ConsigneeCity { get; set; }
		public string ConsigneePostalCode { get; set; }
		public string ConsigneeCountryName { get; set; }
		public string ConsigneePurchasingDepartment { get; set; }
		public string ConsigneeUnloadingPoint { get; set; }
		public string ConsigneeStorageLocation { get; set; }

		// > Consignee >> Contact
		public string ConsigneeContactName { get; set; }
		public string ConsigneeContactTelephone { get; set; }
		public string ConsigneeContactFax { get; set; }

		public List<Element.NotCalculatedElementModel> Elements { get; set; } = new List<Element.NotCalculatedElementModel>();
	}
}
