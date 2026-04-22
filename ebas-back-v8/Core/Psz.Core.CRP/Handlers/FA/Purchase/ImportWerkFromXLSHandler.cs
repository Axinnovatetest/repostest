using OfficeOpenXml;
using Psz.Core.Common.Models;
using Psz.Core.CRP.Helpers;
using Psz.Core.SharedKernel.Interfaces;


namespace Psz.Core.CRP.Handlers.FA.Purchase
{
	public class ImportWerkFromXLSHandler: IHandle<Identity.Models.UserModel, ResponseModel<Infrastructure.Services.Reporting.Models.CTS.FAWerkUpdateReportModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Core.Common.Models.ImportFileModel _data { get; set; }
		public ImportWerkFromXLSHandler(Identity.Models.UserModel user, Core.Common.Models.ImportFileModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<Infrastructure.Services.Reporting.Models.CTS.FAWerkUpdateReportModel> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				botransaction.beginTransaction();

				#region // -- transaction-based logic -- //
				var errors = new List<string>();
				Infrastructure.Services.Reporting.Models.CTS.FAWerkUpdateReportModel Lists = new Infrastructure.Services.Reporting.Models.CTS.FAWerkUpdateReportModel();
				var FAWerkUpdates = ReadFromExcel(this._data.FilePath, out errors);

				if(!FAWerkUpdates.Success)
					return ResponseModel<Infrastructure.Services.Reporting.Models.CTS.FAWerkUpdateReportModel>.FailureResponse(errors);
				else
				{
					var oldFAs = new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
					var newFAs = new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
					var Updated = new List<Infrastructure.Services.Reporting.Models.CTS.FAUpdatedFromExcelModel>();
					var Notupdated = new List<Infrastructure.Services.Reporting.Models.CTS.FANotUpdatedFromExcelModel>();
					List<int> OrdersIds = new List<int>();
					if(FAWerkUpdates != null && FAWerkUpdates.Body.Count > 0)
					{
						OrdersIds = FAWerkUpdates.Body.Select(x => x.Item1).ToList();
						oldFAs = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummerWithTransaction(OrdersIds, botransaction.connection, botransaction.transaction);

						//// - 2023-02-03
						//var frZone = DateTime.Today.AddDays(7 * Module.CTS.ProductionFrozenZoneKWCount);
						//var _errors = new List<string>();
						//foreach(var faItem in FAWerkUpdates.Body)
						//{
						//	var f = oldFAs.FirstOrDefault(x => x.Fertigungsnummer == faItem.Item1);
						//	if(this._user?.Access?.CustomerService?.FAWerkWunshAdmin != true && faItem.Item2 <= frZone && ((f?.Termin_Bestatigt1 ?? new DateTime(1900, 1, 1)) > frZone || (f?.Termin_Bestatigt1 ?? new DateTime(1900, 1, 1)) < DateTime.Today))
						//	{
						//		_errors.Add($"Production date invalid: can not add FA before Frozen Zone limit [{frZone.ToString("dd/MM/yyyy")}].");
						//	}
						//}
						//// - 
						//if(_errors.Count > 0)
						//{
						//	return ResponseModel<Infrastructure.Services.Reporting.Models.CTS.FAWerkUpdateReportModel>.FailureResponse(_errors);
						//}

						// - 
						var toUpddate = new List<Tuple<int, DateTime, string>>();
						var toUpddateBemerkung = new List<Tuple<int, DateTime, string>>();
						foreach(var item in FAWerkUpdates.Body)
						{
							var fa = oldFAs.FirstOrDefault(x => x.Fertigungsnummer == item.Item1);
							if(fa == null)
								continue;

							// - 2023-08-04 - update Termin_Bestatigt2 only once & only if has valid value
							if(fa.Termin_Bestatigt2_Updated == true)
							{
								Notupdated.Add(new Infrastructure.Services.Reporting.Models.CTS.FANotUpdatedFromExcelModel(fa, "Termin Werk already updated || only Bemerkung was updated"));
								toUpddateBemerkung.Add(item);
							}
							else
							{
								if(fa.Termin_Bestatigt2 == null)
								{
									Notupdated.Add(new Infrastructure.Services.Reporting.Models.CTS.FANotUpdatedFromExcelModel(fa, "Cannot update empty Termin Werk || only Bemerkung was updated"));
									toUpddateBemerkung.Add(item);
								}
								else
								{
									toUpddate.Add(item);
								}
							}
						}
						// - 
						OrdersIds = toUpddate.Select(x => x.Item1).ToList();
						if(toUpddate.Count > 0)
						{
							oldFAs = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummerWithTransaction(OrdersIds, botransaction.connection, botransaction.transaction);
							Infrastructure.Data.Access.Tables.PRS.FertigungAccess.UpdateTerminWerk(toUpddate, botransaction.connection, botransaction.transaction);
						}
						if(toUpddateBemerkung.Count > 0)
						{
							Infrastructure.Data.Access.Tables.PRS.FertigungAccess.UpdateBemerkung(toUpddateBemerkung, botransaction.connection, botransaction.transaction);
						}
					}

					int insertedId = 0;
					if(OrdersIds != null && OrdersIds.Count > 0)
					{
						var OrdersEntities = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummerWithTransaction(OrdersIds, botransaction.connection, botransaction.transaction);
						// - 2022-05-19 - k var NotupdatedOrdersEntity = OrdersEntities?.Where(x => !x.FA_Druckdatum.HasValue).ToList();
						//
						foreach(var item in OrdersEntities)
						{
							var fa = oldFAs.FirstOrDefault(x => x.Fertigungsnummer == item.Fertigungsnummer);
							if(fa != null)
							{
								if(fa.Termin_Bestatigt2 == item.Termin_Bestatigt2)
								{
									Notupdated.Add(new Infrastructure.Services.Reporting.Models.CTS.FANotUpdatedFromExcelModel(fa, "gleiches Datum"));
								}
								else
								{
									fa.Termin_Bestatigt2 = FAWerkUpdates.Body.FirstOrDefault(x => x.Item1 == item.Fertigungsnummer)?.Item2;
									Updated.Add(new Infrastructure.Services.Reporting.Models.CTS.FAUpdatedFromExcelModel(1, fa));
									newFAs.Add(item);
								}
							}
							else
							{
								Notupdated.Add(new Infrastructure.Services.Reporting.Models.CTS.FANotUpdatedFromExcelModel(item, "FA not found"));
							}
						}

						if(Updated != null && Updated.Count > 0)
						{
							//Logging
							insertedId = Infrastructure.Data.Access.Tables.CTS.FA_Werk_Wunsh_UpdateAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_UpdateEntity { Typ = "Werk", Dateupdate = DateTime.Now, IdUser = this._user.Id, userName = this._user.Name }, botransaction.connection, botransaction.transaction);

							Infrastructure.Data.Access.Tables.CTS.FA_Werk_Wunsh_Update_detailsAccess.InsertWithTransaction(
								Updated.Select(
									x => new Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity
									{
										Id_update = insertedId,
										FA = int.TryParse(x.Fertigungsnummer, out var f) ? f : 0,
										Werk = DateTime.TryParse(x.Werk_termin, out var d) ? d : null,
										updated = true
									}).ToList(), botransaction.connection, botransaction.transaction);

							// - logging -  2022-09-06
							var _logs = new List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity>();
							foreach(var newFa in newFAs)
							{
								var oldFA = FAWerkUpdates.Body.FirstOrDefault(x => x.Item1 == newFa.Fertigungsnummer);
								_logs.Add(new LogHelper(newFa.Fertigungsnummer ?? -1, 0, 0, "Fertigung", LogHelper.LogType.MODIFICATIONOBJECT, "CTS", _user)
									.LogCTS($"Termin_Bestätigt2", $"{oldFA.Item2}", $"{newFa.Termin_Bestatigt2}", 0));
								_logs.Add(new LogHelper(newFa.Fertigungsnummer ?? -1, 0, 0, "Fertigung", LogHelper.LogType.MODIFICATIONOBJECT, "CTS", _user)
									.LogCTS($"Bemerkung II Planung", $"{oldFA.Item3}", $"{newFa.Bemerkung_II_Planung}", 0));
							}
							Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.InsertWithTransaction(_logs, botransaction.connection, botransaction.transaction);
						}
					}

					if(Notupdated != null && Notupdated.Count > 0)
					{
						//Logging
						insertedId = Infrastructure.Data.Access.Tables.CTS.FA_Werk_Wunsh_UpdateAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_UpdateEntity { Typ = "Werk", Dateupdate = DateTime.Now, IdUser = this._user.Id, userName = this._user.Name }, botransaction.connection, botransaction.transaction);

						Infrastructure.Data.Access.Tables.CTS.FA_Werk_Wunsh_Update_detailsAccess.InsertWithTransaction(
							Notupdated.Select(
								x => new Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity
								{
									Id_update = insertedId,
									FA = x.Fertigungsnummer,
									Werk = (!string.IsNullOrEmpty(x.Produktionstermin) && !string.IsNullOrEmpty(x.Produktionstermin)) ? Convert.ToDateTime(x.Produktionstermin) : (DateTime?)null,
									updated = false
								}).ToList(), botransaction.connection, botransaction.transaction);
					}
					Lists = new Infrastructure.Services.Reporting.Models.CTS.FAWerkUpdateReportModel { IdUpdate = insertedId, Updated = Updated, NotUpdated = Notupdated };
				}

				#endregion // -- transaction-based logic -- //

				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					return ResponseModel<Infrastructure.Services.Reporting.Models.CTS.FAWerkUpdateReportModel>.SuccessResponse(Lists);
				}
				else
				{
					return ResponseModel<Infrastructure.Services.Reporting.Models.CTS.FAWerkUpdateReportModel>.FailureResponse(key: "1", value: "Transaction error");
				}
			} catch(Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<Infrastructure.Services.Reporting.Models.CTS.FAWerkUpdateReportModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Infrastructure.Services.Reporting.Models.CTS.FAWerkUpdateReportModel>.AccessDeniedResponse();
			}

			return ResponseModel<Infrastructure.Services.Reporting.Models.CTS.FAWerkUpdateReportModel>.SuccessResponse();
		}
		internal static ResponseModel<List<Tuple<int, DateTime, string>>> ReadFromExcel(string filePath, out List<string> errors)
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
				List<Tuple<int, DateTime, string>> FAsWerkUpdate = new List<Tuple<int, DateTime, string>>();
				var col1 = Convert.ToString(getCellValue(worksheet.Cells[1, 1]));
				var col2 = Convert.ToString(getCellValue(worksheet.Cells[1, 2]));
				var col3 = Convert.ToString(getCellValue(worksheet.Cells[1, 3]));
				if(rows < 2 || columns < 2)  // (col1 != "Fertigungsnummer" || col2 != "Termin" || col3 != "Bemerkung2")
				{
					errors.Add($"Excel Columns Incompatible please download the right DRAFT");
					return ResponseModel<List<Tuple<int, DateTime, string>>>.FailureResponse(errors);
				}
				if(rows > 1 && columns > 1)
				{

					// loop through the worksheet rows and columns
					for(int i = startRowNumber; i <= rowEnd; i++)
					{
						try
						{
							var FA = int.TryParse(getCellValue(worksheet.Cells[i, startColNumber]).ToString(), out var val) ? val : 0;
							var Werk = getCellValue(worksheet.Cells[i, startColNumber + 1]);
							var Bemerkung = Convert.ToString(getCellValue(worksheet.Cells[i, startColNumber + 2]));
							if(FA != 0)
							{
								if(Werk != null && DateTime.TryParse(Werk, out var v))
								{
									FAsWerkUpdate.Add(new Tuple<int, DateTime, string>(FA, v, Bemerkung));
								}
								else
								{
									errors.Add($"Row {i}: invalid Date [{Werk}].");
								}
							}
							else
							{
								//errors.Add($"Row {i}: invalid FA number [{FA}].");
							}
						} catch(System.Exception exceptionInternal)
						{
							Infrastructure.Services.Logging.Logger.Log(exceptionInternal.Message + "\n" + exceptionInternal.StackTrace);
							errors.Add($"Row {i}: unknown error. " + exceptionInternal.Message);
						}
					}
					var faList = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(FAsWerkUpdate?.Select(x => x.Item1).ToList());
					for(int i = 0; i < FAsWerkUpdate.Count; i++)
					{
						if(!faList.Exists(x => x.Fertigungsnummer == FAsWerkUpdate[i].Item1))
							errors.Add($"FA {FAsWerkUpdate[i].Item1} does not exist .");
					}
				}
				else
				{
					errors.Add($"Invalid file format: {rows} Rows X {columns} Columns");
				}

				if(errors != null && errors.Count > 0)
					return ResponseModel<List<Tuple<int, DateTime, string>>>.FailureResponse(errors);
				else
					return ResponseModel<List<Tuple<int, DateTime, string>>>.SuccessResponse(FAsWerkUpdate);
			} catch(System.Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.Message + "\n" + exception.StackTrace);
				throw;
			}
		}
		internal static string getCellValue(ExcelRange cell)
		{
			var val = cell.Value;
			if(val == null)
			{
				return "";
			}

			return val.ToString();
		}
	}
}