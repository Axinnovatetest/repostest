using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Lagebewegung;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Lagerbewegung
{
	public class InternTransfer2DetailsExcelHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _type { get; set; }




		public InternTransfer2DetailsExcelHandler(int Type, Identity.Models.UserModel user)
		{
			_user = user;
			_type = Type;


		}
		public InternTransfer2DetailsExcelHandler()
		{

		}

		public ResponseModel<byte[]> Handle()
		{
			try
			{
				var validationResponse = Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var transfer = new LagerBewegungTreeModel();


				int allCount = 0;
				if(this._type == 1)
				{
					//----------------transfer Intern WS IN--------------------------
					var responseWSIN = new LagerBewegungTreeModel();
					List<int> listlagerVonWSIN = new List<int> { 42, 57 };
					List<int> listlagerNachWSIN = new List<int> { 90 };
					var transferWSIN = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.GetListBewegung(listlagerVonWSIN, listlagerNachWSIN);
					if(transferWSIN != null && transferWSIN.Count > 0)
					{
						responseWSIN.details = transferWSIN
								.Select(d => new LagerbewegungDetailsModel(d)).ToList();
					}

					transfer = responseWSIN;
				}
				else if(this._type == 2)
				{
					//----------------transfer Intern IN WS--------------------------
					var responseINWS = new LagerBewegungTreeModel();
					List<int> listlagerVonINWS = new List<int> { 90 };
					List<int> listlagerNachINWS = new List<int> { 42, 57 };
					var transferINWS = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.GetListBewegung(listlagerVonINWS, listlagerNachINWS);
					if(transferINWS != null && transferINWS.Count > 0)
					{
						responseINWS.details = transferINWS
								.Select(d => new LagerbewegungDetailsModel(d)).ToList();
					}


					transfer = responseINWS;
				}
				else if(this._type == 3)
				{
					//----------------transfer Intern TN IN--------------------------
					var responseTNIN = new LagerBewegungTreeModel();
					List<int> listlagerVonTNIN = new List<int> { 7, 60, 56, 61 };
					List<int> listlagerNachTNIN = new List<int> { 90 };
					var transferTNIN = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.GetListBewegung(listlagerVonTNIN, listlagerNachTNIN);
					if(transferTNIN != null && transferTNIN.Count > 0)
					{
						responseTNIN.details = transferTNIN
								.Select(d => new LagerbewegungDetailsModel(d)).ToList();

					}

					transfer = responseTNIN;
				}
				else if(this._type == 4)
				{
					//----------------transfer Intern IN WS--------------------------
					var responseINTN = new LagerBewegungTreeModel();
					List<int> listlagerVonINTN = new List<int> { 90 };
					List<int> listlagerNachINTN = new List<int> { 7, 60, 56, 61 };
					var transferINTN = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.GetListBewegung(listlagerVonINTN, listlagerNachINTN);
					if(transferINTN != null && transferINTN.Count > 0)
					{
						responseINTN.details = transferINTN
								.Select(d => new LagerbewegungDetailsModel(d)).ToList();

					}

					transfer = responseINTN;
				}




				return ResponseModel<byte[]>.SuccessResponse(SaveToExcelFile(transfer, this._type));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<byte[]> Validate()
		{
			if(_user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<byte[]>.AccessDeniedResponse();
			}

			return ResponseModel<byte[]>.SuccessResponse();
		}
		internal byte[] SaveToExcelFile(LagerBewegungTreeModel transfer, int type)
		{
			try
			{

				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"InternTransfer-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");

				var file = new FileInfo(filePath);
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Intern Tansfer");

					// Keep track of the row that we're on, but start with four to skip the header

					var headerRowNumber = 3;
					var startColumnNumber = 1;
					var numberOfColumns = 6;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					worksheet.DefaultRowHeight = 8;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
					worksheet.DefaultRowHeight = 18;


					using(var range = worksheet.Cells[1, 1, 1, 6])
					{
						worksheet.Cells[1, 1, 1, 6].Merge = true;
						string titre = "";
						if(type == 1)
						{
							titre = "Transfer Intern  WS -> Intern";
						}
						else if(type == 2)
						{
							titre = "Transfer Intern  Intern -> WS";
						}
						else if(type == 3)
						{
							titre = "Transfer Intern  TN / BETN -> Intern";
						}
						else if(type == 4)
						{
							titre = "Transfer Intern  Intern -> TN / BETN";
						}
						worksheet.Cells[1, 1].Value = titre;
						worksheet.Cells[1, 1, 1, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
						range.Style.Font.Color.SetColor(Color.Black);
						range.Style.Font.Size = 17;
						range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;


					}


					worksheet.SelectedRange["A3:F3"].Style.Font.Bold = true;
					worksheet.SelectedRange["A3:F3"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					worksheet.SelectedRange["A3:F3"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));
					// Start adding the header
					//worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Kunde";
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Datum";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Anzahl";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Lager von";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Lager nach";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Gerbucht von";




					var rowNumber = headerRowNumber + 1;

					//Loop through
					if(transfer != null && transfer.details.Count > 0)
					{
						foreach(var p in transfer.details)
						{
							//worksheet.Cells[rowNumber, startColumnNumber].Value = p.kunde;
							worksheet.Cells[rowNumber, startColumnNumber].Value = p.datum != null ? p.datum.Value.ToString("dd/MM/yyyy") : "n/a";
							worksheet.Cells[rowNumber, startColumnNumber].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = p.artikelnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = p.anzahl;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = p.lagerVon;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = p.lagerNach;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = p.gebuchtVon;


							worksheet.Row(rowNumber).Height = 18;
							rowNumber += 1;

						}
					}
					// Set some document properties
					if(transfer != null && transfer.details.Count > 0)
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

					// Fit the columns according to its content
					for(int i = 1; i <= numberOfColumns; i++)
					{
						worksheet.Column(i).AutoFit();
					}
					// Thick countour
					using(var range = worksheet.Cells[1, 1, rowNumber - 1, numberOfColumns])
					{
						range.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thick);
					}

					// Set some document properties
					package.Workbook.Properties.Title = $"Intern Transfer";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";

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

	}
}