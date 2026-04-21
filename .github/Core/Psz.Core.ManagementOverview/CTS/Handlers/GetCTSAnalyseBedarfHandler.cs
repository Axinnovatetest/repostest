using MoreLinq;
using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.ManagementOverview.CTS.Models;
using Psz.Core.ManagementOverview.Statistics.Enums;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Psz.Core.ManagementOverview.CTS.Handlers
{
	public class GetCTSAnalyseBedarfHandler: IHandle<BedarfBestandRequestModel, ResponseModel<IPaginatedResponseModel<BedarfBestandItemClass>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private BedarfBestandRequestModel _data { get; set; }

		public GetCTSAnalyseBedarfHandler(Identity.Models.UserModel user, BedarfBestandRequestModel data)
		{
			this._user = user;
			_data = data;
		}

		public ResponseModel<IPaginatedResponseModel<BedarfBestandItemClass>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}


				Infrastructure.Data.Access.Settings.PaginModel dataPaging = null;
				var date = this._data.DateTill ?? DateTime.Today.AddMonths(3);

				if(_data.isPaginated)
				{
					dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
					{
						FirstRowNumber = this._data.PageSize > 0 ? (this._data.RequestedPage * this._data.PageSize) : 0,
						RequestRows = this._data.PageSize
					};
				}

				var data = new List<BedarfBestandItemClass>();
				switch(this._data.ProductType)
				{
					case ProductType.ExtraROH:
						List<BedarfBestandItemModel> allExtraROH =
						Infrastructure.Data.Access.Joins.MGO.MainViewsAccess.GetCTSBedarfAnalyseSecAccess(date, (int)StatisticType.BedrafExtra, dataPaging)
						?.Select(x => new BedarfBestandItemModel(x))?.ToList();
						data = getBedraf(allExtraROH, ProductType.ExtraROH);
						return ResponseModel<IPaginatedResponseModel<BedarfBestandItemClass>>.SuccessResponse(
						new IPaginatedResponseModel<BedarfBestandItemClass>
						{
							Items = getBedraf(allExtraROH, ProductType.ExtraROH),
							PageRequested = this._data.RequestedPage,
							PageSize = this._data.PageSize,
							TotalCount = allExtraROH?.FirstOrDefault().TotalCount ?? 0,
							TotalPageCount = this._data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(allExtraROH?.FirstOrDefault().TotalCount ?? 0) / this._data.PageSize)) : 0
						}
					);
					case ProductType.MissingROH:
					default:
						List<BedarfBestandItemModel> allMissingROH =
							Infrastructure.Data.Access.Joins.MGO.MainViewsAccess.GetCTSBedarfAnalyseSecAccess(date, (int)StatisticType.BedrafNonExtra, dataPaging)
							?.Select(x => new BedarfBestandItemModel(x))?.ToList();
						data = getBedraf(allMissingROH, ProductType.MissingROH);
						return ResponseModel<IPaginatedResponseModel<BedarfBestandItemClass>>.SuccessResponse(
							new IPaginatedResponseModel<BedarfBestandItemClass>
							{
								Items = getBedraf(allMissingROH, ProductType.MissingROH),
								PageRequested = this._data.RequestedPage,
								PageSize = this._data.PageSize,
								TotalCount = allMissingROH?.FirstOrDefault().TotalCount ?? 0,
								TotalPageCount = this._data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(allMissingROH?.FirstOrDefault().TotalCount ?? 0) / this._data.PageSize)) : 0
							}
						);
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}


		public ResponseModel<IPaginatedResponseModel<BedarfBestandItemClass>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<IPaginatedResponseModel<BedarfBestandItemClass>>.AccessDeniedResponse();
			}

			// - 
			var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
			if(userEntity == null)
				return ResponseModel<IPaginatedResponseModel<BedarfBestandItemClass>>.FailureResponse(key: "1", value: "User not found");

			return ResponseModel<IPaginatedResponseModel<BedarfBestandItemClass>>.SuccessResponse();
		}

		//public ResponseModel<BedarfBestandResponseModel> Handle()
		//{
		//	

		//}
		//public ResponseModel<BedarfBestandResponseModel> Validate()
		//{
		//	if(this._user == null/*|| this._user.Access.____*/)
		//	{
		//		return ResponseModel<BedarfBestandResponseModel>.AccessDeniedResponse();
		//	}

		//	// - 
		//	var userEntity = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(this._user.Id);
		//	if(userEntity == null)
		//		return ResponseModel<BedarfBestandResponseModel>.FailureResponse(key: "1", value: "User not found");

		//	return ResponseModel<BedarfBestandResponseModel>.SuccessResponse();
		//}

		private List<BedarfBestandItemClass> getBedraf(List<BedarfBestandItemModel> allROH, ProductType productType)
		{
			decimal? maxRoh = allROH.Max(x => x.DiffPrice);
			decimal? minRoh = allROH.Where(x => x.DiffPrice > -20).Min(x => x.DiffPrice);
			decimal? moyenRoh = allROH.Where(x => x.DiffPrice > -20).Sum(x => x.DiffPrice) / allROH.Count;

			decimal pasClassRoh = 0;

			int classAvalue = (int)(maxRoh - pasClassRoh);
			int classBvalue = (int)(minRoh + pasClassRoh);
			int classCvalue = (int)(pasClassRoh);
			int classDvalue = (int)(minRoh + (int)pasClassRoh);

			if(productType == ProductType.ExtraROH)
			{
				classAvalue = 5000;
				classBvalue = 2000;
				classCvalue = 500;
				classDvalue = 100;
			}
			if(productType == ProductType.MissingROH)
			{
				classAvalue = -10;
				classBvalue = -200;
				classCvalue = -400;
				classDvalue = -900;
			}
			if(maxRoh.HasValue && minRoh.HasValue && moyenRoh.HasValue)
			{
				pasClassRoh = Math.Abs((moyenRoh.Value) / (4 / 2));
			}

			List<BedarfBestandItemClass> ItemsROH = new List<BedarfBestandItemClass>
				{
					new BedarfBestandItemClass
					{
						ClassName="D",
						ClassValue="D: Diff Preis > "+classAvalue.ToString(),
						Items=allROH.Where(x=>x.DiffPrice>=classAvalue).OrderByDescending(x=>x.DiffPrice).ToList()
					},
					new BedarfBestandItemClass
					{
						ClassName="C",
						ClassValue="C: Diff Preis ["+classBvalue.ToString()+".."+(classAvalue).ToString()+"]",
						Items=allROH.Where(x=>x.DiffPrice<classAvalue
						&& x.DiffPrice>=classBvalue).OrderByDescending(x=>x.DiffPrice).ToList()
					},
					new BedarfBestandItemClass
					{
						ClassName="B",
						ClassValue="B: Diff Preis ["+classCvalue.ToString()+".."+classBvalue.ToString()+"]",
						Items=allROH.Where(x=>x.DiffPrice>=classCvalue
						&& x.DiffPrice<classBvalue).OrderByDescending(x=>x.DiffPrice).ToList()
					}
					,
					new BedarfBestandItemClass
					{
						ClassName="A",
						ClassValue="A: Diff Preis < "+classCvalue.ToString(),
						Items=allROH.Where(x=> x.DiffPrice<classCvalue).OrderByDescending(x=>x.DiffPrice).ToList()
					}
				};
			return ItemsROH;
		}

		public byte[] GetDataXLS()
		{
			try
			{
				Infrastructure.Data.Access.Settings.PaginModel dataPaging = null;
				var date = this._data.DateTill ?? DateTime.Today.AddMonths(3);
				List<BedarfBestandItemClass> lsItems = new List<BedarfBestandItemClass>();
				string fileName = string.Empty;

				switch(this._data.ProductType)
				{
					case ProductType.ExtraROH:
						List<BedarfBestandItemModel> allExtraROH =
						Infrastructure.Data.Access.Joins.MGO.MainViewsAccess.GetCTSBedarfAnalyseSecAccess(date, (int)StatisticType.BedrafExtra, dataPaging)
						?.Select(x => new BedarfBestandItemModel(x))?.ToList();
						lsItems = getBedraf(allExtraROH, ProductType.MissingROH);
						break;
					case ProductType.MissingROH:
						List<BedarfBestandItemModel> allMissingROH =
							Infrastructure.Data.Access.Joins.MGO.MainViewsAccess.GetCTSBedarfAnalyseSecAccess(date, (int)StatisticType.BedrafNonExtra, dataPaging)
							?.Select(x => new BedarfBestandItemModel(x))?.ToList();
						lsItems = getBedraf(allMissingROH, ProductType.MissingROH);

						break;
					default:
						break;
				}


				// make sure that we have data to work with.
				if(lsItems.Count == 0)
					return null;

				fileName = this._data.ProductType == ProductType.ExtraROH ? "ExtraROH" : "MissingROH";
				//var data = this.Handle();

				// -
				var tempFolder = System.IO.Path.GetTempPath();


				var filePath = System.IO.Path.Combine(tempFolder, $"{fileName}-{DateTime.Now.ToString("yyyyMMddTHHmmff")}.xlsx");

				var file = new FileInfo(filePath);

				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage(file))
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"{fileName}");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 10;

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
					worksheet.Cells[1, 1].Value = $"{fileName}";
					worksheet.Cells[1, 1].Style.Font.Size = 16;

					var rowNumber = 1;
					#region Items

					headerRowNumber = rowNumber + 1;
					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber + 0].Value = "Artikel Nummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Diff Price";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Bestell Nr";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Diff Quantity";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Einkaufs Preis";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Gesamt Preis";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Name";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "ROH Bestand";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "ROH Quantity";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Wert LagerBestandBedar";

					rowNumber = headerRowNumber + 1;

					//Loop through
					foreach(var x in lsItems)
					{
						foreach(var w in x.Items)
						{
							worksheet.Cells[rowNumber, startColumnNumber + 1].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;

							worksheet.Cells[rowNumber, startColumnNumber + 0].Value = w?.Artikelnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.DiffPrice;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.Bestell_Nr;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = w?.DiffQuantity;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = w?.Einkaufspreis;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = w?.Gesamtpreis;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = w?.Name1;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = w?.ROH_Bestand;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = w?.ROH_Quantity;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = w?.Wert_LagerBestandBedarf;

							worksheet.Row(rowNumber).Height = 18;
							rowNumber += 1;
						}
					}
					#endregion items

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

					//Doc content

					using(var range = worksheet.Cells[headerRowNumber + 1, 1, rowNumber - 1, numberOfColumns])
					{
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.White);
						range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
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
					package.Workbook.Properties.Title = $"{fileName}";
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


	}
}
