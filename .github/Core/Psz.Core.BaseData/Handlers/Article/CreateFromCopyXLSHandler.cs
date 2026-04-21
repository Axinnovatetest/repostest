using System;

namespace Psz.Core.BaseData.Handlers.Article
{
	using OfficeOpenXml;
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Collections.Generic;
	using System.Linq;

	public class CreateFromCopyXLSHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Article.CreateFromCopyXLSRequestModel _data { get; set; }

		public CreateFromCopyXLSHandler(Identity.Models.UserModel user, Models.Article.CreateFromCopyXLSRequestModel data)
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
				var data = ReadFromExcel(this._data.XLSPath, out errors);
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
				var infos = new List<string>();
				var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumbers(data.Select(x => x.FromArticleNumber).ToList());
				foreach(var item in data)
				{
					var fromArticle = articleEntities.FirstOrDefault(x => x.ArtikelNummer?.Trim()?.ToLower() == item.FromArticleNumber.Trim().ToLower());
					var copy = new CreateFromCopyHandler(this._user, new Models.Article.CreateFromCopyRequestModel
					{
						ArticleNr = fromArticle?.ArtikelNr ?? -1,
						NewArticleNumber = item.ToArticleNumber,
						NewArticleDesignation = $"{fromArticle?.Bezeichnung1}-24",
					}).Handle();

					if(!copy.Success)
					{
						errors.Add($"[{item.FromArticleNumber}] > [{item.ToArticleNumber}]: Invalid copy. {string.Join(", ", copy.Errors.Select(x => x.Value))}");
					}
					else
					{
						infos.Add($"[{item.FromArticleNumber}] > [{item.ToArticleNumber}]: copy successful.");
					}
				}

				// -
				return new ResponseModel<int>
				{
					Success = true,
					Infos = infos.Count <= 0 ? new List<string> { "No article copied!" } : infos,
					Warnings = errors
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
		internal List<CopyXLSModel> ReadFromExcel(string filePath, out List<string> errors)
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

				if(rows > 1 && columns >= 1)
				{
					var data = new List<CopyXLSModel> { };

					var numbersArray = new List<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
					// loop through the worksheet rows and columns
					for(int i = startRowNumber; i <= rowEnd; i++)
					{
						try
						{
							var oldArticleNumber = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber]);
							var newArticleNumber = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + 1]);
							var newArticleBz1 = Common.Helpers.Formatters.XLS.GetCellValue(worksheet.Cells[i, startColNumber + 2]);

							if(string.IsNullOrWhiteSpace(oldArticleNumber))
							{
								// - errors.Add($"Row {i}: invalid artikel nummer [{oldArticleNumber.Trim()}].");
								continue;
							}
							//if (string.IsNullOrWhiteSpace(newArticleNumber))
							//{
							//    errors.Add($"Row {i}: invalid artikel nummer [{newArticleNumber.Trim()}].");
							//    continue;
							//}

							// - REM: - fix me
							if(!numbersArray.Contains(oldArticleNumber[oldArticleNumber.Length - 1]))
							{
								int j;
								for(j = oldArticleNumber.Length - 1; j >= 0; j--)
								{
									if(numbersArray.Contains(oldArticleNumber[j]))
									{
										break;
									}
								}
								newArticleNumber = $"{oldArticleNumber.Substring(0, j + 1)}ALT";
							}
							else
							{
								newArticleNumber = $"{oldArticleNumber}ALT";
							}

							// -
							data.Add(new CopyXLSModel
							{
								FromArticleNumber = oldArticleNumber,
								ToArticleDesignation = newArticleBz1,
								ToArticleNumber = newArticleNumber
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
		internal class CopyXLSModel
		{
			public string FromArticleNumber { get; set; }
			public string ToArticleNumber { get; set; }
			public string ToArticleDesignation { get; set; }
		}
	}
}
