using Newtonsoft.Json;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.CsStatistics.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.CustomerService.CsStatistics.Handlers
{
	using OfficeOpenXml;
	using System.Drawing;
	using System.IO;
	using System.Linq;
	public class GetKapazitatCZKurzHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KapazitatKurzModel>>>
	{
		private KapzitatkurzEntryModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetKapazitatCZKurzHandler(Identity.Models.UserModel user, KapzitatkurzEntryModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<KapazitatKurzModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new List<KapazitatKurzModel>();
				var kapazitatKurzEntity = new List<Infrastructure.Data.Entities.Joins.CTS.KapazitatKurzEntity>();
				switch(_data.Lager)
				{
					case (int)Enums.OrderEnums.KapazitatLager.CZ:
						kapazitatKurzEntity = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetKapazitatCZKurz(_data.From, _data.To, _data.ProdTage);
						break;
					case (int)Enums.OrderEnums.KapazitatLager.TN:
						kapazitatKurzEntity = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetKapazitatTHKurz(_data.From, _data.To, _data.ProdTage);
						break;
					case (int)Enums.OrderEnums.KapazitatLager.AL:
						kapazitatKurzEntity = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetKapazitatALKurz(_data.From, _data.To, _data.ProdTage);
						break;
					case (int)Enums.OrderEnums.KapazitatLager.BETN:
						kapazitatKurzEntity = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetKapazitatBETNKurz(_data.From, _data.To, _data.ProdTage);
						break;
					case (int)Enums.OrderEnums.KapazitatLager.WS:
						kapazitatKurzEntity = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetKapazitatKHTNKurz(_data.From, _data.To, _data.ProdTage);
						break;
					case (int)Enums.OrderEnums.KapazitatLager.GZTN:
						kapazitatKurzEntity = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetKapazitatGZTNKurz(_data.From, _data.To, _data.ProdTage);
						break;
					default:
						kapazitatKurzEntity = null;
						break;
				}

				if(kapazitatKurzEntity != null && kapazitatKurzEntity.Count > 0)
					response = kapazitatKurzEntity.Select(x => new KapazitatKurzModel(x)).ToList();

				return ResponseModel<List<KapazitatKurzModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: {JsonConvert.SerializeObject(this._data)}");
				throw;
			}
		}
		public ResponseModel<List<KapazitatKurzModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<KapazitatKurzModel>>.AccessDeniedResponse();
			}
			if(_data.ProdTage.HasValue && _data.ProdTage <= 0)
				return ResponseModel<List<KapazitatKurzModel>>.FailureResponse($"invalid work days");
			return ResponseModel<List<KapazitatKurzModel>>.SuccessResponse();
		}
		public byte[] GetDataXLS()
		{
			try
			{
				var data = this.Handle();

				// -
				var tempFolder = System.IO.Path.GetTempPath();
				var filePath = System.IO.Path.Combine(tempFolder, $"Kapacitat-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

				var file = new FileInfo(filePath);

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Kapacitat Kurz");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 12;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					worksheet.DefaultRowHeight = 11;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
					worksheet.Row(2).Height = 20;
					worksheet.Row(1).Height = 30;
					worksheet.Row(headerRowNumber).Height = 20;

					// Pre Header
					worksheet.Cells[1, 1, 1, numberOfColumns].Merge = true;
					worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
					worksheet.Cells[1, 1].Value = $"Kapacitat Kurz - {DateTime.Now.ToString("dd/MM/yyyy")}";
					worksheet.Cells[1, 1].Style.Font.Size = 16;



					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Kunde";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "FA";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Wunschtermin";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "TerminProd";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Auftragsmenge";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Artickelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Kunden";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Auftragszeit (h)";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Umsatz CZ";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Anzahl MA";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Lagerort ID";
					worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "Stundensatz";



					var rowNumber = headerRowNumber + 1;
					if(data.Success == true && data.Body.Count > 0)
					{
						// Loop through 
						foreach(var w in data.Body)
						{
							worksheet.Cells[rowNumber, startColumnNumber].Value = w?.Kunde;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.FA;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.Wunschtermin.HasValue == true ? w?.Wunschtermin.Value : "";
							worksheet.Cells[rowNumber, startColumnNumber + 2].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.TerminProd.HasValue == true ? w?.TerminProd.Value : "";
							worksheet.Cells[rowNumber, startColumnNumber + 3].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.Auftragsmenge;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.Artikelnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.Kunden;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.Auftragszeit_h;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = w?.UmsatzCZ;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = w?.AnzahlMA;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 10].Value = w?.Lagerort_id;
							worksheet.Cells[rowNumber, startColumnNumber + 11].Value = w?.Stundensatz;
							worksheet.Cells[rowNumber, startColumnNumber + 11].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;

							worksheet.Row(rowNumber).Height = 18;
							rowNumber += 1;
						}
					}

					//Pre + Header // - [FromRow, FromCol, ToRow, ToCol]
					using(var range = worksheet.Cells[1, 1, headerRowNumber, numberOfColumns])
					{
						range.Style.Font.Bold = true;
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#8EA9DB"));
						range.Style.Font.Color.SetColor(Color.Black);
						range.Style.ShrinkToFit = false;
					}
					// Darker Blue in Top cell
					worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D9E1F2"));

					if(data.Success == true)
					{
						// Doc content
						if(data.Body != null && data.Body.Count > 0)
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
					package.Workbook.Properties.Title = $"Kapacitat";
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
