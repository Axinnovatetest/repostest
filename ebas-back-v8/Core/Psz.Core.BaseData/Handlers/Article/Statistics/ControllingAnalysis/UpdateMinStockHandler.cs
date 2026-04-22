using Infrastructure.Data.Access.Tables;
using Infrastructure.Data.Access.Tables.PRS;
using Infrastructure.Data.Entities.Tables.BSD;
using Infrastructure.Data.Entities.Tables.PRS;
using OfficeOpenXml;
using Psz.Core.BaseData.Models.Article.Statistics.ControllingAnalysis;
using Psz.Core.Common.Helpers;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
namespace Psz.Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis
{
	public class UpdateMinStockHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<MinStockUpdateResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private ImportFileModel _data { get; set; }
		public UpdateMinStockHandler(Identity.Models.UserModel user, Core.Common.Models.ImportFileModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<MinStockUpdateResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var errors = new List<string>();
				var articles = new List<KeyValuePair<int, string>>();


				var data = ReadFromExcel(this._data.FilePath, errors, articles, _user);
				if(errors != null && errors.Count > 0)
				{
					return ResponseModel<List<MinStockUpdateResponseModel>>.FailureResponse(errors);
				}

				if(articles == null || articles.Count <= 0)
				{
					return ResponseModel<List<MinStockUpdateResponseModel>>.SuccessResponse(null);
				}
				List<MinStockUpdateResponseModel> results = new List<MinStockUpdateResponseModel>();
				int insertResult = ArticleMinStockUpdateHistoryAccess.Insert(data);
				if(insertResult > 0)
				{
					foreach(var oldStockItem in data)
					{

						LagerAccess.updateLagerMindestbestandByLagerId((int)oldStockItem.LagerId, Convert.ToInt32(oldStockItem.NewMinStock), (int)oldStockItem.ArticleId);

						results.Add(new MinStockUpdateResponseModel
						{
							Articlenumber = oldStockItem.ArticleNumber,
							OldMinStock = oldStockItem.OldMinStock ?? 0,
							NewMinStock = oldStockItem.NewMinStock ?? 0,
							LagerId = (int)oldStockItem.LagerId,
							ArticleId = (int)oldStockItem.ArticleId,
							Updatedate = (DateTime)oldStockItem.UpdateDate,
						});

					}
					return ResponseModel<List<MinStockUpdateResponseModel>>.SuccessResponse(results);
				}
				else
				{
					return ResponseModel<List<MinStockUpdateResponseModel>>.FailureResponse("Error when inserting article min stock changes");
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}


		public static List<ArticleMinStockUpdateHistoryEntity> ReadFromExcel(string filePath, List<string> errors, List<KeyValuePair<int, string>> articleIds, Identity.Models.UserModel user)
		{
			try
			{
				var fileInfo = new System.IO.FileInfo(filePath);

				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				var package = new ExcelPackage(fileInfo);
				var worksheet = package.Workbook.Worksheets[0];
				var rowStart = worksheet.Dimension.Start.Row;
				var rowEnd = worksheet.Dimension.End.Row;
				rowEnd -= 0;
				// get number of rows and columns in the sheet
				var rows = worksheet.Dimension.Rows;
				var columns = worksheet.Dimension.Columns;
				var startRowNumber = 2;
				var startColNumber = 1;
				var MIN_ROWS_COUNT = 1;
				var MIN_COLUMNS_COUNT = 3;

				if(rows >= MIN_ROWS_COUNT && columns >= MIN_COLUMNS_COUNT)
				{
					var data = new List<ArticleMinStockUpdateHistoryEntity> { };

					// loop through the worksheet rows and columns
					for(int i = startRowNumber; i <= rowEnd; i++)
					{
						try
						{
							if(String.IsNullOrEmpty(Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber])))
							{
								errors.Add($"Row {i} is empty");
								continue;
							}

							var _articleNumber = Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber]).Trim();
							var _lageIdFromExcel = Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + 1]);
							var _newMinStock = Formatters.XLS.GetDecimalNullable(worksheet.Cells[i, startColNumber + 2]);
							ArtikelEntity ArtikelByNummer = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByArtikelNummer(_articleNumber);
							var hauptlagers = Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.GetHauptLagerIds();

							int lagerId = Formatters.XLS.GetInt(_lageIdFromExcel);

							LagerEntity lageById = LagerAccess.GetByLagerortIdAndArtikelNR(lagerId, _articleNumber.ToString());
							//article number empty in excel file
							if(_articleNumber is null)
							{
								errors.Add($"Row {i}: Article number empty in excel file");
								continue;
							}
							if(_lageIdFromExcel == "")
							{
								errors.Add($"Row {i}: Lage is empty in excel file");
								continue;
							}
							//new min stock empty in excel file 
							if(_newMinStock is null)
							{
								errors.Add($"Row {i} : New min stock is empty in excel file .");
								continue;
							}
							//MinBestand > 0
							if(_newMinStock < 0)
							{
								errors.Add($"Row {i}: New min stock should be positive .");
								continue;
							}
							//lage id positive
							if(lagerId < 0)
							{
								errors.Add($"Row {i}: Lagerort_id Should be positive");
								continue;
							}
							//lage not found
							if(lageById == null)
							{
								errors.Add($"Row {i}: Lage" + $" [{lagerId}] not found.");
								continue;
							}
							//artikel not found
							if(ArtikelByNummer == null)
							{
								errors.Add($"Row {i}: Article" + $" [{_articleNumber}] not found.");
								continue;
							}
							//Warengruppe = { EF, HW }
							if(String.Compare(ArtikelByNummer.Warengruppe, "EF", StringComparison.OrdinalIgnoreCase) != 0 &&
			String.Compare(ArtikelByNummer.Warengruppe, "HW", StringComparison.OrdinalIgnoreCase) != 0)
							{
								errors.Add($"Row {i}: Article number [{_articleNumber}] Should be of type EF or HW.");
								continue;
							}
							if(hauptlagers.Exists(x => x == lagerId) == false)
							{
								errors.Add($"Row {i}: Lager [{lagerId}] is not a Hauptlager.");
								continue;
							}

							ArticleMinStockUpdateHistoryEntity articleToAdd = new ArticleMinStockUpdateHistoryEntity
							{
								ArticleNumber = _articleNumber.ToString(),
								ArticleId = (int)ArtikelByNummer.ArtikelNr,
								OldMinStock = Convert.ToInt32(lageById.Mindestbestand),
								NewMinStock = (int)Convert.ToInt32(_newMinStock),
								LagerId = lagerId,
								UpdateDate = DateTime.Now,
								UpdateUserId = user.Id,
								UpdateUserName = user.Username
							};
							// - everything is good
							data.Add(articleToAdd);
							articleIds.Add(new KeyValuePair<int, string>(ArtikelByNummer.ArtikelNr, ArtikelByNummer.ArtikelNummer));

						} catch(Exception exceptionInternal)
						{
							Infrastructure.Services.Logging.Logger.Log(exceptionInternal.Message + "\n" + exceptionInternal.StackTrace);
							errors.Add($"Row {i}: unknown error.");
						}
					}
					return data;
				}
				else
				{
					errors.Add($"Invalid file format: {rows} Rows X {columns} Columns");
					return null;
				}
			} catch(System.Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.Message + "\n" + exception.StackTrace);
				throw;
			}
		}

		public ResponseModel<List<MinStockUpdateResponseModel>> Validate()
		{
			if(this._user == null)
			{
				return ResponseModel<List<MinStockUpdateResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<MinStockUpdateResponseModel>>.SuccessResponse();
		}
	}
}
