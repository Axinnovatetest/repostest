using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Blanket;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Psz.Core.CustomerService.Handlers.Blanket
{
	public class ExportConvertedRahmensToExcelHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{

		private List<ConvertedRahmensModel> _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public ExportConvertedRahmensToExcelHandler(Identity.Models.UserModel user, List<ConvertedRahmensModel> data)
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

				byte[] response = null;
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"Converted_Rahmens-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");
				var file = new FileInfo(filePath);
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				using(var package = new ExcelPackage(file))
				{
					var worksheet = package.Workbook.Worksheets.Add("Sheet1");
					var headerRowNumber = 1;
					var startColumnNumber = 1;
					var numberOfColumns = 4;

					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Rahmennummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Supplier";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Errors";

					var rowNumber = headerRowNumber + 1;
					if(_data != null && _data.Count > 0)
					{
						foreach(var p in _data)
						{
							worksheet.Cells[rowNumber, startColumnNumber].Value = p.ArtikelNummer;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = p.RahmenNr;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = p.Supplier;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = p.Errors != null && p.Errors.Count > 0
								? string.Join("||", p.Errors)
								: "";
							rowNumber += 1;
						}
					}
					if(_data != null && _data.Count > 0)
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

					// Fit the columns according to its content
					for(int i = 1; i <= numberOfColumns; i++)
					{
						worksheet.Column(i).AutoFit();
					}

					// Set some document properties
					package.Workbook.Properties.Title = "Converted Rahmens";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";

					// save our new workbook and we are done!
					package.Save();
					response = File.ReadAllBytes(filePath);
				}

				return ResponseModel<byte[]>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<byte[]> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}

			return ResponseModel<byte[]>.SuccessResponse();
		}

	}
}
