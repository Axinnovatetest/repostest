using Infrastructure.Data.Access.Tables.PRS;
using Infrastructure.Data.Entities.Tables.PRS;
using OfficeOpenXml;
using Org.BouncyCastle.Utilities.Collections;
using Psz.Core.Common.Models;
using System.Collections.Generic;
using System.Globalization;

namespace Psz.Core.CRP.Handlers.FA.Purchase
{
	public class WunshUpdate
	{
		public static ResponseModel<Infrastructure.Services.Reporting.Models.CTS.FAWunshUpdateReporAdmintModel> ImportWunshAdmin(Core.Common.Models.ImportFileModel file,
			Identity.Models.UserModel user)
		{
			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				botransaction.beginTransaction();

				#region // -- transaction-based logic -- //
				var errors = new List<string>();
				Infrastructure.Services.Reporting.Models.CTS.FAWunshUpdateReporAdmintModel Lists = new Infrastructure.Services.Reporting.Models.CTS.FAWunshUpdateReporAdmintModel();
				var FAWunshUpdates = ReadFromExcel(file.FilePath, out errors);
				var faEntities = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFAIds(FAWunshUpdates.Select(x => x.Key).ToList() ?? new List<int> { });
				//var technicArticles = Program.BSD.TechnicArticleIds;
				List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity> faHistoryEntities = new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity>();
				var articlesEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(faEntities.Select(x => (int)x.Artikel_Nr).ToList() ?? new List<int> { });


				var angebotArticleEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByFertigungsnummers(FAWunshUpdates.Select(x => x.Key)).ToList();

				foreach(var item in FAWunshUpdates)
				{
					var faEntity = faEntities?.FirstOrDefault(x => x.Fertigungsnummer == item.Key);
					var _newDate = (DateTime.TryParseExact(item.Value, "dd/MM/yyyy", new CultureInfo("de-DE"), DateTimeStyles.None, out var _d) ? _d : new DateTime(1900, 1, 1));
					var _oldDate = (faEntity?.Termin_Bestatigt1 ?? new DateTime(1900, 1, 1));
					var horizonCheck = Psz.Core.CRP.Helpers.HorizonsHelper.userHasFaUpdateTerminHorizonRight(_newDate, _oldDate, user, out List<string> messages);
					if(!horizonCheck && !Core.CRP.Helpers.HorizonsHelper.ArticleIsTechnic(faEntity.Artikel_Nr ?? -1))
						errors.AddRange(messages.Select(x => $"FA [{item.Key}]: {x}"));
					var artikelEntity = articlesEntities.FirstOrDefault(x => x.ArtikelNr == faEntity.Artikel_Nr);

					var angeboteArtikelEntity = angebotArticleEntities.FirstOrDefault(el => el.Fertigungsnummer == faEntity.Fertigungsnummer);

					var historyEntity = new Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity
					{
						Fertigungsnummer = faEntity.Fertigungsnummer,
						Artikelnummer = artikelEntity.ArtikelNummer,
						Bezeichnung = artikelEntity.Bezeichnung1,
						FA_Menge = Convert.ToInt32(faEntity.Anzahl ?? -1),
						Änderungsdatum = DateTime.Now,
						Bemerkung = faEntity.Bemerkung,
						Termin_Wunsch = faEntity.Termin_Fertigstellung,
						Angebot_Nr = angeboteArtikelEntity == null ? 0 : angeboteArtikelEntity.AngebotNr,
						Termin_Angebot = angeboteArtikelEntity == null ? new DateTime(1900, 1, 1) : angeboteArtikelEntity?.Liefertermin,
						CS_Mitarbeiter = "",
						Termin_Bestätigt1 = DateTime.TryParse(FAWunshUpdates.FirstOrDefault(x => x.Key == faEntity.Fertigungsnummer).Value, out var terminFromExcel) ? terminFromExcel : new DateTime(1900, 1, 1),
						Termin_voränderung = faEntity.Termin_Bestatigt1,
						Ursprünglicher_termin = faEntity.Termin_Ursprunglich,
						Mitarbeiter = user.Name,
						Lagerort_id = faEntity.Lagerort_id,
						Erstmuster = faEntity.Erstmuster
					};




					faHistoryEntities.Add(historyEntity);

				}

				Infrastructure.Data.Access.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieAccess.InsertWithTransaction(faHistoryEntities, botransaction.connection, botransaction.transaction);

				if(errors != null && errors.Count > 0)
					return ResponseModel<Infrastructure.Services.Reporting.Models.CTS.FAWunshUpdateReporAdmintModel>.FailureResponse(errors);
				else
				{
					var oldFAs = new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
					var newFAs = new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
					string joint = getJoinTable(FAWunshUpdates);
					if(FAWunshUpdates != null && FAWunshUpdates.Count > 0)
					{
						oldFAs = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummerWithTransaction(FAWunshUpdates.Select(x => x.Key)?.ToList(), botransaction.connection, botransaction.transaction);
						Infrastructure.Data.Access.Tables.PRS.FertigungAccess.UpdateTerminWunshAdmin(joint, botransaction.connection, botransaction.transaction);
						Infrastructure.Data.Access.Tables.CTS.PSZ_Historique_Import_Excel_FAAccess.InsertWusnUpdateAdmin(joint, user.Name, botransaction.connection, botransaction.transaction);
					}
					List<Infrastructure.Services.Reporting.Models.CTS.FAUpdatedFromExcelModel> Updated = new List<Infrastructure.Services.Reporting.Models.CTS.FAUpdatedFromExcelModel>();
					List<Infrastructure.Services.Reporting.Models.CTS.FAUpdatedFromExcelModel> Notupdated = new List<Infrastructure.Services.Reporting.Models.CTS.FAUpdatedFromExcelModel>();
					var Updated_list = Infrastructure.Data.Access.Joins.FAExcelUpdate.FAWunshUpdateAccess.GetUpdatedWunshAdmin(joint, -1, botransaction.connection, botransaction.transaction);
					var NotUpdated_list = Infrastructure.Data.Access.Joins.FAExcelUpdate.FAWunshUpdateAccess.GetUpdatedWunshAdmin(joint, 1, botransaction.connection, botransaction.transaction);
					if(Updated_list != null && Updated_list.Count > 0)
					{
						newFAs = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummerWithTransaction(Updated_list.Select(x => x.Fertigungsnummer ?? -1)?.ToList(), botransaction.connection, botransaction.transaction);
						foreach(var item in Updated_list)
						{
							Updated.Add(new Infrastructure.Services.Reporting.Models.CTS.FAUpdatedFromExcelModel(item));
						}
					}
					if(NotUpdated_list != null && NotUpdated_list.Count > 0)
					{
						foreach(var item in NotUpdated_list)
						{
							Notupdated.Add(new Infrastructure.Services.Reporting.Models.CTS.FAUpdatedFromExcelModel(item));
						}
					}
					//Logging
					int insertedId = 0;
					if((Updated != null && Updated.Count > 0) || (Notupdated != null && Notupdated.Count > 0))
						insertedId = Infrastructure.Data.Access.Tables.CTS.FA_Werk_Wunsh_UpdateAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_UpdateEntity { Typ = "Wunsh", Dateupdate = DateTime.Now, IdUser = user.Id, userName = user.Name }, botransaction.connection, botransaction.transaction);
					if(Updated != null && Updated.Count > 0)
					{
						Infrastructure.Data.Access.Tables.CTS.FA_Werk_Wunsh_Update_detailsAccess.InsertWithTransaction(
							Updated.Select(
								x => new Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity
								{
									Id_update = insertedId,
									FA = int.TryParse(x.Fertigungsnummer, out var f) ? f : 0,
									Werk = (!string.IsNullOrEmpty(x.Werk_termin) && !string.IsNullOrEmpty(x.Werk_termin)) ? Convert.ToDateTime(x.Werk_termin) : (DateTime?)null,
									updated = true
								}).ToList(), botransaction.connection, botransaction.transaction);

						// - logging -  2022-09-06
						var _logs = new List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity>();
						foreach(var updatedFa in newFAs)
						{
							var oldFA = oldFAs.FirstOrDefault(x => x.Fertigungsnummer == updatedFa.Fertigungsnummer);
							_logs.Add(new Core.CRP.Helpers.LogHelper(oldFA.Fertigungsnummer ?? -1, 0, 0, "Fertigung", Core.CRP.Helpers.LogHelper.LogType.MODIFICATIONOBJECT, "CTS", user)
								.LogCTS($"Termin_Bestätigt1", $"{oldFA.Termin_Bestatigt1}", $"{updatedFa.Termin_Bestatigt1}", 0));
						}
						Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.InsertWithTransaction(_logs, botransaction.connection, botransaction.transaction);
					}
					if(Notupdated != null && Notupdated.Count > 0)
					{
						Infrastructure.Data.Access.Tables.CTS.FA_Werk_Wunsh_Update_detailsAccess.InsertWithTransaction(
							Notupdated.Select(
								x => new Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity
								{
									Id_update = insertedId,
									FA = int.TryParse(x.Fertigungsnummer, out var f2) ? f2 : 0,
									Werk = (!string.IsNullOrEmpty(x.Werk_termin) && !string.IsNullOrEmpty(x.Werk_termin)) ? Convert.ToDateTime(x.Werk_termin) : (DateTime?)null,
									updated = false
								}).ToList(), botransaction.connection, botransaction.transaction);
					}
					Lists = new Infrastructure.Services.Reporting.Models.CTS.FAWunshUpdateReporAdmintModel { IdUpdate = insertedId, Updated = Updated, NotUpdated = Notupdated };
				}
				#endregion // -- transaction-based logic -- //

				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					// - 2025-08-06 // deactivate ALL FA Email notifs - Khelil
					//SendFaTerminChangeEmail(faEntities, user, faHistoryEntities, FAWunshUpdates);
					return ResponseModel<Infrastructure.Services.Reporting.Models.CTS.FAWunshUpdateReporAdmintModel>.SuccessResponse(Lists);
				}
				else
				{
					return ResponseModel<Infrastructure.Services.Reporting.Models.CTS.FAWunshUpdateReporAdmintModel>.FailureResponse(key: "1", value: "Transaction error");
				}
			} catch(Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public static ResponseModel<Infrastructure.Services.Reporting.Models.CTS.FAWunshUpdateReporUsertModel> ImportWunshUser(Core.Common.Models.ImportFileModel file,
			Identity.Models.UserModel user, int daysCount)
		{

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				botransaction.beginTransaction();

				#region // -- transaction-based logic -- //
				var errors = new List<string>();
				Infrastructure.Services.Reporting.Models.CTS.FAWunshUpdateReporUsertModel Lists = new Infrastructure.Services.Reporting.Models.CTS.FAWunshUpdateReporUsertModel();
				var FAWunshUpdates = ReadFromExcel(file.FilePath, out errors);
				var faEntities = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFAIds(FAWunshUpdates.Select(x => x.Key).ToList() ?? new List<int> { });

				List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity> faHistoryEntities = new List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity>();
				var articlesEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(faEntities.Select(x => (int)x.Artikel_Nr).ToList() ?? new List<int> { });
				var angebotArticleEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByFertigungsnummers(FAWunshUpdates.Select(x => x.Key)).ToList();

				if(errors != null && errors.Count > 0)
					return ResponseModel<Infrastructure.Services.Reporting.Models.CTS.FAWunshUpdateReporUsertModel>.FailureResponse(errors);
				else
				{
					var oldFAs = new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
					var newFAs = new List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity>();
					string joint = getJoinTable(FAWunshUpdates);

					if(FAWunshUpdates != null && FAWunshUpdates.Count > 0)
					{
						// -
						oldFAs = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummerWithTransaction(FAWunshUpdates.Select(x => x.Key)?.ToList(), botransaction.connection, botransaction.transaction);


						// - 2023-02-03 // 2024-01-25 - Khelil change H1 to 41 days
						var frZone = DateTime.Today.AddDays(daysCount);
						var _errors = new List<string>();
						//var technicArticles = Program.BSD.TechnicArticleIds;
						foreach(var faItem in FAWunshUpdates)
						{
							var f = oldFAs.FirstOrDefault(x => x.Fertigungsnummer == faItem.Key);
							var _newDate = (DateTime.TryParseExact(faItem.Value, "dd/MM/yyyy", new CultureInfo("de-DE"), DateTimeStyles.None, out var _d) ? _d : new DateTime(1900, 1, 1));
							var _oldDate = (f?.Termin_Bestatigt1 ?? new DateTime(1900, 1, 1));
							var faEntity = faEntities.FirstOrDefault(el => el.Fertigungsnummer == faItem.Key);
							var artikelEntity = articlesEntities.FirstOrDefault(x => x.ArtikelNr == faEntity.Artikel_Nr);

							var angeboteArtikelEntity = angebotArticleEntities.FirstOrDefault(el => el.Fertigungsnummer == faEntity.Fertigungsnummer);
							if(user?.Access?.CustomerService?.FAWerkWunshAdmin != true && user.Access?.CustomerService?.FaAdmin != true && f.Lagerort_id != 6 && f.Technik != true && !Module.BSD.TechnicArticleIds.Exists(x => x == f.Artikel_Nr))
							{

								if(_newDate < DateTime.Today)
								{
									_errors.Add($"Production date invalid: can not add FA [{faItem.Key}] [{_newDate.ToString("dd/MM/yyyy")}] in the past.");
								}
								if(_newDate <= frZone && _oldDate > frZone)
								{
									_errors.Add($"Production date invalid: can not bring FA [{faItem.Key}] [{_newDate.ToString("dd/MM/yyyy")}] in Frozen Zone [{frZone.ToString("dd/MM/yyyy")}].");
								}
								if(_newDate > frZone && _oldDate <= frZone)
								{
									_errors.Add($"Production date invalid: can not move FA [{faItem.Key}] [{_newDate.ToString("dd/MM/yyyy")}] out of Frozen Zone [{frZone.ToString("dd/MM/yyyy")}].");
								}
							}
							var horizonCheck = Psz.Core.CRP.Helpers.HorizonsHelper.userHasFaUpdateTerminHorizonRight(_newDate, _oldDate, user, out List<string> messages);
							if(!horizonCheck && !Core.CRP.Helpers.HorizonsHelper.ArticleIsTechnic(f.Artikel_Nr ?? -1))
								_errors.AddRange(messages.Select(x => $"FA [{faItem.Key}]: {x}"));


							var historyEntity = new Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity
							{
								Fertigungsnummer = faEntity.Fertigungsnummer,
								Artikelnummer = artikelEntity.ArtikelNummer,
								Bezeichnung = artikelEntity.Bezeichnung1,
								FA_Menge = Convert.ToInt32(faEntity.Anzahl ?? -1),
								Änderungsdatum = DateTime.Now,
								Bemerkung = faEntity.Bemerkung,
								Termin_Wunsch = faEntity.Termin_Fertigstellung,
								Angebot_Nr = angeboteArtikelEntity == null ? 0 : angeboteArtikelEntity.AngebotNr,
								Termin_Angebot = angeboteArtikelEntity == null ? new DateTime(1900, 1, 1) : angeboteArtikelEntity?.Liefertermin,
								CS_Mitarbeiter = "",
								Termin_Bestätigt1 = DateTime.TryParse(FAWunshUpdates.FirstOrDefault(x => x.Key == faEntity.Fertigungsnummer).Value, out var terminFromExcel) ? terminFromExcel : new DateTime(1900, 1, 1),
								Termin_voränderung = faEntity.Termin_Bestatigt1,
								Ursprünglicher_termin = faEntity.Termin_Ursprunglich,
								Mitarbeiter = user.Name,
								Lagerort_id = faEntity.Lagerort_id,
								Erstmuster = faEntity.Erstmuster
							};
							faHistoryEntities.Add(historyEntity);
						}
						Infrastructure.Data.Access.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieAccess.InsertWithTransaction(faHistoryEntities, botransaction.connection, botransaction.transaction);
						// - 
						if(_errors.Count > 0)
						{
							return ResponseModel<Infrastructure.Services.Reporting.Models.CTS.FAWunshUpdateReporUsertModel>.FailureResponse(_errors);
						}

						// - Begin Updates
						Infrastructure.Data.Access.Tables.PRS.FertigungAccess.UpdateTerminWunshUser(joint);
						Infrastructure.Data.Access.Tables.CTS.PSZ_Historique_Import_Excel_FAAccess.InsertWusnUpdateUser(joint, user.Name, botransaction.connection, botransaction.transaction);
					}
					List<Infrastructure.Services.Reporting.Models.CTS.FAUpdatedFromExcelModel> Updated = new List<Infrastructure.Services.Reporting.Models.CTS.FAUpdatedFromExcelModel>();
					List<Infrastructure.Services.Reporting.Models.CTS.FANotUpdatedwunshUserModel> Notupdated = new List<Infrastructure.Services.Reporting.Models.CTS.FANotUpdatedwunshUserModel>();

					var Updated_list = Infrastructure.Data.Access.Joins.FAExcelUpdate.FAWunshUpdateAccess.GetUpdatedWunshUser(joint, botransaction.connection, botransaction.transaction);
					var NotUpdated_list = Infrastructure.Data.Access.Joins.FAExcelUpdate.FAWunshUpdateAccess.GetNotUpdatedWunshUser(joint, botransaction.connection, botransaction.transaction);

					if(Updated_list != null && Updated_list.Count > 0)
					{
						newFAs = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummerWithTransaction(Updated_list.Select(x => x.Fertigungsnummer ?? -1)?.ToList(), botransaction.connection, botransaction.transaction);
						foreach(var item in Updated_list)
						{
							Updated.Add(new Infrastructure.Services.Reporting.Models.CTS.FAUpdatedFromExcelModel(item));
						}
					}
					if(NotUpdated_list != null && NotUpdated_list.Count > 0)
					{
						foreach(var item in NotUpdated_list)
						{
							Notupdated.Add(new Infrastructure.Services.Reporting.Models.CTS.FANotUpdatedwunshUserModel(item));
						}
					}

					//Logging
					int insertedId = 0;
					//if ((Updated != null && Updated.Count > 0) || (Notupdated != null && Notupdated.Count > 0))
					insertedId = Infrastructure.Data.Access.Tables.CTS.FA_Werk_Wunsh_UpdateAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_UpdateEntity { Typ = "Wunsh", Dateupdate = DateTime.Now, IdUser = user.Id, userName = user.Name }, botransaction.connection, botransaction.transaction);
					if(Updated != null && Updated.Count > 0)
					{
						Infrastructure.Data.Access.Tables.CTS.FA_Werk_Wunsh_Update_detailsAccess.InsertWithTransaction(
							Updated.Select(
								x => new Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity
								{
									Id_update = insertedId,
									FA = int.TryParse(x.Fertigungsnummer, out var f) ? f : 0,
									Werk = Convert.ToDateTime(x.Werk_termin),
									updated = true
								}).ToList(), botransaction.connection, botransaction.transaction);


						// - logging -  2022-09-06
						var _logs = new List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity>();
						foreach(var updatedFa in newFAs)
						{
							var oldFA = oldFAs.FirstOrDefault(x => x.Fertigungsnummer == updatedFa.Fertigungsnummer);
							_logs.Add(new Core.CRP.Helpers.LogHelper(oldFA.Fertigungsnummer ?? -1, 0, 0, "Fertigung", Core.CRP.Helpers.LogHelper.LogType.MODIFICATIONOBJECT, "CTS", user)
								.LogCTS($"Termin_Bestätigt1", $"{oldFA.Termin_Bestatigt1}", $"{updatedFa.Termin_Bestatigt1}", 0));
						}

						Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.InsertWithTransaction(_logs, botransaction.connection, botransaction.transaction);
					}
					if(Notupdated != null && Notupdated.Count > 0)
					{
						Infrastructure.Data.Access.Tables.CTS.FA_Werk_Wunsh_Update_detailsAccess.InsertWithTransaction(
							Notupdated.Select(
								x => new Infrastructure.Data.Entities.Tables.CTS.FA_Werk_Wunsh_Update_detailsEntity
								{
									Id_update = insertedId,
									FA = int.TryParse(x.Fertigungsnummer, out var f2) ? f2 : 0,
									Werk = (!string.IsNullOrEmpty(x.ProduKtionstermin) && !string.IsNullOrEmpty(x.ProduKtionstermin)) ? Convert.ToDateTime(x.ProduKtionstermin) : (DateTime?)null,
									updated = false
								}).ToList(), botransaction.connection, botransaction.transaction);
					}
					Lists = new Infrastructure.Services.Reporting.Models.CTS.FAWunshUpdateReporUsertModel { IdUpdate = insertedId, Updated = Updated, NotUpdated = Notupdated };
				}

				#endregion // -- transaction-based logic -- //

				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					// - 2025-08-06 // deactivate ALL FA Email notifs - Khelil
					//SendFaTerminChangeEmail(faEntities, user, faHistoryEntities, FAWunshUpdates);
					return ResponseModel<Infrastructure.Services.Reporting.Models.CTS.FAWunshUpdateReporUsertModel>.SuccessResponse(Lists);
				}
				else
				{
					return ResponseModel<Infrastructure.Services.Reporting.Models.CTS.FAWunshUpdateReporUsertModel>.FailureResponse(key: "1", value: "Transaction error");
				}
			} catch(Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public static byte[] GetReport(int id, Identity.Models.UserModel user)
		{

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				botransaction.beginTransaction();

				#region // -- transaction-based logic -- //
				byte[] response = null;
				var updateEntity = Infrastructure.Data.Access.Tables.CTS.FA_Werk_Wunsh_UpdateAccess.GetWithTransaction(id, botransaction.connection, botransaction.transaction);

				var _right = user.Access.CustomerService.FAWerkWunshAdmin || user.Access.CustomerService.FaAdmin;
				if(updateEntity != null)
				{
					var UpdateDetails = Infrastructure.Data.Access.Tables.CTS.FA_Werk_Wunsh_Update_detailsAccess.GetByUpdateId(id, botransaction.connection, botransaction.transaction);
					var udpateList = UpdateDetails?.Select(x => new KeyValuePair<int, string>((int)x.FA, Convert.ToString(x.Werk))).ToList();
					string joint = (udpateList != null && udpateList.Count > 0) ? getJoinTable(udpateList) : "";
					if(_right)
					{
						//if (UpdateDetails != null && UpdateDetails.Count > 0)
						//{
						var Updated_list = (UpdateDetails != null && UpdateDetails.Count > 0) ? Infrastructure.Data.Access.Joins.FAExcelUpdate.FAWunshUpdateAccess.GetUpdatedWunshAdmin(joint, -1, botransaction.connection, botransaction.transaction)
							: new List<Infrastructure.Data.Entities.Joins.FAExcelUpdate.FAWunshUpdateEntity> { };
						var NotUpdated_list = (UpdateDetails != null && UpdateDetails.Count > 0) ? Infrastructure.Data.Access.Joins.FAExcelUpdate.FAWunshUpdateAccess.GetUpdatedWunshAdmin(joint, 1, botransaction.connection, botransaction.transaction)
							: new List<Infrastructure.Data.Entities.Joins.FAExcelUpdate.FAWunshUpdateEntity> { };
						var Updated = Updated_list?.Select(x => new Infrastructure.Services.Reporting.Models.CTS.FAUpdatedFromExcelModel(x)).ToList();
						var Notupdated = NotUpdated_list?.Select(x => new Infrastructure.Services.Reporting.Models.CTS.FAUpdatedFromExcelModel(x)).ToList();

						var Lists = new Infrastructure.Services.Reporting.Models.CTS.FAWunshUpdateReporAdmintModel { IdUpdate = updateEntity.Id, Updated = Updated, NotUpdated = Notupdated };
						response = Module.CRP_ReportingService.GenerateWunshTerminAdminUpdateReport(Infrastructure.Services.Reporting.Helpers.ReportType.CTS_WUNSH_TERMIN_ADMIN, Lists);
						//}
					}
					else
					{

						//if (UpdateDetails != null && UpdateDetails.Count > 0)
						//{
						var Updated_list = (UpdateDetails != null && UpdateDetails.Count > 0) ? Infrastructure.Data.Access.Joins.FAExcelUpdate.FAWunshUpdateAccess.GetUpdatedWunshUser(joint, botransaction.connection, botransaction.transaction)
						: new List<Infrastructure.Data.Entities.Joins.FAExcelUpdate.FANotUpdatedwunshUserEntity> { };
						var NotUpdated_list = (UpdateDetails != null && UpdateDetails.Count > 0) ? Infrastructure.Data.Access.Joins.FAExcelUpdate.FAWunshUpdateAccess.GetNotUpdatedWunshUser(joint, botransaction.connection, botransaction.transaction)
						: new List<Infrastructure.Data.Entities.Joins.FAExcelUpdate.FANotUpdatedwunshUserEntity> { };
						var Updated = Updated_list?.Select(x => new Infrastructure.Services.Reporting.Models.CTS.FAUpdatedFromExcelModel(x)).ToList();
						var Notupdated = NotUpdated_list?.Select(x => new Infrastructure.Services.Reporting.Models.CTS.FANotUpdatedwunshUserModel(x)).ToList();

						var Lists = new Infrastructure.Services.Reporting.Models.CTS.FAWunshUpdateReporUsertModel() { Updated = Updated, NotUpdated = Notupdated, IdUpdate = id };
						response = Module.CRP_ReportingService.GenerateWunshTerminUserUpdateReport(Infrastructure.Services.Reporting.Helpers.ReportType.CTS_WUNSH_TERMIN_USER, Lists);
						//}
					}
				}

				#endregion // -- transaction-based logic -- //

				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					return response;
				}
				else
				{
					return null; //  ResponseModel<object>.FailureResponse(key: "1", value: "Transaction error");
				}
			} catch(Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		#region Helpers
		internal static List<KeyValuePair<int, string>> ReadFromExcel(string filePath, out List<string> errors)
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
					var col1 = Convert.ToString(getCellValue(worksheet.Cells[1, 1]));
					var col2 = Convert.ToString(getCellValue(worksheet.Cells[1, 2]));
					if(col1 != "Fertigungsnummer" || col2 != "Termin")
					{
						errors.Add($"Excel Columns Incompatible please download the right DRAFT");
						return null;
					}
					List<KeyValuePair<int, string>> FAsWerkUpdate = new List<KeyValuePair<int, string>>();

					// loop through the worksheet rows and columns
					for(int i = startRowNumber; i <= rowEnd; i++)
					{
						try
						{
							var FA = int.TryParse(getCellValue(worksheet.Cells[i, startColNumber]).ToString(), out var val) ? val : 0;
							var Wunsh = getCellValue(worksheet.Cells[i, startColNumber + 1]);
							if(FA != 0)
							{
								if(Wunsh != null && DateTime.TryParse(Wunsh, out var v))
								{
									FAsWerkUpdate.Add(new KeyValuePair<int, string>(FA, v.ToString("dd/MM/yyyy")));
								}
								else
								{
									errors.Add($"Row {i}: invalid Date [{Wunsh}].");
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
					var faList = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFAIds(FAsWerkUpdate?.Select(x => x.Key).ToList());
					for(int i = 0; i < FAsWerkUpdate.Count; i++)
					{
						if(!faList.Exists(x => x.Fertigungsnummer == FAsWerkUpdate[i].Key))
							errors.Add($"FA {FAsWerkUpdate[i].Key} does not exist .");
					}
					return FAsWerkUpdate;
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
		internal static string getCellValue(ExcelRange cell)
		{
			var val = cell.Value;
			if(val == null)
			{
				return "";
			}

			return val.ToString().Trim();
		}
		internal static string getJoinTable(List<KeyValuePair<int, string>> _data)
		{
			string result = "";
			for(int i = 0; i < _data.Count; i++)
			{
				string date = (!string.IsNullOrEmpty(_data[i].Value) && !string.IsNullOrWhiteSpace(_data[i].Value)) ? "convert(datetime,'" + _data[i].Value + "') as Termin" : "null as Termin";
				result += "select " + _data[i].Key + " as Fertigungsnummer," + date + " ";
				if(i < _data.Count - 1)
					result += "union all ";
			}
			return result;
		}
		public static void SendFaTerminChangeEmail(List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> faEntities,
			Identity.Models.UserModel _user,
			List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity> historyEntities,
			List<KeyValuePair<int, string>> FAWunshUpdates
			)
		{
			#region Mail notification
			var addresses = new List<string>();

			var _lagerCompany = Infrastructure.Data.Access.Tables.CTS.lagerCompanyAccess.GetByLagerIds(faEntities.Select(x => (int)x.Lagerort_id).ToList());
			List<int> articleNrs = faEntities.Select(x => (int)x.Artikel_Nr).ToList();
			var articleEntities = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(articleNrs);

			List<long> CompanyIds = _lagerCompany.Select(x => (long)x.Company_id).ToList();
			var _companies = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(CompanyIds.Select(id => id).ToList());
			if(_companies.Count > 0)
				addresses.AddRange(_companies.Select(_company => _company.DirectorEmail));

			// should distinct _kundeMitarbiters
			var fertigungNummers = faEntities.Select(fa => (int)fa.Fertigungsnummer).ToList();
			var mitarbeiters = Infrastructure.Data.Access.Joins.CTS.Divers.FAMitarbiters(fertigungNummers);
			var _kundeMitarbiters = mitarbeiters.Distinct().ToList();
			string subject = "";

			if(_kundeMitarbiters.Count > 0)
			{
				var _mitarbeiterUsers = Infrastructure.Data.Access.Tables.COR.UserAccess.GetByUsernames(_kundeMitarbiters);
				addresses.AddRange(_mitarbeiterUsers.Select(_mitarbeiterUser => _mitarbeiterUser.Email).ToList());
			}

			addresses.Add(_user.Email);

			/// - 2025-04-26 Add Fa notification users 
			List<int> faUsersIds = Infrastructure.Data.Access.Tables.CRP.CRP_FA_EmailUsersAccess
				.Get()
				.Where(x => x.UserId.HasValue)
				.Select(x => x.UserId.Value)
				.ToList();
			List<string> faUsersSiteEmails = Infrastructure.Data.Access.Tables.COR.UserAccess.GetByUsersAndLagerorts(faUsersIds, faEntities.Select(faEntity => (int)faEntity.Lagerort_id).ToList())
				.Where(user => user.IsActivated == true)
				.Select(x => x.Email).ToList();

			addresses.AddRange(faUsersSiteEmails);

			string content = GenerateFaDatesChangesAsHtmlContent(faEntities, historyEntities, _user, ref subject);
			try
			{
				// - 2025-07-31 - remove Fr. Hejdukova - correct - 2025-08-05 - remove even coming from other email configs
				if(Program.CTS?.IgnoreFaCreateNotificationEmails?.Count > 0)
				{
					addresses = addresses.Where(x => Program.CTS?.IgnoreFaCreateNotificationEmails?.Exists(y => y.IsSameAs(x, false, true)) == false)?.ToList();
				}
				sendEmailNotification(subject, content, addresses);
			} catch(Exception exm)
			{
				Infrastructure.Services.Logging.Logger.Log(exm);
			}
			#endregion
		}

		public static string GenerateFaDatesChangesAsHtmlContent(
			List<Infrastructure.Data.Entities.Tables.PRS.FertigungEntity> faEntities,
			List<Infrastructure.Data.Entities.Tables.CTS.PSZ_Fertigungsauftrag_ÄnderungshistorieEntity> historyEntities,
			Identity.Models.UserModel _user,
			ref string subject
			)
		{

			var frozenZone = DateTime.Today.AddDays(Module.CTS.FAHorizons.H1LengthInDays);

			var content = $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new CultureInfo("en-US"))}<br/>";
			content += $"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/>";
			content += $"<br/><span style='font-size:1.15em;'><strong>{_user.Name?.ToUpper()}</strong> has just changed the Termin of the following FAs.</span><br/><br/></div>";

			int countFaInFrozenZone = 0;
			List<int> faEntitiesWithPlanningDateViolationNummers = new List<int>();

			foreach(var history in historyEntities)
			{
				if(history.Termin_Bestätigt1 <= frozenZone && history.Termin_voränderung > frozenZone)
				{
					faEntitiesWithPlanningDateViolationNummers.Add(history.Fertigungsnummer ?? 0);
					countFaInFrozenZone++;
				}
			}


			if(faEntitiesWithPlanningDateViolationNummers.Count > 0)
			{
				var fertigungsToUpdate = FertigungAccess.GetByFertigungsnummer(faEntitiesWithPlanningDateViolationNummers).ToList();
				fertigungsToUpdate.ForEach(item => item.PlanningDateViolation = true);
				Infrastructure.Data.Access.Tables.PRS.FertigungAccess.Update(fertigungsToUpdate);
			}

			if(countFaInFrozenZone > 1)
			{
				content += "<hr><div style='background-color:#ffdddd;border-left:6px solid #d9534f;padding:12px;margin:0 0 8px 0;font-size:1.1em;'>⚠️ <strong>Alert:</strong> Some new FA date(s) are in the Frozen Zone. Please review carefully.</div>";
			}


			// Add table header
			content += "<table border='1' cellpadding='2' cellswpacing='0' style='border-collapse:collapse; font-family:Arial, sans-serif; font-size:14px; width:100%; max-width:100%;'>";
			content += "<thead style='background-color:#f2f2f2; font-weight:bold;'>";
			content += @"<tr><th style=""min-width:120px;"">FA</th>";
			content += @"<th style=""min-width:120px;"">FA Menge</th>";
			content += @"<th style=""min-width:120px;"">Artikel</th>";
			content += @"<th style=""min-width:120px;"">Artikel Bezeichnung</th>";
			content += @"<th style=""min-width:120px;"">Termin vor Änderung</th>";
			content += @"<th style=""min-width:120px;"">Neuer Termin</th>";
			content += @"<th style=""min-width:120px;"">Materialproblem</th>";
			content += @"<th style=""min-width:120px;"">Materialproblem Grund</th>";
			content += @"<th style=""min-width:120px;"">Kapazitätsproblem</th>";
			content += @"<th style=""min-width:120px;"">Kapazitätsproblem Grund</th>";
			content += @"<th style=""min-width:120px;"">Werkzeugproblem</th>";
			content += @"<th style=""min-width:120px;"">Werkzeugproblem Grund</th>";
			content += @"<th style=""min-width:120px;"">CS Wunsch</th>";
			content += @"<th style=""min-width:120px;"">CS Grund</th>";
			content += @"<th style=""min -width:120px;"">Neuer Termin Status</th>";
			content += @"</tr></thead>";

			int lengthInDays = Module.CTS.FAHorizons.H1LengthInDays;

			// Add data rows
			content += "<tbody>";
			foreach(var history in historyEntities)
			{

				string faLink = $"{Module.EmailAppDomaineName}{Module.EmailingService.EmailParamtersModel.AppDomaineName}/#/fertigung/details/{faEntities.FirstOrDefault(el => el.Fertigungsnummer == history.Fertigungsnummer).ID}";

				content += "<tr>";
				content += @$"<td style=""min-width:120px;text-align:center;""><a href='{faLink}'>{history.Fertigungsnummer}</a></td>";
				content += @$"<td style=""min-width:120px;text-align:center;"">{history.FA_Menge}</td>";
				content += $@"<td style=""min-width:120px;text-align:center;"">{history.Artikelnummer}</td>";
				content += @$"<td style=""min-width:120px;text-align:center;"">{history.Bezeichnung}</td>";
				content += @$"<td style=""min-width:120px;"">{history.Termin_voränderung.Value.ToString("dd-MM-yyyy", new CultureInfo("en-US"))}</td>";
				content += @$"<td style=""min-width:120px;"">{history.Termin_Bestätigt1.Value.ToString("dd-MM-yyyy", new CultureInfo("en-US"))}</td>";
				content += @$"<td style=""min-width:120px;"">{((history.Materialproblem.HasValue && history.Materialproblem.Value) ? "Ja" : "Nein")}</td>";
				content += @$"<td style=""min-width:120px;"">{history.Materialproblematik}</td>";
				content += @$"<td style=""min-width:120px;"">{((history.Kapazitätsproblem.HasValue && history.Kapazitätsproblem.Value) ? "Ja" : "Nein")}</td>";
				content += @$"<td style=""min-width:120px;"">{history.Kapazitätsproblematik}</td>";
				content += @$"<td style=""min-width:120px;"">{((history.Werkzeugproblem.HasValue && history.Werkzeugproblem.Value) ? "Ja" : "Nein")}</td>";
				content += @$"<td style=""min-width:120px;"">{history.Werkzeugproblematik}</td>";
				content += @$"<td style=""min-width:120px;"">{((history.Wunsch_CS.HasValue && history.Wunsch_CS.Value) ? "Ja" : "Nein")}</td>";
				content += @$"<td style=""min-width:120px;text-align:center;"">{history.Grund_CS}</td>";

				if(history.Termin_Bestätigt1 <= frozenZone && history.Termin_voränderung > frozenZone)
				{
					content += @"<td style=""min-width:120px;color:red; background: #ffdddd"">In Frozen Zone</td>";
					subject = "Group FA date update - ⚠️ [ALERT] FA in Frozen Zone";
				}
				else
				{
					content += @"<td style=""min-width:120px;color:green; background: #ddffdd"">Out of Frozen Zone</td>";
					subject = "Group FA date update";
				}
				content += "</tr>";
			}
			content += "</tbody></table>";

			// Add additional information
			content += "<br/><br/>";
			content += "<br/><span style='font-size:1.em;font-weight:bold'>Regards,</span>";
			content += "<br/><span style='font-size:1.em;font-weight:bold'>IT Department </span></br>";


			return content;
		}
		public static void sendEmailNotification(string title, string contentHtml, List<string> toEmailAddresses)
		{
			try
			{
				Module.EmailingService.SendEmailAsync(title, contentHtml, toEmailAddresses, null);
			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(new Exception($"Unable to send email to [{string.Join(",", Module.EmailingService.EmailParamtersModel.BOMEmailDestinations)}]"));
				Infrastructure.Services.Logging.Logger.Log(ex);
			}
		}
		#endregion
	}
}