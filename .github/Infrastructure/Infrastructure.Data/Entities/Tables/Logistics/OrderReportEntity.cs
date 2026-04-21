using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.Logistics
{
	public class OrderReportEntity
	{
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
		public int Id { get; set; }
		public int ImportLogoImageId { get; set; }
		public string ItemsFooter1 { get; set; }
		public string ItemsFooter2 { get; set; }
		public string ItemsHeader { get; set; }
		public int LanguageId { get; set; }
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
		public int? LastUpdateUserId { get; set; }
		public string Lieferadresse { get; set; }
		public string OrderDate { get; set; }
		public string OrderNumber { get; set; }
		public string OrderNumberPO { get; set; }
		public int OrderTypeId { get; set; }
		public string Position { get; set; }
		public string ShippingMethod { get; set; }
		public string SummarySum { get; set; }
		public string SummaryTotal { get; set; }
		public string SummaryUST { get; set; }

		public OrderReportEntity() { }
		public OrderReportEntity(DataRow dataRow)
		{
			Abladestelle = (dataRow["Abladestelle"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Abladestelle"]);
			Address1 = (dataRow["Address1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Address1"]);
			Address2 = (dataRow["Address2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Address2"]);
			Address3 = (dataRow["Address3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Address3"]);
			Address4 = (dataRow["Address4"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Address4"]);
			Article = (dataRow["Article"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Article"]);
			ArtikelCountry = (dataRow["ArtikelCountry"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArtikelCountry"]);
			ArtikelPrice = (dataRow["ArtikelPrice"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArtikelPrice"]);
			ArtikelQuantity = (dataRow["ArtikelQuantity"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArtikelQuantity"]);
			ArtikelStock = (dataRow["ArtikelStock"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArtikelStock"]);
			ArtikelWeight = (dataRow["ArtikelWeight"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ArtikelWeight"]);
			ClientNumber = (dataRow["ClientNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ClientNumber"]);
			CompanyLogoImageId = Convert.ToInt32(dataRow["CompanyLogoImageId"]);
			Cu_Surcharge = (dataRow["Cu_Surcharge"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Cu_Surcharge"]);
			CustomerDate = (dataRow["CustomerDate"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerDate"]);
			CustomerNumber = (dataRow["CustomerNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerNumber"]);
			Description = (dataRow["Description"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Description"]);
			Designation1 = (dataRow["Designation1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Designation1"]);
			Designation2 = (dataRow["Designation2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Designation2"]);
			DocumentType = (dataRow["DocumentType"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["DocumentType"]);
			Footer11 = (dataRow["Footer11"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer11"]);
			Footer12 = (dataRow["Footer12"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer12"]);
			Footer13 = (dataRow["Footer13"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer13"]);
			Footer14 = (dataRow["Footer14"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer14"]);
			Footer15 = (dataRow["Footer15"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer15"]);
			Footer16 = (dataRow["Footer16"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer16"]);
			Footer17 = (dataRow["Footer17"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer17"]);
			Footer21 = (dataRow["Footer21"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer21"]);
			Footer22 = (dataRow["Footer22"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer22"]);
			Footer23 = (dataRow["Footer23"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer23"]);
			Footer24 = (dataRow["Footer24"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer24"]);
			Footer25 = (dataRow["Footer25"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer25"]);
			Footer26 = (dataRow["Footer26"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer26"]);
			Footer27 = (dataRow["Footer27"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer27"]);
			Footer31 = (dataRow["Footer31"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer31"]);
			Footer32 = (dataRow["Footer32"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer32"]);
			Footer33 = (dataRow["Footer33"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer33"]);
			Footer34 = (dataRow["Footer34"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer34"]);
			Footer35 = (dataRow["Footer35"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer35"]);
			Footer36 = (dataRow["Footer36"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer36"]);
			Footer37 = (dataRow["Footer37"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer37"]);
			Footer41 = (dataRow["Footer41"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer41"]);
			Footer42 = (dataRow["Footer42"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer42"]);
			Footer43 = (dataRow["Footer43"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer43"]);
			Footer44 = (dataRow["Footer44"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer44"]);
			Footer45 = (dataRow["Footer45"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer45"]);
			Footer46 = (dataRow["Footer46"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer46"]);
			Footer47 = (dataRow["Footer47"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer47"]);
			Footer51 = (dataRow["Footer51"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer51"]);
			Footer52 = (dataRow["Footer52"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer52"]);
			Footer53 = (dataRow["Footer53"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer53"]);
			Footer54 = (dataRow["Footer54"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer54"]);
			Footer55 = (dataRow["Footer55"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer55"]);
			Footer56 = (dataRow["Footer56"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer56"]);
			Footer57 = (dataRow["Footer57"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer57"]);
			Footer61 = (dataRow["Footer61"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer61"]);
			Footer62 = (dataRow["Footer62"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer62"]);
			Footer63 = (dataRow["Footer63"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer63"]);
			Footer64 = (dataRow["Footer64"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer64"]);
			Footer65 = (dataRow["Footer65"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer65"]);
			Footer66 = (dataRow["Footer66"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer66"]);
			Footer67 = (dataRow["Footer67"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer67"]);
			Footer71 = (dataRow["Footer71"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer71"]);
			Footer72 = (dataRow["Footer72"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer72"]);
			Footer73 = (dataRow["Footer73"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer73"]);
			Footer74 = (dataRow["Footer74"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer74"]);
			Footer75 = (dataRow["Footer75"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer75"]);
			Footer76 = (dataRow["Footer76"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer76"]);
			Footer77 = (dataRow["Footer77"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Footer77"]);
			Header = (dataRow["Header"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Header"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			ImportLogoImageId = Convert.ToInt32(dataRow["ImportLogoImageId"]);
			ItemsFooter1 = (dataRow["ItemsFooter1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ItemsFooter1"]);
			ItemsFooter2 = (dataRow["ItemsFooter2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ItemsFooter2"]);
			ItemsHeader = (dataRow["ItemsHeader"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ItemsHeader"]);
			LanguageId = Convert.ToInt32(dataRow["LanguageId"]);
			LastPageText1 = (dataRow["LastPageText1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LastPageText1"]);
			LastPageText2 = (dataRow["LastPageText2"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LastPageText2"]);
			LastPageText3 = (dataRow["LastPageText3"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LastPageText3"]);
			LastPageText4 = (dataRow["LastPageText4"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LastPageText4"]);
			LastPageText5 = (dataRow["LastPageText5"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LastPageText5"]);
			LastPageText6 = (dataRow["LastPageText6"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LastPageText6"]);
			LastPageText7 = (dataRow["LastPageText7"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LastPageText7"]);
			LastPageText8 = (dataRow["LastPageText8"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LastPageText8"]);
			LastPageText9 = (dataRow["LastPageText9"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["LastPageText9"]);
			LastUpdateTime = (dataRow["LastUpdateTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["LastUpdateTime"]);
			LastUpdateUserId = (dataRow["LastUpdateUserId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LastUpdateUserId"]);
			Lieferadresse = (dataRow["Lieferadresse"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lieferadresse"]);
			OrderDate = (dataRow["OrderDate"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OrderDate"]);
			OrderNumber = (dataRow["OrderNumber"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OrderNumber"]);
			OrderNumberPO = (dataRow["OrderNumberPO"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["OrderNumberPO"]);
			OrderTypeId = Convert.ToInt32(dataRow["OrderTypeId"]);
			Position = (dataRow["Position"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Position"]);
			ShippingMethod = (dataRow["ShippingMethod"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["ShippingMethod"]);
			SummarySum = (dataRow["SummarySum"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SummarySum"]);
			SummaryTotal = (dataRow["SummaryTotal"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SummaryTotal"]);
			SummaryUST = (dataRow["SummaryUST"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SummaryUST"]);
		}
	}
}

