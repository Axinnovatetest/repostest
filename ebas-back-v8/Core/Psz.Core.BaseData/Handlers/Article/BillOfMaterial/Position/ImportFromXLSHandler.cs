using System;

namespace Psz.Core.BaseData.Handlers.Article.BillOfMaterial.Position
{
	using Newtonsoft.Json;
	using OfficeOpenXml;
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Collections.Generic;
	using System.Linq;

	public class ImportFromXLSHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Article.BillOfMaterial.ImportPositionXLSRequestModel _data { get; set; }


		public ImportFromXLSHandler(Identity.Models.UserModel user, Models.Article.BillOfMaterial.ImportPositionXLSRequestModel data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// TODO: Check if article in ongoing production
				var parentArticleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleId);
				var errors = new List<string>();
				var bomArticles = ReadFromExcel(this._data.AttachmentFilePath, parentArticleEntity, out errors);
				if(errors != null && errors.Count > 0)
				{
					return ResponseModel<int>.FailureResponse(errors);
				}

				var articleEntites = (Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumbers(bomArticles.Select(x => x.ArticleNumber?.Trim())?.ToList(), null)
					?? new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity>())
					.Where(x => x.aktiv != false)?.ToList();
				var oldPositions = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticle(this._data.ArticleId);
				var maxPosition = oldPositions == null || oldPositions.Count <= 0
					? 0
					: oldPositions.Max(x => int.TryParse(x.Position, out var _max) ? _max : 0);

				if(this._data.Overwrite)
				{
					maxPosition = 0;
				}

				// -
				errors = new List<string>();
				var bomEntities = new List<Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity>();

				for(var n = 0; n < bomArticles.Count; n++)
				{
					var item = bomArticles[n];
					var articleEntity = articleEntites.Find(x => x.ArtikelNummer?.Trim() == item.ArticleNumber?.Trim());
					if(articleEntity == null)
					{
						errors.Add($"Article [{item.ArticleNumber}] not found");
						continue;
					}
					var oldArticle = oldPositions?.Find(x => x.Artikel_Nr_des_Bauteils == articleEntity.ArtikelNr);
					if(!this._data.Overwrite && oldArticle != null && oldArticle.Anzahl != (double?)item.Quantity)
					{
						errors.Add($"Article [{item.ArticleNumber}] already exists in BOM");
						continue;
					}

					var newArticle = bomEntities?.Find(x => x.Artikel_Nr_des_Bauteils == articleEntity.ArtikelNr);
					if(newArticle != null && newArticle.Anzahl != (double?)item.Quantity)
					{
						errors.Add($"Article [{item.ArticleNumber}] duplicated in BOM");
						continue;
					}

					bomEntities.Add(new Infrastructure.Data.Entities.Tables.BSD.StucklistenPositionEntity
					{
						Anzahl = (Single?)item.Quantity,
						Artikel_Nr = this._data.ArticleId,
						Artikel_Nr_des_Bauteils = articleEntity.ArtikelNr,
						Artikelnummer = articleEntity.ArtikelNummer,
						Bezeichnung_des_Bauteils = articleEntity.Bezeichnung1,
						DocumentId = null,
						Nr = 0,
						Position = ((n + 1) * 10 + maxPosition).ToString("D4"),
						Variante = "0", // ridha - 2022-02-17
						Vorgang_Nr = null
					});
				}

				if(errors != null && errors.Count > 0)
				{
					return ResponseModel<int>.FailureResponse(errors);
				}

				if(this._data.Overwrite)
				{
					var bomItemEntities = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.GetByArticle(parentArticleEntity.ArtikelNr);
					if(bomItemEntities != null && bomItemEntities.Count > 0)
					{
						Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.DeleteByArticleID(parentArticleEntity.ArtikelNr);

						// -- BOM level logs
						var logOv = Psz.Core.BaseData.Handlers.ObjectLogHelper.getBOMLog(this._user, null, 0, parentArticleEntity.ArtikelNummer, bomItemEntities.Count.ToString(), null,
							null, null, Enums.ObjectLogEnums.BOMLogType.Overwrite);

						Infrastructure.Data.Access.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionAccess.Insert(logOv);
					}
				}
				var insertedRows = Infrastructure.Data.Access.Tables.BSD.StucklistenPositionAccess.Insert(bomEntities);

