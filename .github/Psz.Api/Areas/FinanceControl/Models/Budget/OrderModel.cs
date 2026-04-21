using System;
using System.Collections.Generic;
using System.Linq;
namespace Psz.Api.Areas.FinanceControl.Models.Budget
{
	public class OrderModel
	{
		//    public int Id_Order { get; set; }
		//    public string Number { get; set; }
		//    public string Type { get; set; }
		//    public int? ProjectId { get; set; }
		//    public string ProjectName { get; set; }
		//    public string Description { get; set; }
		//    public string ProjectState { get; set; }
		//    public string ProjectClientName { get; set; }
		//    public string ProjectClientNumber { get; set; }
		//    public int? DepartementId { get; set; }
		//    public string DepartementName { get; set; }
		//    public int? Level { get; set; }
		//    public int? Status { get; set; }
		//    public int LandId { get; set; }
		//    public string LandName { get; set; }
		//    public int? SupplierId { get; set; }
		//    public string SupplierName { get; set; }
		//    public int? CurrencyId { get; set; }
		//    public string CurrencyName { get; set; }
		//    public int ResponsableId { get; set; }
		//    public string ResponsableName { get; set; }
		//    public DateTime? OrderDate { get; set; }

		//    public DateTime? ApprovalTime { get; set; }
		//    public int? ApprovalUserId { get; set; }
		//    public int? LastRejectionLevel { get; set; }
		//    public DateTime? LastRejectionTime { get; set; }
		//    public int? LastRejectionUserId { get; set; }


		//    public string DeliveryAddress { get; set; }
		//    public bool? Archived { get; set; }
		//    public DateTime? ArchiveTime { get; set; }
		//    public int? ArchiveUserId { get; set; }
		//    public bool? Deleted { get; set; }
		//    public DateTime? DeleteTime { get; set; }
		//    public int? DeleteUserId { get; set; }

		//    public int? StorageLocationId { get; set; }
		//    public string StorageLocationName { get; set; }


		#region ------------
		public int Id { get; set; }
		public int Id_Order { get; set; }
		public string Number { get; set; }
		public string Type { get; set; }
		public int? ProjectId { get; set; }
		public string ProjectName { get; set; }
		public string Description { get; set; }
		public string ProjectState { get; set; }
		public string ProjectClientName { get; set; }
		public string ProjectClientNumber { get; set; }
		public int? DepartmentId { get; set; }
		public string DepartmentName { get; set; }
		public int? Status { get; set; }
		public int? Level { get; set; }
		public int CompanyId { get; set; }
		public string CompanyName { get; set; }
		public int? SupplierId { get; set; }
		public string SupplierName { get; set; }
		public int? MandantId { get; set; }
		public int? CurrencyId { get; set; }
		public string CurrencyName { get; set; }
		public int ResponsableId { get; set; }
		public string ResponsableName { get; set; }
		public DateTime? OrderDate { get; set; }
		public decimal TotalAmount { get; set; }

		public DateTime? ApprovalTime { get; set; }
		public int? ApprovalUserId { get; set; }
		public int? LastRejectionLevel { get; set; }
		public DateTime? LastRejectionTime { get; set; }
		public int? LastRejectionUserId { get; set; }
		public List<Psz.Core.FinanceControl.Models.Budget.Order.ValidatorModel> Validators { get; set; }


		public string DeliveryAddress { get; set; }
		public int DeliveryCompanyId { get; set; }
		public string DeliveryCompanyName { get; set; }
		public int? DeliveryDepartmentId { get; set; }
		public string DeliveryDepartmentName { get; set; }

		public bool? Archived { get; set; }
		public DateTime? ArchiveTime { get; set; }
		public int? ArchiveUserId { get; set; }
		public bool? Deleted { get; set; }
		public DateTime? DeleteTime { get; set; }
		public int? DeleteUserId { get; set; }

		public int? StorageLocationId { get; set; }
		public string StorageLocationName { get; set; }



		public string SupplierAnrede { get; set; }
		public string SupplierType { get; set; }
		public string SupplierName1 { get; set; }
		public string SupplierName2 { get; set; }
		public string SupplierName3 { get; set; }
		public string SupplierContact { get; set; }
		public string SupplierDepartment { get; set; }
		public string SupplierStreetPostalCode { get; set; }
		public string SupplierCityCountry { get; set; }
		public string SupplierSalutations { get; set; }

