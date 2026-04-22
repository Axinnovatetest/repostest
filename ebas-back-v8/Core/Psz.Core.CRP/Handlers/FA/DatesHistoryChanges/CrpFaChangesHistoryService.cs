using Infrastructure.Data.Access.Tables.CTS;
using Infrastructure.Data.Access.Tables.PRS;
using Infrastructure.Data.Entities.Joins.CTS;
using Infrastructure.Data.Entities.Tables.PRS;
using OfficeOpenXml;
using Org.BouncyCastle.Asn1.Ocsp;
using Psz.Core.Common.Helpers;
using Psz.Core.Common.Models;
using Psz.Core.CRP.Interfaces;
using Psz.Core.CRP.Models.FA;
using Psz.Core.Identity.Models;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using static Psz.Core.CRP.Enums.FAEnums;

namespace Psz.Core.CRP.Handlers.FA.DatesHistoryChanges
{
	public class CrpFaChangesHistoryService: ICrpFaChangesHistoryService
	{
		public ResponseModel<GetFaChangesHistoryResponseModel> GetFaDatesChangeHistory(UserModel user, FaChangesHistoryRequestModel data)
		{
			try
			{
				Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
				Infrastructure.Data.Access.Settings.PaginModel dataPaging = null;


				if(user == null)
				{
					return ResponseModel<GetFaChangesHistoryResponseModel>.AccessDeniedResponse();
				}


				if(!data.FullData)
				{
					dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
					{
						FirstRowNumber = data.PageSize > 0 ? (data.RequestedPage * data.PageSize) : 0,
						RequestRows = data.PageSize
					};
				}

				if(!string.IsNullOrWhiteSpace(data.SortField))
				{
					var sortFieldName = "";
					switch(data.SortField.ToLower())
					{
						default:
						case "diffindays":
							sortFieldName = "Abs(datediff(DAY,fa.Termin_voränderung,fa.Termin_Bestätigt1))";
							break;
						case "anderungsdatum":
							sortFieldName = "fa.[Änderungsdatum]";
							break;
						case "fertigungsnummer":
							sortFieldName = "fa.[Fertigungsnummer]";
							break;
						case "artikelnummer":
							sortFieldName = "fa.[Artikelnummer]";
							break;

						case "bemerkung":
							sortFieldName = "[Bemerkung]";
							break;
						case "cs_mitarbeiter":
							sortFieldName = "fa.[CS_Mitarbeiter]";
							break;
						case "erstmuster":
							sortFieldName = "fa.[Erstmuster]";
							break;
						case "fa_menge":
							sortFieldName = "fa.[FA_Menge]";
							break;
						case "grund_cs":
							sortFieldName = "fa.[Grund_CS]";
							break;
						case "mitarbeiter":
							sortFieldName = "fa.[Mitarbeiter]";
							break;
						case "termin_bestatigt1":
							sortFieldName = "fa.Termin_Bestätigt1";
							break;
						case "termin_voranderung":
							sortFieldName = "fa.[Termin_voränderung]";
							break;
						case "termin_wunsch":
							sortFieldName = "fa.[Termin_Wunsch]";
							break;
						case "wunsch_cs":
							sortFieldName = "fa.[Wunsch_CS]";
							break;
					}

					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = data.SortDesc,
					};
				}

				List<FaChangesHistoryResponseModel> response = new List<FaChangesHistoryResponseModel>();
				List<FertigungAuftragChangeEntity> faDateChangesList = new List<FertigungAuftragChangeEntity>();
				int userLager1 = (int)lagerCompanyAccess.GetByCompanyId(user.CompanyId)[0].Lagerort_id;
				int lengthInDays = Module.CTS.FAHorizons.H1LengthInDays;
				int allCount = 0;
				if(user.SuperAdministrator || user.IsGlobalDirector)
				{
					faDateChangesList = PSZ_Fertigungsauftrag_ÄnderungshistorieAccess.GetByHorizon(data.SearchValue,
					data.BroughtIntoH1, data.SendOutOfH1, data.IncludeDelayBacklog, lengthInDays, data.From, data.To, data.FaStatus ?? "", data.FullData, data.LagerId, dataPaging, dataSorting);
					allCount = PSZ_Fertigungsauftrag_ÄnderungshistorieAccess.GetByHorizon_Count(data.SearchValue,
					data.BroughtIntoH1, data.SendOutOfH1, data.IncludeDelayBacklog, lengthInDays, data.From, data.To, data.FaStatus ?? "", data.FullData, data.LagerId);
				}
				else
				{
					faDateChangesList = PSZ_Fertigungsauftrag_ÄnderungshistorieAccess.GetByHorizon(data.SearchValue,
					data.BroughtIntoH1, data.SendOutOfH1, data.IncludeDelayBacklog, lengthInDays, data.From, data.To, data.FaStatus ?? "", data.FullData, userLager1, dataPaging, dataSorting);
					allCount = PSZ_Fertigungsauftrag_ÄnderungshistorieAccess.GetByHorizon_Count(data.SearchValue,
					data.BroughtIntoH1, data.SendOutOfH1, data.IncludeDelayBacklog, lengthInDays, data.From, data.To, data.FaStatus ?? "", data.FullData, userLager1);
				}

				if(faDateChangesList != null && faDateChangesList.Count > 0)
					response = faDateChangesList.Select(x => new FaChangesHistoryResponseModel(x)).ToList();

				var faEntities = new List<FertigungEntity>();

				var fertigungsnummers = faDateChangesList.Select(x => x.Fertigungsnummer ?? 0).ToList();

				faEntities = FertigungAccess.GetByFAIds(fertigungsnummers).ToList();

				if(faEntities?.Count <= 10)
				{
					foreach(var faDateChange in faDateChangesList)
					{
						var fa = faEntities.FirstOrDefault(x => x.Fertigungsnummer == faDateChange.Fertigungsnummer);
						faDateChange.ID = fa?.ID ?? 0;
					}
				}
				return ResponseModel<GetFaChangesHistoryResponseModel>.SuccessResponse(new GetFaChangesHistoryResponseModel
				{

					Items = faDateChangesList.Select(x => new FaChangesHistoryResponseModel(x)).ToList(),
					PageRequested = data.RequestedPage,
					PageSize = data.PageSize,
					TotalCount = allCount > 0 ? allCount : 0,
					TotalPageCount = data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(allCount > 0 ? allCount : 0) / data.PageSize)) : 0,
				});


			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<byte[]> GetFaDatesChangesHistoryXLS(UserModel user, FaChangesHistoryRequestModel data)
		{
			try
			{
				if(user == null)
				{
					return ResponseModel<byte[]>.AccessDeniedResponse();
				}

				var response = SaveFaDatesChangesToExcelFile(user,data);

				return ResponseModel<byte[]>.SuccessResponse(response);
			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<GetFaHoursChangesResponseModel> GetFaHoursMovement(UserModel user, FaHoursChangesRequestModel data)
		{
			try
			{
				Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
				Infrastructure.Data.Access.Settings.PaginModel dataPaging = null;


				if(user == null)
				{
					return ResponseModel<GetFaHoursChangesResponseModel>.AccessDeniedResponse();
				}


				dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = data.PageSize > 0 ? (data.RequestedPage * data.PageSize) : 0,
					RequestRows = data.PageSize
				};

				if(!string.IsNullOrWhiteSpace(data.SortField))
				{
					var sortFieldName = "";
					switch(data.SortField.ToLower())
					{
						default:
						case "week":
							sortFieldName = "KW";
							break;
						case "year":
							sortFieldName = " KW_YEAR";
							break;
						case "hours":
							sortFieldName = "Stunden_Sum";
							break;
						case "fapositionzone":
							sortFieldName = "FaPositionZone";
							break;
					}

					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = data.SortDesc,
					};
				}

				List<FaHoursChangesResponseModel> response = new List<FaHoursChangesResponseModel>();
				List<FAHoursChangesEntity> faHourschangesList = new List<FAHoursChangesEntity>();

				int lengthInDays = Module.CTS.FAHorizons.H1LengthInDays;

				faHourschangesList = Infrastructure.Data.Access.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieAccess.GetFaHoursByWeekAndLager(data.Weeks,
					data.Lagerort,
					data.Year,
					lengthInDays,
					data.FaPositionZone ?? null,
					dataPaging,
					dataSorting);

				int allCount = faHourschangesList.FirstOrDefault() == null ? 0 : faHourschangesList.FirstOrDefault().TotalCount;

				if(faHourschangesList != null && faHourschangesList.Count > 0)
					response = faHourschangesList.Select(x => new FaHoursChangesResponseModel(x)).ToList();


				return ResponseModel<GetFaHoursChangesResponseModel>.SuccessResponse(new GetFaHoursChangesResponseModel
				{

					Items = response,
					PageRequested = data.RequestedPage,
					PageSize = data.PageSize,
					TotalCount = allCount > 0 ? allCount : 0,
					TotalPageCount = data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(allCount > 0 ? allCount : 0) / data.PageSize)) : 0,
				});


			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public byte[] SaveFaDatesChangesToExcelFile(UserModel user, FaChangesHistoryRequestModel _data)
		{
			try
			{
				var data = GetFaDatesChangeHistory(user, _data);
				if(data == null || !data.Success || data.Body == null || data.Body.Items.Count <= 0)
				{
					return null;
				}
				var tempFolder = Path.GetTempPath();
				var filePath = Path.Combine(tempFolder, $"FA-Date-Changes-{DateTime.Now.ToString("yyyyMMddTHHmmss")}.xlsx");

				var file = new FileInfo(filePath);
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

				using(var package = new ExcelPackage(file))
				{
					ExcelWorksheet worksheet = package.Workbook.Worksheets.Add($"FA Date Changes");

					var headerRowNumber = 1;
					var startColumnNumber = 1;
					var numberOfColumns = 18;

					// Add some formatting to the worksheet
					worksheet.TabColor = Color.Yellow;
					worksheet.HeaderFooter.FirstFooter.LeftAlignedText = $"Generated: {DateTime.Now.ToString("yyyy MM dd")}";
					worksheet.Row(1).Style.Font.Bold = true;
					worksheet.Row(1).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
					worksheet.Row(1).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#D9E1F2"));

					// Start adding the header
					worksheet.Cells[headerRowNumber, startColumnNumber].Value = "Diff In Days";
					worksheet.Cells[headerRowNumber, startColumnNumber + 1].Value = "Hours Left";
					worksheet.Cells[headerRowNumber, startColumnNumber + 2].Value = "Change Date";
					worksheet.Cells[headerRowNumber, startColumnNumber + 3].Value = "Production number";
					worksheet.Cells[headerRowNumber, startColumnNumber + 4].Value = "Article number";
					worksheet.Cells[headerRowNumber, startColumnNumber + 5].Value = "Lager";
					worksheet.Cells[headerRowNumber, startColumnNumber + 6].Value = "Remark";
					worksheet.Cells[headerRowNumber, startColumnNumber + 7].Value = "CS employee";
					worksheet.Cells[headerRowNumber, startColumnNumber + 8].Value = "FA Quantity";
					worksheet.Cells[headerRowNumber, startColumnNumber + 9].Value = "Reason CS";
					worksheet.Cells[headerRowNumber, startColumnNumber + 10].Value = "Employees";
					worksheet.Cells[headerRowNumber, startColumnNumber + 11].Value = "Date Confirmed";
					worksheet.Cells[headerRowNumber, startColumnNumber + 12].Value = "Date before change";
					worksheet.Cells[headerRowNumber, startColumnNumber + 13].Value = "Appointment request";
					worksheet.Cells[headerRowNumber, startColumnNumber + 14].Value = "Desired CS";
					worksheet.Cells[headerRowNumber, startColumnNumber + 15].Value = "Position";
					worksheet.Cells[headerRowNumber, startColumnNumber + 16].Value = "Erstmuster";
					worksheet.Cells[headerRowNumber, startColumnNumber + 17].Value = "FA Status";

					var rowNumber = headerRowNumber + 1;
					var elements = data.Body.Items;
					if(elements.Count > 0)
					{
						foreach(var p in elements)
						{

							worksheet.Cells[rowNumber, startColumnNumber].Value = p.DiffInDays;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Value = p.HoursLeft;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Value = p.Anderungsdatum;
							worksheet.Cells[rowNumber, startColumnNumber + 3].Value = p.Fertigungsnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 4].Value = p.Artikelnummer;
							worksheet.Cells[rowNumber, startColumnNumber + 5].Value = p.LagerId;
							worksheet.Cells[rowNumber, startColumnNumber + 6].Value = p.Bemerkung;
							worksheet.Cells[rowNumber, startColumnNumber + 7].Value = p.CS_Mitarbeiter;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Value = p.FA_Menge;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Value = p.Grund_CS;
							worksheet.Cells[rowNumber, startColumnNumber + 10].Value = p.Mitarbeiter;
							worksheet.Cells[rowNumber, startColumnNumber + 11].Value = p.Termin_Bestatigt1;
							worksheet.Cells[rowNumber, startColumnNumber + 12].Value = p.Termin_voranderung;
							worksheet.Cells[rowNumber, startColumnNumber + 13].Value = p.Termin_Wunsch;
							worksheet.Cells[rowNumber, startColumnNumber + 14].Value = p.Wunsch_CS;

							worksheet.Cells[rowNumber, startColumnNumber].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 1].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 9].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 8].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 11].Style.Numberformat.Format = Module.XLS_FORMAT_NUMBER;
							worksheet.Cells[rowNumber, startColumnNumber + 2].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
							worksheet.Cells[rowNumber, startColumnNumber + 11].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
							worksheet.Cells[rowNumber, startColumnNumber + 12].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;
							worksheet.Cells[rowNumber, startColumnNumber + 13].Style.Numberformat.Format = Module.XLS_FORMAT_DATE;

