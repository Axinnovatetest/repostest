using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Order
{
	using MoreLinq;
	using OfficeOpenXml;
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.IO;
	using System.Linq;
	public class GetAllByIdHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Budget.Order.OrderModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetAllByIdHandler(Identity.Models.UserModel user, int idProject)
		{
			this._user = user;
			this._data = idProject;
		}

		public ResponseModel<List<Models.Budget.Order.OrderModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var currentUser = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
				var companyExtensionEntities = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.Get();
				var projectEntity = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(this._data);

				var ordersEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByProject(projectEntity.Id);
				if(ordersEntities == null)
				{
					return ResponseModel<List<Models.Budget.Order.OrderModel>>.SuccessResponse();
				}

				var orderIds = ordersEntities.Select(x => x.OrderId)?.ToList();
				var bestellungEntities = Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.Get(orderIds);

				Helpers.Processings.Budget.Order.updateArticlePrices(orderIds); // - HEAVY processing

				var articleEntites = Infrastructure.Data.Access.Tables.FNC.BestellteArtikelExtensionAccess.GetByOrderIds(orderIds);
				var bestellteArticleEntities = Infrastructure.Data.Access.Tables.FNC.Bestellte_ArtikelAccess.Get(articleEntites?.Select(x => x.BestellteArtikelNr)?.ToList());
				var fileEntities = Infrastructure.Data.Access.Tables.FNC.Budget_JointFile_OrderAccess.GetByIdsOrder(orderIds);

				var response = new List<Models.Budget.Order.OrderModel>();

				ordersEntities = ordersEntities.OrderBy(x => x.Id, OrderByDirection.Descending)?.ToList();
				foreach(var orderEntity in ordersEntities)
				{
					var bestellungEntity = bestellungEntities?.Find(x => x.Nr == orderEntity.OrderId);
					var articleEntity = articleEntites?.FindAll(x => x.OrderId == orderEntity.OrderId)?.ToList();
					var articlesEntities = Infrastructure.Data.Access.Tables.FNC.Artikel_BudgetAccess.Get(articleEntity?.Select(x => x.ArticleId)?.ToList());
					var bestellteArticelEntity = bestellteArticleEntities?.FindAll(x => articleEntity.Select(y => y.BestellteArtikelNr)?.ToList()?.Exists(a => a == x.Nr) == true)?.ToList();
					var fileEntity = fileEntities?.FindAll(x => x.Id_Order == orderEntity.OrderId);

					// Validators
					var validators = Validators.getByOrderId(orderEntity.OrderId, out List<string> errors);
					if(errors != null && errors.Count > 0)
						ResponseModel<List<Models.Budget.Order.OrderModel>>.FailureResponse(string.Join(", ", errors));

					var uValidators = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(validators?.Select(x => x.Id_Validator)?.ToList());
					var orderCompany = companyExtensionEntities.Find(x => x.CompanyId == orderEntity.CompanyId);

					response.Add(new Models.Budget.Order.OrderModel(orderEntity, bestellungEntity, projectEntity, articleEntity, bestellteArticelEntity, articlesEntities, fileEntity, validators, uValidators,
						this._user,
						Infrastructure.Data.Access.Tables.FNC.BestellungenAccess.GetReceptionsByOrderId_Count(orderEntity.OrderId)));
				}

				return ResponseModel<List<Models.Budget.Order.OrderModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Budget.Order.OrderModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Budget.Order.OrderModel>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(this._data) == null)
				return ResponseModel<List<Models.Budget.Order.OrderModel>>.FailureResponse("Project not found");

			return ResponseModel<List<Models.Budget.Order.OrderModel>>.SuccessResponse();
		}

		static byte[] getXLS(List<Infrastructure.Data.Entities.Tables.FNC.BestellungenEntity> bestellungEntities,
			List<Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity> bestellungenExtensionEntities,
			List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity> bestellte_ArtikelEntities,
			List<Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity> bestellteArtikelExtensionEntities, int? languageId, bool isFinal = false)
		{
			if(bestellungEntities == null || bestellungEntities.Count <= 0)
				return null;

			//var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(invoiceEntity?.IssuerId ?? -1);

			//int customerCompanyId = invoiceEntity?.CompanyId ?? -1;
			//var customerCompanyExtensionEntity = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(customerCompanyId);
			//var customerCompanyEntity = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(customerCompanyId)
			//	?? Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get()?[0];

			//var invoiceItemEntites = Infrastructure.Data.Access.Tables.FNC.InvoiceItemAccess.GetByInvoiceId(invoiceEntity.Id);

			var lang = languageId.HasValue
				? (Infrastructure.Services.Reporting.Models.FNC.ReportLanguage)languageId.Value
				: Infrastructure.Services.Reporting.Models.FNC.ReportLanguage.English;

			var billingCompanyExtension = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(1);
			var billingCompanyEntity = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(1);

			// - XLS process
			try
			{
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"Orders-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

				var file = new FileInfo(filePath);

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"OrderInvoice");

					// Keep track of the row that we're on, but start with four to skip the header
					var numberOfColumns = 12;

					// Add some formatting to the worksheet
					worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";

					// -
					foreach(var bestellung in bestellungEntities)
					{
						var bExtension = bestellungenExtensionEntities.FirstOrDefault(x => x.OrderId == bestellung.Nr);
						var bestelltArtikel = bestellte_ArtikelEntities.Where(x => x.Bestellung_Nr == bestellung.Nr)?.ToList();
						var bestelltExtension = bestellteArtikelExtensionEntities.Where(x => x.OrderId == bestellung.Nr)?.ToList();
						//
						addPo(worksheet, 1, 12, bestellung, bExtension, bestelltArtikel, bestelltExtension);
					}

					// Set some document properties
					package.Workbook.Properties.Title = $"Purchase Order";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";

					// save our new workbook and we are done!
					package.Save();

					return File.ReadAllBytes(filePath);
				}
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.StackTrace);
				throw;
			}
		}
		static void addPo(ExcelWorksheet worksheet, int startRowNumber, int columnsCount,
			Infrastructure.Data.Entities.Tables.FNC.BestellungenEntity bestellungenEntity,
			Infrastructure.Data.Entities.Tables.FNC.BestellungenExtensionEntity bestellungenExtensionEntity,
			List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity> bestellte_ArtikelEntities,
			List<Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity> bestellteArtikelExtensionEntities)
		{
			//int startColumnNumber = 1;


			// -
			worksheet.Cells[startRowNumber, columnsCount].Value = "PO";
			worksheet.Row(startRowNumber).Style.Font.Size = 16;
			worksheet.Row(startRowNumber).Style.Font.Bold = true;
			worksheet.Row(startRowNumber).Style.Font.UnderLine = true;

			//worksheet.Cells[startRowNumber + 1, columnsCount - 1].Value = "Invoice #:";
			//worksheet.Cells[startRowNumber + 1, columnsCount - 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
			//worksheet.Cells[startRowNumber + 1, columnsCount].Value = item.Number;
			//worksheet.Cells[startRowNumber + 1, columnsCount].Style.Font.Size = 14;
			//worksheet.Cells[startRowNumber + 1, columnsCount].Style.Font.Bold = true;
			//// -
			//worksheet.Cells[startRowNumber + 2, columnsCount - 1].Value = "Date:";
			//worksheet.Cells[startRowNumber + 2, columnsCount - 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
			//worksheet.Cells[startRowNumber + 2, columnsCount].Value = item.Date;
			//worksheet.Cells[startRowNumber + 2, columnsCount].Style.Font.Size = 14;
			//worksheet.Cells[startRowNumber + 2, columnsCount].Style.Font.Bold = true;
			//// -
			//worksheet.Cells[startRowNumber + 3, columnsCount - 1].Value = "PO #:";
			//worksheet.Cells[startRowNumber + 3, columnsCount - 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
			//worksheet.Cells[startRowNumber + 3, columnsCount].Value = item.PONumber;
			//worksheet.Cells[startRowNumber + 3, columnsCount].Style.Font.Size = 14;
			//worksheet.Cells[startRowNumber + 3, columnsCount].Style.Font.Bold = true;

			//// -
			//worksheet.Cells[startRowNumber + 4, startColumnNumber].Value = companyItem.Name;
			//worksheet.Cells[startRowNumber + 5, startColumnNumber].Value = $"{companyItem.Address}, {companyItem.PostalCode}"?.Trim(',')?.Trim();
			//worksheet.Cells[startRowNumber + 6, startColumnNumber].Value = $"{companyItem.City}, {companyItem.Country}"?.Trim(',')?.Trim();
			//worksheet.Cells[startRowNumber + 7, startColumnNumber].Value = $"Tel.: {companyItem.Telephone}, Fax:{companyItem.Fax}"?.Trim(',')?.Trim();
			//worksheet.Cells[startRowNumber + 8, startColumnNumber].Value = $"{companyItem.Email}"?.Trim(',')?.Trim();

			worksheet.Row(startRowNumber + 4).Style.Font.Size = 16;
			worksheet.Row(startRowNumber + 4).Style.Font.Bold = true;
		}
	}
}
