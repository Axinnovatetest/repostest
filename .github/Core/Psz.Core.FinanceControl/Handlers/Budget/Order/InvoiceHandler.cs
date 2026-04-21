using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget.Order
{
	using OfficeOpenXml;
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Drawing;
	using System.IO;
	using System.IO.Compression;

	public class InvoiceHandler: IHandle<UserModel, ResponseModel<string>>
	{
		private UserModel _user { get; set; }
		private int _data { get; set; }
		private string _languageCode { get; set; }
		public InvoiceHandler(int orderId, string langCode, UserModel user)
		{
			_user = user;
			_data = orderId;
			_languageCode = langCode;
		}
		public ResponseModel<string> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				//
				var invoiceItems = Infrastructure.Data.Access.Tables.FNC.InvoiceAccess.GetByOrderId(this._data);
				return ResponseModel<string>.SuccessResponse(generateInvoiceReport(invoiceItems[0].Id, null));

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<string> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<string>.AccessDeniedResponse();
			}

			var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(this._data, null);
			if(orderEntity == null)
				return ResponseModel<string>.FailureResponse(key: "1", value: "Order not found");

			var bookingItems = Infrastructure.Data.Access.Tables.FNC.InvoiceAccess.GetByOrderId(this._data);
			if(bookingItems == null || bookingItems.Count <= 0)
				return ResponseModel<string>.FailureResponse(key: "1", value: "No invoice found");

			if(bookingItems.Count > 1)
				return ResponseModel<string>.FailureResponse(key: "1", value: "Multiple invoices found");

			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<string>.FailureResponse(key: "1", value: "User not found");


			return ResponseModel<string>.SuccessResponse();
		}


		internal static string generateInvoiceReport(int orderId, int? languageId)
		{
			var report = generateReportDataPDF(orderId, languageId, true);
			return report != null ? Convert.ToBase64String(report) : string.Empty;
		}
		public static byte[] generateReportDataPDF(int invoiceId, int? languageId, bool isFinal = false)
		{
			var invoiceEntity = Infrastructure.Data.Access.Tables.FNC.InvoiceAccess.Get(invoiceId);
			if(invoiceEntity != null)
			{
				var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(invoiceEntity?.IssuerId ?? -1);

				int customerCompanyId = Reception.BookHandler.replaceInvoicingSite(invoiceEntity); // - 2023-04-13 PSP & GZ will be billed to TN  - invoiceEntity?.CompanyId ?? -1;
				var customerCompanyExtensionEntity = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(customerCompanyId);
				var customerCompanyEntity = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(customerCompanyId)
					?? Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get()?[0];

				var invoiceItemEntites = Infrastructure.Data.Access.Tables.FNC.InvoiceItemAccess.GetByInvoiceId(invoiceId);

				var lang = languageId.HasValue
					? (Infrastructure.Services.Reporting.Models.FNC.ReportLanguage)languageId.Value
					: Infrastructure.Services.Reporting.Models.FNC.ReportLanguage.English;

				var billingCompanyExtension = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(1);
				var billingCompanyEntity = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(1);

				// -
				return Module.ReportingService.GenerateFNCInvoiceReport(
						new Infrastructure.Services.Reporting.Models.FNC.InvoiceModel(userEntity, customerCompanyExtensionEntity, billingCompanyEntity, billingCompanyExtension, invoiceEntity, invoiceItemEntites),
						new Infrastructure.Services.Reporting.Models.FNC.InvoiceTemplateModel(invoiceEntity, invoiceItemEntites, customerCompanyEntity, lang), isFinal);
			}
			return null;
		}

		public static byte[] GetInvoiceXLS(List<int> orderIds)
		{
			var tempFolder = Path.Combine(Path.GetTempPath(), $"{DateTime.Now.ToString("yyyyMMddTHHmmss")}");
			int nbFiles = 0;
			if(Directory.Exists(tempFolder))
				Directory.Delete(tempFolder, true);

			Directory.CreateDirectory(tempFolder);

			foreach(var orderId in orderIds)
			{
				var orderEntity = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByOrderId(orderId);
				var invoiceEntities = Infrastructure.Data.Access.Tables.FNC.InvoiceAccess.GetByOrderId(orderId);
				if(invoiceEntities != null && invoiceEntities.Count > 0)
				{
					var poFolder = Path.Combine(tempFolder, orderEntity.OrderId.ToString());
					Directory.CreateDirectory(poFolder);
					foreach(var invoiceItem in invoiceEntities)
					{
						//var pdfContent = InvoiceHandler.generateReportDataPDF(invoiceItem.Id, null, true);
						var content = generateReportDataXLS(invoiceItem, null, true);
						System.IO.File.WriteAllBytes(Path.Combine(poFolder, $"INV-{invoiceItem.Id}.xlsx"), content);
						nbFiles++;
					}
				}
			}

			if(nbFiles > 0)
			{
				string zipPath = Path.Combine(Path.GetTempPath(), $"{DateTime.Now.ToString("yyyyMMddTHHmmss")}.zip");
				ZipFile.CreateFromDirectory(tempFolder, zipPath);
				return File.ReadAllBytes(zipPath);
			}

			return null;
		}
		static byte[] generateReportDataXLS(Infrastructure.Data.Entities.Tables.FNC.InvoiceEntity invoiceEntity, int? languageId, bool isFinal = false)
		{
			if(invoiceEntity == null)
				return null;

			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(invoiceEntity?.IssuerId ?? -1);

			int customerCompanyId = Reception.BookHandler.replaceInvoicingSite(invoiceEntity);// - 2023-04-13 PSP & GZ will be billed to TN  - invoiceEntity?.CompanyId ?? -1;
			var customerCompanyExtensionEntity = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(customerCompanyId);
			var customerCompanyEntity = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(customerCompanyId)
				?? Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get()?[0];

			var invoiceItemEntites = Infrastructure.Data.Access.Tables.FNC.InvoiceItemAccess.GetByInvoiceId(invoiceEntity.Id);

			var lang = languageId.HasValue
				? (Infrastructure.Services.Reporting.Models.FNC.ReportLanguage)languageId.Value
				: Infrastructure.Services.Reporting.Models.FNC.ReportLanguage.English;

			var billingCompanyExtension = Infrastructure.Data.Access.Tables.FNC.CompanyExtensionAccess.GetByCompany(1);
			var billingCompanyEntity = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(1);

			// -
			var template = new Infrastructure.Services.Reporting.Models.FNC.InvoiceModel(userEntity, customerCompanyExtensionEntity, billingCompanyEntity, billingCompanyExtension, invoiceEntity, invoiceItemEntites);
			var data = new Infrastructure.Services.Reporting.Models.FNC.InvoiceTemplateModel(invoiceEntity, invoiceItemEntites, customerCompanyEntity, lang);

			// - XLS process
			try
			{
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"OrderInvoice-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

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
					addHeader(worksheet, 1, 12, billingCompanyEntity, data);
					addBilledTo(worksheet, 10, 7, data);
					addItems(worksheet, 17, numberOfColumns, template?.OrderItems,
						data.SummarySubtotal, data.SummaryDiscount, data.SummaryTotal,
						data.LastPageText1, data.LastPageText2, data.LastPageText3, data.LastPageText4, data.LastPageText5,
						data.LastPageText6, data.LastPageText7, data.LastPageText8, data.LastPageText9, data.LastPageText10);

					addFooter(worksheet, 17 + (template?.OrderItems?.Count ?? 0) + 15, 12, template, data);

					// Set some document properties
					package.Workbook.Properties.Title = $"Order Invoice";
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
		static void addHeader(ExcelWorksheet worksheet, int startRowNumber, int columnsCount,
			Infrastructure.Data.Entities.Tables.STG.CompanyEntity companyItem,
			Infrastructure.Services.Reporting.Models.FNC.InvoiceTemplateModel item)
		{
			int startColumnNumber = 1;

			// - logo
			var picture = worksheet.Drawings.AddPicture("logo", new MemoryStream(companyItem.Logo));
			picture.SetPosition(0, 0, 0, 0);
			picture.SetSize(40);

			// -
			worksheet.Cells[startRowNumber, columnsCount].Value = "INVOICE";
			worksheet.Row(startRowNumber).Style.Font.Size = 16;
			worksheet.Row(startRowNumber).Style.Font.Bold = true;
			worksheet.Row(startRowNumber).Style.Font.UnderLine = true;

			worksheet.Cells[startRowNumber + 1, columnsCount - 1].Value = "Invoice #:";
			worksheet.Cells[startRowNumber + 1, columnsCount - 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
			worksheet.Cells[startRowNumber + 1, columnsCount].Value = item.Number;
			worksheet.Cells[startRowNumber + 1, columnsCount].Style.Font.Size = 14;
			worksheet.Cells[startRowNumber + 1, columnsCount].Style.Font.Bold = true;
			// -
			worksheet.Cells[startRowNumber + 2, columnsCount - 1].Value = "Date:";
			worksheet.Cells[startRowNumber + 2, columnsCount - 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
			worksheet.Cells[startRowNumber + 2, columnsCount].Value = item.Date;
			worksheet.Cells[startRowNumber + 2, columnsCount].Style.Font.Size = 14;
			worksheet.Cells[startRowNumber + 2, columnsCount].Style.Font.Bold = true;
			// -
			worksheet.Cells[startRowNumber + 3, columnsCount - 1].Value = "PO #:";
			worksheet.Cells[startRowNumber + 3, columnsCount - 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
			worksheet.Cells[startRowNumber + 3, columnsCount].Value = item.PONumber;
			worksheet.Cells[startRowNumber + 3, columnsCount].Style.Font.Size = 14;
			worksheet.Cells[startRowNumber + 3, columnsCount].Style.Font.Bold = true;

			// -
			worksheet.Cells[startRowNumber + 4, startColumnNumber].Value = companyItem.Name;
			worksheet.Cells[startRowNumber + 5, startColumnNumber].Value = $"{companyItem.Address}, {companyItem.PostalCode}"?.Trim(',')?.Trim();
			worksheet.Cells[startRowNumber + 6, startColumnNumber].Value = $"{companyItem.City}, {companyItem.Country}"?.Trim(',')?.Trim();
			worksheet.Cells[startRowNumber + 7, startColumnNumber].Value = $"Tel.: {companyItem.Telephone}, Fax:{companyItem.Fax}"?.Trim(',')?.Trim();
			worksheet.Cells[startRowNumber + 8, startColumnNumber].Value = $"{companyItem.Email}"?.Trim(',')?.Trim();
			worksheet.Row(startRowNumber + 4).Style.Font.Size = 16;
			worksheet.Row(startRowNumber + 4).Style.Font.Bold = true;
		}
		static void addBilledTo(ExcelWorksheet worksheet, int startRowNumber, int columnsCount,
			Infrastructure.Services.Reporting.Models.FNC.InvoiceTemplateModel item)
		{
			int billingSpan = 7;
			int startColumnNumber = 1;
			columnsCount = 7;

			startRowNumber++;
			worksheet.Cells[startRowNumber, startColumnNumber].Value = "BILL TO";
			worksheet.Row(startRowNumber).Style.Font.Size = 14;
			worksheet.Row(startRowNumber).Style.Font.Bold = true;

			worksheet.Cells[startRowNumber + 1, startColumnNumber].Value = item.CustomerCompanyName;
			worksheet.Cells[startRowNumber + 2, startColumnNumber].Value = item.CustomerCompanyStreet;
			worksheet.Cells[startRowNumber + 3, startColumnNumber].Value = item.CustomerCompanyCity;
			worksheet.Cells[startRowNumber + 4, startColumnNumber].Value = item.CustomerCompanyPhone;
			worksheet.Row(startRowNumber + 1).Style.Font.Size = 16;
			worksheet.Row(startRowNumber + 1).Style.Font.Bold = true;
			// - merge
			worksheet.Cells[startRowNumber + 0, startColumnNumber, startRowNumber + 0, startColumnNumber + billingSpan].Merge = true;
			worksheet.Cells[startRowNumber + 1, startColumnNumber, startRowNumber + 1, startColumnNumber + billingSpan].Merge = true;
			worksheet.Cells[startRowNumber + 2, startColumnNumber, startRowNumber + 2, startColumnNumber + billingSpan].Merge = true;
			worksheet.Cells[startRowNumber + 3, startColumnNumber, startRowNumber + 3, startColumnNumber + billingSpan].Merge = true;
			worksheet.Cells[startRowNumber + 4, startColumnNumber, startRowNumber + 4, startColumnNumber + billingSpan].Merge = true;
			// - borders
			worksheet.Cells[startRowNumber, 1, startRowNumber, columnsCount + 1].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
			worksheet.Cells[startRowNumber, 1, startRowNumber, columnsCount + 1].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
			worksheet.Cells[startRowNumber + 4, 1, startRowNumber + 4, columnsCount + 1].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
			worksheet.Cells[startRowNumber, columnsCount + 1, startRowNumber + 4, columnsCount + 1].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
			// - colors
			worksheet.Cells[startRowNumber, 1, startRowNumber, columnsCount].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
			worksheet.Cells[startRowNumber, 1, startRowNumber, columnsCount].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D8D6D6"));
		}
		static void addItems(ExcelWorksheet worksheet, int startRowNumber, int columnsCount,
			List<Infrastructure.Services.Reporting.Models.FNC.InvoiceModel.OrderItemModel> items,
			string subTotal, string discount, string total,
			string lastPageText1, string lastPageText2, string lastPageText3, string lastPageText4, string lastPageText5,
			string lastPageText6, string lastPageText7, string lastPageText8, string lastPageText9, string lastPageText10)
		{
			int startColumnNumber = 1;
			int headerRowNumber = startRowNumber;

			worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Items";
			worksheet.Row(headerRowNumber).Style.Font.Size = 14;
			worksheet.Row(headerRowNumber).Style.Font.Bold = true;
			headerRowNumber += 1;

			int designationSpan = 6;
			// Start adding the header
			worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Pos";
			worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Designation";
			worksheet.Cells[headerRowNumber, startColumnNumber + 2 + designationSpan].Value = "Quantity";
			worksheet.Cells[headerRowNumber, startColumnNumber + 3 + designationSpan].Value = "Unit Price";
			worksheet.Cells[headerRowNumber, startColumnNumber + 4 + designationSpan].Value = "Handling Fees";
			worksheet.Cells[headerRowNumber, startColumnNumber + 5 + designationSpan].Value = "Amount";
			// - span designation to 7 cols
			worksheet.Cells[headerRowNumber, startColumnNumber + 1, headerRowNumber, startColumnNumber + 1 + designationSpan].Merge = true;
			worksheet.Row(headerRowNumber).Style.Font.Size = 12;
			worksheet.Row(headerRowNumber).Style.Font.Bold = true;
			worksheet.Cells[headerRowNumber, 1, headerRowNumber, columnsCount].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
			worksheet.Cells[headerRowNumber, 1, headerRowNumber, columnsCount].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D8D6D6"));
			worksheet.Cells[headerRowNumber, 1, headerRowNumber, columnsCount].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
			worksheet.Cells[headerRowNumber, 1, headerRowNumber, columnsCount].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
			worksheet.Cells[headerRowNumber, 1, headerRowNumber, columnsCount].Style.ShrinkToFit = false;

			var rowNumber = headerRowNumber + 1;
			// Loop through 
			foreach(var w in items)
			{
				worksheet.Cells[rowNumber, startColumnNumber + 0].Value = w?.Position;
				worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Designation;
				worksheet.Cells[rowNumber, startColumnNumber + 2 + designationSpan].Value = w?.Quantity;
				worksheet.Cells[rowNumber, startColumnNumber + 3 + designationSpan].Value = w?.UnitPrice;
				worksheet.Cells[rowNumber, startColumnNumber + 4 + designationSpan].Value = w?.HandlingFees;
				worksheet.Cells[rowNumber, startColumnNumber + 5 + designationSpan].Value = w?.Amount;

				worksheet.Row(rowNumber).Height = 16;
				// - span designation to 7 cols
				worksheet.Cells[rowNumber, startColumnNumber + 1, rowNumber, startColumnNumber + 1 + designationSpan].Merge = true;
				worksheet.Cells[rowNumber, 1, rowNumber, columnsCount].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
				worksheet.Cells[rowNumber, 1, rowNumber, columnsCount].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
				rowNumber += 1;
			}

			// - itmes footer
			worksheet.Cells[rowNumber + 0, columnsCount - 1].Value = "Subtotal:";
			worksheet.Cells[rowNumber + 1, columnsCount - 1].Value = "Discount:";
			worksheet.Cells[rowNumber + 2, columnsCount - 1].Value = "Total:";
			worksheet.Cells[rowNumber + 0, columnsCount].Value = subTotal;
			worksheet.Cells[rowNumber + 1, columnsCount].Value = discount;
			worksheet.Cells[rowNumber + 2, columnsCount].Value = total;
			// - align right
			worksheet.Cells[rowNumber + 0, columnsCount - 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
			worksheet.Cells[rowNumber + 1, columnsCount - 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
			worksheet.Cells[rowNumber + 2, columnsCount - 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
			// - border
			worksheet.Cells[rowNumber + 2, columnsCount, rowNumber + 2, columnsCount].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
			// - bold
			worksheet.Cells[rowNumber + 2, columnsCount - 1, rowNumber + 2, columnsCount].Style.Font.Bold = true;

			// - texts
			worksheet.Cells[rowNumber + 3, 1].Value = lastPageText10;
			worksheet.Row(rowNumber + 3).Style.Font.Size = 10;
			worksheet.Cells[rowNumber + 4, 1].Value = lastPageText3;
			worksheet.Row(rowNumber + 4).Style.Font.Size = 10;
			worksheet.Cells[rowNumber + 5, 1].Value = lastPageText4;
			worksheet.Row(rowNumber + 5).Style.Font.Size = 10;
			worksheet.Cells[rowNumber + 6, 1].Value = lastPageText5;
			worksheet.Row(rowNumber + 6).Style.Font.Size = 10;
			worksheet.Cells[rowNumber + 7, 1].Value = lastPageText6;
			worksheet.Row(rowNumber + 7).Style.Font.Size = 10;
			worksheet.Cells[rowNumber + 8, 1].Value = lastPageText7;
			worksheet.Row(rowNumber + 8).Style.Font.Size = 10;
			worksheet.Cells[rowNumber + 9, 1].Value = lastPageText8;
			worksheet.Row(rowNumber + 9).Style.Font.Size = 10;
			worksheet.Cells[rowNumber + 10, 1].Value = lastPageText9;
			worksheet.Row(rowNumber + 10).Style.Font.Size = 9;
		}
		static void addFooter(ExcelWorksheet worksheet, int startRowNumber, int columnsCount,
			Infrastructure.Services.Reporting.Models.FNC.InvoiceModel item,
			Infrastructure.Services.Reporting.Models.FNC.InvoiceTemplateModel invoiceItem)
		{
			// - borders
			worksheet.Cells[startRowNumber, 1, startRowNumber, columnsCount].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

			// -

			worksheet.Cells[startRowNumber, 1].Value = item.FooterAddress;
			worksheet.Cells[startRowNumber, 4].Value = item.FooterVAT_Title;
			worksheet.Cells[startRowNumber, 5].Value = item.FooterVAT;
			worksheet.Cells[startRowNumber, 7].Value = item.FooterManager_Title;
			worksheet.Cells[startRowNumber, 8].Value = $"{item.FooterManager1}";
			worksheet.Cells[startRowNumber, columnsCount - 1].Value = item.FooterHRB_Title;
			worksheet.Cells[startRowNumber, columnsCount].Value = item.FooterHRB;
			worksheet.Row(startRowNumber).Style.Font.Size = 7;

			// - row 2
			startRowNumber++;
			worksheet.Cells[startRowNumber, 1].Value = $"{item.FooterCity} {item.FooterPostalCode}";
			worksheet.Cells[startRowNumber, 4].Value = item.FooterSite;
			worksheet.Cells[startRowNumber, 8].Value = item.FooterManager2;
			worksheet.Cells[startRowNumber, columnsCount - 1].Value = item.FooterEmail_Title;
			worksheet.Cells[startRowNumber, columnsCount].Value = item.FooterEmail;
			worksheet.Row(startRowNumber).Style.Font.Size = 7;

			// - row 3
			startRowNumber++;
			worksheet.Cells[startRowNumber, 1].Value = $"{item.FooterPhone}";
			worksheet.Cells[startRowNumber, 4].Value = item.FooterFax_Title;
			worksheet.Cells[startRowNumber, 5].Value = item.FooterFax;
			worksheet.Cells[startRowNumber, 7].Value = item.FooterTaxNumber_Tilte;
			worksheet.Cells[startRowNumber, 8].Value = item.FooterTaxNumber;
			worksheet.Cells[startRowNumber, columnsCount - 1].Value = item.FooterCustomsNumber_Title;
			worksheet.Cells[startRowNumber, columnsCount].Value = item.FooterCustomsNumber;
			worksheet.Row(startRowNumber).Style.Font.Size = 7;

			// - row 4
			startRowNumber++;
			startRowNumber++;
			worksheet.Cells[startRowNumber, 1].Value = $"{item.FooterBankDetails_Title}";
			worksheet.Cells[startRowNumber, 4].Value = item.FooterAccount_Title;
			worksheet.Cells[startRowNumber, 5].Value = item.FooterBLZ_Title;
			worksheet.Cells[startRowNumber, 7].Value = item.FooterIBAN_Title;
			worksheet.Cells[startRowNumber, columnsCount - 1].Value = item.FooterSWIFT_Title;
			worksheet.Row(startRowNumber).Style.Font.Size = 7;
			// - row 5
			startRowNumber++;
			worksheet.Cells[startRowNumber, 1].Value = $"{item.FooterBankDetails1}";
			worksheet.Cells[startRowNumber, 4].Value = item.FooterAccount1;
			worksheet.Cells[startRowNumber, 5].Value = item.FooterBLZ1;
			worksheet.Cells[startRowNumber, 7].Value = item.FooterIBAN1;
			worksheet.Cells[startRowNumber, columnsCount - 1].Value = item.FooterSWIFT1;
			worksheet.Row(startRowNumber).Style.Font.Size = 7;
			// - row 6
			startRowNumber++;
			worksheet.Cells[startRowNumber, 1].Value = $"{item.FooterBankDetails2}";
			worksheet.Cells[startRowNumber, 4].Value = item.FooterAccount2;
			worksheet.Cells[startRowNumber, 5].Value = item.FooterBLZ2;
			worksheet.Cells[startRowNumber, 7].Value = item.FooterIBAN2;
			worksheet.Cells[startRowNumber, columnsCount - 1].Value = item.FooterSWIFT2;
			worksheet.Row(startRowNumber).Style.Font.Size = 7;
			// - row 6
			startRowNumber++;
			worksheet.Cells[startRowNumber, 1].Value = $"{item.FooterBankDetails3}";
			worksheet.Cells[startRowNumber, 4].Value = item.FooterAccount3;
			worksheet.Cells[startRowNumber, 5].Value = item.FooterBLZ3;
			worksheet.Cells[startRowNumber, 7].Value = item.FooterIBAN3;
			worksheet.Cells[startRowNumber, columnsCount - 1].Value = item.FooterSWIFT3;
			worksheet.Row(startRowNumber).Style.Font.Size = 7;

			// - row 7
			startRowNumber++;
			startRowNumber++;
			worksheet.Cells[startRowNumber, columnsCount].Value = item.FormNumber;
			worksheet.Row(startRowNumber).Style.Font.Size = 7;
		}
	}
}
