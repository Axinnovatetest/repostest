using System;

namespace Psz.Core.CustomerService.Models.E_Rechnung
{
	public class RechnungReportRequestModel
	{
		public int LanguageId { get; set; }
		public int TypeId { get; set; }
		public int RechnungId { get; set; }
	}
	public class CreateModel
	{
		public int Id { get; set; }
		public int LanguageId { get; set; }
		public int OrderTypeId { get; set; }
		public int CompanyLogoImageId { get; set; }
		public int ImportLogoImageId { get; set; }
		public string Header { get; set; }
		public string ItemsHeader { get; set; }
		public string ItemsFooter1 { get; set; }
		public string ItemsFooter2 { get; set; }

		public string Footer11 { get; set; }
		public string Footer12 { get; set; }
		public string Footer13 { get; set; }
		public string Footer14 { get; set; }
		public string Footer15 { get; set; }
		public string Footer16 { get; set; }
		public string Footer17 { get; set; }
		public string Footer21 { get; set; }
		public string Footer22 { get; set; }
		public string Footer23 { get; set; }
		public string Footer24 { get; set; }
		public string Footer25 { get; set; }
		public string Footer26 { get; set; }
		public string Footer27 { get; set; }
		public string Footer31 { get; set; }
		public string Footer32 { get; set; }
		public string Footer33 { get; set; }
		public string Footer34 { get; set; }
		public string Footer35 { get; set; }
		public string Footer36 { get; set; }
		public string Footer37 { get; set; }
		public string Footer41 { get; set; }
		public string Footer42 { get; set; }
		public string Footer43 { get; set; }
		public string Footer44 { get; set; }
		public string Footer45 { get; set; }
		public string Footer46 { get; set; }
		public string Footer47 { get; set; }
		public string Footer51 { get; set; }
		public string Footer52 { get; set; }
		public string Footer53 { get; set; }
		public string Footer54 { get; set; }
		public string Footer55 { get; set; }
		public string Footer56 { get; set; }
		public string Footer57 { get; set; }
		public string Footer61 { get; set; }
		public string Footer62 { get; set; }
		public string Footer63 { get; set; }
		public string Footer64 { get; set; }
		public string Footer65 { get; set; }
		public string Footer66 { get; set; }
		public string Footer67 { get; set; }
		public string Footer71 { get; set; }
		public string Footer72 { get; set; }
		public string Footer73 { get; set; }
		public string Footer74 { get; set; }
		public string Footer75 { get; set; }
		public string Footer76 { get; set; }
		public string Footer77 { get; set; }

		//
		public DateTime LastUpdateTime { get; set; }
		public int LastUpdateUserId { get; set; }
		// 
		public byte[] CompanyLogoImage { get; set; }
		public string CompanyLogoImageExtension { get; set; }
		public byte[] ImportLogoImage { get; set; }
		public string ImportLogoImageExtension { get; set; }

		//
		public string Language { get; set; }
		public string OrderType { get; set; }
		public string LastUpdateUser { get; set; }


		// PSZ Address
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string Address2_2 { get; set; }
		public string Address2_3 { get; set; }
		public string Address3 { get; set; }
		public string Address4 { get; set; }
		public string Address5 { get; set; }

		// Document
		public string OrderNumberPO { get; set; }
		public string DocumentType { get; set; }
		public string OrderNumber { get; set; }
		public string OrderDate { get; set; }

		// Client
		public string ClientNumber { get; set; }
		public string InternalNumber { get; set; }
		public string ShippingMethod { get; set; }
		public string PaymentMethod { get; set; }
		public string PaymentTarget { get; set; }

		public string UST_ID { get; set; }

		// Items
		public string Position { get; set; }
		public string Article { get; set; }
		public string Description { get; set; }
		public string Amount { get; set; }
		public string PE { get; set; }
		public string BasisPrice150 { get; set; }
		public string Cu_G { get; set; }
		public string Cu_Surcharge { get; set; }
		public string UnitPrice { get; set; }
		public string Designation { get; set; }
		public string Unit { get; set; }
		public string TotalPrice150 { get; set; }
		public string DEL { get; set; }
		public string Cu_Total { get; set; }
		public string UnitTotal { get; set; }

