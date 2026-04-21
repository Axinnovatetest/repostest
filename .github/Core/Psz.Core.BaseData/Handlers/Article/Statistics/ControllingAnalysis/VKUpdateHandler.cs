using System;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.ControllingAnalysis
{
	using OfficeOpenXml;
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Collections.Generic;
	using System.Drawing;
	using System.IO;
	using System.Linq;

	public class VKUpdateHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Article.Statistics.ControllingAnalysis.VKUpdateResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Core.Common.Models.ImportFileModel _data { get; set; }


		public VKUpdateHandler(Identity.Models.UserModel user, Core.Common.Models.ImportFileModel data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<List<Models.Article.Statistics.ControllingAnalysis.VKUpdateResponseModel>> Handle()
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

				// -
				var errors = new List<string>();
				var articles = new List<KeyValuePair<int, string>>();
				var Staffelpreis_KonditionzuordnungEntities = new List<Infrastructure.Data.Entities.Tables.PRS.Staffelpreis_KonditionzuordnungEntity_2>();
				var ArtikelSalesExtensionEntities = new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity>();

				// -
				var data = ReadFromExcel(this._data.FilePath, errors, articles, Staffelpreis_KonditionzuordnungEntities, ArtikelSalesExtensionEntities);
				if(errors != null && errors.Count > 0)
				{
					return ResponseModel<List<Models.Article.Statistics.ControllingAnalysis.VKUpdateResponseModel>>.FailureResponse(errors);
				}

				if(articles == null || articles.Count <= 0)
				{
					return ResponseModel<List<Models.Article.Statistics.ControllingAnalysis.VKUpdateResponseModel>>.SuccessResponse(null);
				}

				var articleIds = articles?.Select(x => x.Key)?.ToList();
				// - before shot
				var oldPriceEntities = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNrs(articleIds, botransaction.connection, botransaction.transaction);
				if(oldPriceEntities is { Count: > 0 } && data is { Count: > 0 })
				{
					var dataByArtikel = data.ToDictionary(
			  x => x.Artikel_Nr,
			  x => new Infrastructure.Data.Entities.Tables.PRS.PreisgruppenStatisticsEntity
					  {
						  Artikel_Nr = x.Artikel_Nr,
						  // copier toutes les autres propriétés ici
						  Artikelnummer = x.Artikelnummer,
						  Bis1 = x.Bis1,
						  Bis2 = x.Bis2,
						  Bis3 = x.Bis3,
						  Bis4 = x.Bis4,
						  Nr = x.Nr,
						  Preis1 = x.Preis1,
						  Preis2 = x.Preis2,
						  Preis3 = x.Preis3,
						  Preis4 = x.Preis4,

					  });

					foreach(var oldItem in oldPriceEntities)
					{
						if(!dataByArtikel.TryGetValue(oldItem.Artikel_Nr, out var row))
							continue;

						if(oldItem.Staffelpreis4 is null)
						{ row.Preis4 = null; row.Bis4 = null; }
						if(oldItem.Staffelpreis3 is null)
						{ row.Preis3 = null; row.Bis3 = null; }
						if(oldItem.Staffelpreis2 is null)
						{ row.Preis2 = null; row.Bis2 = null; }
						if(oldItem.Staffelpreis1 is null)
						{ row.Preis1 = null; row.Bis1 = null; }
					}
				}

				var results = new List<Models.Article.Statistics.ControllingAnalysis.VKUpdateResponseModel>();
				Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.UpdateStatisticsSalesPriceTrans(data, this._user.Name, botransaction.connection, botransaction.transaction);

				var extensionLogs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
				foreach(var item in data)
				{
					if(item.Preis1 is null)
					{
						Staffelpreis_KonditionzuordnungEntities.RemoveAll(x => x.Artikel_Nr == item.Artikel_Nr
							&& string.Equals(x.Staffelpreis_Typ, Enums.ArticleEnums.CustomPriceType.S1.GetDescription().RemoveAllSpaces(), StringComparison.OrdinalIgnoreCase));
					}
					if(item.Preis2 is null)
					{
						Staffelpreis_KonditionzuordnungEntities.RemoveAll(x => x.Artikel_Nr == item.Artikel_Nr
							&& string.Equals(x.Staffelpreis_Typ, Enums.ArticleEnums.CustomPriceType.S2.GetDescription().RemoveAllSpaces(), StringComparison.OrdinalIgnoreCase));
					}
					if(item.Preis3 is null)
					{
						Staffelpreis_KonditionzuordnungEntities.RemoveAll(x => x.Artikel_Nr == item.Artikel_Nr
							&& string.Equals(x.Staffelpreis_Typ, Enums.ArticleEnums.CustomPriceType.S3.GetDescription().RemoveAllSpaces(), StringComparison.OrdinalIgnoreCase));
					}
					if(item.Preis4 is null)
					{
						Staffelpreis_KonditionzuordnungEntities.RemoveAll(x => x.Artikel_Nr == item.Artikel_Nr
							&& string.Equals(x.Staffelpreis_Typ, Enums.ArticleEnums.CustomPriceType.S4.GetDescription().RemoveAllSpaces(), StringComparison.OrdinalIgnoreCase));
					}
				}
				if(Staffelpreis_KonditionzuordnungEntities?.Count > 0)
				{
					// -
					extensionLogs.AddRange(
						Staffelpreis_KonditionzuordnungEntities.Select(x =>
							ObjectLogHelper.getLog(this._user, x.Artikel_Nr ?? 0,
							$"Article Custom Sales | {x.Type} | MOQ",
							$"Excel",
							$"{x.LotSize}",
							Enums.ObjectLogEnums.Objects.Article.GetDescription(),
							Enums.ObjectLogEnums.LogType.Edit)));
					extensionLogs.AddRange(
						Staffelpreis_KonditionzuordnungEntities.Select(x =>
							ObjectLogHelper.getLog(this._user, x.Artikel_Nr ?? 0,
							$"Article Custom Sales | {x.Type} | DeliveryTime",
							$"Excel",
							$"{x.DeliveryTime}",
							Enums.ObjectLogEnums.Objects.Article.GetDescription(),
							Enums.ObjectLogEnums.LogType.Edit)));
					extensionLogs.AddRange(
						Staffelpreis_KonditionzuordnungEntities.Select(x =>
							ObjectLogHelper.getLog(this._user, x.Artikel_Nr ?? 0,
							$"Article Custom Sales | {x.Type} | ProdTime",
							$"Excel",
							$"{x.ProduKtionzeit}",
							Enums.ObjectLogEnums.Objects.Article.GetDescription(),
							Enums.ObjectLogEnums.LogType.Edit)));
					Infrastructure.Data.Access.Tables.PRS.Staffelpreis_KonditionzuordnungAccess_2.UpdateDeliveryLotProductionData(Staffelpreis_KonditionzuordnungEntities, botransaction.connection, botransaction.transaction);

					// -
					SalesExtension.CustomPrice.AddCustomPriceHandler.SetLastCustomPriceQuantityToInf(Staffelpreis_KonditionzuordnungEntities.Select(x => x.Artikel_Nr ?? 0).ToList(), botransaction);
				}
				if(ArtikelSalesExtensionEntities?.Count > 0)
				{
					// -
					extensionLogs.AddRange(
						ArtikelSalesExtensionEntities.Select(x =>
							ObjectLogHelper.getLog(this._user, x.ArticleNr,
							$"Article Custom Sales | {x.ArticleSalesType} | MOQ",
							$"Excel",
							$"{x.Losgroesse}",
							Enums.ObjectLogEnums.Objects.Article.GetDescription(),
							Enums.ObjectLogEnums.LogType.Edit)));
					extensionLogs.AddRange(
						ArtikelSalesExtensionEntities.Select(x =>
							ObjectLogHelper.getLog(this._user, x.ArticleNr,
							$"Article Custom Sales | {x.ArticleSalesType} | DeliveryTime",
							$"Excel",
							$"{x.Lieferzeit}",
							Enums.ObjectLogEnums.Objects.Article.GetDescription(),
							Enums.ObjectLogEnums.LogType.Edit)));
					extensionLogs.AddRange(
						ArtikelSalesExtensionEntities.Select(x =>
							ObjectLogHelper.getLog(this._user, x.ArticleNr,
							$"Article Custom Sales | {x.ArticleSalesType} | ProdTime",
							$"Excel",
							$"{x.Profuktionszeit}",
							Enums.ObjectLogEnums.Objects.Article.GetDescription(),
							Enums.ObjectLogEnums.LogType.Edit)));
					Infrastructure.Data.Access.Tables.BSD.ArtikelSalesExtensionAccess.UpdateDeliveryLotProductionData(ArtikelSalesExtensionEntities, botransaction.connection, botransaction.transaction);
					// - 
					var serieSalesPrices = ArtikelSalesExtensionEntities.Where(x => x.ArticleSalesTypeId == ((int)Common.Enums.ArticleEnums.SalesItemType.Serie));
					if(serieSalesPrices?.Count() > 0)
					{
						extensionLogs.AddRange(
						serieSalesPrices.Select(x =>
							ObjectLogHelper.getLog(this._user, x.ArticleNr,
							$"Article | LotSize",
							$"Excel",
							$"{x.Losgroesse}",
							Enums.ObjectLogEnums.Objects.Article.GetDescription(),
							Enums.ObjectLogEnums.LogType.Edit)));
						extensionLogs.AddRange(
						serieSalesPrices.Select(x =>
							ObjectLogHelper.getLog(this._user, x.ArticleNr,
							$"Article | Production Time",
							$"Excel",
							$"{x.Profuktionszeit}",
							Enums.ObjectLogEnums.Objects.Article.GetDescription(),
							Enums.ObjectLogEnums.LogType.Edit)));
						Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.UpdateLotProductionData(serieSalesPrices.Select(x => (articleNr: x.ArticleNr, lotSize: x.Losgroesse, productionTime: x.Profuktionszeit)).ToList(), botransaction.connection, botransaction.transaction);
						Infrastructure.Data.Access.Tables.PRS.ArtikelKalkulatorischeKostenAccess.UpdateProductionCost(serieSalesPrices.Select(x => new KeyValuePair<int, decimal>(x.ArticleNr, (x.Stundensatz ?? 0) * (x.Profuktionszeit ?? 0) / 60)), botransaction.connection, botransaction.transaction);
					}
				}

				// - after shot
				var newPriceEntities = Infrastructure.Data.Access.Tables.PRS.PreisgruppenAccess.GetByArtikelNrs(articleIds, botransaction.connection, botransaction.transaction);
				foreach(var oldPriceItem in oldPriceEntities)
				{
					var newPriceItem = newPriceEntities.Find(x => x.Artikel_Nr == oldPriceItem.Artikel_Nr);
					if(newPriceItem != null)
					{
						var articleIdx = articles.FindIndex(x => x.Key == oldPriceItem.Artikel_Nr);
						if(articleIdx >= 0)
						{
							results.Add(new Models.Article.Statistics.ControllingAnalysis.VKUpdateResponseModel
							{
								//FIXME: TBD
								ArticleNumber = articles[articleIdx].Value,
								OldVKPrice = oldPriceItem.Verkaufspreis ?? 0,
								NewVKPrice = newPriceItem.Verkaufspreis ?? 0,
								ArticleNr = articles[articleIdx].Key
							});
						}
					}
				}

				// -- Article level Logging
				if(results != null && results.Count > 0)
				{
					var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumbers(results?.Select(x => x.ArticleNumber)?.ToList())
						?? new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity>();

					var logs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
					foreach(var resultItem in results)
					{
						if(resultItem.OldVKPrice != resultItem.NewVKPrice)
						{
							var articleEntity = articleEntities.Find(x => x.ArtikelNummer.Trim().ToLower() == resultItem.ArticleNumber.Trim().ToLower());
							logs.Add(ObjectLogHelper.getLog(this._user, articleEntity?.ArtikelNr ?? 0,
							$"UpdateVKOnly",
							$"{resultItem.OldVKPrice}",
							$"{resultItem.NewVKPrice}",
							Enums.ObjectLogEnums.Objects.Article.GetDescription(),
							Enums.ObjectLogEnums.LogType.Edit));
						}
					}
					if(logs.Count > 0)
						Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(logs, botransaction.connection, botransaction.transaction);
				}
				// - 
				if(extensionLogs.Count > 0)
				{
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(extensionLogs, botransaction.connection, botransaction.transaction);
				}


				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					return ResponseModel<List<Models.Article.Statistics.ControllingAnalysis.VKUpdateResponseModel>>.SuccessResponse(results);
				}
				else
				{
					return ResponseModel<List<Models.Article.Statistics.ControllingAnalysis.VKUpdateResponseModel>>.FailureResponse("Transaction error");
				}
			} catch(Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Article.Statistics.ControllingAnalysis.VKUpdateResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Article.Statistics.ControllingAnalysis.VKUpdateResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Article.Statistics.ControllingAnalysis.VKUpdateResponseModel>>.SuccessResponse();
		}

		internal static List<Infrastructure.Data.Entities.Tables.PRS.PreisgruppenStatisticsEntity> ReadFromExcel(string filePath, List<string> errors, List<KeyValuePair<int, string>> articleIds
			, List<Infrastructure.Data.Entities.Tables.PRS.Staffelpreis_KonditionzuordnungEntity_2> staffelpreis_KonditionzuordnungEntities
			, List<Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity> ArtikelSalesExtensionEntities)
		{
			try
			{
				var fileInfo = new System.IO.FileInfo(filePath);

				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				var package = new ExcelPackage(fileInfo);
				var worksheet = package.Workbook.Worksheets[0];
				var rowStart = worksheet.Dimension.Start.Row;
				var rowEnd = worksheet.Dimension.End.Row;

				// footer rows
				rowEnd -= 0;

				// get number of rows and columns in the sheet
				var rows = worksheet.Dimension.Rows;
				var columns = worksheet.Dimension.Columns;
				var startRowNumber = 2;
				var startColNumber = 1;
				var MIN_ROWS_COUNT = 1;
				var MIN_COLUMNS_COUNT = 8;


				if(rows >= MIN_ROWS_COUNT && columns >= MIN_COLUMNS_COUNT)
				{
					var data = new List<Infrastructure.Data.Entities.Tables.PRS.PreisgruppenStatisticsEntity> { };
					var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetMinimal()
						?? new List<Infrastructure.Data.Entities.Tables.PRS.MinimalSalesArtikelEntity>();
					staffelpreis_KonditionzuordnungEntities = staffelpreis_KonditionzuordnungEntities ?? new List<Infrastructure.Data.Entities.Tables.PRS.Staffelpreis_KonditionzuordnungEntity_2>();
					ArtikelSalesExtensionEntities = ArtikelSalesExtensionEntities ?? new List<Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity>();

					// loop through the worksheet rows and columns
					for(int i = startRowNumber; i <= rowEnd; i++)
					{
						try
						{
							var _articleNumber = Core.Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber]);
							if(string.IsNullOrWhiteSpace(_articleNumber))
							{
								//errors.Add($"Row {i}: invalid Article number [{_articleNumber}].");
								continue;
							}
							var _articleIdx = articleEntities.FindIndex(x => x.ArtikelNummer.Trim().ToLower() == _articleNumber.Trim().ToLower());
							if(_articleIdx < 0)
							{
								errors.Add($"Row {i}: Article number [{_articleNumber}] not found.");
								continue;
							}

							var _price1 = Core.Common.Helpers.Formatters.XLS.GetDecimalNullable(worksheet.Cells[i, startColNumber + 1]);
							var _bis1 = Core.Common.Helpers.Formatters.XLS.GetDecimalNullable(worksheet.Cells[i, startColNumber + 2]);
							var _price2 = Core.Common.Helpers.Formatters.XLS.GetDecimalNullable(worksheet.Cells[i, startColNumber + 3]);
							var _bis2 = Core.Common.Helpers.Formatters.XLS.GetDecimalNullable(worksheet.Cells[i, startColNumber + 4]);
							var _price3 = Core.Common.Helpers.Formatters.XLS.GetDecimalNullable(worksheet.Cells[i, startColNumber + 5]);
							var _bis3 = Core.Common.Helpers.Formatters.XLS.GetDecimalNullable(worksheet.Cells[i, startColNumber + 6]);
							var _price4 = Core.Common.Helpers.Formatters.XLS.GetDecimalNullable(worksheet.Cells[i, startColNumber + 7]);
							// - erstmuster
							var _erstmusterMoq = Core.Common.Helpers.Formatters.XLS.GetDecimalNullable(worksheet.Cells[i, startColNumber + 8]);
							var _erstmusterDeliveryTime = Core.Common.Helpers.Formatters.XLS.GetDecimalNullable(worksheet.Cells[i, startColNumber + 9]);
							var _erstmusterProdTime = Core.Common.Helpers.Formatters.XLS.GetDecimalNullable(worksheet.Cells[i, startColNumber + 10]);
							// - Nullserie
							var _nullserieMoq = Core.Common.Helpers.Formatters.XLS.GetDecimalNullable(worksheet.Cells[i, startColNumber + 11]);
							var _nullserieDeliveryTime = Core.Common.Helpers.Formatters.XLS.GetDecimalNullable(worksheet.Cells[i, startColNumber + 12]);
							var _nullserieProdTime = Core.Common.Helpers.Formatters.XLS.GetDecimalNullable(worksheet.Cells[i, startColNumber + 13]);
							//  - Prototy
							var _prototypMoq = Core.Common.Helpers.Formatters.XLS.GetDecimalNullable(worksheet.Cells[i, startColNumber + 14]);
							var _prototypDeliveryTime = Core.Common.Helpers.Formatters.XLS.GetDecimalNullable(worksheet.Cells[i, startColNumber + 15]);
							var _prototypProdTime = Core.Common.Helpers.Formatters.XLS.GetDecimalNullable(worksheet.Cells[i, startColNumber + 16]);
							//  - Serie
							var _serieMoq = Core.Common.Helpers.Formatters.XLS.GetDecimalNullable(worksheet.Cells[i, startColNumber + 17]);
							var _serieDeliveryTime = Core.Common.Helpers.Formatters.XLS.GetDecimalNullable(worksheet.Cells[i, startColNumber + 18]);
							var _serieProdTime = Core.Common.Helpers.Formatters.XLS.GetDecimalNullable(worksheet.Cells[i, startColNumber + 19]);
							// - S1
							var _s1Moq = Core.Common.Helpers.Formatters.XLS.GetDecimalNullable(worksheet.Cells[i, startColNumber + 20]);
							var _s1DeliveryTime = Core.Common.Helpers.Formatters.XLS.GetDecimalNullable(worksheet.Cells[i, startColNumber + 21]);
							var _s1ProdTime = Core.Common.Helpers.Formatters.XLS.GetDecimalNullable(worksheet.Cells[i, startColNumber + 22]);
							// - S2
							var _s2Moq = Core.Common.Helpers.Formatters.XLS.GetDecimalNullable(worksheet.Cells[i, startColNumber + 23]);
							var _s2DeliveryTime = Core.Common.Helpers.Formatters.XLS.GetDecimalNullable(worksheet.Cells[i, startColNumber + 24]);
							var _s2ProdTime = Core.Common.Helpers.Formatters.XLS.GetDecimalNullable(worksheet.Cells[i, startColNumber + 25]);
							// - S3
							var _s3Moq = Core.Common.Helpers.Formatters.XLS.GetDecimalNullable(worksheet.Cells[i, startColNumber + 26]);
							var _s3DeliveryTime = Core.Common.Helpers.Formatters.XLS.GetDecimalNullable(worksheet.Cells[i, startColNumber + 27]);
							var _s3ProdTime = Core.Common.Helpers.Formatters.XLS.GetDecimalNullable(worksheet.Cells[i, startColNumber + 28]);
							// - S4
							var _s4Moq = Core.Common.Helpers.Formatters.XLS.GetDecimalNullable(worksheet.Cells[i, startColNumber + 29]);
							var _s4DeliveryTime = Core.Common.Helpers.Formatters.XLS.GetDecimalNullable(worksheet.Cells[i, startColNumber + 30]);
							var _s4ProdTime = Core.Common.Helpers.Formatters.XLS.GetDecimalNullable(worksheet.Cells[i, startColNumber + 31]);

							var idy = articleIds.FindIndex(x => x.Key == articleEntities[_articleIdx].ArtikelNr);
							if(idy >= 0)
							{
								errors.Add($"Row {i}: duplicate article number [{_articleNumber}].");
								continue;
							}

							if(_price1 <= 0)
							{
								errors.Add($"Row {i}: invalid value: Price1 [{_price1}].");
								continue;
							}
							// - 2022-03-02 Frischholz
							//if (_price2 <= 0)
							//{
							//    errors.Add($"Row {i}: invalid value: Price2 [{_price2}].");
							//    continue;
							//}
							//if (_price3 <= 0)
							//{
							//    errors.Add($"Row {i}: Invalid value. Price3 [{_price3}].");
							//    continue;
							//}

							if(_price1 < _price2)
							{
								errors.Add($"Row {i}: Invalid value. Price1 [{_price1}] must be greater than Price2 [{_price2}].");
							}
							if(_price2 > 0 && _price2 < _price3)
							{
								errors.Add($"Row {i}: Invalid value. Price2 [{_price2}] must be greater than Price3 [{_price3}].");
							}
							if(_price2 > 0 && _price3 > 0 && _price3 < _price4)
							{
								errors.Add($"Row {i}: Invalid value. Price3 [{_price3}] must be greater than Price4 [{_price4}].");
							}

							if(_price1 > 0 && (!_bis1.HasValue || _bis1.Value < 0))
							{
								errors.Add($"Row {i}: Invalid value. Bis1 [{_bis1}] must be entered when Price1 [{_price1}] is specified.");
							}
							if(_price2 > 0 && (!_bis1.HasValue || _bis1.Value < 0))
							{
								errors.Add($"Row {i}: Invalid value. Bis1 [{_bis1}] must be entered when Price2 [{_price2}] is specified.");
							}
							if(_price3 > 0 && (!_bis2.HasValue || _bis2.Value < 0))
							{
								errors.Add($"Row {i}: Invalid value. Bis2 [{_bis2}] must be entered when Price3 [{_price3}] is specified.");
							}
							if(_price4 > 0 && (!_bis3.HasValue || _bis3.Value < 0))
							{
								errors.Add($"Row {i}: Invalid value. Bis3 [{_bis3}] must be entered when Price4 [{_price4}] is specified.");
							}

							if(_bis1.HasValue && _bis2.HasValue && _bis1.Value > _bis2.Value)
							{
								errors.Add($"Row {i}: Invalid value. bis1 [{_bis1}] must be smaller than bis2 [{_bis2}].");
							}
							if(_bis2.HasValue && _bis3.HasValue && _bis2.Value > _bis3.Value)
							{
								errors.Add($"Row {i}: Invalid value. bis2 [{_bis2}] must be smaller than bis3 [{_bis3}].");
							}

							// - 2026-01-12
							if(_price1.HasValue)
							{
								//if(!_s1Moq.HasValue)
								//{
								//	errors.Add($"Row {i}: Invalid value. MOQ S1 [{_s1Moq}] must have a value if P1 has a value [{_price1}].");
								//}
								//if(!_s1DeliveryTime.HasValue)
								//{
								//	errors.Add($"Row {i}: Invalid value. Lieferzeit S1 [{_s1DeliveryTime}] must have a value if P1 has a value [{_price1}].");
								//}
								if(!_s1ProdTime.HasValue)
								{
									errors.Add($"Row {i}: Invalid value. Arbeitzeit S1 [{_s1ProdTime}] must have a value if P1 has a value [{_price1}].");
								}
							}
							if(_price2.HasValue)
							{
								if(!_s2ProdTime.HasValue)
								{
									errors.Add($"Row {i}: Invalid value. Arbeitzeit S2 [{_s2ProdTime}] must have a value if P2 has a value [{_price2}].");
								}
							}
							if(_price3.HasValue)
							{
								if(!_s3ProdTime.HasValue)
								{
									errors.Add($"Row {i}: Invalid value. Arbeitzeit S3 [{_s3ProdTime}] must have a value if P3 has a value [{_price3}].");
								}
							}
							if(_price4.HasValue)
							{
								if(!_s4ProdTime.HasValue)
								{
									errors.Add($"Row {i}: Invalid value. Arbeitzeit S4 [{_s4ProdTime}] must have a value if P4 has a value [{_price4}].");
								}
							}
							if(errors?.Count > 0)
							{
								continue;
							}
							// - everything is good
							data.Add(
								new Infrastructure.Data.Entities.Tables.PRS.PreisgruppenStatisticsEntity
								{
									Artikelnummer = _articleNumber,
									Artikel_Nr = articleEntities[_articleIdx].ArtikelNr,
									Bis1 = _bis1,
									Bis2 = _bis2,
									Bis3 = _bis3,
									Nr = -1,
									Preis1 = _price1,
									Preis2 = _price2,
									Preis3 = _price3,
									Preis4 = _price4
								});

							// - Erstmuster
							if(_erstmusterDeliveryTime.HasValue || _erstmusterMoq.HasValue || _erstmusterProdTime.HasValue)
							{
								ArtikelSalesExtensionEntities.Add(new Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity
								{
									ArticleNr = articleEntities[_articleIdx].ArtikelNr,
									Profuktionszeit = _erstmusterProdTime,
									Produktionskosten = 0, // ProductionCosts - to be calculated later = HourlyRate * ProductionTime/60
									Lieferzeit = _erstmusterDeliveryTime,
									Losgroesse = (int?)_erstmusterMoq, // - MOQ column exists in table but not used, Losgroesse is used instead
									ArticleSalesTypeId = (int)Common.Enums.ArticleEnums.SalesItemType.FirstSample,
									ArticleSalesType = Common.Enums.ArticleEnums.SalesItemType.FirstSample.GetDescription()
								});
							}
							// - Nullserie
							if(_nullserieDeliveryTime.HasValue || _nullserieMoq.HasValue || _nullserieProdTime.HasValue)
							{
								ArtikelSalesExtensionEntities.Add(new Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity
								{
									ArticleNr = articleEntities[_articleIdx].ArtikelNr,
									Profuktionszeit = _nullserieProdTime,
									Produktionskosten = 0, // ProductionCosts - to be calculated later = HourlyRate * ProductionTime/60
									Lieferzeit = _nullserieDeliveryTime,
									Losgroesse = (int?)_nullserieMoq, // - MOQ column exists in table but not used, Losgroesse is used instead
									ArticleSalesTypeId = (int)Common.Enums.ArticleEnums.SalesItemType.NullSerie,
									ArticleSalesType = Common.Enums.ArticleEnums.SalesItemType.NullSerie.GetDescription()
								});
							}
							// - Prototyp
							if(_prototypDeliveryTime.HasValue || _prototypMoq.HasValue || _prototypProdTime.HasValue)
							{
								ArtikelSalesExtensionEntities.Add(new Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity
								{
									ArticleNr = articleEntities[_articleIdx].ArtikelNr,
									Profuktionszeit = _prototypProdTime,
									Produktionskosten = 0, // ProductionCosts - to be calculated later = HourlyRate * ProductionTime/60
									Lieferzeit = _prototypDeliveryTime,
									Losgroesse = (int?)_prototypMoq, // - MOQ column exists in table but not used, Losgroesse is used instead
									ArticleSalesTypeId = (int)Common.Enums.ArticleEnums.SalesItemType.Prototype,
									ArticleSalesType = Common.Enums.ArticleEnums.SalesItemType.Prototype.GetDescription()
								});
							}
							// - Serie
							if(_serieDeliveryTime.HasValue || _serieMoq.HasValue || _serieProdTime.HasValue)
							{
								ArtikelSalesExtensionEntities.Add(new Infrastructure.Data.Entities.Tables.BSD.ArtikelSalesExtensionEntity
								{
									ArticleNr = articleEntities[_articleIdx].ArtikelNr,
									Profuktionszeit = _serieProdTime,
									Produktionskosten = 0, // ProductionCosts - to be calculated later = HourlyRate * ProductionTime/60
									Lieferzeit = _serieDeliveryTime,
									Losgroesse = (int?)_serieMoq, // - MOQ column exists in table but not used, Losgroesse is used instead
									ArticleSalesTypeId = (int)Common.Enums.ArticleEnums.SalesItemType.Serie,
									ArticleSalesType = Common.Enums.ArticleEnums.SalesItemType.Serie.GetDescription(),
									Stundensatz = articleEntities[_articleIdx].Stundensatz
								});
							}

							// - S1
							if(_s1DeliveryTime.HasValue || _s1Moq.HasValue || _s1ProdTime.HasValue)
							{
								staffelpreis_KonditionzuordnungEntities.Add(new Infrastructure.Data.Entities.Tables.PRS.Staffelpreis_KonditionzuordnungEntity_2
								{
									Artikel_Nr = articleEntities[_articleIdx].ArtikelNr,
									ProduKtionzeit = (double?)_s1ProdTime,
									Betrag = 0, // ProductionCosts - to be calculated later = HourlyRate * ProductionTime/60
									DeliveryTime = _s1DeliveryTime?.ToString(),
									LotSize = (int?)_s1Moq,
									TypeId = (int)Enums.ArticleEnums.CustomPriceType.S1,
									Type = Enums.ArticleEnums.CustomPriceType.S1.GetDescription(),
									Staffelpreis_Typ = Enums.ArticleEnums.CustomPriceType.S1.GetDescription().RemoveAllSpaces()
								});
							}
							// - S2
							if(_s2DeliveryTime.HasValue || _s2Moq.HasValue || _s2ProdTime.HasValue)
							{
								staffelpreis_KonditionzuordnungEntities.Add(new Infrastructure.Data.Entities.Tables.PRS.Staffelpreis_KonditionzuordnungEntity_2
								{
									Artikel_Nr = articleEntities[_articleIdx].ArtikelNr,
									ProduKtionzeit = (double?)_s2ProdTime,
									Betrag = 0, // ProductionCosts - to be calculated later = HourlyRate * ProductionTime/60
									DeliveryTime = _s2DeliveryTime?.ToString(),
									LotSize = (int?)_s2Moq,
									TypeId = (int)Enums.ArticleEnums.CustomPriceType.S2,
									Type = Enums.ArticleEnums.CustomPriceType.S2.GetDescription(),
									Staffelpreis_Typ = Enums.ArticleEnums.CustomPriceType.S2.GetDescription().RemoveAllSpaces()
								});
							}
							// - S3
							if(_s3DeliveryTime.HasValue || _s3Moq.HasValue || _s3ProdTime.HasValue)
							{
								staffelpreis_KonditionzuordnungEntities.Add(new Infrastructure.Data.Entities.Tables.PRS.Staffelpreis_KonditionzuordnungEntity_2
								{
									Artikel_Nr = articleEntities[_articleIdx].ArtikelNr,
									ProduKtionzeit = (double?)_s3ProdTime,
									Betrag = 0, // ProductionCosts - to be calculated later = HourlyRate * ProductionTime/60
									DeliveryTime = _s3DeliveryTime?.ToString(),
									LotSize = (int?)_s3Moq,
									TypeId = (int)Enums.ArticleEnums.CustomPriceType.S3,
									Type = Enums.ArticleEnums.CustomPriceType.S3.GetDescription(),
									Staffelpreis_Typ = Enums.ArticleEnums.CustomPriceType.S3.GetDescription().RemoveAllSpaces()
								});
							}
							// - S4
							if(_s4DeliveryTime.HasValue || _s4Moq.HasValue || _s4ProdTime.HasValue)
							{
								staffelpreis_KonditionzuordnungEntities.Add(new Infrastructure.Data.Entities.Tables.PRS.Staffelpreis_KonditionzuordnungEntity_2
								{
									Artikel_Nr = articleEntities[_articleIdx].ArtikelNr,
									ProduKtionzeit = (double?)_s4ProdTime,
									Betrag = 0, // ProductionCosts - to be calculated later = HourlyRate * ProductionTime/60
									DeliveryTime = _s4DeliveryTime?.ToString(),
									LotSize = (int?)_s4Moq,
									TypeId = (int)Enums.ArticleEnums.CustomPriceType.S4,
									Type = Enums.ArticleEnums.CustomPriceType.S4.GetDescription(),
									Staffelpreis_Typ = Enums.ArticleEnums.CustomPriceType.S4.GetDescription().RemoveAllSpaces()
								});
							}
							// -
							articleIds.Add(new KeyValuePair<int, string>(articleEntities[_articleIdx].ArtikelNr, articleEntities[_articleIdx].ArtikelNummer));
						} catch(System.Exception exceptionInternal)
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

		public static byte[] GetDataXLS(List<Models.Article.Statistics.ControllingAnalysis.VKUpdateResponseModel> data)
		{
			try
			{
				// -
				// FIXME: Replace EPPlus by NPOI, or some other alt
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				// Create the package and make sure you wrap it in a using statement
				using(var package = new ExcelPackage())
				{
					// add a new worksheet to the empty workbook
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"VKUpdate");

					// Keep track of the row that we're on, but start with four to skip the header
					var headerRowNumber = 2;
					var startColumnNumber = 1;
					var numberOfColumns = 3;

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
					worksheet.Cells[1, 1].Value = $"VK-Update - {DateTime.Now.ToString("dd/MM/yyyy")}";
					worksheet.Cells[1, 1].Style.Font.Size = 16;



					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Artikelnummer";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Alter Preis";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Neuer Preis";


					var rowNumber = headerRowNumber + 1;
					{
						if(data != null && data.Count > 0)
						{
							// Loop through 
							foreach(var w in data)
							{
								worksheet.Cells[rowNumber, startColumnNumber].Value = w?.ArticleNumber;
								worksheet.Cells[rowNumber, startColumnNumber + 1].Value = w?.OldVKPrice;
								worksheet.Cells[rowNumber, startColumnNumber + 2].Value = w?.NewVKPrice;

								worksheet.Row(rowNumber).Height = 18;
								rowNumber += 1;
							}
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

					// Doc content
					if(data != null && data.Count > 0)
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
					package.Workbook.Properties.Title = $"VKUpdate - OUT";
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
