using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Psz.Core.BaseData.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Models.Article.ROH.OfferRequests;

public class ArticleOfferGroupedRequestsModel
{
	public DateTime? CreatedDate { get; set; }
	public int? CreatedUserId { get; set; }
	public string CreatedUserName { get; set; }
	public bool? EmailStatus { get; set; }
	public int Id { get; set; }
	public string ManufactuerNumber { get; set; }
	public bool? RequestStatus { get; set; }
	public string RequestUI { get; set; }
	public string SupplierContactName { get; set; }
	public int? SupplierId { get; set; }
	public string SupplierName { get; set; }
	public decimal? YearlyQuantity { get; set; }
	public int? numberofRequestsInTheSameOffer { get; set; }
	public ArticleOfferGroupedRequestsModel(List<Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.ArticleOfferRequestsEntity> entity)
	{
		CreatedDate = entity.First().CreatedDate;
		CreatedUserId = entity.First().CreatedUserId;
		CreatedUserName = entity.First().CreatedUserName;
		EmailStatus = entity.First().EmailStatus;
		ManufactuerNumber = string.Join("-", entity.Select(x => x.ManufactuerNumber).ToList());
		RequestUI = entity.First().RequestUI;
		SupplierContactName = string.Join("-", entity.Select(x => x.SupplierContactName).ToList());
		SupplierName = string.Join("-", entity.Select(x => x.SupplierName).ToList());
	}
}
public class ArticleOfferRequestsModel
{
	public DateTime? ClosedDate { get; set; }
	public int? ClosedUserId { get; set; }
	public string ClosedUserName { get; set; }
	public DateTime? CreatedDate { get; set; }
	public int? CreatedUserId { get; set; }
	public string CreatedUserName { get; set; }
	public decimal? CustomQuantity { get; set; }
	public string EmailId { get; set; }
	public bool? EmailStatus { get; set; }
	public int Id { get; set; }
	public string ManufactuerNumber { get; set; }
	public bool? RequestStatus { get; set; }
	public string RequestUI { get; set; }
	public DateTime? SentDate { get; set; }
	public int? SentUserId { get; set; }
	public string SentUserName { get; set; }
	public string SupplierContactEmail { get; set; }
	public string SupplierContactName { get; set; }
	public int? SupplierId { get; set; }
	public string SupplierName { get; set; }
	public decimal? YearlyQuantity { get; set; }
	public int? numberofRequestsInTheSameOffer { get; set; }
	public string unitName { get; set; }

	public RequestStatusIntermidiateModel ongoingStatus { get; set; }

	public ArticleOfferRequestsModel(Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.ArticleOfferRequestsEntity entity)
	{
		ClosedDate = entity.ClosedDate;
		ClosedUserId = entity.ClosedUserId;
		ClosedUserName = entity.ClosedUserName;
		CreatedDate = entity.CreatedDate;
		CreatedUserId = entity.CreatedUserId;
		CreatedUserName = entity.CreatedUserName;
		CustomQuantity = entity.CustomQuantity;
		EmailId = entity.EmailId;
		EmailStatus = entity.EmailStatus;
		Id = entity.Id;
		ManufactuerNumber = entity.ManufactuerNumber;
		RequestStatus = entity.RequestStatus;
		RequestUI = entity.RequestUI;
		SentDate = entity.SentDate;
		SentUserId = entity.SentUserId;
		SentUserName = entity.SentUserName;
		SupplierContactEmail = entity.SupplierContactEmail;
		SupplierContactName = entity.SupplierContactName;
		SupplierId = entity.SupplierId;
		SupplierName = entity.SupplierName;
		YearlyQuantity = entity.YearlyQuantity;
		ongoingStatus = OfferRequestsManager.VerifyAndParseRequestStatus(entity.ongoingStatus);
		unitName = OfferRequestsManager.GetUnitByIDForUI(int.Parse(entity.unit));

	}
}
public class ArticleOfferRequestsSubViewModel
{
	[JsonPropertyName("ManufactuerNumber")]
	public string ManufactuerNumber { get; set; }

	[JsonPropertyName("YearlyQuantity")]
	public decimal? YearlyQuantity { get; set; }

	[JsonPropertyName("CustomQuantity")]
	public decimal? CustomQuantity { get; set; }

	[JsonPropertyName("warengrupId")]
	public string WarengrupId { get; set; }

