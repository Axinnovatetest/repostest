using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.CsStatistics.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Psz.Core.CustomerService.CsStatistics.Handlers
{
	public class GetAuswertungFAHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private AuswertungFAEntryModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetAuswertungFAHandler(Identity.Models.UserModel user, AuswertungFAEntryModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<byte[]> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var AuswertungFAEntity = Infrastructure.Data.Access.Tables.CTS.Sicht_FA_AenderuengshistorieAccess.GetByLagerAndDate(_data.Lager, _data.From, _data.To);
				var response = SaveToExcelFile(AuswertungFAEntity, _data.From, _data.To);


				return ResponseModel<byte[]>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		internal static byte[] SaveToExcelFile(
		   List<Infrastructure.Data.Entities.Tables.CTS.Sicht_FA_AenderuengshistorieEntity> ErstelltFGEntity, DateTime DateFrom, DateTime DateTo)
		{
			try
			{
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"Auswertung FA-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");

				var file = new FileInfo(filePath);
				ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Auswertung FA");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 1;
					var startColumnNumber = 1;
					var numberOfColumns = 30;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					//
					//worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
					worksheet.Row(1).Style.Font.Bold = true;
					worksheet.Row(1).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					worksheet.Row(1).Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));

					// Start adding the header
					IEnumerable<PropertyDescriptor> props = TypeDescriptor.GetProperties(typeof(Infrastructure.Data.Entities.Tables.CTS.Sicht_FA_AenderuengshistorieEntity)).OfType<PropertyDescriptor>();
					var count = 0;
					foreach(var prop in props.ToList())
					{
						worksheet.Cells[headerRowNumber, startColumnNumber + count].Value = prop.Name;
						count++;
					}
					worksheet.Cells[headerRowNumber, startColumnNumber + count + 1].Value = "Datum Von";
					worksheet.Cells[headerRowNumber, startColumnNumber + count + 2].Value = "Datum Bis";


					var rowNumber = headerRowNumber + 1;
					//Loop through
					if(ErstelltFGEntity != null && ErstelltFGEntity.Count > 0)
					{
						foreach(var p in ErstelltFGEntity)
						{
							var count2 = 0;
							foreach(PropertyInfo prop in p.GetType().GetProperties())
							{
								if(prop.PropertyType == typeof(DateTime?))
									worksheet.Cells[rowNumber, startColumnNumber + count2].Value = prop.GetValue(p, null)?.ToString();
								else
									worksheet.Cells[rowNumber, startColumnNumber + count2].Value = prop.GetValue(p, null);

								count2++;
							}
							worksheet.Cells[rowNumber, startColumnNumber + count2 + 1].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
							worksheet.Cells[rowNumber, startColumnNumber + count2 + 1].Value = DateFrom;
							worksheet.Cells[rowNumber, startColumnNumber + count2 + 2].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
							worksheet.Cells[rowNumber, startColumnNumber + count2 + 2].Value = DateTo;
							rowNumber += 1;
						}
					}
					// Doc content
					if(ErstelltFGEntity != null && ErstelltFGEntity.Count > 0)
					{
						using(var range = worksheet.Cells[headerRowNumber + 1, 1, rowNumber - 1, numberOfColumns])
						{
							range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
							range.Style.Fill.BackgroundColor.SetColor(Color.White);
							range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						}
					}

					// Thick countour
					using(var range = worksheet.Cells[1, 1, rowNumber - 1, numberOfColumns])
					{
						range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
					}
					// Set some document properties
					package.Workbook.Properties.Title = "Auswertung FA";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";

					worksheet.Column(1).Width = 25;
					worksheet.Column(2).Width = 15;
					worksheet.Column(9).Width = 25;
					// - for Formulas
					//worksheet.Calculate();
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
		public ResponseModel<byte[]> Validate()
		{
			if(this._user == null/*
                || this._user.access.____*/)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}
			return ResponseModel<byte[]>.SuccessResponse();
		}
	}
}
