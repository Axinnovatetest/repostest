using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Lagerbewegung
{
	public class EntnahmeWertExcelHandler: IHandle<Identity.Models.UserModel, ResponseModel<byte[]>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private DateTime _DateD { get; set; }
		private DateTime _DateF { get; set; }
		private int _lager1 { get; set; }
		private int _lager2 { get; set; }
		private int _type { get; set; }
		public string _artikelnummer { get; set; }




		public EntnahmeWertExcelHandler(DateTime D1, DateTime D2, int L1, int Type, string Artikelnummer, Identity.Models.UserModel user)
		{
			_user = user;
			_DateD = D1;
			_DateF = D2;
			_lager1 = L1;
			_type = Type;
			_artikelnummer = Artikelnummer;


		}
		public EntnahmeWertExcelHandler()
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

				var ModelLGT = Psz.Core.Logistics.Module.LGT.LGTList.Where(x => x.Lager_Id == this._lager1).ToList();
				this._lager2 = 0;
				if(ModelLGT != null && ModelLGT.Count() > 0)
				{
					this._lager2 = ModelLGT[0].Lager_P_Id;
				}



				//----------------Emtnahme Wert with EK--------------------------
				var response = new List<Models.Lagebewegung.EntnahmeWertTreeDetailsModel>();
				var entnahmeWertWithEKEntities = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.GetEntnahmeWertWithEK(this._DateD, this._DateF, this._lager1, this._lager2, this._type, this._artikelnummer);
				if(entnahmeWertWithEKEntities != null && entnahmeWertWithEKEntities.Count > 0)
					response = entnahmeWertWithEKEntities.Select(k => new Models.Lagebewegung.EntnahmeWertTreeDetailsModel(k)).ToList();

				//----------------Emtnahme Wert without EK--------------------------
				var response0 = new List<Models.Lagebewegung.EntnahmeWertTreeDetailsModel>();
				var entnahmeWertWithoutEKEntities = Infrastructure.Data.Access.Tables.Logistics.LagebewegungAccess.GetEntnahmeWertWithoutEK(_DateD, _DateF, this._lager1, this._lager2, this._type, this._artikelnummer);
				if(entnahmeWertWithoutEKEntities != null && entnahmeWertWithoutEKEntities.Count > 0)
					response0 = entnahmeWertWithoutEKEntities.Select(k => new Models.Lagebewegung.EntnahmeWertTreeDetailsModel(k)).ToList();




				return ResponseModel<byte[]>.SuccessResponse(SaveToExcelFile(response, response0, this._DateD, this._DateF, this._type));
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
		internal byte[] SaveToExcelFile(List<Models.Lagebewegung.EntnahmeWertTreeDetailsModel> entnahmeWertWithEKEntities, List<Models.Lagebewegung.EntnahmeWertTreeDetailsModel> entnahmeWertWithoutEKEntities, DateTime d1, DateTime d2, int type)
		{
			try
			{

				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"EntnahmeWert-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");

				var file = new FileInfo(filePath);
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"EntnahmeWert");

					// Keep track of the row that we're on, but start with four to skip the header

					var headerRowNumber = 3;
					var startColumnNumber = 1;
					var numberOfColumns = 7;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					worksheet.DefaultRowHeight = 8;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
					worksheet.DefaultRowHeight = 18;


					using(var range = worksheet.Cells[1, 1, 1, 7])
					{
						worksheet.Cells[1, 1, 1, 7].Merge = true;
						string titre = "";
						if(type == 1)
						{
							titre = "Auswertung mit Berechnung";
						}
						else if(type == 2)
						{
							titre = "Auswertung ohne Berechnung";
						}
						worksheet.Cells[1, 1].Value = titre;
						worksheet.Cells[1, 1, 1, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
						range.Style.Font.Color.SetColor(Color.Black);
						range.Style.Font.Size = 17;
						range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;


					}
					using(var range = worksheet.Cells[2, 1, 2, 7])
					{
						string x = "Vom : " + (d1 != null ? d1.ToString("dd/MM/yyyy") : "n/a") + " Bis : " + (d2 != null ? d2.ToString("dd/MM/yyyy") : "n/a");
						worksheet.Cells[2, 1, 2, 7].Merge = true;
						worksheet.Cells[2, 1].Value = x;

						worksheet.Cells[2, 1, 2, 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#FFE699");
						range.Style.Fill.BackgroundColor.SetColor(colFromHex);
						range.Style.Font.Color.SetColor(Color.Black);
						range.Style.Font.Size = 15;
						range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;



					}

					worksheet.SelectedRange["A3:G3"].Style.Font.Bold = true;
					worksheet.SelectedRange["A3:G3"].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					worksheet.SelectedRange["A3:G3"].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));
					// Start adding the header
					//worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Kunde";
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Datum";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Bezeichnung 1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Anzahl";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Zu FA";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Grund";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Kosten";



					var rowNumber = headerRowNumber + 1;
					var rowNumberHeader2 = headerRowNumber + 1;
					//Loop through
					if(entnahmeWertWithEKEntities != null && entnahmeWertWithEKEntities.Count > 0)
					{
						foreach(var p in entnahmeWertWithEKEntities)
						{
							//worksheet.Cells[rowNumber, startColumnNumber].Value = p.kunde;
							worksheet.Cells[rowNumber, startColumnNumber].Value = p.datum != null ? p.datum.Value.ToString("dd/MM/yyyy") : "n/a";
							worksheet.Cells[rowNumber, startColumnNumber].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = p.artikelnummer;
							//	worksheet.Cells[rowNumber, startColumnNumber + 3].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = p.bezeichnung1;//
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = p.anzahl;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = p.zuFA;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = p.grund;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = p.kosten;


							worksheet.Row(rowNumber).Height = 18;
							rowNumber += 1;
							rowNumberHeader2 += 1;
						}
					}
					if(entnahmeWertWithoutEKEntities != null && entnahmeWertWithoutEKEntities.Count > 0)
					{
						using(var range = worksheet.Cells[rowNumber, 1, rowNumber, numberOfColumns])
						{
							worksheet.Cells[rowNumber, 1, rowNumber, 3].Merge = true;
							range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
							range.Style.Fill.BackgroundColor.SetColor(Color.White);
							range.Style.Font.Color.SetColor(Color.Red);
							;
							range.Style.Font.Size = 17;


						}
						worksheet.Cells[rowNumber, startColumnNumber].Value = "Artikel ohne defeniert EK";
						rowNumber += 1;
						foreach(var p in entnahmeWertWithoutEKEntities)
						{
							//worksheet.Cells[rowNumber, startColumnNumber].Value = p.kunde;
							worksheet.Cells[rowNumber, startColumnNumber].Value = p.datum != null ? p.datum.Value.ToString("dd/MM/yyyy") : "n/a";
							worksheet.Cells[rowNumber, startColumnNumber].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = p.artikelnummer;
							//	worksheet.Cells[rowNumber, startColumnNumber + 3].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = p.bezeichnung1;//
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = p.anzahl;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = p.zuFA;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = p.grund;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = p.kosten;


							worksheet.Row(rowNumber).Height = 18;
							rowNumber += 1;
						}


					}
					//-------Somme------------------

					// Set some document properties
					if(entnahmeWertWithEKEntities != null && entnahmeWertWithEKEntities.Count > 0)
					{
						using(var range = worksheet.Cells[headerRowNumber + 1, 1, rowNumberHeader2 - 1, numberOfColumns])
						{
							range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
							range.Style.Fill.BackgroundColor.SetColor(Color.White);
							range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						}

					}
					if(entnahmeWertWithoutEKEntities != null && entnahmeWertWithoutEKEntities.Count > 0)
					{
						using(var range = worksheet.Cells[rowNumberHeader2 + 1, 1, rowNumber - 1, numberOfColumns])
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
					package.Workbook.Properties.Title = $"EntnahmeWert";
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