		public string SupplierNumber { get; set; }
		public string SupplierTradingTerm { get; set; }
		public string SupplierPaymentMethod { get; set; }
		public string SupplierPaymentTerm { get; set; }
		public string SupplierTelephone { get; set; }
		public string SupplierFax { get; set; }
		#endregion

		public List<Microsoft.AspNetCore.Http.IFormFile> AttachmentFile { get; set; }
		public string AttachmentFileExtension { get; set; }

		public List<Psz.Core.FinanceControl.Models.Budget.Order.Article.ArticleModel> Articles { get; set; }

		public Psz.Core.FinanceControl.Models.Budget.Order.OrderModel OrderToBussModel()
		{
			var supllierAdd = SupplierStreetPostalCode?.Split('|');
			var supplierCity = SupplierCityCountry?.Split('|');
			return new Psz.Core.FinanceControl.Models.Budget.Order.OrderModel
			{
				Id = Id,
				Id_Order = Id_Order,
				CurrencyId = CurrencyId,
				CurrencyName = CurrencyName,
				Number = Number,
				Type = Type,
				ProjectId = ProjectId ?? -1,
				ProjectName = ProjectName,
				Description = Description,
				ProjectState = ProjectState,
				ProjectClientName = ProjectClientName,
				ProjectClientNumber = ProjectClientNumber,
				DepartmentId = DepartmentId,
				DepartmentName = DepartmentName,
				CompanyId = CompanyId,
				CompanyName = CompanyName,
				SupplierId = SupplierId,
				SupplierName = SupplierName,
				ResponsableId = ResponsableId,
				ResponsableName = ResponsableName,
				OrderDate = OrderDate,
				Status = Status,
				Level = Level,
				MandantId = MandantId,

				Deleted = Deleted,
				DeleteTime = DeleteTime,
				DeleteUserId = DeleteUserId,
				DeliveryAddress = DeliveryAddress,
				DeliveryDepartmentId = DeliveryDepartmentId,
				DeliveryDepartmentName = DeliveryDepartmentName,
				DeliveryCompanyId = DeliveryCompanyId,
				DeliveryCompanyName = DeliveryCompanyName,
				Archived = Archived,
				ArchiveTime = ArchiveTime,
				ArchiveUserId = ArchiveUserId,
				StorageLocationId = StorageLocationId,
				StorageLocationName = StorageLocationName,

				SupplierAnrede = SupplierAnrede,
				SupplierName2 = SupplierName2,
				SupplierName3 = SupplierName3,
				SupplierContact = SupplierContact,
				SupplierDepartment = SupplierDepartment,
				SupplierStreet = supllierAdd?[0],
				SupplierPostalCode = supllierAdd?.Count() > 1 ? supllierAdd?[1] : "",
				SupplierCity = supplierCity?[0],
				SupplierCountry = supplierCity?.Count() > 1 ? supplierCity[1] : "",
				SupplierSalutations = SupplierSalutations,
				SupplierNumber = SupplierNumber,
				SupplierTradingTerm = SupplierTradingTerm,
				SupplierPaymentMethod = SupplierPaymentMethod,
				SupplierPaymentTerm = SupplierPaymentTerm,
				SupplierTelephone = SupplierTelephone,
				SupplierFax = SupplierFax,

				Articles = Articles.Select(x => new Core.FinanceControl.Models.Budget.Order.Article.ArticleModel
				{
					Currency_Article = x?.Currency_Article,
					Id_Currency_Article = x?.Id_Currency_Article,
					Name_Article = x?.Name_Article,
					Quantity = x?.Quantity,
					TotalCost_Article = x?.TotalCost_Article,
					Unit_Price = x?.Unit_Price,

					Id_Article = x.Id_Article,
				})?.ToList(),

				File = AttachmentFile == null || AttachmentFile.Count <= 0
					? null
					: AttachmentFile.Select(x => new Core.FinanceControl.Models.Budget.Files.FilesModel
					{
						fileDate = DateTime.Now,
						DocumentData = getBytes(x),
						DocumentExtension = System.IO.Path.GetExtension(x.FileName) // AttachmentFileExtension,
					})?.ToList()
			};
		}
		internal static byte[] getBytes(Microsoft.AspNetCore.Http.IFormFile file)
		{
			if(file == null)
				return null;

			if(file.Length <= 0)
				return null;

			using(var ms = new System.IO.MemoryStream())
			{
				file.CopyTo(ms);
				return ms.ToArray();
			}
		}
	}
}
