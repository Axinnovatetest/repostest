using System;
using System.Collections.Generic;

namespace Psz.Core.CustomerService.Models.Blanket
{
	public class RahmenReportModel
	{
		public List<RahmenHeadersReportModel> Headers { get; set; }
		public List<RahmenPositionsReportModel> Positions { get; set; }
		public List<RahmenSumsReportModel> Sums { get; set; }
		public List<RahmenFooterReportModel> Footer { get; set; }
	}

	public class RahmenHeadersReportModel
	{
		public string Adress1 { get; set; }
		public string Adress2 { get; set; }
		public string Adress3 { get; set; }
		public string Adress4 { get; set; }
		public string Adress5 { get; set; }
		public string Adress6 { get; set; }
		public string Adress7 { get; set; }
		public string Adress8 { get; set; }
		public string Adress9 { get; set; }
		public string Adress10 { get; set; }
		//--
		public string Nr { get; set; }
		public string Date { get; set; }
		public string ExtensionDate { get; set; }
		public string TradingTerm { get; set; }
		public string PayementTerm { get; set; }
		public string Customer { get; set; }
		public string Supplier { get; set; }
		public string Payement { get; set; }
	}

	public class RahmenPositionsReportModel
	{
		public string Position { get; set; }
		public string Artikel { get; set; }
		public string Description { get; set; }
		public string Quantity { get; set; }
		public string UOM { get; set; }
		public string Price { get; set; }
		public string VAT { get; set; }
		public string Price_UOM { get; set; }
		public string Total { get; set; }
		public string WishData { get; set; }

	}
	public class RahmenSumsReportModel
	{
		public string Total { get; set; }
		public string TotalVat { get; set; }
		public string TotalAmount { get; set; }
		public string UST { get; set; }
	}

	public class RahmenReportRequestModel
	{
		public int LanguageId { get; set; }
		public int TypeId { get; set; }
		public int OrderId { get; set; }
	}

	public class RahmenFooterReportModel
	{
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

		public byte[] CompanyLogoImage { get; set; }
		public string CompanyLogoImageExtension { get; set; }
		public byte[] ImportLogoImage { get; set; }
		public string ImportLogoImageExtension { get; set; }

		public RahmenFooterReportModel(Infrastructure.Data.Entities.Tables.PRS.OrderReportEntity entity)
		{
			Footer11 = entity.Footer11;
			Footer12 = entity.Footer12;
			Footer13 = entity.Footer13;
			Footer14 = entity.Footer14;
			Footer15 = entity.Footer15;
			Footer16 = entity.Footer16;
			Footer17 = entity.Footer17;
			Footer21 = entity.Footer21;
			Footer22 = entity.Footer22;
			Footer23 = entity.Footer23;
			Footer24 = entity.Footer24;
			Footer25 = entity.Footer25;
			Footer26 = entity.Footer26;
			Footer27 = entity.Footer27;
			Footer31 = entity.Footer31;
			Footer32 = entity.Footer32;
			Footer33 = entity.Footer33;
			Footer34 = entity.Footer34;
			Footer35 = entity.Footer35;
			Footer36 = entity.Footer36;
			Footer37 = entity.Footer37;
			Footer41 = entity.Footer41;
			Footer42 = entity.Footer42;
			Footer43 = entity.Footer43;
			Footer44 = entity.Footer44;
			Footer45 = entity.Footer45;
			Footer46 = entity.Footer46;
			Footer47 = entity.Footer47;
			Footer51 = entity.Footer51;
			Footer52 = entity.Footer52;
			Footer53 = entity.Footer53;
			Footer54 = entity.Footer54;
			Footer55 = entity.Footer55;
			Footer56 = entity.Footer56;
			Footer57 = entity.Footer57;
			Footer61 = entity.Footer61;
			Footer62 = entity.Footer62;
			Footer63 = entity.Footer63;
			Footer64 = entity.Footer64;
			Footer65 = entity.Footer65;
			Footer66 = entity.Footer66;
			Footer67 = entity.Footer67;
			Footer71 = entity.Footer71;
			Footer72 = entity.Footer72;
			Footer73 = entity.Footer73;
			Footer74 = entity.Footer74;
			Footer75 = entity.Footer75;
			Footer76 = entity.Footer76;
			Footer77 = entity.Footer77;

			try
			{
				var comanyLogo = Module.FilesManager.GetFile(entity.CompanyLogoImageId);
				CompanyLogoImage = comanyLogo?.FileBytes;
				CompanyLogoImageExtension = comanyLogo?.FileExtension;
			} catch(Exception)
			{
				CompanyLogoImage = null;
				CompanyLogoImageExtension = string.Empty;
			}

			try
			{
				var importLogo = Module.FilesManager.GetFile(entity.ImportLogoImageId);
				ImportLogoImage = importLogo?.FileBytes;
				ImportLogoImageExtension = importLogo?.FileExtension;
			} catch(Exception)
			{
				ImportLogoImage = null;
				ImportLogoImageExtension = string.Empty;
			}
		}
	}
}