		public string Bestellt { get; set; }
		public string Geliefert { get; set; }
		public string Liefertermin { get; set; }
		public string Offen { get; set; }
		public string Abladestelle { get; set; }

		// Summary
		public string SummarySum { get; set; }
		public string SummaryUST { get; set; }
		public string SummaryTotal { get; set; }
		//
		public string LastPageText1 { get; set; }
		public string LastPageText2 { get; set; }
		public string LastPageText3 { get; set; }
		public string LastPageText4 { get; set; }
		public string LastPageText5 { get; set; }
		public string LastPageText6 { get; set; }
		public string LastPageText7 { get; set; }
		public string LastPageText8 { get; set; }
		public string LastPageText9 { get; set; }

		public string Lieferadresse { get; set; }
		public string Index_Kunde { get; set; }
		public CreateModel()
		{

		}

		public CreateModel(Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity orderReportEntity)
		{
			if(orderReportEntity == null)
				return;

			Id = orderReportEntity.Id;
			LanguageId = orderReportEntity.LanguageId;
			OrderTypeId = orderReportEntity.OrderTypeId;

			CompanyLogoImageId = orderReportEntity.CompanyLogoImageId;
			ImportLogoImageId = orderReportEntity.ImportLogoImageId;

			Header = orderReportEntity.Header;
			ItemsHeader = orderReportEntity.ItemsHeader;
			ItemsFooter1 = orderReportEntity.ItemsFooter1;
			ItemsFooter2 = orderReportEntity.ItemsFooter2;

			Footer11 = orderReportEntity.Footer11;
			Footer12 = orderReportEntity.Footer12;
			Footer13 = orderReportEntity.Footer13;
			Footer14 = orderReportEntity.Footer14;
			Footer15 = orderReportEntity.Footer15;
			Footer16 = orderReportEntity.Footer16;
			Footer17 = orderReportEntity.Footer17;

			Footer21 = orderReportEntity.Footer21;
			Footer22 = orderReportEntity.Footer22;
			Footer23 = orderReportEntity.Footer23;
			Footer24 = orderReportEntity.Footer24;
			Footer25 = orderReportEntity.Footer25;
			Footer26 = orderReportEntity.Footer26;
			Footer27 = orderReportEntity.Footer27;

			Footer31 = orderReportEntity.Footer31;
			Footer32 = orderReportEntity.Footer32;
			Footer33 = orderReportEntity.Footer33;
			Footer34 = orderReportEntity.Footer34;
			Footer35 = orderReportEntity.Footer35;
			Footer36 = orderReportEntity.Footer36;
			Footer37 = orderReportEntity.Footer37;

			Footer41 = orderReportEntity.Footer41;
			Footer42 = orderReportEntity.Footer42;
			Footer43 = orderReportEntity.Footer43;
			Footer44 = orderReportEntity.Footer44;
			Footer45 = orderReportEntity.Footer45;
			Footer46 = orderReportEntity.Footer46;
			Footer47 = orderReportEntity.Footer47;

			Footer51 = orderReportEntity.Footer51;
			Footer52 = orderReportEntity.Footer52;
			Footer53 = orderReportEntity.Footer53;
			Footer54 = orderReportEntity.Footer54;
			Footer55 = orderReportEntity.Footer55;
			Footer56 = orderReportEntity.Footer56;
			Footer57 = orderReportEntity.Footer57;

			Footer61 = orderReportEntity.Footer61;
			Footer62 = orderReportEntity.Footer62;
			Footer63 = orderReportEntity.Footer63;
			Footer64 = orderReportEntity.Footer64;
			Footer65 = orderReportEntity.Footer65;
			Footer66 = orderReportEntity.Footer66;
			Footer67 = orderReportEntity.Footer67;

			Footer71 = orderReportEntity.Footer71;
			Footer72 = orderReportEntity.Footer72;
			Footer73 = orderReportEntity.Footer73;
			Footer74 = orderReportEntity.Footer74;
			Footer75 = orderReportEntity.Footer75;
			Footer76 = orderReportEntity.Footer76;
			Footer77 = orderReportEntity.Footer77;

			LastUpdateTime = Convert.ToDateTime(orderReportEntity.LastUpdateTime);
			LastUpdateUserId = Convert.ToInt32(orderReportEntity.LastUpdateUserId);

			try
			{
				var comanyLogo = Module.FilesManager.GetFile(CompanyLogoImageId);
				CompanyLogoImage = comanyLogo?.FileBytes;
				CompanyLogoImageExtension = comanyLogo?.FileExtension;
			} catch(Exception)
			{
				CompanyLogoImage = null;
				CompanyLogoImageExtension = string.Empty;
			}

			try
			{
				var importLogo = Module.FilesManager.GetFile(ImportLogoImageId);
				ImportLogoImage = importLogo?.FileBytes;
				ImportLogoImageExtension = importLogo?.FileExtension;
			} catch(Exception)
			{
				ImportLogoImage = null;
				ImportLogoImageExtension = string.Empty;
			}

			//
			var user = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(LastUpdateUserId);
			LastUpdateUser = user == null ? string.Empty : user.Username;
			var orderType = Infrastructure.Data.Access.Tables.STG.OrderTypesAccess.Get(OrderTypeId);
			OrderType = orderType == null ? string.Empty : orderType?.Type;
			var language = Infrastructure.Data.Access.Tables.STG.SprachenAccess.Get(LanguageId);
			Language = language == null ? string.Empty : language?.Sprache;


			// PSZ Address
			Address1 = orderReportEntity.Address1;
			Address2 = orderReportEntity.Address2;
			Address2_2 = ""; // 2023-04-27 empty label for Name2
			Address2_3 = "";// 2023-04-27 empty label  for Name3
			Address3 = orderReportEntity.Address3;
			Address4 = orderReportEntity.Address4;

			// Document
			OrderNumberPO = orderReportEntity.OrderNumberPO;
			DocumentType = orderReportEntity.DocumentType;
			OrderNumber = orderReportEntity.OrderNumber;
			OrderDate = orderReportEntity.OrderDate;

			// Client
			ClientNumber = orderReportEntity.ClientNumber;
			InternalNumber = orderReportEntity.InternalNumber;
			ShippingMethod = orderReportEntity.ShippingMethod;
			PaymentMethod = orderReportEntity.PaymentMethod;
			PaymentTarget = orderReportEntity.PaymentTarget;

			UST_ID = orderReportEntity.UST_ID;

			// Items
			Position = orderReportEntity.Position;
			Article = orderReportEntity.Article;
			Description = orderReportEntity.Description;
			Amount = orderReportEntity.Amount;
			PE = orderReportEntity.PE;
			BasisPrice150 = orderReportEntity.BasisPrice150;
			Cu_G = orderReportEntity.Cu_G;
			Cu_Surcharge = orderReportEntity.Cu_Surcharge;
			UnitPrice = orderReportEntity.UnitPrice;
			Designation = orderReportEntity.Designation;
			Unit = orderReportEntity.Unit;
			TotalPrice150 = orderReportEntity.TotalPrice150;
			DEL = orderReportEntity.DEL;
			Cu_Total = orderReportEntity.Cu_Total;
			UnitTotal = orderReportEntity.UnitTotal;

			//
			Bestellt = orderReportEntity.Bestellt;
			Geliefert = orderReportEntity.Geliefert;
			Liefertermin = orderReportEntity.Liefertermin;
			Offen = orderReportEntity.Offen;
			Abladestelle = orderReportEntity.Abladestelle;

			// Summary
			SummarySum = orderReportEntity.SummarySum;
			SummaryUST = orderReportEntity.SummaryUST;
			SummaryTotal = orderReportEntity.SummaryTotal;

			//
			LastPageText1 = orderReportEntity.LastPageText1 ?? "";
			LastPageText2 = orderReportEntity.LastPageText2 ?? "";
			LastPageText3 = orderReportEntity.LastPageText3 ?? "";
			LastPageText4 = orderReportEntity.LastPageText4 ?? "";
			LastPageText5 = orderReportEntity.LastPageText5 ?? "";
			LastPageText6 = orderReportEntity.LastPageText6 ?? "";
			LastPageText7 = orderReportEntity.LastPageText7 ?? "";
			LastPageText8 = orderReportEntity.LastPageText8 ?? "";
			LastPageText9 = orderReportEntity.LastPageText9 ?? "";

			Lieferadresse = orderReportEntity.Lieferadresse;
			Index_Kunde = orderReportEntity.Index_Kunde;
		}
		public Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity ToOrderReportEntity()
		{
			return new Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity
			{
				Id = Id,
				LanguageId = LanguageId,
				OrderTypeId = OrderTypeId,

				CompanyLogoImageId = Helpers.ImageFileHelper.updateImage(CompanyLogoImageId, CompanyLogoImage, CompanyLogoImageExtension),
				ImportLogoImageId = Helpers.ImageFileHelper.updateImage(ImportLogoImageId, ImportLogoImage, ImportLogoImageExtension),

				Header = Header,
				ItemsHeader = ItemsHeader,
				ItemsFooter1 = ItemsFooter1,
				ItemsFooter2 = ItemsFooter2,

				Footer11 = Footer11,
				Footer12 = Footer12,
				Footer13 = Footer13,
				Footer14 = Footer14,
				Footer15 = Footer15,
				Footer16 = Footer16,
				Footer17 = Footer17,

				Footer21 = Footer21,
				Footer22 = Footer22,
				Footer23 = Footer23,
				Footer24 = Footer24,
				Footer25 = Footer25,
				Footer26 = Footer26,
				Footer27 = Footer27,

				Footer31 = Footer31,
				Footer32 = Footer32,
				Footer33 = Footer33,
				Footer34 = Footer34,
				Footer35 = Footer35,
				Footer36 = Footer36,
				Footer37 = Footer37,

				Footer41 = Footer41,
				Footer42 = Footer42,
				Footer43 = Footer43,
				Footer44 = Footer44,
				Footer45 = Footer45,
				Footer46 = Footer46,
				Footer47 = Footer47,

				Footer51 = Footer51,
				Footer52 = Footer52,
				Footer53 = Footer53,
				Footer54 = Footer54,
				Footer55 = Footer55,
				Footer56 = Footer56,
				Footer57 = Footer57,

				Footer61 = Footer61,
				Footer62 = Footer62,
				Footer63 = Footer63,
				Footer64 = Footer64,
				Footer65 = Footer65,
				Footer66 = Footer66,
				Footer67 = Footer67,

				Footer71 = Footer71,
				Footer72 = Footer72,
				Footer73 = Footer73,
				Footer74 = Footer74,
				Footer75 = Footer75,
				Footer76 = Footer76,
				Footer77 = Footer77,

				LastUpdateTime = LastUpdateTime,
				LastUpdateUserId = LastUpdateUserId,

				// PSZ Address
				Address1 = Address1,
				Address2 = Address2,
				Address3 = Address3,
				Address4 = Address4,

				// Document
				OrderNumberPO = OrderNumberPO,
				DocumentType = DocumentType,
				OrderNumber = OrderNumber,
				OrderDate = OrderDate,

				// Client
				ClientNumber = ClientNumber,
				InternalNumber = InternalNumber,
				ShippingMethod = ShippingMethod,
				PaymentMethod = PaymentMethod,
				PaymentTarget = PaymentTarget,

				UST_ID = UST_ID,

				// Items
				Position = Position,
				Article = Article,
				Description = Description,
				Amount = Amount,
				PE = PE,
				BasisPrice150 = BasisPrice150,
				Cu_G = Cu_G,
				Cu_Surcharge = Cu_Surcharge,
				UnitPrice = UnitPrice,
				Designation = Designation,
				Unit = Unit,
				TotalPrice150 = TotalPrice150,
				DEL = DEL,
				Cu_Total = Cu_Total,
				UnitTotal = UnitTotal,

				//
				Bestellt = Bestellt,
				Geliefert = Geliefert,
				Liefertermin = Liefertermin,
				Offen = Offen,
				Abladestelle = Abladestelle,

				// Summary
				SummarySum = SummarySum,
				SummaryUST = SummaryUST,
				SummaryTotal = SummaryTotal,

				//
				LastPageText1 = LastPageText1,
				LastPageText2 = LastPageText2,
				LastPageText3 = LastPageText3,
				LastPageText4 = LastPageText4,
				LastPageText5 = LastPageText5,
				LastPageText6 = LastPageText6,
				LastPageText7 = LastPageText7,
				LastPageText8 = LastPageText8,
				LastPageText9 = LastPageText9,

				Lieferadresse = Lieferadresse,
				Index_Kunde = Index_Kunde,
			};
		}
		// Reporting 
		public RechnungReportingModel ToInvoiceFields()
		{
			return new RechnungReportingModel
			{
				Id = Id,

				CompanyLogoImage = CompanyLogoImage,
				ImportLogoImage = ImportLogoImage,
				CompanyLogoImageId = CompanyLogoImageId,
				ImportLogoImageId = ImportLogoImageId,

				Header = Header,
				ItemsHeader = ItemsHeader,
				ItemsFooter1 = ItemsFooter1,
				ItemsFooter2 = ItemsFooter2,

				Footer11 = Footer11,
				Footer12 = Footer12,
				Footer13 = Footer13,
				Footer14 = Footer14,
				Footer15 = Footer15,
				Footer16 = Footer16,
				Footer17 = Footer17,

				Footer21 = Footer21,
				Footer22 = Footer22,
				Footer23 = Footer23,
				Footer24 = Footer24,
				Footer25 = Footer25,
				Footer26 = Footer26,
				Footer27 = Footer27,

				Footer31 = Footer31,
				Footer32 = Footer32,
				Footer33 = Footer33,
				Footer34 = Footer34,
				Footer35 = Footer35,
				Footer36 = Footer36,
				Footer37 = Footer37,

				Footer41 = Footer41,
				Footer42 = Footer42,
				Footer43 = Footer43,
				Footer44 = Footer44,
				Footer45 = Footer45,
				Footer46 = Footer46,
				Footer47 = Footer47,

				Footer51 = Footer51,
				Footer52 = Footer52,
				Footer53 = Footer53,
				Footer54 = Footer54,
				Footer55 = Footer55,
				Footer56 = Footer56,
				Footer57 = Footer57,

				Footer61 = Footer61,
				Footer62 = Footer62,
				Footer63 = Footer63,
				Footer64 = Footer64,
				Footer65 = Footer65,
				Footer66 = Footer66,
				Footer67 = Footer67,

				Footer71 = Footer71,
				Footer72 = Footer72,
				Footer73 = Footer73,
				Footer74 = Footer74,
				Footer75 = Footer75,
				Footer76 = Footer76,
				Footer77 = Footer77,


				// PSZ Address
				Address1 = Address1,
				Address2 = Address2,
				Address2_2 = Address2_2,
				Address2_3 = Address2_3,
				Address3 = Address3,
				Address4 = Address4,
				Address5 = Address5,

				// Document
				OrderNumberPO = OrderNumberPO,
				DocumentType = DocumentType,
				OrderNumber = OrderNumber,
				OrderDate = OrderDate,

				// Client
				ClientNumber = ClientNumber,
				InternalNumber = InternalNumber,
				ShippingMethod = ShippingMethod,
				PaymentMethod = PaymentMethod,
				PaymentTarget = PaymentTarget,

				UST_ID = UST_ID,

				// Items
				Position = Position,
				Article = Article,
				Description = Description,
				Amount = Amount,
				PE = PE,
				BasisPrice150 = BasisPrice150,
				Cu_G = Cu_G,
				Cu_Surcharge = Cu_Surcharge,
				UnitPrice = UnitPrice,
				Designation = Designation,
				Unit = Unit,
				TotalPrice150 = TotalPrice150,
				DEL = DEL,
				Cu_Total = Cu_Total,
				UnitTotal = UnitTotal,

				//
				Bestellt = Bestellt,
				Geliefert = Geliefert,
				Liefertermin = Liefertermin,
				Offen = Offen,
				Abladestelle = Abladestelle,

				// Summary
				SummarySum = SummarySum,
				SummaryUST = SummaryUST,
				SummaryTotal = SummaryTotal,

				//
				LastPageText1 = LastPageText1 ?? "",
				LastPageText2 = LastPageText2 ?? "",
				LastPageText3 = LastPageText3 ?? "",
				LastPageText4 = LastPageText4 ?? "",
				LastPageText5 = LastPageText5 ?? "",
				LastPageText6 = LastPageText6 ?? "",
				LastPageText7 = LastPageText7 ?? "",
				LastPageText8 = LastPageText8 ?? "",
				LastPageText9 = LastPageText9 ?? "",

				Lieferadresse = Lieferadresse,
				Index_Kunde = Index_Kunde ?? ""
			};
		}
	}