				// -- Article level logs
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
					ObjectLogHelper.getLog(this._user, this._data.ArticleId, $"Import [{bomArticles.Count}] BOM Position",
					$"", "",
					$"{Enums.ObjectLogEnums.Objects.Article.GetDescription()}",
					Enums.ObjectLogEnums.LogType.Add));

				// -- BOM level logs
				var log = Psz.Core.BaseData.Handlers.ObjectLogHelper.getBOMLog(this._user, null, 0, parentArticleEntity.ArtikelNummer, bomArticles.Count.ToString(), null,
					null, null, Enums.ObjectLogEnums.BOMLogType.ImportExcel);

				Infrastructure.Data.Access.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionAccess.Insert(log);

				// -- if Bom was VALIDATED, change it to InPrep & Increment Version
				var articleExtEntity = Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.GetByArticle(this._data.ArticleId);
				if(articleExtEntity.BomStatusId == (int)Enums.ArticleEnums.BomStatus.Approved)
				{
					articleExtEntity.LastUpdateTime = DateTime.Now;
					articleExtEntity.LastUpdateUserId = this._user.Id;
					articleExtEntity.BomStatusId = (int)Enums.ArticleEnums.BomStatus.InPreparation;
					articleExtEntity.BomStatus = Enums.ArticleEnums.BomStatus.InPreparation.GetDescription();
					articleExtEntity.BomVersion = articleExtEntity.BomVersion + 1;

					// -- Article level Logging
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> {
                                    // - Status change log
                                    ObjectLogHelper.getLog(this._user, this._data.ArticleId, "Article BOM Status from",
									$"{Enums.ArticleEnums.BomStatus.Approved.GetDescription()}",
									$"{Enums.ArticleEnums.BomStatus.InPreparation.GetDescription()}",
									Enums.ObjectLogEnums.Objects.Article.GetDescription(),
									Enums.ObjectLogEnums.LogType.Edit),
                                    // - Version change log
                                    ObjectLogHelper.getLog(this._user, this._data.ArticleId, "Article BOM Version from", $"{articleExtEntity.BomVersion - 1}",
									$"{articleExtEntity.BomVersion?? 0}",
									Enums.ObjectLogEnums.Objects.Article.GetDescription(),
									Enums.ObjectLogEnums.LogType.Edit)
						});

					// -- BOM level logging
					Infrastructure.Data.Access.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionAccess.Insert(
						new List<Infrastructure.Data.Entities.Tables.BSD.Protokolierung_Stücklisten_Bei_AktionEntity> {
                                    // - Status change log
                                    Psz.Core.BaseData.Handlers.ObjectLogHelper.getBOMLog(this._user, 0, 0, articleExtEntity.ArticleNumber, null, null,
									Enums.ArticleEnums.BomStatus.Approved.GetDescription(), Enums.ArticleEnums.BomStatus.InPreparation.GetDescription(), Enums.ObjectLogEnums.BOMLogType.StatusChange),
                                    // - Version change log
                                    Psz.Core.BaseData.Handlers.ObjectLogHelper.getBOMLog(this._user, 0, 0, articleExtEntity.ArticleNumber, null, null,
									((articleExtEntity.BomVersion??0) - 1).ToString(), articleExtEntity.BomVersion?.ToString(), Enums.ObjectLogEnums.BOMLogType.Version)
						});
					// --
					Infrastructure.Data.Access.Tables.BSD.StucklistenArticleExtensionAccess.Update(articleExtEntity);
				}

				// --
				return ResponseModel<int>.SuccessResponse(bomEntities != null && bomEntities.Count > 0 ? insertedRows : 0);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data.ArticleId);
			if(articleEntity == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: "Article not found");

			if(this._user.IsGlobalDirector != true && this._user.SuperAdministrator != true && articleEntity.IsEDrawing == true && this._user.Access.MasterData.EDrawingEdit != true)
			{
				return ResponseModel<int>.FailureResponse($"Edit aborted: User cannot edit this article because E-Drawing is activated.");
			}

			return ResponseModel<int>.SuccessResponse();
		}

		internal static List<Models.Article.BillOfMaterial.BomPositionImport> ReadFromExcel(string filePath,
			Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity parentArtikel, out List<string> errors)
		{
			errors = new List<string> { };
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
				var startRowNumber = 4;
				var startColNumber = 1;

				// Get Article 3rd row, 1st col
				var articleNumber = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[2, 1]);
				if(string.IsNullOrEmpty(articleNumber) || string.IsNullOrWhiteSpace(articleNumber))
				{
					errors.Add("Article Number invalid");
					return null;
				}
				if(parentArtikel.ArtikelNummer?.Trim().ToLower() != articleNumber.Trim().ToLower())
				{
					errors.Add("XLS and selected article Numbers do not match");
					return null;
				}

				if(rows > 1 && columns > 1)
				{
					var bomPositions = new List<Models.Article.BillOfMaterial.BomPositionImport> { };

					// loop through the worksheet rows and columns
					for(int i = startRowNumber; i <= rowEnd; i++)
					{
						try
						{
							var bomArticleNumber = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber]);
							var quantity = Common.Helpers.Formatters.XLS.GetDecimal(worksheet.Cells[i, startColNumber + 1]);
							if(!string.IsNullOrWhiteSpace(bomArticleNumber))
							{
								if(parentArtikel.ArtikelNummer?.ToLower()?.Trim() == bomArticleNumber.ToLower().Trim())
								{
									errors.Add($"Row {i}: artikel nummer same as Parent Article [{bomArticleNumber.Trim()}].");
									return null;
								}
								else
								{
									if(quantity > 0)
									{
										bomPositions.Add(new Models.Article.BillOfMaterial.BomPositionImport
										{
											ArticleNumber = bomArticleNumber,
											Quantity = quantity
										});
										Infrastructure.Services.Logging.Logger.Log($" >>>> ImportXLS - BOM {JsonConvert.SerializeObject(bomPositions)}");
									}
									else
									{
										errors.Add($"Row {i}: invalid quantity [{quantity}].");
									}
								}
							}
							//else
							//{
							//    errors.Add($"Row {i}: invalid Article number [{bomArticleNumber}].");
							//}
						} catch(System.Exception exceptionInternal)
						{
							Infrastructure.Services.Logging.Logger.Log(exceptionInternal.Message + "\n" + exceptionInternal.StackTrace);
							errors.Add($"Row {i}: unknown error.");
						}
					}

					// - 2022-09-21 - check Article existence
					var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumbers(bomPositions.Select(x => x.ArticleNumber)?.ToList())
						?? new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity>();
					var missingArticles = bomPositions.Where(x => articleEntities.Exists(y => y.ArtikelNummer?.ToLower()?.Trim() == x.ArticleNumber.ToLower().Trim()) == false);
					if(missingArticles != null && missingArticles.Count() > 0)
					{
						foreach(var missingItem in missingArticles)
						{
							errors.Add($"Article [{missingItem.ArticleNumber}] does not exist.");
						}
					}

					// -- Sum quantity for same article
					var _bomPositions = new List<Models.Article.BillOfMaterial.BomPositionImport>();
					foreach(var item in bomPositions)
					{
						var idx = _bomPositions.FindIndex(x => x.ArticleNumber == item.ArticleNumber);
						if(idx <= 0)
						{
							_bomPositions.Add(item);
						}
						else
						{
							_bomPositions[idx].Quantity += item.Quantity;
						}
					}

					return _bomPositions;
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
	}
}
