using System;
using System.Collections.Generic;

namespace Psz.Core.CustomerService.Models.OrderProcessing
{
	public class OrderModel
	{
		public int Version { get; set; }
		public bool IsManualCreation { get; set; }
		public int Id { get; set; }
		public string Type { get; set; } // Anrede
		public string Name { get; set; } // Vorname/NameFirma
		public string Name2 { get; set; }
		public string Name3 { get; set; }
		public string Contact { get; set; } // Ansprechpartner
		public string Department { get; set; } // Abteilung
		public string StreetPOBox { get; set; } // Straße/Postfach
		public string CountryPostcode { get; set; } //Land/PLZ/Ort
		public string Shipping { get; set; } // Versandart
		public string Payment { get; set; } // Zahlungsweise
		public string Conditions { get; set; } // Konditionen
		public int? CustomerId { get; set; }
		public int? CustomerNumber { get; set; } // Unser Zeichen
		public int? AdressCustomerNumber { get; set; }
		public string SuppliedNumber { get; set; } // Ihr Zeichen
		public bool Vat { get; set; } // USt_Berechnen
		public DateTime? DueDate { get; set; } //Fälligkeit
		public string OrderTitle { get; set; } // Briefanrede
		public int? PersonalNumber { get; set; } //Personal-Nr
		public string Freetext { get; set; } // Freitext
		public string Freetext2 { get; set; } // Freitext
		public string ShippingAddress { get; set; } // Lieferadresse
		public int? RepairNumber { get; set; } // reparatur_nr
		public int? AbId { get; set; }
		public int? NrBv { get; set; }
		public int? NrRa { get; set; }
		public int? NrKanban { get; set; }
		public int? NrAuf { get; set; }
		public int? NrLie { get; set; }
		public int? NrRec { get; set; }
		public int? NrPro { get; set; }
		public int? NrGut { get; set; }
		public int? NrSto { get; set; }
		public int? Belegkreis { get; set; }
		public int? New { get; set; }
		public bool NewOrder { get; set; }
		public string LCountryZIPLocation { get; set; }
		public string LClientName { get; set; }
		public string LStreetMailbox { get; set; }
		public string LType { get; set; } // Lanrede
		public string LName { get; set; } // LVorname/NameFirma
		public string LName2 { get; set; }
		public string LName3 { get; set; }
		public string LContact { get; set; } // Lansprechpartner
		public string LDepartment { get; set; } // Labteilung
		public string LStreetPOBox { get; set; } // LStraße/Postfach
		public string LCountryPostcode { get; set; } // LLand/PLZ/Ort
		public string LOrderTitle { get; set; } // Lbriefanrede

		public string ProjectNumber { get; set; }
		public DateTime? Date { get; set; } // Datum
		public DateTime? DesiredDate { get; set; } // Wunschtermin
		public DateTime? DeliveryDate { get; set; } // Fälligkeit
		public string VorfailNr { get; set; }

		public int? ValidationUserId { get; set; }
		public string ValidationUser { get; set; }
		public DateTime? ValidationTime { get; set; }

		public string DocumentNumber { get; set; }

		public DateTime? ShippingDate { get; set; }
		public bool? Done { get; set; }
		public bool? Booked { get; set; }
		public bool? CanDelete { get; set; }
		public bool? CanArchive { get; set; }

		public ChangesModel Changes { get; set; } = new ChangesModel();

		public class ChangesModel
		{
			public int NewItems { get; set; }
			public int ChangedItems { get; set; }
			public int CanceledItems { get; set; }

			public int GlobalChangeId { get; set; } = -1;
			public List<GlobalChange> GlobalChanges { get; set; } = new List<GlobalChange>();
			public class GlobalChange
			{
				public string Key { get; set; }
				public string Value { get; set; }
			}
		}

		// > Buyer
		public BuyerModel Buyer { get; set; } = new BuyerModel();
		public class BuyerModel
		{
			public int BuyerDuns { get; set; }
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
			public string BuyerContactName { get; set; }
			public string BuyerContactTelephone { get; set; }
			public string BuyerContactFax { get; set; }

			public BuyerModel() { }
			public BuyerModel(Infrastructure.Data.Entities.Tables.PRS.OrderExtensionBuyerEntity buyer)
			{
				if(buyer != null)
				{
					BuyerDuns = int.Parse(buyer.DUNS);
					BuyerPartyIdentification = buyer.PartyIdentification;
					BuyerPartyIdentificationCodeListQualifier = buyer.PartyIdentificationCodeListQualifier;
					BuyerName = buyer.Name;
					BuyerName2 = buyer.Name2;
					BuyerName3 = buyer.Name3;
					BuyerStreet = buyer.Street;
					BuyerPostalCode = buyer.PostalCode;
					BuyerCity = buyer.City;
					BuyerCountryName = buyer.CountryName;
					BuyerPurchasingDepartment = buyer.PurchasingDepartment;
					BuyerContactName = buyer.ContactName;
					BuyerContactTelephone = buyer.ContactTelephone;
					BuyerContactFax = buyer.ContactFax;
				}
			}
		}

		// > Consignee
		public ConsigneeModel Consignee { get; set; } = new ConsigneeModel();
		public OrderModel()
		{

		}
	}
}