	public class RechnungReportingModel
	{
		public int Id { get; set; }
		public int LanguageId { get; set; }
		public int OrderTypeId { get; set; }
		public int CompanyLogoImageId { get; set; }
		public int ImportLogoImageId { get; set; }
		public string Header { get; set; }
		public string ItemsHeader { get; set; }
		public string ItemsFooter1 { get; set; }
		public string ItemsFooter2 { get; set; }

		public string Footer11 { get; set; }
		public string Footer12 { get; set; }
		public string Footer13 { get; set; }
		public string Footer14 { get; set; }
		public string Footer15 { get; set; }
		public string Footer16 { get; set; }
		public string Footer17 { get; set; }
		public string Footer21 { get; set; }
		public string Footer22 { get; set; }
		public string Footer23 { get; set; }
		public string Footer24 { get; set; }
		public string Footer25 { get; set; }
		public string Footer26 { get; set; }
		public string Footer27 { get; set; }
		public string Footer31 { get; set; }
		public string Footer32 { get; set; }
		public string Footer33 { get; set; }
		public string Footer34 { get; set; }
		public string Footer35 { get; set; }
		public string Footer36 { get; set; }
		public string Footer37 { get; set; }
		public string Footer41 { get; set; }
		public string Footer42 { get; set; }
		public string Footer43 { get; set; }
		public string Footer44 { get; set; }
		public string Footer45 { get; set; }
		public string Footer46 { get; set; }
		public string Footer47 { get; set; }
		public string Footer51 { get; set; }
		public string Footer52 { get; set; }
		public string Footer53 { get; set; }
		public string Footer54 { get; set; }
		public string Footer55 { get; set; }
		public string Footer56 { get; set; }
		public string Footer57 { get; set; }
		public string Footer61 { get; set; }
		public string Footer62 { get; set; }
		public string Footer63 { get; set; }
		public string Footer64 { get; set; }
		public string Footer65 { get; set; }
		public string Footer66 { get; set; }
		public string Footer67 { get; set; }
		public string Footer71 { get; set; }
		public string Footer72 { get; set; }
		public string Footer73 { get; set; }
		public string Footer74 { get; set; }
		public string Footer75 { get; set; }
		public string Footer76 { get; set; }
		public string Footer77 { get; set; }
		//
		public string Footer78 { get; set; }
		public string Footer79 { get; set; }
		public string Footer80 { get; set; }
		public string Footer81 { get; set; }
		public string Footer82 { get; set; }
		//
		public DateTime LastUpdateTime { get; set; }
		public int LastUpdateUserId { get; set; }
		// 
		public byte[] CompanyLogoImage { get; set; }
		public byte[] ImportLogoImage { get; set; }

