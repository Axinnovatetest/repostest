using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;

namespace Psz.Core.Logistics.Models.Lagebewegung
{
	public class FormatXmlFileRequestModel
	{
		public DateTime DataDate { get; set; }
		public int LagerFrom { get; set; }
		public int LagerTo { get; set; }
		public int? TransferType { get; set; }
	}
	public class FormatXmlFileRequestCountryDateModel
	{
		public DateTime DataDate { get; set; }
		public string Country { get; set; }
		public Enums.FormatEnums.TransferTypes TransferType { get; set; }
	}
	public class FormatXmlFileRequestCountryMonthModel
	{
		public int Month { get; set; }
		public string PlantId { get; set; }
		public Enums.FormatEnums.TransferTypes TransferType { get; set; }
	}

	public class FormatRecentTransfer
	{
		public DateTime TransferDate { get; set; }
		public int LagerFrom { get; set; }
		public int LagerTo { get; set; }
		public string LagerName { get; set; }
		public Enums.FormatEnums.TransferTypes TransferType { get; set; }
		public int SiteId { get; set; }
		public string SiteName { get; set; }
		// - 2024-06-27 - return sending to Format log data
		public DateTime LogDate { get; set; }
		public int LogUserId { get; set; }
		public string LogUsername { get; set; }
		public FormatRecentTransfer(Infrastructure.Data.Entities.Joins.Logistics.LagerbewegungEntity entity, string siteName, Enums.FormatEnums.TransferTypes type)
		{
			if(entity == null)
				return;
			// - 
			TransferDate = entity.TransferDate ?? DateTime.Today;
			LagerFrom = entity.LagerFrom;
			LagerTo = entity.LagerTo;
			SiteId = entity.SiteFromId;
			SiteName =siteName;
			TransferType = type;
		}
	}
	public class FormatXmlDayFileBySiteRequestModel
	{
		public DateTime DataDate { get; set; }
		public int SiteId { get; set; }
		public string SiteName { get; set; }
		public int TransferType { get; set; }
	}
	public class FormatXmlMonthFileBySiteRequestModel
	{
		public DateTime DataDate { get; set; }
		public int SiteId { get; set; }
		public string SiteName { get; set; }
		public int TransferType { get; set; }
	}
	public class FormatTransferSiteResponseModel
	{
		public int SiteId { get; set; }
		public string SiteName { get; set; }
		public string SiteShortName { get; set; }
		public int CountryId { get; set; }
		public string CountryName { get; set; }
		public int CompanyId { get; set; }
	}
	public class FormatRecentTransfersBySiteResponseModel
	{
		#region defs
		public class TransferDateModel
		{
			public DateTime Value { get; set; }
			public int? Key { get; set; }
			public int? LagerId { get; set; }
		}
		public class MonthTransferModel
		{
			public DateTime MonthDate { get; set; }
			public int MonthNumber { get; set; }
			public List<TransferDateModel> TransferDates { get; set; }
		}
		public class TransferModel
		{
			public MonthTransferModel PreviousMonth { get; set; }
			public MonthTransferModel CurrentMonth { get; set; }
		}
		#endregion defs

		#region props
		public int SiteId { get; set; }
		public string SiteName { get; set; }
		public int CountryId { get; set; }
		public string CountryName { get; set; }
		public TransferModel SentTransfers { get; set; }
		public TransferModel ReceivedTransfers { get; set; }
		#endregion props
	}
}
