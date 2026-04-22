namespace Psz.Core.Apps.EDI.Models.Order
{
	public class ConsigneeModel
	{
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

		public ConsigneeModel() { }
		public ConsigneeModel(Infrastructure.Data.Entities.Tables.PRS.OrderExtensionConsigneeEntity consigneeEntity)
		{
			ConsigneeIdentification = consigneeEntity?.PartyIdentification;
			ConsigneeIdentificationCodeListQualifier = consigneeEntity?.PartyIdentificationCodeListQualifier;
			ConsigneeDUNS = consigneeEntity?.DUNS;
			ConsigneeName = consigneeEntity?.Name;
			ConsigneeName2 = consigneeEntity?.Name2;
			ConsigneeName3 = consigneeEntity?.Name3;
			ConsigneeStreet = consigneeEntity?.Street;
			ConsigneeCity = consigneeEntity?.City;
			ConsigneePostalCode = consigneeEntity?.PostalCode;
			ConsigneeCountryName = consigneeEntity?.CountryName;
			ConsigneePurchasingDepartment = consigneeEntity?.PurchasingDepartment;
			ConsigneeUnloadingPoint = consigneeEntity?.UnloadingPoint;
			ConsigneeStorageLocation = consigneeEntity?.StorageLocation;

			// > Consignee >> Contact
			ConsigneeContactName = consigneeEntity?.ContactName;
			ConsigneeContactTelephone = consigneeEntity?.ContatTelephone;
			ConsigneeContactFax = consigneeEntity?.ContactFax;
		}
	}
}