		//
		public string AddressName { get; set; }
		public string AddressAddress { get; set; }
		public string AddressPhone { get; set; }
		public string AddressFax { get; set; }


		// PSZ Address
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string Address2_2 { get; set; } // name2
		public string Address2_3 { get; set; } // name3
		public string Address2_4 { get; set; } // ansprechpartner
		public string Address2_5 { get; set; } // abteilung normal
		public string Address2_6 { get; set; } // abteilung for RG-data
		public string Address3 { get; set; }
		public string Address4 { get; set; }
		public string Address5 { get; set; }
		public string Address6 { get; set; }

		// Document
		public string OrderNumberPO { get; set; }
		public string DocumentType { get; set; }
		public string OrderNumber { get; set; }
		public string OrderDate { get; set; }
		public string RechnungNummer { get; set; }

		// Client
		public string ClientNumber { get; set; }
		public string InternalNumber { get; set; }// Unser Zeichen
		public string ShippingMethod { get; set; }// Versandart
		public string PaymentMethod { get; set; }// Zahlungsweise
		public string PaymentTarget { get; set; }// Zahlungsart

		public string UST_ID { get; set; }

		// Items
		public string Position { get; set; }
		public string Article { get; set; }
		public string Description { get; set; }
		public string Amount { get; set; }
		public string PE { get; set; }
		public string BasisPrice150 { get; set; }
		public string Cu_G { get; set; }
		public string Cu_Surcharge { get; set; }
		public string UnitPrice { get; set; }
		public string Designation { get; set; }
		public string Unit { get; set; }
		public string TotalPrice150 { get; set; }
		public string DEL { get; set; }
		public string Cu_Total { get; set; }
		public string UnitTotal { get; set; }

