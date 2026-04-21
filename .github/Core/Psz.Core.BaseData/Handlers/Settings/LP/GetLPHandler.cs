using Geocoding;
using iText.Layout.Font;
using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.LP
{
	public class GetLPHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.CustomerSupplierLP.LPExtendedModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetLPHandler(Identity.Models.UserModel user, int nr)
		{
			this._user = user;
			this._data = nr;
		}
		public ResponseModel<List<Models.CustomerSupplierLP.LPExtendedModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var LPEntity = Infrastructure.Data.Access.Joins.Kunden_LieferantenAccess.GetLPExtended(this._data);
				var response = new List<Models.CustomerSupplierLP.LPExtendedModel>();
				if(LPEntity != null && LPEntity.Count > 0)
				{
					foreach(var item in LPEntity)
					{
						response.Add(new Models.CustomerSupplierLP.LPExtendedModel(item));
					}
				}
				return ResponseModel<List<Models.CustomerSupplierLP.LPExtendedModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<Models.CustomerSupplierLP.LPExtendedModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.CustomerSupplierLP.LPExtendedModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.CustomerSupplierLP.LPExtendedModel>>.SuccessResponse();
		}
		public byte[] GetDataXLS()
		{
			try
			{
				var data = this.Handle();

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage())
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"Lieferant - LP");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 18;

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
					worksheet.Cells[1, 1].Value = $"Lieferant - LP - {DateTime.Now.ToString("dd/MM/yyyy")}";
					worksheet.Cells[1, 1].Style.Font.Size = 16;



					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Name";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Standardlieferant";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Bezeichnung 1";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Bezeichnung 2";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Bestell-Nr";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Einkaufspreis";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "Angebot";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "Datum";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Telefon";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Fax";
					worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "Email";
					worksheet.Cells[headerRowNumber, startColumnNumber + 12].Value = "Mindestbestellmenge";
					worksheet.Cells[headerRowNumber, startColumnNumber + 13].Value = "Wiederbeschäffungszeitraum";
					worksheet.Cells[headerRowNumber, startColumnNumber + 14].Value = "Aktuelle VPE/Losgröße";
					worksheet.Cells[headerRowNumber, startColumnNumber + 15].Value = "Aktiv seit 2 Jahre";
					worksheet.Cells[headerRowNumber, startColumnNumber + 16].Value = "Bedarf PO 360 Tage";
					worksheet.Cells[headerRowNumber, startColumnNumber + 17].Value = "Verwendungsnachweis";


					var rowNumber = headerRowNumber + 1;
					var redCellrows = new List<int>();
					if(data.Success == true && data.Body.Count > 0)
					{
						// -
						var proofOfUsageEntities = Infrastructure.Data.Access.Joins.Kunden_LieferantenAccess.GetLpProofOfUsage(data.Body.Select(x => x.ArticleId));
						// Loop through 
						foreach(var w in data.Body)
						{
							var proofOfUsage = proofOfUsageEntities.Where(x => x.Key == w?.ArticleId)?.Select(x => x.Value);
							worksheet.Cells[rowNumber, startColumnNumber].Value = w?.Articlenumber;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.Name1;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.Standard_supplier == true ? "Yes" : "No";
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.Article_Designation;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.Article_Designation_2;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.Order_number;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.purchasing_price;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.Offer;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = w?.Offer_date.HasValue == true ? w.Offer_date.Value : "";
							worksheet.Cells[rowNumber, startColumnNumber + 8].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = w?.Phone;
							worksheet.Cells[rowNumber, startColumnNumber + 10].Value = w?.Fax;
							worksheet.Cells[rowNumber, startColumnNumber + 11].Value = w?.Email;
							worksheet.Cells[rowNumber, startColumnNumber + 12].Value = w?.Minimum_Order_Quantity;
							worksheet.Cells[rowNumber, startColumnNumber + 13].Value = w?.Replacement_period;
							worksheet.Cells[rowNumber, startColumnNumber + 14].Value = w?.PackagingUnit;
							worksheet.Cells[rowNumber, startColumnNumber + 15].Value = w?.ActiveSince24Months == true ? "Yes" : "No";
							worksheet.Cells[rowNumber, startColumnNumber + 16].Value = w?.OpenQuantityNext360Days;
							//worksheet.Cells[rowNumber, startColumnNumber + 17].Value =  string.Join(", ", proofOfUsage);

							int nbProofOfUsage = proofOfUsage.Count();
							int iterator = 0;

							if(proofOfUsage.Count() > 5000)
							{
								redCellrows.Add(rowNumber);
								nbProofOfUsage = 5000;
								iterator = 1;
							}

							for(int i = iterator; i < nbProofOfUsage; i++)
							{
								worksheet.Cells[rowNumber, startColumnNumber + 17 + i].Value = proofOfUsage.ElementAt(i);
							}

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
							if(redCellrows.Count > 0)
							{
								foreach(var item in redCellrows)
								{
									worksheet.Cells[item, startColumnNumber + 17].Value = $"Article is used in more then 5000 FG, we show only the first 5000";
									worksheet.Cells[item, startColumnNumber + 17].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
									worksheet.Cells[item, startColumnNumber + 17].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D00"));
								}
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
					package.Workbook.Properties.Title = $"Lieferant - LP";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";

					// - for Formulas
					//worksheet.Calculate();

					// save our new workbook and we are done!
					package.Save();

					return package.GetAsByteArray();
				}
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.StackTrace);
				throw;
			}
		}
	}
}
