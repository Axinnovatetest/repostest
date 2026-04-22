using System;

namespace Psz.Core.Logistics.Reporting.Models
{
	public class LSDruckFooterReportModel
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
		public LSDruckFooterReportModel()
		{

		}

		public LSDruckFooterReportModel(Infrastructure.Data.Entities.Tables.Logistics.OrderReportEntity orderReportEntity)
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
			Address3 = orderReportEntity.Address3;
			Address4 = orderReportEntity.Address4;

			// Document
			OrderNumberPO = orderReportEntity.OrderNumberPO;
			DocumentType = orderReportEntity.DocumentType;
			OrderNumber = orderReportEntity.OrderNumber;
			OrderDate = orderReportEntity.OrderDate;

			// Client
			ClientNumber = orderReportEntity.ClientNumber;

			ShippingMethod = orderReportEntity.ShippingMethod;


			// Items
			Position = orderReportEntity.Position;
			Article = orderReportEntity.Article;
			Description = orderReportEntity.Description;


			//

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

		}
		public Infrastructure.Data.Entities.Tables.Logistics.OrderReportEntity ToOrderReportEntity()
		{
			return new Infrastructure.Data.Entities.Tables.Logistics.OrderReportEntity
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

				ShippingMethod = ShippingMethod,



				// Items
				Position = Position,
				Article = Article,
				Description = Description,

				Cu_Surcharge = Cu_Surcharge,


				//

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

			};
		}


	}
}