							switch(p.Erstmuster)
							{
								case true:
									worksheet.Cells[rowNumber, startColumnNumber + 16].Value = $"Yes";
									worksheet.Cells[rowNumber, startColumnNumber + 16].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
									worksheet.Cells[rowNumber, startColumnNumber + 16].Style.Fill.BackgroundColor.SetColor(Color.Green);
									worksheet.Cells[rowNumber, startColumnNumber + 16].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
									worksheet.Cells[rowNumber, startColumnNumber + 16].Style.Font.Color.SetColor(Color.White);
									worksheet.Cells[rowNumber, startColumnNumber + 16].Style.Font.Bold = true;
									worksheet.Cells[rowNumber, startColumnNumber + 16].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
									worksheet.Cells[rowNumber, startColumnNumber + 16].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
									worksheet.Cells[rowNumber, startColumnNumber + 16].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
									worksheet.Cells[rowNumber, startColumnNumber + 16].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

									break;
								case false:
									worksheet.Cells[rowNumber, startColumnNumber + 16].Value = $"No";
									worksheet.Cells[rowNumber, startColumnNumber + 16].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
									worksheet.Cells[rowNumber, startColumnNumber + 16].Style.Fill.BackgroundColor.SetColor(Color.Red);
									worksheet.Cells[rowNumber, startColumnNumber + 16].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
									worksheet.Cells[rowNumber, startColumnNumber + 16].Style.Font.Color.SetColor(Color.White);
									worksheet.Cells[rowNumber, startColumnNumber + 16].Style.Font.Bold = true;
									worksheet.Cells[rowNumber, startColumnNumber + 16].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
									worksheet.Cells[rowNumber, startColumnNumber + 16].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
									worksheet.Cells[rowNumber, startColumnNumber + 16].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
									worksheet.Cells[rowNumber, startColumnNumber + 16].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
									break;
								default:
									break;
							}
							switch(p.FaPositionZone)
							{
								case 1:
									worksheet.Cells[rowNumber, startColumnNumber + 15].Value = $"Changed into FZ";
									worksheet.Cells[rowNumber, startColumnNumber + 15].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
									worksheet.Cells[rowNumber, startColumnNumber + 15].Style.Fill.BackgroundColor.SetColor(Color.Red);
									worksheet.Cells[rowNumber, startColumnNumber + 15].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
									worksheet.Cells[rowNumber, startColumnNumber + 15].Style.Font.Color.SetColor(Color.White);
									worksheet.Cells[rowNumber, startColumnNumber + 15].Style.Font.Bold = true;
									worksheet.Cells[rowNumber, startColumnNumber + 15].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
									worksheet.Cells[rowNumber, startColumnNumber + 15].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
									worksheet.Cells[rowNumber, startColumnNumber + 15].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
									worksheet.Cells[rowNumber, startColumnNumber + 15].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
									break;
								case 2:
									worksheet.Cells[rowNumber, startColumnNumber + 15].Value = $"Changed out of FZ";
									worksheet.Cells[rowNumber, startColumnNumber + 15].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
									worksheet.Cells[rowNumber, startColumnNumber + 15].Style.Fill.BackgroundColor.SetColor(Color.Green);
									worksheet.Cells[rowNumber, startColumnNumber + 15].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
									worksheet.Cells[rowNumber, startColumnNumber + 15].Style.Font.Color.SetColor(Color.White);
									worksheet.Cells[rowNumber, startColumnNumber + 15].Style.Font.Bold = true;
									worksheet.Cells[rowNumber, startColumnNumber + 15].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
									worksheet.Cells[rowNumber, startColumnNumber + 15].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
									worksheet.Cells[rowNumber, startColumnNumber + 15].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
									worksheet.Cells[rowNumber, startColumnNumber + 15].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
									break;
								default:
									worksheet.Cells[rowNumber, startColumnNumber + 15].Value = $"";
									worksheet.Cells[rowNumber, startColumnNumber + 15].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
									worksheet.Cells[rowNumber, startColumnNumber + 15].Style.Fill.BackgroundColor.SetColor(Color.White);
									worksheet.Cells[rowNumber, startColumnNumber + 15].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
									worksheet.Cells[rowNumber, startColumnNumber + 15].Style.Font.Bold = true;
									worksheet.Cells[rowNumber, startColumnNumber + 15].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
									worksheet.Cells[rowNumber, startColumnNumber + 15].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
									worksheet.Cells[rowNumber, startColumnNumber + 15].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
									worksheet.Cells[rowNumber, startColumnNumber + 15].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
									break;
							}
							switch(p.FaStatus.ToString().ToLower())
							{
								case "offen":
									worksheet.Cells[rowNumber, startColumnNumber + 17].Value = $"{Enum.GetName(FaStatus.Offen.GetType(), FaStatus.Offen)}";
									worksheet.Cells[rowNumber, startColumnNumber + 17].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
									worksheet.Cells[rowNumber, startColumnNumber + 17].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
									worksheet.Cells[rowNumber, startColumnNumber + 17].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
									worksheet.Cells[rowNumber, startColumnNumber + 17].Style.Font.Bold = true;
									worksheet.Cells[rowNumber, startColumnNumber + 17].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
									worksheet.Cells[rowNumber, startColumnNumber + 17].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
									worksheet.Cells[rowNumber, startColumnNumber + 17].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
									worksheet.Cells[rowNumber, startColumnNumber + 17].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
									break;
									;
								case "storno":
									worksheet.Cells[rowNumber, startColumnNumber + 17].Value = $"{Enum.GetName(FaStatus.storno.GetType(), FaStatus.storno)}";
									worksheet.Cells[rowNumber, startColumnNumber + 17].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
									worksheet.Cells[rowNumber, startColumnNumber + 17].Style.Fill.BackgroundColor.SetColor(Color.Red);
									worksheet.Cells[rowNumber, startColumnNumber + 17].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
									worksheet.Cells[rowNumber, startColumnNumber + 17].Style.Font.Color.SetColor(Color.White);
									worksheet.Cells[rowNumber, startColumnNumber + 17].Style.Font.Bold = true;
									worksheet.Cells[rowNumber, startColumnNumber + 17].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
									worksheet.Cells[rowNumber, startColumnNumber + 17].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
									worksheet.Cells[rowNumber, startColumnNumber + 17].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
									worksheet.Cells[rowNumber, startColumnNumber + 17].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
									break;
								case "erledigt":
									worksheet.Cells[rowNumber, startColumnNumber + 17].Value = $"{Enum.GetName(FaStatus.erledigt.GetType(), FaStatus.erledigt)}";
									worksheet.Cells[rowNumber, startColumnNumber + 17].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
									worksheet.Cells[rowNumber, startColumnNumber + 17].Style.Fill.BackgroundColor.SetColor(Color.Green);
									worksheet.Cells[rowNumber, startColumnNumber + 17].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
									worksheet.Cells[rowNumber, startColumnNumber + 17].Style.Font.Color.SetColor(Color.White);
									worksheet.Cells[rowNumber, startColumnNumber + 17].Style.Font.Bold = true;
									worksheet.Cells[rowNumber, startColumnNumber + 17].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
									worksheet.Cells[rowNumber, startColumnNumber + 17].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
									worksheet.Cells[rowNumber, startColumnNumber + 17].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
									worksheet.Cells[rowNumber, startColumnNumber + 17].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
									break;
								default:
									worksheet.Cells[rowNumber, startColumnNumber + 17].Value = $"";
									worksheet.Cells[rowNumber, startColumnNumber + 17].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
									worksheet.Cells[rowNumber, startColumnNumber + 17].Style.Fill.BackgroundColor.SetColor(Color.White);
									worksheet.Cells[rowNumber, startColumnNumber + 17].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
									worksheet.Cells[rowNumber, startColumnNumber + 17].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
									worksheet.Cells[rowNumber, startColumnNumber + 17].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
									worksheet.Cells[rowNumber, startColumnNumber + 17].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
									worksheet.Cells[rowNumber, startColumnNumber + 17].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
									break;
							}
							rowNumber += 1;
						}
					}
					// Doc content
					if(elements != null && elements.Count > 0)
					{
						using(var range = worksheet.Cells[headerRowNumber, 1, rowNumber - 1, numberOfColumns - 3])
						{
							range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
							range.Style.Fill.BackgroundColor.SetColor(Color.White);
							range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
							range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						}
					}

					//// Pre + Header
					using(var range = worksheet.Cells[headerRowNumber, 1, headerRowNumber, numberOfColumns])
					{
						range.Style.Font.Bold = true;
						range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
						range.Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);
						range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
						range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

					}
					for(int i = 1; i <= numberOfColumns; i++)
					{
						worksheet.Column(i).Width = 25;
					}

					// Set some document properties
					package.Workbook.Properties.Title = $"Fa-Dates-Change-History-{DateTime.Now.ToString("yyyyMMddTHHmmss")}";
					package.Workbook.Properties.Author = "PSZ ERP";
					package.Workbook.Properties.Company = "PSZ ERP";
					// save our new workbook and we are done!

					return package.GetAsByteArray();
				}
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception.StackTrace);
				throw;
			}
		}

		public ResponseModel<FaDataChartResponseModel> GetFaMovementChartData(UserModel user,FaChartDataRequestModel data)
		{
			try
			{
				if(user == null)
				{
					return ResponseModel<FaDataChartResponseModel>.AccessDeniedResponse();
				}

				var result = new FaDataChartResponseModel();
				result.z = new List<List<decimal>>();

				var rangeX = new List<(int Week, int Year)>();
				var rangeY = new List<(int Week, int Year)>();
				var today = DateTime.Today;


				// ---------------------- Y-Axis:Weeks from affectedStartDate to  +10 weeks ----------------------
				var affectedDateStart = data.AffectedWeekStartDate ?? today;

				for(int j = 0; j < 10; j++)
				{
					var affectedDate = affectedDateStart.AddDays(7 * j);
					int week = ISOWeek.GetWeekOfYear(affectedDate);
					int year = (affectedDate.Month == 12 && week == 1) ? affectedDate.Year + 1 : affectedDate.Year;

					rangeY.Add((week, year));
				}

				int lengthInDays = Module.CTS.FAHorizons.H1LengthInDays;

				var baseDate = (data.AffectedWeekStartDate ?? today).AddDays(-lengthInDays);

				// ---------------------- X-Axis: Weeks from (Affected - 41 days) to +10 weeks ----------------------
				for(int i = 0; i < 10; i++)
				{
					var changedDate = baseDate.AddDays(7 * i);
					int week = ISOWeek.GetWeekOfYear(changedDate);
					int year = (changedDate.Month == 12 && week == 1) ? changedDate.Year + 1 : changedDate.Year;

					rangeX.Add((week, year));
				}

				rangeX = rangeX.OrderBy(r => r.Year).ThenBy(r => r.Week).ToList();
				rangeY = rangeY.OrderByDescending(r => r.Year).ThenByDescending(r => r.Week).ToList();

				foreach(var _ in rangeY)
				{
					result.z.Add(Enumerable.Repeat(0m, rangeX.Count).ToList());
				}

				result.x = rangeX.Select(x => $"{x.Year} / KW {x.Week.ToString("00")}").ToList();
				result.y = rangeY.Select(x => $"{x.Year} / KW {x.Week.ToString("00")}").ToList();

				result.z = rangeY
					.Select(_ => Enumerable.Repeat(0m, rangeX.Count).ToList())
					.ToList();


				var faChartData = PSZ_Fertigungsauftrag_ÄnderungshistorieAccess.GetFaChartData(
					data.LagerIds,
					lengthInDays,
					data.AffectedWeekStartDate
				);

				if(faChartData != null && faChartData.Count > 0)
				{
					foreach(var couple in faChartData)
					{
						if(couple.HoursLeft > 0)
						{
							var xLabel = $"{couple.ChangedYear} / KW {couple.ChangeWeek:00}";
							var yLabel = $"{couple.AffectedYear} / KW {couple.AffectedWeek:00}";

							int i = result.x.IndexOf(xLabel);
							int j = result.y.IndexOf(yLabel);

							if(i >= 0 && j >= 0)
							{
								result.z[j][i] = MathHelper.RoundDecimal(couple.HoursLeft ?? 0, 2);
							}
						}
					}
				}

				return ResponseModel<FaDataChartResponseModel>.SuccessResponse(result);
			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(ex);
				throw;
			}

		}

		public ResponseModel<FASearchResponseModel> GetFaPlanningViolation(UserModel user, FaPlanningViolationRequestModel _data)
		{
			try
			{
				if(user == null)
				{
					return ResponseModel<FASearchResponseModel>.AccessDeniedResponse();
				}


				var orders = new List<FAListModule>();
				int allCount = 0;

				#region > Data sorting & paging
				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = _data.PageSize > 0 ? (_data.RequestedPage * _data.PageSize) : 0,
					RequestRows = _data.PageSize
				};

				Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
				if(!string.IsNullOrWhiteSpace(_data.SortField))
				{
					var sortFieldName = "";
					switch(_data.SortField.ToLower())
					{
						default:
						case "fertigungsnummer":
							sortFieldName = "[Fertigungsnummer]";
							break;
						case "artikelnummer":
							sortFieldName = "[Artikelnummer]";
							break;
						case "bezeichung_1":
							sortFieldName = "[Bezeichung 1]";
							break;
						case "fa_menge":
							sortFieldName = "[Originalanzahl]";
							break;
						case "fa_status":
							sortFieldName = "[Planungsstatus]";
							break;
						case "produktionstermin":
							sortFieldName = "[Termin_Bestätigt1]";
							break;
						case "lager":
							sortFieldName = "[Lagerort_id]";
							break;
						case "gestart":
							sortFieldName = "[FA_Gestartet]";
							break;
					}

					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = _data.SortDesc,
					};
				}

				#endregion

				var FAEntities = FertigungAccess.GetByPlanningViolation(_data.FaStatus, _data.From, _data.To, _data.Lagerort,dataSorting, dataPaging);

				if(FAEntities != null && FAEntities.Count > 0)
				{
					allCount = FertigungAccess.GetByPlanningViolationCount(_data.FaStatus, _data.From, _data.To, _data.Lagerort);
					orders = FAEntities.Select(x => new FAListModule(x)).ToList();
				}

				return ResponseModel<Models.FA.FASearchResponseModel>.SuccessResponse(
					new Models.FA.FASearchResponseModel()
					{
						Orders = orders,
						RequestedPage = _data.RequestedPage,
						ItemsPerPage = _data.PageSize,
						AllCount = allCount > 0 ? allCount : 0,
						AllPagesCount = _data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(allCount > 0 ? allCount : 0) / _data.PageSize)) : 0,
					});

			} catch(Exception ex )
			{
				Infrastructure.Services.Logging.Logger.Log(ex);
				throw;
			}
		}
	}
}
