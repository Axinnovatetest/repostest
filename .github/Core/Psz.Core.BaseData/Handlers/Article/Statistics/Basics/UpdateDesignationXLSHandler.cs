using System;

namespace Psz.Core.BaseData.Handlers.Article.Statistics.Basics
{
	using OfficeOpenXml;
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Collections.Generic;
	using System.Linq;

	public class UpdateDesignationXLSHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private string _data { get; set; }

		public UpdateDesignationXLSHandler(Identity.Models.UserModel user, string data)
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

				var errors = new List<string>();
				var data = ReadFromExcel(this._data, out errors);
				if(errors.Count > 0)
				{
					return ResponseModel<int>.FailureResponse(errors);
				}

				if(data == null || data.Count <= 0)
				{
					return ResponseModel<int>.SuccessResponse(0);
				}

				// -
				errors = new List<string>();
				var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumbers(data.Select(x => x.ArticleNumber).ToList());
				var missingArticles = data.Where(x => articleEntities.Exists(y => y.ArtikelNummer?.Trim()?.ToLower() == x.ArticleNumber) == false)?.ToList();
				data = data.Where(x => !missingArticles.Exists(y => y.ArticleNumber == x.ArticleNumber))?.ToList();

				var articles = new List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity>();
				var logs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
				foreach(var item in data)
				{
					var entity = articleEntities.FirstOrDefault(x => x.ArtikelNummer?.Trim()?.ToLower() == item.ArticleNumber);
					if(entity != null)
					{
						articles.Add(new Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity
						{
							ArtikelNr = entity.ArtikelNr,
							ArtikelNummer = item.ArticleNumber,
							Bezeichnung1 = entity.Bezeichnung1,
							Bezeichnung2 = item.NewDesignation,
							Bezeichnung3 = entity.Bezeichnung3,
							BezeichnungAL = entity.BezeichnungAL
						});
						logs.Add(ObjectLogHelper.getLog(this._user, entity.ArtikelNr, "Bezeichnung2", entity.Bezeichnung1, item.NewDesignation, Enums.ObjectLogEnums.Objects.Article.GetDescription(), Enums.ObjectLogEnums.LogType.Edit));
					}
				}
				// -
				Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.EditDesignation(articles);
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(logs);


				// -
				return new ResponseModel<int>
				{
					Success = true,
					Warnings = missingArticles.Select(x => $"Article [{x.ArticleNumber}] not found")?.ToList()
				};
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

			return ResponseModel<int>.SuccessResponse();
		}
		internal List<Models.Article.Statistics.Basics.UpdateDesignationRequestModel> ReadFromExcel(string filePath, out List<string> errors)
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
				var startRowNumber = 2;
				var startColNumber = 1;

				if(rows > 1 && columns > 1)
				{
					var data = new List<Models.Article.Statistics.Basics.UpdateDesignationRequestModel> { };

					// loop through the worksheet rows and columns
					for(int i = startRowNumber; i <= rowEnd; i++)
					{
						try
						{
							var articleNumber = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber]);
							var newArticleDesignation = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + 1]);

							if(string.IsNullOrWhiteSpace(articleNumber))
							{
								// - errors.Add($"Row {i}: invalid artikel nummer [{articleNumber.Trim()}].");
								continue;
							}
							if(string.IsNullOrWhiteSpace(newArticleDesignation))
							{
								errors.Add($"Row {i}: invalid designation [{newArticleDesignation.Trim()}].");
								continue;
							}

							// -
							data.Add(new Models.Article.Statistics.Basics.UpdateDesignationRequestModel
							{
								ArticleNumber = articleNumber.Trim().ToLower(),
								NewDesignation = newArticleDesignation
							});
						} catch(System.Exception exceptionInternal)
						{
							Infrastructure.Services.Logging.Logger.Log(exceptionInternal.Message + "\n" + exceptionInternal.StackTrace);
							errors.Add($"Row {i}: unknown error.");
						}
					}

					// -
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

	}
}
