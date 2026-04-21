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
using System.Linq;
namespace Psz.Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis
{
	public class UpdateMinStockRohHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<MinStockUpdateResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private ImportFileModel _data { get; set; }
		public UpdateMinStockRohHandler(Identity.Models.UserModel user, Core.Common.Models.ImportFileModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<MinStockUpdateResponseModel>> Handle()
		{

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				botransaction.beginTransaction();

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

				// -
				List<MinStockUpdateResponseModel> results = new List<MinStockUpdateResponseModel>();

				#region // -- transaction-based logic -- //
				var updatedResults = LagerAccess.UpdateLagerMindestbestand(data.Select(x => new Tuple<int, int, decimal>(x.LagerId ?? 0, x.ArticleId ?? 0, x.NewMinStock ?? 0)), botransaction.connection, botransaction.transaction);
				var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetMinimal(data.Select(x => x.ArticleId ?? 0)?.ToList(), botransaction.connection, botransaction.transaction);
				var logs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
				foreach(var item in updatedResults)
				{
					var article = articleEntities.FirstOrDefault(x => x.ArtikelNr == item.Item2);
					var newData = data.FirstOrDefault(x => x.ArticleId == item.Item2 && x.LagerId == item.Item1);
					results.Add(new MinStockUpdateResponseModel
					{
						Articlenumber = article.ArtikelNummer,
						OldMinStock = item.Item3,
						NewMinStock = newData.NewMinStock ?? 0,
						LagerId = item.Item1,
						ArticleId = item.Item2,
						Updatedate = DateTime.Now,
					});
					logs.Add(ObjectLogHelper.getLog(this._user, item.Item2, $"Mindestbestand | Lager [{item.Item1}]", $"{item.Item3}",
								$"{newData.NewMinStock ?? 0}", $"{Enums.ObjectLogEnums.Objects.Article.GetDescription()}", Enums.ObjectLogEnums.LogType.Edit));
				}
				// - save logs 
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(logs, botransaction.connection, botransaction.transaction);

				#endregion // -- transaction-based logic -- //


				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					// -
					return ResponseModel<List<MinStockUpdateResponseModel>>.SuccessResponse(results);
				}
				else
				{
					return ResponseModel<List<MinStockUpdateResponseModel>>.FailureResponse("Transaction error");
				}
			} catch(Exception e)
			{
				botransaction.rollback();
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

					var prodLagers = Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.GetProductionLagers(Module.AppSettings.ProductionLagerIds);
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

							int lagerId = Formatters.XLS.GetInt(_lageIdFromExcel);

							LagerEntity lagerById = LagerAccess.GetByLagerortIdAndArtikelNR(lagerId, _articleNumber.ToString());
							//article number empty in excel file
							if(_articleNumber is null)
							{
								errors.Add($"Row {i}: Article number empty in excel file.");
								continue;
							}
							if(_lageIdFromExcel == "")
							{
								errors.Add($"Row {i}: Lager is empty in excel file.");
								continue;
							}
							//new min stock empty in excel file 
							if(_newMinStock is null)
							{
								errors.Add($"Row {i} : New min stock is empty in excel file.");
								continue;
							}
							//MinBestand > 0
							if(_newMinStock < 0)
							{
								errors.Add($"Row {i}: New min stock should be positive.");
								continue;
							}
							//lage id positive
							if(lagerId < 0)
							{
								errors.Add($"Row {i}: Lagerort ID Should be positive.");
								continue;
							}
							//lage not found
							if(lagerById == null)
							{
								errors.Add($"Row {i}: Lager [{lagerId}] not found.");
								continue;
							}
							//artikel not found
							if(ArtikelByNummer == null)
							{
								errors.Add($"Row {i}: Article [{_articleNumber}] not found.");
								continue;
							}
							//Warengruppe = ROH
							if(String.Compare(ArtikelByNummer.Warengruppe, "ROH", StringComparison.OrdinalIgnoreCase) != 0)
							{
								errors.Add($"Row {i}: Article number [{_articleNumber}] Should be of type ROH.");
								continue;
							}
							if(prodLagers.Exists(x => x.Lagerort_id == lagerId) == false)
							{
								errors.Add($"Row {i}: Update Min stock for Lager [{lagerId}] is not allowed.");
								continue;
							}

							ArticleMinStockUpdateHistoryEntity articleToAdd = new ArticleMinStockUpdateHistoryEntity
							{
								ArticleNumber = _articleNumber.ToString(),
								ArticleId = (int)ArtikelByNummer.ArtikelNr,
								OldMinStock = Convert.ToInt32(lagerById.Mindestbestand),
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