		public string Bestellt { get; set; }
		public string Geliefert { get; set; }
		public string Liefertermin { get; set; }
		public string Offen { get; set; }

		public string Abladestelle { get; set; }

		// Summary
		public string SummarySum { get; set; }
		public decimal SummarySumValue { get; set; }
		public string SummaryUST { get; set; }
		public decimal SummaryUSTValue { get; set; }
		public string SummaryTotal { get; set; }
		public decimal SummaryTotalValue { get; set; }


		// Delivery Address
		public string LAnrede { get; set; }
		public string LVorname { get; set; }
		public string LName2 { get; set; }
		public string LName3 { get; set; }
		public string Labteilung { get; set; }
		public string Lansprechpartner { get; set; }
		public string LStrabe { get; set; }
		public string LLandPLZOrt { get; set; }

		//
		public string LastPageText1 { get; set; }
		public string LastPageText2 { get; set; }
		public string LastPageText3 { get; set; }
		public string LastPageText4 { get; set; }
		public string LastPageText5 { get; set; }
		public string LastPageText6 { get; set; }
		public string LastPageText7 { get; set; }
		public string LastPageText8 { get; set; }
		public string LastPageText9 { get; set; }

		public string Lieferadresse { get; set; }
		public string Index_Kunde { get; set; }
		public string Ust { get; set; }

