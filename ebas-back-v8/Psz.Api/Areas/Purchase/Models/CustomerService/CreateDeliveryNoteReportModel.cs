using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Psz.Api.Areas.Purchase.Models.CustomerService
{
	public class CreateDeliveryNoteReportModel
	{
		public int Id { get; set; }
		public int LanguageId { get; set; }
		public int OrderTypeId { get; set; }

		public string Abladestelle { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string Address3 { get; set; }
		public string Address4 { get; set; }
		public string Article { get; set; }
		public string ArtikelCountry { get; set; }
		public string ArtikelPrice { get; set; }
		public string ArtikelQuantity { get; set; }
		public string ArtikelStock { get; set; }
		public string ArtikelWeight { get; set; }
		public string ClientNumber { get; set; }
		public int CompanyLogoImageId { get; set; }
		public string Cu_Surcharge { get; set; }
		public string CustomerDate { get; set; }
		public string CustomerNumber { get; set; }
		public string Description { get; set; }
		public string Designation1 { get; set; }
		public string Designation2 { get; set; }
		public string DocumentType { get; set; }
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
		public string Header { get; set; }
		public int ImportLogoImageId { get; set; }
		public string ItemsFooter1 { get; set; }
		public string ItemsFooter2 { get; set; }
		public string ItemsHeader { get; set; }
		public string LastPageText1 { get; set; }
		public string LastPageText2 { get; set; }
		public string LastPageText3 { get; set; }
		public string LastPageText4 { get; set; }
		public string LastPageText5 { get; set; }
		public string LastPageText6 { get; set; }
		public string LastPageText7 { get; set; }
		public string LastPageText8 { get; set; }
		public string LastPageText9 { get; set; }
		public DateTime? LastUpdateTime { get; set; }
		public int LastUpdateUserId { get; set; }
		public string Lieferadresse { get; set; }
		public string OrderDate { get; set; }
		public string OrderNumber { get; set; }
		public string OrderNumberPO { get; set; }
		public string Position { get; set; }
		public string ShippingMethod { get; set; }
		public string SummarySum { get; set; }
		public string SummaryTotal { get; set; }
		public string SummaryUST { get; set; }
		//
		public IFormFile CompanyLogoImage { get; set; }
		public string CompanyLogoImageExtension { get; set; }
		public IFormFile ImportLogoImage { get; set; }
		public string ImportLogoImageExtension { get; set; }

		//
		public string Language { get; set; }
		public string OrderType { get; set; }
		public string LastUpdateUser { get; set; }

		public Psz.Core.Apps.Purchase.Models.CustomerService.DeliveryNote.CreateDelieveryNoteModel ToBusinessModel()
		{
			return new Core.Apps.Purchase.Models.CustomerService.DeliveryNote.CreateDelieveryNoteModel
			{

				Id = Id,
				LanguageId = LanguageId,
				OrderTypeId = OrderTypeId,

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

				LastUpdateTime = Convert.ToDateTime(LastUpdateTime),
				LastUpdateUserId = Convert.ToInt32(LastUpdateUserId),

				CompanyLogoImage = getBytes(CompanyLogoImage),
				CompanyLogoImageExtension = CompanyLogoImageExtension,

				ImportLogoImage = getBytes(ImportLogoImage),
				ImportLogoImageExtension = ImportLogoImageExtension,

				//
				Language = Language,
				OrderType = OrderType,
				LastUpdateUser = LastUpdateUser,

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

				// Items
				Position = Position,
				Article = Article,
				Description = Description,
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
				ClientNumber = ClientNumber,
				ShippingMethod = ShippingMethod,
				Designation1 = Designation1,
				Designation2 = Designation2,
				CustomerNumber = CustomerNumber,
				CustomerDate = CustomerDate,
				ArtikelCountry = ArtikelCountry,
				ArtikelStock = ArtikelStock,
				ArtikelPrice = ArtikelPrice,
				ArtikelWeight = ArtikelWeight,
				ArtikelQuantity = ArtikelQuantity,

			};
		}

		internal static byte[] getBytes(IFormFile file)
		{
			if(file == null)
				return null;

			if(file.Length <= 0)
				return null;

			using(var ms = new MemoryStream())
			{
				file.CopyTo(ms);
				return ms.ToArray();
			}
		}

	}
}
