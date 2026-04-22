using Infrastructure.Data.Entities.Tables.Logistics.InventroyStock;
using Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities;
using Infrastructure.Services.Utils;
using Psz.Core.Identity.Models;
using Psz.Core.Logistics.Models.InverntoryStockModels;
using System.Reflection;

namespace Psz.Core.Logistics.Handlers.InventoryStock
{
	public class UpdateWipFaHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private readonly UserModel _user;
		private readonly UploadReport4WipRequestModel _data;

		public UpdateWipFaHandler(UserModel user, UploadReport4WipRequestModel data)
		{
			_user = user;
			_data = data;
		}

		public ResponseModel<int> Handle()
		{
			var validationResponse = Validate();
			if(!validationResponse.Success)
				return validationResponse;

			try
			{

				var errors = new List<string>();
				var transaction = new TransactionsManager();

				var response = ReadFromExcel(_data.AttachmentFilePath, _data.LagerId ?? -1, transaction, out errors);

				if(errors.Any())
					return ResponseModel<int>.FailureResponse(errors);

				return ResponseModel<int>.SuccessResponse(response);
			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(ex);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(_user == null)
				return ResponseModel<int>.AccessDeniedResponse();

			if(string.IsNullOrEmpty(_data.AttachmentFilePath) || !File.Exists(_data.AttachmentFilePath))
				return ResponseModel<int>.FailureResponse("Attachment file not found.");

			return ResponseModel<int>.SuccessResponse();
		}

		internal int ReadFromExcel(string filePath, int lagerId, TransactionsManager transaction, out List<string> errors)
		{
			errors = new List<string>();
			var positions = new List<ProductionWipEntity>();

			try
			{
				transaction.beginTransaction();
				var fileInfo = new FileInfo(filePath);

				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				using var package = new ExcelPackage(fileInfo);
				var worksheet = package.Workbook.Worksheets[0];

				int startRow = 3; // header row
				int rowEnd = worksheet.Dimension.End.Row;

				for(int i = startRow; i <= rowEnd; i++)
				{
					try
					{
						// Read cells safely
						var faString = Common.Helpers.Formatters.XLS.GetCellValueFg(worksheet.Cells[i, 1]); // Fertigungsauftrag
																											//var openQtyStr = Common.Helpers.Formatters.XLS.GetCellValueFgAsNumerique(worksheet.Cells[i, 2]); // FA-Offene-Menge pro Bereich
																											//var artikelNrStr = Common.Helpers.Formatters.XLS.GetCellValueFg(worksheet.Cells[i, 3]); // Artikel-Nr
																											//var dueStr = Common.Helpers.Formatters.XLS.GetCellValueFg(worksheet.Cells[i, 4]); // Due
																											//var pickedStr = Common.Helpers.Formatters.XLS.GetCellValueFgAsNumerique(worksheet.Cells[i, 5]); // Picked
																											//var cutStr = Common.Helpers.Formatters.XLS.GetCellValueFgAsNumerique(worksheet.Cells[i, 6]); // Cut
						var prepedStr = Common.Helpers.Formatters.XLS.GetCellValueFgAsNumerique(worksheet.Cells[i, 5]); // Preped
						var assembledStr = Common.Helpers.Formatters.XLS.GetCellValueFgAsNumerique(worksheet.Cells[i, 6]); // Assembled
						var crimpedStr = Common.Helpers.Formatters.XLS.GetCellValueFgAsNumerique(worksheet.Cells[i, 7]); // Crimped
						var eInspectedStr = Common.Helpers.Formatters.XLS.GetCellValueFgAsNumerique(worksheet.Cells[i, 8]); // VInspected
						var oInspectedStr = Common.Helpers.Formatters.XLS.GetCellValueFgAsNumerique(worksheet.Cells[i, 9]); // OInspected

						// Validate required columns
						if(string.IsNullOrWhiteSpace(faString))
							errors.Add($"Row {i}: Fertigungsauftrag is empty.");
						var fa = int.TryParse(faString, out var _fa) ? _fa : 0;
						if(fa <= 0)
							errors.Add($"Row {i}: Fertigungsauftrag is invalid.");
						//if(string.IsNullOrWhiteSpace(artikelNrStr))
						//	errors.Add($"Row {i}: Artikel-Nr is empty.");


						// Parse numeric columns safely
						//bool parsedPicked = decimal.TryParse(pickedStr, out var picked);
						//bool parsedCut = decimal.TryParse(cutStr, out var cut);
						bool parsedPreped = decimal.TryParse(prepedStr, out var preped);
						bool parsedAssembled = decimal.TryParse(assembledStr, out var assembled);
						bool parsedCrimped = decimal.TryParse(crimpedStr, out var crimped);
						bool parsedOInspected = decimal.TryParse(oInspectedStr, out var oInspected);
						bool parsedEInspected = decimal.TryParse(eInspectedStr, out var eInspected);

						//if(!parsedOpenQty)
						//	errors.Add($"Row {i}: FA-Offene-Menge pro Bereich invalid.");
						//if(!parsedPicked)
						//	errors.Add($"Row {i}: Picked invalid [{picked}]");
						//if(!parsedCut)
						//	errors.Add($"Row {i}: Cut invalid [{cut}].");
						if(!parsedPreped)
							errors.Add($"[FA {fa}] Row {i}: Prepared invalid [{preped}].");
						if(!parsedAssembled)
							errors.Add($"[FA {fa}] Row {i}: Assembled invalid [{assembled}].");
						if(!parsedCrimped)
							errors.Add($"[FA {fa}] Row {i}: Crimped invalid [{crimped}].");
						if(!parsedOInspected)
							errors.Add($"[FA {fa}] Row {i}: Optical Inspected invalid [{oInspected}].");
						if(!parsedEInspected)
							errors.Add($"[FA {fa}] Row {i}: Electrical Inspected invalid [{eInspected}].");

						// step value percent must be bewtween 0 and Fa Qty (check later)
						//if(picked < 0 || picked > 100)
						//	errors.Add($"Row {i}: Picked invalid percent [{picked}].");
						//if(cut < 0 && cut >= 100 )
						//	errors.Add($"Row {i}: Cut invalid percent [{cut}].");
						if(preped < 0 || preped > 100)
							errors.Add($"[FA {fa}] Row {i}: Prepared invalid percent [{preped}].");
						if(assembled < 0)
							errors.Add($"[FA {fa}] Row {i}: Assembled invalid value [{assembled}].");
						if(crimped < 0)
							errors.Add($"[FA {fa}] Row {i}: Crimped invalid value [{crimped}].");
						if(eInspected < 0)
							errors.Add($"[FA {fa}] Row {i}: Electrical Inspected invalid value [{eInspected}].");
						if(oInspected < 0)
							errors.Add($"[FA {fa}] Row {i}: Optical Inspected invalid value [{oInspected}].");

						// step must be ordred
						//if(cut > picked)
						//	errors.Add($"Row {i}: Cut percent [{cut}] must be less or equal than picked [{picked}].");
						//if(preped > cut)
						//	errors.Add($"Row {i}: Prepared percent [{preped}] must be less or equal than Cut [{cut}].");
						//if(assembled > preped)
						//	errors.Add($"Row {i}: Assembled percent [{assembled}] must be less or equal than Prepared [{preped}].");
						//if(crimped > assembled)
						//	errors.Add($"[FA {fa}] Row {i}: Crimped [{crimped}] must be less or equal than Assembled [{assembled}].");
						//if(oInspected > crimped)
						//	errors.Add($"[FA {fa}] Row {i}: Optical Insp. [{oInspected}] must be less or equal than Crimped [{crimped}].");
						if(oInspected > eInspected)
							errors.Add($"[FA {fa}] Row {i}: Optical Insp. [{oInspected}] must be less or equal than Electrical Insp. [{eInspected}].");


						// Skip row if any critical error
						if(errors.Any())
							continue;

						// Add to positions
						positions.Add(new ProductionWipEntity
						{
							FA = fa,
							//Picked = parsedPicked ? picked : 0,
							//Cut = parsedCut ? cut : 0,
							UserPreped = parsedPreped ? preped : 0,
							UserAssembled = parsedAssembled ? assembled : 0,
							UserCrimped = parsedCrimped ? crimped : 0,
							UserOpticalInspected = parsedOInspected ? oInspected : 0,
							UserElectricalInspected = parsedEInspected ? eInspected : 0
						});
					} catch(Exception exRow)
					{
						errors.Add($"Row {i}: {exRow.Message}");
					}
				}

				var fas = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummers(positions.Select(x => x.FA)?.ToList());
				var wipFas = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.ProductionWipAccess.GetByFa(fas?.Select(x => x.Fertigungsnummer ?? 0));
				List<int> errorFas = new List<int>();
				bool errorIteration = false;
				var j = startRow;
				for(int i = 0; i < positions.Count; i++)
				{
					j += i;
					var item = positions[i];
					var f = fas.FirstOrDefault(x => x.Fertigungsnummer == item.FA);
					if(f is null)
					{
						errors.Add($"Row {j}: Fa [{item.FA}] is not found in system.");
						errorFas.Add(item.FA);
						continue;
					}

					var wipFa = wipFas.FirstOrDefault(x => x.FA == item.FA);
					if(wipFa is null)
					{
						errors.Add($"Row {j}: Fa [{item.FA}] is not found in WIP or closed.");
						errorFas.Add(item.FA);
						continue;
					}

					// -
					item.UserPrepedPercent = item.UserPreped;
					item.UserAssembledPercent = item.UserAssembled / (f.Anzahl ?? 1) * 100;
					item.UserCrimpedPercent = item.UserCrimped / (f.Anzahl ?? 1) * 100;
					item.UserElectricalInspectedPercent = item.UserElectricalInspected / (f.Anzahl ?? 1) * 100;
					item.UserOpticalInspectedPercent = item.UserOpticalInspected / (f.Anzahl ?? 1) * 100;
					// -
					if(item.UserAssembled > f.Anzahl)
					{
						errors.Add($"[FA {item.FA}] Row {j}: Montage [{item.UserAssembled}] must be less or equal than Fa quantity [{f.Anzahl}].");
						errorFas.Add(item.FA);
						errorIteration = true;
					}
					if(item.UserCrimped > f.Anzahl)
					{
						errors.Add($"[FA {item.FA}] Row {j}: Crimp [{item.UserCrimped}] must be less or equal than Fa quantity [{f.Anzahl}].");
						errorFas.Add(item.FA);
						errorIteration = true;
					}
					if(item.UserElectricalInspected > f.Anzahl)
					{
						errors.Add($"[FA {item.FA}] Row {j}: Electrical Inspection [{item.UserElectricalInspected}] must be less or equal than Fa quantity [{f.Anzahl}].");
						errorFas.Add(item.FA);
						errorIteration = true;
					}
					if(item.UserOpticalInspected > f.Anzahl)
					{
						errors.Add($"[FA {item.FA}] Row {j}: Optical Inspection [{item.UserOpticalInspected}] must be less or equal than Fa quantity [{f.Anzahl}].");
						errorFas.Add(item.FA);
						errorIteration = true;
					}
					//if(item.UserPrepedPercent > wipFa.UserCutPercent)
					//{
					//	errors.Add($"[FA {item.FA}] Row {j}: Bereit percent [{item.UserPrepedPercent}] must be less or equal than Schneiderei [{wipFa.UserCutPercent}].");
					//	errorFas.Add(item.FA);
					//	errorIteration = true;
					//}
					//if(item.UserAssembledPercent > item.UserPrepedPercent)
					//{
					//	errors.Add($"[FA {item.FA}] Row {j}: Assembled percent [{item.UserAssembledPercent}] must be less or equal than Bereit [{item.UserPrepedPercent}].");
					//	errorFas.Add(item.FA);
					//	errorIteration = true;
					//}
					//if(item.UserCrimpedPercent > item.UserAssembledPercent)
					//{
					//	errors.Add($"[FA {item.FA}] Row {j}: Crimp percent [{item.UserCrimpedPercent}] must be less or equal than Assembled [{item.UserAssembledPercent}].");
					//	errorFas.Add(item.FA);
					//	errorIteration = true;
					//}
					//if(item.UserOpticalInspectedPercent > item.UserCrimpedPercent)
					//{
					//	errors.Add($"[FA {item.FA}] Row {j}: Optical Insp. percent [{item.UserOpticalInspectedPercent}] must be less or equal than Crimp [{item.UserCrimpedPercent}].");
					//	errorFas.Add(item.FA);
					//	errorIteration = true;
					//}
					if(item.UserOpticalInspectedPercent > item.UserElectricalInspectedPercent)
					{
						errors.Add($"[FA {item.FA}] Row {j}: Optical Inspection percent [{item.UserOpticalInspectedPercent}] must be less or equal than Electrical Inspection [{item.UserElectricalInspectedPercent}].");
						errorFas.Add(item.FA);
						errorIteration = true;
					}

					if(errorIteration)
					{
						continue;
					}

					// -
				}
				if(errorFas.Any())
				{
					errorFas = errorFas.Distinct().ToList();
					positions.RemoveAll(x => errorFas.Exists(y => y == x.FA));
				}

				// Update DB
				if(positions?.Count > 0)
				{
					Infrastructure.Data.Access.Tables.Logistics.InventoryStock.ProductionWipAccess.UpdatePercent(positions, lagerId != 6, transaction.connection, transaction.transaction);
				}

				#region add logs
				var InsertLogsResult = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.LogsAccess.InsertWithTransaction(new LogsEntity
				{
					LogTime = DateTime.Now,
					LogUserId = _user.Id,
					ObjectId = _data.LagerId,
					ObjectName = "InventoryStock",
					LogDescription = $"The WIP with [{positions?.Count}] FAs has been updated in Lager [{lagerId}] at [{DateTime.Now:yyyy-MM-dd HH:mm}] by [{_user.Name}]",
					LogsType = 2,
					LogUserName = _user.Name,
					LagerId = lagerId
				}, transaction.connection, transaction.transaction);
				#endregion add logs

				if(!transaction.commit())
				{
					transaction.rollback();
					errors.Add("Transaction failed.");
				}

				return 1;
			} catch(Exception ex)
			{
				transaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(ex);
				throw;
			}
		}
	}
}