		public string FooterText1 { get; set; }
		public string FooterText2 { get; set; }
		public string FooterText3 { get; set; }
		public string FooterText4 { get; set; }

	}
	public class RechnungReportingItemModel
	{
		public int InvoiceId { get; set; }
		public int Id { get; set; }

		public string PositionNumber { get; set; } // Pos
		public string ItemNumber { get; set; } // Artikel
		public string Description { get; set; } // Beschreibung
		public string Designation { get; set; } // Bezeichnung
		public string Amount { get; set; }  // Menge
		public string PE { get; set; } // PE
		public string Unit { get; set; } // Einheit
		public decimal BasePrice { get; set; } // basispreis
		public decimal TotalPrice { get; set; } // Gesamppreis
		public string TotalCopper { get; set; } // Cu-G
		public string DEL { get; set; } // DEL
		public decimal SurchargeCopper { get; set; } // Cu-Zuschlag
		public decimal TotalSurchargeCopper { get; set; } // Cu-Zuschlag Gesamt
		public decimal UnitPrice { get; set; } // Einzelpreis
		public decimal TotalUnitPrice { get; set; } // Einzelpreis Gesamt

		public string AB_Pos_zu_RA_Pos { get; set; }
		public string Liefertermin { get; set; }
		public string Geliefert { get; set; }
		public decimal Anzahl { get; set; }
		public string Bestellt { get; set; }
		public string Offen { get; set; }
		public string Abladestelle { get; set; }
		public string Postext { get; set; }
		public string DELFixiert { get; set; }
		public string Index_Kunde { get; set; }
		public string DelFixedText { get; set; }
		public string ExternComment { get; set; }
		public string TotalUnitSurcharge { get; set; }
		public string TotalSurcharge { get; set; }
		public string LSBezug { get; set; }
		public bool Factoring { get; set; }
		public string Ursprungsland { get; set; }

	}
}