	[JsonPropertyName("Graduatedquantity1")]
	public int? Graduatedquantity1 { get; set; }

	[JsonPropertyName("Graduatedquantity2")]
	public int? Graduatedquantity2 { get; set; }

	[JsonPropertyName("Graduatedquantity3")]
	public int? Graduatedquantity3 { get; set; }

	[JsonPropertyName("Graduatedprice1")]
	public decimal? Graduatedprice1 { get; set; }

	[JsonPropertyName("Graduatedprice2")]
	public decimal? Graduatedprice2 { get; set; }

	[JsonPropertyName("Graduatedprice3")]
	public decimal? Graduatedprice3 { get; set; }
	public string warengrupId { get; set; }
	public string OfferRequestArticleDescription { get; set; }

}

public class ArticleOfferRequestsViewModel
{
	public ArticleOfferRequestsViewModel()
	{
		offers = new List<ArticleOfferRequestsSubViewModel>();
	}

	public int? SupplierId { get; set; }
	public string ProjectName { get; set; }
	public string EndCustomer { get; set; }
	public string SupplierEmail { get; set; }
	public List<ArticleOfferRequestsSubViewModel> offers { get; set; }
}

public class OfferrequestVM
{
	public OfferrequestVM()
	{
		offers = new List<ArticleOfferRequestsViewModel>();
	}
	public List<ArticleOfferRequestsViewModel> offers { get; set; }
}
public class OfferRequestEMail
{
	public string AdditionalCC { get; set; }
	public string Recipient { get; set; }
	public string CCEmail { get; set; }
	public string MaterialTable { get; set; } = null;
	public string EmailContent { get; set; } = null;
	public string EmailSubject { get; set; } = null;
	public bool multipleSuppliers { get; set; } = false;
	public bool EMailContentEdited { get; set; } = false;
	public bool EmailSent { get; set; } = false;
	public string RequestUI { get; set; } = "";
	public string ProjectName { get; set; }
	public string EndCustomer { get; set; }
	public int index { get; set; }
	public bool AttachmentExists { get; set; }
	public List<int> Ids { get; set; } = new List<int>();
	public List<Attachments> attachments { get; set; } = new List<Attachments>();
}

public class OfferRequestPerSupplierModel
{
	public OfferRequestPerSupplierModel()
	{
		OfferRequests = new List<MaterialRequestModel>();
	}
	public int Id { get; set; }
	public int RequestUI { get; set; }
	public string SupplierContactEmail { get; set; }
	public string SupplierContactName { get; set; }
	public int? SupplierId { get; set; }
	public string SupplierName { get; set; }
	public List<MaterialRequestModel> OfferRequests { get; set; }
}
public class MaterialRequestModel
{
	public string? MatNr { get; set; }
	public string Bez { get; set; }
	public string Hersteller { get; set; }
	public decimal? Jahresmenge { get; set; }
	public string unit { get; set; }
}

public class EditOfferRequestEMail
{
	[Required]
	public string Recipient { get; set; }
	[Required]
	public string CCEmail { get; set; }
	[Required]
	public string EmailContent { get; set; }
	[Required]
	public string EmailSubject { get; set; }
	public string AdditionalCC { get; set; }
	public bool multipleSuppliers { get; set; }
	public List<int> Ids { get; set; } = new List<int>();
}
public class EditOfferForSingleManufacturerNumberModel
{
	public DateTime? CreatedDate { get; set; }
	public int? CreatedUserId { get; set; }
	public string CreatedUserName { get; set; }
	public decimal? CustomQuantity { get; set; }
	public string EmailId { get; set; }
	public bool? EmailStatus { get; set; }
	public decimal? Graduatedprice1 { get; set; }
	public decimal? Graduatedprice2 { get; set; }
	public decimal? Graduatedprice3 { get; set; }
	public int? Graduatedquantity1 { get; set; }
	public int? Graduatedquantity2 { get; set; }
	public int? Graduatedquantity3 { get; set; }
	public int Id { get; set; }
	public string LatestEmailContent { get; set; }
	public string ManufactuerNumber { get; set; }
	public bool? RequestStatus { get; set; }
	public string RequestUI { get; set; }
	public DateTime? SentDate { get; set; }
	public int? SentUserId { get; set; }
	public string SentUserName { get; set; }
	public string SupplierContactEmail { get; set; }
	public string SupplierContactName { get; set; }
	public int? SupplierId { get; set; }
	public string SupplierName { get; set; }
	public string warengrupId { get; set; }
	public decimal? YearlyQuantity { get; set; }

