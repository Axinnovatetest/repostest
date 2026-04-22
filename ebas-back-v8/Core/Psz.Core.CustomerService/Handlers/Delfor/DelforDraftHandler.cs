using OfficeOpenXml;
using OfficeOpenXml.Style;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace Psz.Core.CustomerService.Handlers.Delfor
{
	public class DelforDraftHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{

		private Identity.Models.UserModel _user { get; set; }
		public DelforDraftHandler(Identity.Models.UserModel user)
		{
			this._user = user;
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

				var response = GenerateEmptyDraft();

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


		internal static byte[] GenerateEmptyDraft()
		{
			var tempFolder = System.IO.Path.GetTempPath();
			var filePath = System.IO.Path.Combine(tempFolder, $"Delfor Draft-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");

			var file = new FileInfo(filePath);
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
			using(var package = new ExcelPackage(file))
			{
				ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Delfor");
				//line item
				worksheet.Cells[1, 1].Value = "Position";
				worksheet.Cells[2, 1].Value = "Material";
				worksheet.Cells[3, 1].Value = "PSZ Artikelnnumer";
				worksheet.Cells[4, 1].Value = "Eingeteilte Menge";
				worksheet.Cells[5, 1].Value = "Gelieferte Menge";
				worksheet.Cells[6, 1].Value = "Letzter Wareneing";
				worksheet.Cells[7, 1].Value = "Letzte Lieferung";
				worksheet.Cells[8, 1].Value = "Am";
				worksheet.Cells[9, 1].Value = "Lieferscheinnummer";
				//line item plans
				worksheet.Cells["C10"].Value = "Liefertermin";
				worksheet.Cells["D10"].Value = "Einteilungs-FZ";
				worksheet.Cells["E10"].Value = "Menge";
				worksheet.Cells["F10"].Value = "Abw.";
				//styling
				string[] titlesCells = { "C10", "D10", "E10", "F10", "B10" };
				worksheet.Column(2).Width = 50;
				foreach(var cell in titlesCells)
				{
					worksheet.Cells[cell].Style.Border.Top.Style = ExcelBorderStyle.Thin;
					worksheet.Cells[cell].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
					worksheet.Cells[cell].Style.Border.Left.Style = ExcelBorderStyle.None;
					worksheet.Cells[cell].Style.Border.Right.Style = ExcelBorderStyle.None;
					worksheet.Cells[cell].Style.VerticalAlignment = ExcelVerticalAlignment.Top;

				}
				var autofitColumns = new List<int> { 1, 3, 4, 5, 6 };
				foreach(int col in autofitColumns)
				{
					worksheet.Column(col).AutoFit();
				}
				package.Workbook.Properties.Title = "Delfor Draft";
				package.Workbook.Properties.Author = "PSZ ERP";
				package.Workbook.Properties.Company = "PSZ ERP";

				package.Save();

				return File.ReadAllBytes(filePath);
			}
		}
	}
}