	public string OfferRequestArticleDescription { get; set; }
	public string ProjectName { get; set; }
	public string EndCustomer { get; set; }
}
public class CloseOfferRequestModel
{
	public CloseOfferRequestModel()
	{
	}
	public DateTime? ClosedDate { get; set; }
	public int? ClosedUserId { get; set; }
	public string ClosedUserName { get; set; }
	public decimal? CustomQuantity { get; set; }
	public int Id { get; set; }
	public string ManufactuerNumber { get; set; }
	public string RequestUI { get; set; }
	public decimal? YearlyQuantity { get; set; }
	public string Feedback { get; set; }
	public int? FileId { get; set; }
	public string OfferNumber { get; set; }
	public DateTime? PriceExpiryDate { get; set; }
	public decimal? UnitPrice { get; set; }
	public decimal? Graduatedprice1 { get; set; }
	public decimal? Graduatedprice2 { get; set; }
	public decimal? Graduatedprice3 { get; set; }
	public int? Graduatedquantity1 { get; set; }
	public int? Graduatedquantity2 { get; set; }
	public int? Graduatedquantity3 { get; set; }
	//
	public int MinOrderQuantity { get; set; }
	public int PackagingUnit { get; set; }
	public int? DeliveryTime { get; set; }
	public decimal? ExportWeight { get; set; }
	public string CustomTariffNumber { get; set; }

	public string CountryOfOrigin { get; set; }

	public DateTime? AngebotDatum { get; set; }
}

public class CloseOfferRequestIncludingAttachmentModel
{
	public CloseOfferRequestIncludingAttachmentModel()
	{
	}
	public DateTime? ClosedDate { get; set; }
	public int? ClosedUserId { get; set; }
	public string ClosedUserName { get; set; }
	public decimal? CustomQuantity { get; set; }
	public int Id { get; set; }
	public string ManufactuerNumber { get; set; }
	public string RequestUI { get; set; }
	public decimal? YearlyQuantity { get; set; }
	public string Feedback { get; set; }
	public int? FileId { get; set; }
	public IFormFile file { get; set; }
	public string OfferNumber { get; set; }
	public DateTime? PriceExpiryDate { get; set; }
	public decimal? UnitPrice { get; set; }
	public decimal? Graduatedprice1 { get; set; }
	public decimal? Graduatedprice2 { get; set; }
	public decimal? Graduatedprice3 { get; set; }
	public int? Graduatedquantity1 { get; set; }
	public int? Graduatedquantity2 { get; set; }
	public int? Graduatedquantity3 { get; set; }
	//
	public int MinOrderQuantity { get; set; }
	public int PackagingUnit { get; set; }
	public int? DeliveryTime { get; set; }
	public decimal? ExportWeight { get; set; }
	public string CustomTariffNumber { get; set; }
	public string CountryOfOrigin { get; set; }

	//
	public DateTime? AngebotDatum { get; set; }

}
//
public class AddFileToClosedOfferRequestModel
{
	public AddFileToClosedOfferRequestModel()
	{
	}
	public int Id { get; set; }
	public string ManufactuerNumber { get; set; }
	public string RequestUI { get; set; }
	public decimal? YearlyQuantity { get; set; }
	public string Feedback { get; set; }
	public int? FileId { get; set; }
	public IFormFile file { get; set; }
	public string OfferNumber { get; set; }
	public DateTime? PriceExpiryDate { get; set; }
	public decimal? UnitPrice { get; set; }

}

public class CloseOfferMinimalModel
{
	public CloseOfferMinimalModel(Infrastructure.Data.Entities.Tables.BSD.ROH.OfferRequests.ArticleOfferRequestsEntity entity)
	{
		CreatedDate = entity.CreatedDate;
		CreatedUserId = entity.CreatedUserId;
		CreatedUserName = entity.CreatedUserName;
		CustomQuantity = entity.CustomQuantity;
		EmailStatus = entity.EmailStatus;
		Id = entity.Id;
		ManufactuerNumber = entity.ManufactuerNumber;
		RequestStatus = entity.RequestStatus;
		RequestUI = entity.RequestUI;
		SentDate = entity.SentDate;
		SentUserId = entity.SentUserId;
		SentUserName = entity.SentUserName;
		SupplierContactEmail = entity.SupplierContactEmail;
		SupplierContactName = entity.SupplierContactName;
		SupplierName = entity.SupplierName;
		YearlyQuantity = entity.YearlyQuantity;
		Feedback = entity.Feedback;
		PriceExpiryDate = entity.PriceExpiryDate;
		UnitPrice = entity.UnitPrice;
		OfferNumber = entity.OfferNumber;
		FileId = entity.FileId;

		Graduatedprice1 = entity.Graduatedprice1;
		Graduatedprice2 = entity.Graduatedprice2;
		Graduatedprice3 = entity.Graduatedprice3;

		Graduatedquantity1 = entity.Graduatedquantity1;
		Graduatedquantity2 = entity.Graduatedquantity2;
		Graduatedquantity3 = entity.Graduatedquantity3;

		SupplierId = entity.SupplierId;
		ExportWeight = entity.ExportWeight;
		MinOrderQuantity = entity.MinOrderQuantity;
		PackagingUnit = entity.PackagingUnit;
		DeliveryTime = entity.DeliveryTime;
		CountryOfOrigin = entity.CountryOfOrigin;
		AngebotDatum = entity.AngebotDatum;
		CustomTariffNumber = entity.CustomTariffNumber;
		OfferRequestArticleDescription = entity.OfferRequestArticleDescription;
		ProjectName = entity.ProjectName;
		EndCustomer = entity.EndCustomer;
		unitName = OfferRequestsManager.GetUnitByIDForUI(int.Parse(entity.unit));
	}

	public int? SupplierId { get; set; }
	public bool AlreadyEkCreated { get; set; }
	public DateTime? CreatedDate { get; set; }
	public int? CreatedUserId { get; set; }
	public string CreatedUserName { get; set; }
	public decimal? CustomQuantity { get; set; }
	public bool? EmailStatus { get; set; }
	public decimal? Graduatedprice1 { get; set; }
	public decimal? Graduatedprice2 { get; set; }
	public decimal? Graduatedprice3 { get; set; }
	public int? Graduatedquantity1 { get; set; }
	public int? Graduatedquantity2 { get; set; }
	public int? Graduatedquantity3 { get; set; }
	public int Id { get; set; }
	public string ManufactuerNumber { get; set; }
	public int? FileId { get; set; }
	public bool? RequestStatus { get; set; }
	public string RequestUI { get; set; }
	public DateTime? SentDate { get; set; }
	public int? SentUserId { get; set; }
	public string SentUserName { get; set; }
	public string SupplierContactEmail { get; set; }
	public string SupplierContactName { get; set; }

	public string SupplierName { get; set; }

	public decimal? YearlyQuantity { get; set; }

	public string Feedback { get; set; }
	public DateTime? PriceExpiryDate { get; set; }
	public decimal? UnitPrice { get; set; }
	public string OfferNumber { get; set; }
	public string unitName { get; set; }

	public decimal? ExportWeight { get; set; }
	public decimal? MinOrderQuantity { get; set; }
	public int? PackagingUnit { get; set; }
	public int? DeliveryTime { get; set; }
	public string CountryOfOrigin { get; set; }

	public string OfferRequestArticleDescription { get; set; }
	public string ProjectName { get; set; }
	public string EndCustomer { get; set; }
	public string CustomTariffNumber { get; set; }


	public DateTime? AngebotDatum { get; set; }
}
public class FilterdOfferRequestModel
{
	public string status { get; set; }
	public bool loadOpenOnly { get; set; }
}

public class OfferToArticleEKModel
{
	public int? ArtikelNr { get; set; }
	public int? BestellnummernNr { get; set; }
	public int Id { get; set; }
	public DateTime? LastUpdate { get; set; }
	public int? OfferId { get; set; }
	public string RequestUI { get; set; }
	public int? SupplierId { get; set; }
}

public class ChangeEmailLanguageModel
{
	public string EmailContent { get; set; } = null;
	public string MaterialTable { get; set; } = null;
	public string EmailSubject { get; set; } = null;
}
public class Attachments
{
	public string FileName { get; set; }
	public int FiledId { get; set; }
}

