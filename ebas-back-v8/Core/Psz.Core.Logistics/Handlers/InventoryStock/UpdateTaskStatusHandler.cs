using Infrastructure.Data.Entities.Tables.Logistics.InventroyStock;
using Org.BouncyCastle.Asn1.Ocsp;
using Psz.Core.Logistics.Models.InverntoryStockModels;
using static Psz.Core.CustomerService.Enums.InsideSalesEnums;

namespace Psz.Core.Logistics.Handlers.InventoryStock
{
	public class UpdateTaskStatusHandler
	{
		private UpdateTaskRequestModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public UpdateTaskStatusHandler(Identity.Models.UserModel user, UpdateTaskRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<int> Handle()
		{

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			Infrastructure.Services.Utils.TransactionsManager kstransaction = null;
			try
			{
				botransaction.beginTransaction();
				#region 

				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				int warehouseId = this._data.lagerId ?? 0;
				int prodWarehouseId = Psz.Core.Logistics.Helpers.LagerHelperHandler.GetProductionLager((Enums.Lager)warehouseId);
				var warehouseIds = new List<int>
				{
					warehouseId,
					prodWarehouseId
				};
				if(warehouseId == (int)Enums.Lager.WSTN)
				{
					warehouseIds.Add((int)Enums.Lager.TN);
				}
				kstransaction = new Infrastructure.Services.Utils.TransactionsManager(
					((Enums.Lager)warehouseId) switch
					{
						Enums.Lager.WSTN => Infrastructure.Services.Utils.TransactionsManager.Database.KWS,
						Enums.Lager.GZTN => Infrastructure.Services.Utils.TransactionsManager.Database.KGZ,
						Enums.Lager.AL => Infrastructure.Services.Utils.TransactionsManager.Database.KAL,
						_ => Infrastructure.Services.Utils.TransactionsManager.Database.KWS
					}
					);
				kstransaction.beginTransaction();
				// - 
				bool sendApprovalRequestNotification = false;
				var taskBeforeChange = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.TaskByRoleAccess.Get(_data.Id ?? -1, warehouseId, botransaction.connection, botransaction.transaction);
				var entity = new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity
				{
					Id = this._data.Id ?? -1,
					status = this._data.status ?? -1,
					lagerId = _data.lagerId ?? -1
				};

				var result = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.TaskByRoleAccess.UpdateTaskStatus(entity, botransaction.connection, botransaction.transaction);
				if(result > 0)
				{
					var updatedTask = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.TaskByRoleAccess.GetWithTransaction(entity.Id, botransaction.connection, botransaction.transaction);
					var taskTitle = updatedTask.title?.TrimStart().Substring(0, 2);
					var todoStatus = (int)Enums.WarehouseMouvementStatus.Todo;
					var inprogressStatus = (int)Enums.WarehouseMouvementStatus.Inprogress;
					var completeStatus = (int)Enums.WarehouseMouvementStatus.Complete;
					switch(taskTitle)
					{
						case "a.":
						case "b.":
						case "c.":
						case "d.":
							{
								var tasksIndices = new List<char> { 'a', 'b', 'c', 'd' };
								var completedTasksCount = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.TaskByRoleAccess.CountLogisticsTasksByStatus(tasksIndices, warehouseId, completeStatus, botransaction.connection, botransaction.transaction);
								// - complete all 4 tasks
								if(completedTasksCount == tasksIndices.Count)
								{
									Infrastructure.Data.Access.Tables.Logistics.InventoryStock.AccessInventurAccess.BlockT1HauptToProd(warehouseIds, true, botransaction.connection, botransaction.transaction); // - no typ1 commissioning to prod
									Infrastructure.Data.Access.Tables.Logistics.InventoryStock.AccessInventurAccess.BlockFaStart(warehouseIds, true, botransaction.connection, botransaction.transaction); // - no fa Start
																																																		   // -
									Infrastructure.Data.Access.Tables.Logistics.InventoryStock.TaskByRoleAccess.UpdateTaskStatus(new List<string> { "2" }, warehouseId, completeStatus, botransaction.connection, botransaction.transaction);
								}
								else
								{
									var inprogressTasksCount = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.TaskByRoleAccess.CountLogisticsTasksByStatus(tasksIndices, warehouseId, inprogressStatus, botransaction.connection, botransaction.transaction);
									// - in-progress all 4 tasks
									if(inprogressTasksCount == tasksIndices.Count)
									{
										Infrastructure.Data.Access.Tables.Logistics.InventoryStock.AccessInventurAccess.BlockT1HauptToProd(warehouseIds, false, botransaction.connection, botransaction.transaction); // - no typ1 commissioning to prod
										Infrastructure.Data.Access.Tables.Logistics.InventoryStock.AccessInventurAccess.BlockFaStart(warehouseIds, false, botransaction.connection, botransaction.transaction); // - no fa Start
																																																				// -
										Infrastructure.Data.Access.Tables.Logistics.InventoryStock.TaskByRoleAccess.UpdateTaskStatus(new List<string> { "2" }, warehouseId, todoStatus, botransaction.connection, botransaction.transaction);
									}
								}
							}
							break;
						case "f.":
							{
								// -
								if(entity.status == completeStatus)
								{
									// -
									Infrastructure.Data.Access.Tables.Logistics.InventoryStock.AccessInventurAccess.BlockT2HauptToProd(warehouseIds, true, botransaction.connection, botransaction.transaction); // - no typ2 commissioning to prod
									Infrastructure.Data.Access.Tables.Logistics.InventoryStock.AccessInventurAccess.BlockSchneiderei(warehouseIds, true, botransaction.connection, botransaction.transaction); // - no Schneiderei

									Infrastructure.Data.Access.Tables.Logistics.InventoryStock.TaskByRoleAccess.UpdateTaskStatus(new List<string> { "3", "4", "5", "6" }, warehouseId, completeStatus, botransaction.connection, botransaction.transaction);
									var resultFour = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.ProductionWipAccess.InitializeWipTable(warehouseId, botransaction.connection, botransaction.transaction);
								}
								else
								{
									// - reverting from Complete to NOT
									if(taskBeforeChange.status == completeStatus)
									{
										// -
										Infrastructure.Data.Access.Tables.Logistics.InventoryStock.AccessInventurAccess.BlockT2HauptToProd(warehouseIds, false, botransaction.connection, botransaction.transaction); // - no typ2 commissioning to prod
										Infrastructure.Data.Access.Tables.Logistics.InventoryStock.AccessInventurAccess.BlockSchneiderei(warehouseIds, false, botransaction.connection, botransaction.transaction); // - no Schneiderei

										Infrastructure.Data.Access.Tables.Logistics.InventoryStock.TaskByRoleAccess.UpdateTaskStatus(new List<string> { "3", "4", "5", "6" }, warehouseId, todoStatus, botransaction.connection, botransaction.transaction);
										var resultFour = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.ProductionWipAccess.ResetWipTable(warehouseId, botransaction.connection, botransaction.transaction);
									}
								}
							}
							break;
						case "e.":
						case "g.":
							{
								// - start WIP - close retoure
								if(taskBeforeChange.status == todoStatus && entity.status == inprogressStatus)
								{
									Infrastructure.Data.Access.Tables.Logistics.InventoryStock.AccessInventurAccess.BlockT1ProdToHaupt(warehouseIds, true, botransaction.connection, botransaction.transaction); // - no typ1 retour
									Infrastructure.Data.Access.Tables.Logistics.InventoryStock.AccessInventurAccess.BlockT2ProdToHaupt(warehouseIds, true, botransaction.connection, botransaction.transaction); // - no typ2 retour
									Infrastructure.Data.Access.Tables.Logistics.InventoryStock.AccessInventurAccess.BlockWarehouses(warehouseIds, true, botransaction.connection, botransaction.transaction); // - no warehouse mvmt in EBAS
									Infrastructure.Data.Access.Joins.Logistics.InventoryAccess.BlockPco(warehouseId, true, kstransaction.connection, kstransaction.transaction); // - no PCO changes
									Infrastructure.Data.Access.Tables.Logistics.InventoryStock.TaskByRoleAccess.UpdateTaskStatus(new List<string> { "7", "8", "9" }, warehouseId, completeStatus, botransaction.connection, botransaction.transaction);
								}
								else
								{
									// - close WIP
									if(entity.status == completeStatus)
									{
										if(taskBeforeChange.status == todoStatus)
										{
											// - take care of changes when task goes from todo to in-progress. JIC user goes from todo to complete
											Infrastructure.Data.Access.Tables.Logistics.InventoryStock.AccessInventurAccess.BlockT1ProdToHaupt(warehouseIds, true, botransaction.connection, botransaction.transaction); // - no typ1 retour
											Infrastructure.Data.Access.Tables.Logistics.InventoryStock.AccessInventurAccess.BlockT2ProdToHaupt(warehouseIds, true, botransaction.connection, botransaction.transaction); // - no typ2 retour
											Infrastructure.Data.Access.Tables.Logistics.InventoryStock.AccessInventurAccess.BlockWarehouses(warehouseIds, true, botransaction.connection, botransaction.transaction); // - no warehouse mvmt in EBAS
											Infrastructure.Data.Access.Joins.Logistics.InventoryAccess.BlockPco(warehouseId, true, kstransaction.connection, kstransaction.transaction); // - no PCO changes																												  //Infrastructure.Data.Access.Tables.Logistics.InventoryStock.AccessInventurAccess.BlockWarehouses(warehouseIds, true, botransaction.connection, botransaction.transaction); // - no PCO chnages
											Infrastructure.Data.Access.Tables.Logistics.InventoryStock.TaskByRoleAccess.UpdateTaskStatus(new List<string> { "7", "8", "9" }, warehouseId, completeStatus, botransaction.connection, botransaction.transaction);
										}
										// - when task goes to complete
										Infrastructure.Data.Access.Tables.Logistics.InventoryStock.TaskByRoleAccess.UpdateTaskStatus(new List<string> { "10" }, warehouseId, completeStatus, botransaction.connection, botransaction.transaction);
										// -- update wip
										Infrastructure.Data.Access.Tables.Logistics.InventoryStock.ProductionWipAccess.UpdatePercent(warehouseId, warehouseId != 6, botransaction.connection, botransaction.transaction);
									}

									// - reverting to todo 
									if(entity.status == todoStatus)
									{
										// - open retoure
										Infrastructure.Data.Access.Tables.Logistics.InventoryStock.AccessInventurAccess.BlockT1ProdToHaupt(warehouseIds, false, botransaction.connection, botransaction.transaction); // - allow back typ1 retour
										Infrastructure.Data.Access.Tables.Logistics.InventoryStock.AccessInventurAccess.BlockT2ProdToHaupt(warehouseIds, false, botransaction.connection, botransaction.transaction); // - allow back typ2 retour
										Infrastructure.Data.Access.Tables.Logistics.InventoryStock.AccessInventurAccess.BlockWarehouses(warehouseIds, false, botransaction.connection, botransaction.transaction); // - allow back warehouse mvmt in EBAS
										Infrastructure.Data.Access.Joins.Logistics.InventoryAccess.BlockPco(warehouseId, false, kstransaction.connection, kstransaction.transaction); // - allow back PCO changes																				   //Infrastructure.Data.Access.Tables.Logistics.InventoryStock.AccessInventurAccess.BlockWarehouses(warehouseIds, true, botransaction.connection, botransaction.transaction); // - no PCO chnages
										Infrastructure.Data.Access.Tables.Logistics.InventoryStock.TaskByRoleAccess.UpdateTaskStatus(new List<string> { "7", "8", "9" }, warehouseId, todoStatus, botransaction.connection, botransaction.transaction);

									}
									// - close - revert reports 6 - 10
									if(taskBeforeChange.status == completeStatus)
									{
										Infrastructure.Data.Access.Tables.Logistics.InventoryStock.TaskByRoleAccess.UpdateTaskStatus(new List<string> { "10" }, warehouseId, todoStatus, botransaction.connection, botransaction.transaction);
										// -- reset wip
										Infrastructure.Data.Access.Tables.Logistics.InventoryStock.ProductionWipAccess.ResetPercent(warehouseId, botransaction.connection, botransaction.transaction);
									}
								}
							}
							break;
						case "h.":
							{
								if(entity.status == completeStatus)
								{
									// - set Responsible
									Infrastructure.Data.Access.Tables.Logistics.InventoryStock.InventoryFaStatsAccess.WarehouseValidateInventory(warehouseId, _user.Id, _user.Username, botransaction.connection, botransaction.transaction);
									Infrastructure.Data.Access.Tables.Logistics.InventoryStock.LogsAccess.InsertWithTransaction(new LogsEntity
									{
										LogTime = DateTime.Now,
										LogUserId = _user.Id,
										ObjectId = warehouseId,
										ObjectName = "InventoryStock",
										LogDescription = $"The inventory stock has been [Plant] validated in Lager [{warehouseId}] at [{DateTime.Now:yyyy-MM-dd HH:mm}] by [{_user.Name}]",
										LogsType = 2,
										LogUserName = _user.Name,
										LagerId = warehouseId
									}, botransaction.connection, botransaction.transaction);

									// - send request after transaction commits
									sendApprovalRequestNotification = true;
								}
								/*
								 // - 2025-10-28 - These c´hanges will be manually done 
								if(entity.status == completeStatus)
								{
									Infrastructure.Data.Access.Tables.Logistics.InventoryStock.AccessInventurAccess.BlockScanner(warehouseIds, true, botransaction.connection, botransaction.transaction); // - no scanner changes

									Infrastructure.Data.Access.Tables.Logistics.InventoryStock.AccessInventurAccess.BlockFaStart(warehouseIds, false, botransaction.connection, botransaction.transaction); // - allow back fa Start																				   //Infrastructure.Data.Access.Tables.Logistics.InventoryStock.AccessInventurAccess.BlockScanner(warehouseIds, false, botransaction.connection, botransaction.transaction); // - allow PCO changes
									Infrastructure.Data.Access.Tables.Logistics.InventoryStock.AccessInventurAccess.BlockT1HauptToProd(warehouseIds, false, botransaction.connection, botransaction.transaction); // - allow typ1 mvmt
									Infrastructure.Data.Access.Tables.Logistics.InventoryStock.AccessInventurAccess.BlockT1ProdToHaupt(warehouseIds, false, botransaction.connection, botransaction.transaction); // - allow typ1 mvmt
									Infrastructure.Data.Access.Tables.Logistics.InventoryStock.AccessInventurAccess.BlockT2HauptToProd(warehouseIds, false, botransaction.connection, botransaction.transaction); // - allow typ2 mvmt
									Infrastructure.Data.Access.Tables.Logistics.InventoryStock.AccessInventurAccess.BlockT2ProdToHaupt(warehouseIds, false, botransaction.connection, botransaction.transaction); // - allow typ2 mvmt
									Infrastructure.Data.Access.Tables.Logistics.InventoryStock.AccessInventurAccess.BlockSchneiderei(warehouseIds, false, botransaction.connection, botransaction.transaction); // - allow back warehouse mvmt in EBAS
									Infrastructure.Data.Access.Tables.Logistics.InventoryStock.AccessInventurAccess.BlockWarehouses(warehouseIds, false, botransaction.connection, botransaction.transaction); // - allow back warehouse mvmt in EBAS
									Infrastructure.Data.Access.Joins.Logistics.InventoryAccess.BlockPco(warehouseId, false, kstransaction.connection, kstransaction.transaction); // - allow back PCO changes		
									Infrastructure.Data.Access.Tables.Logistics.InventoryStock.TaskByRoleAccess.UpdateTaskStatus(new List<string> { "11", "12", "13", "14" }, warehouseId, completeStatus, botransaction.connection, botransaction.transaction); // - 
								}
								else
								{
									// - reveerting from complete
									if(taskBeforeChange.status == completeStatus)
									{
										Infrastructure.Data.Access.Tables.Logistics.InventoryStock.AccessInventurAccess.BlockScanner(warehouseIds, false, botransaction.connection, botransaction.transaction); // - allow back Scanner changes

										Infrastructure.Data.Access.Tables.Logistics.InventoryStock.AccessInventurAccess.BlockFaStart(warehouseIds, true, botransaction.connection, botransaction.transaction); // - no fa Start																						//Infrastructure.Data.Access.Tables.Logistics.InventoryStock.AccessInventurAccess.BlockScanner(warehouseIds, false, botransaction.connection, botransaction.transaction); // - allow PCO changes
										Infrastructure.Data.Access.Tables.Logistics.InventoryStock.AccessInventurAccess.BlockT1HauptToProd(warehouseIds, true, botransaction.connection, botransaction.transaction); // - no typ1 mvmt
										Infrastructure.Data.Access.Tables.Logistics.InventoryStock.AccessInventurAccess.BlockT1ProdToHaupt(warehouseIds, true, botransaction.connection, botransaction.transaction); // - no typ1 mvmt
										Infrastructure.Data.Access.Tables.Logistics.InventoryStock.AccessInventurAccess.BlockT2HauptToProd(warehouseIds, true, botransaction.connection, botransaction.transaction); // - no typ2 mvmt
										Infrastructure.Data.Access.Tables.Logistics.InventoryStock.AccessInventurAccess.BlockT2ProdToHaupt(warehouseIds, true, botransaction.connection, botransaction.transaction); // - no typ2 mvmt
										Infrastructure.Data.Access.Tables.Logistics.InventoryStock.AccessInventurAccess.BlockSchneiderei(warehouseIds, true, botransaction.connection, botransaction.transaction); // - no typ2 mvmt
										Infrastructure.Data.Access.Tables.Logistics.InventoryStock.AccessInventurAccess.BlockWarehouses(warehouseIds, true, botransaction.connection, botransaction.transaction); // - no typ2 mvmt
										Infrastructure.Data.Access.Joins.Logistics.InventoryAccess.BlockPco(warehouseId, true, kstransaction.connection, kstransaction.transaction); // - no PCO changes		
										Infrastructure.Data.Access.Tables.Logistics.InventoryStock.TaskByRoleAccess.UpdateTaskStatus(new List<string> { "11", "12", "13", "14" }, warehouseId, todoStatus, botransaction.connection, botransaction.transaction); // - 
									}
								}
								*/
							}
							break;
						case "12":
							{
								// - closing IT Stock levels corrections - send reports
								if(entity.status == completeStatus)
								{
									SendReports(warehouseId, botransaction);
								}
							}
							break;
						default:
							break;
					}
				}

				#region add logs
				var logs = Psz.Core.Logistics.Helpers.InventoryStockLogHelper.GenerateLogForUpdateIstTaskStatus(_user, taskBeforeChange, _data);
				var InsertLogsResult = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.LogsAccess.InsertWithTransaction(logs, botransaction.connection, botransaction.transaction);
				#endregion add logs
				#endregion

				if(botransaction.commit() && kstransaction.commit())
				{
					if(sendApprovalRequestNotification)
					{
						// - do it :-)
						sendApprovalRequestEmail();
					}
					return ResponseModel<int>.SuccessResponse(result);
				}
				else
				{
					return ResponseModel<int>.FailureResponse("Transaction error");
				}
			} catch(Exception e)
			{
				botransaction.rollback();
				kstransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			int warehouseId = this._data.lagerId ?? 0;
			var taskBeforeChange = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.TaskByRoleAccess.Get(_data.Id ?? -1);
			var entity = new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity
			{
				Id = this._data.Id ?? -1,
				status = this._data.status ?? -1,
				lagerId = _data.lagerId ?? -1
			};


			var updatedTask = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.TaskByRoleAccess.Get(entity.Id);
			var taskTitle = updatedTask.title?.TrimStart().Substring(0, 2);
			var todoStatus = (int)Enums.WarehouseMouvementStatus.Todo;
			var inprogressStatus = (int)Enums.WarehouseMouvementStatus.Inprogress;
			var completeStatus = (int)Enums.WarehouseMouvementStatus.Complete;
			var inventoryReleaseTask = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.TaskByRoleAccess.GetByNumber($"h", warehouseId);
			var inventoryReleased = inventoryReleaseTask?.status == (int)Enums.WarehouseMouvementStatus.Complete;
			switch(taskTitle)
			{
				case "a.":
				case "b.":
				case "c.":
				case "d.":
					{
						if(inventoryReleased)
						{
							return ResponseModel<int>.FailureResponse($"This change is forbiden because task [{inventoryReleaseTask.title}] is complete.");
						}
					}
					break;
				case "f.":
					{
						if(inventoryReleased)
						{
							return ResponseModel<int>.FailureResponse($"This change is forbiden because task [{inventoryReleaseTask.title}] is complete.");
						}
					}
					break;
				case "e.":
				case "g.":
					{
						if(inventoryReleased)
						{
							return ResponseModel<int>.FailureResponse($"This change is forbiden because task [{inventoryReleaseTask.title}] is complete.");
						}
					}
					break;
				case "h.":
					{
						if(!_user.IsGlobalDirector && !_user.SuperAdministrator && _user.Access.Logistics.InventoryWarehouseValidate != true)
						{
							return ResponseModel<int>.FailureResponse("Inventory release not allowed. User does not have access.");
						}
						if(inventoryReleased)
						{
							return ResponseModel<int>.FailureResponse($"This change is forbiden because task [{inventoryReleaseTask.title}] is complete.");
						}
						// - prevent reverting from h. complete to other status
						if(entity.status != completeStatus && taskBeforeChange.status == completeStatus)
						{
							return ResponseModel<int>.FailureResponse("Inventory release cannot be opened. Inventory has already been released.");
						}
					}
					break;
				case "11":
				case "12":
				case "13":
				case "14":
					{
						if(!inventoryReleased)
						{
							return ResponseModel<int>.FailureResponse($"This change is forbiden because task [{inventoryReleaseTask.title}] is not yet complete.");
						}
						else
						{
							var inventoryData = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.InventoryFaStatsAccess.GetByWarehouse(warehouseId);
							if(entity.status == completeStatus && inventoryData.HqValidatorValidateTime is null)
							{
								return ResponseModel<int>.FailureResponse($"This change is forbiden because inventory is not yet validated by VOH.");
							}
						}
					}
					break;
				default:
					break;
			}

			int itTaskNumber = updatedTask.role == "IT"
				? (int.TryParse(updatedTask.title.Substring(0, updatedTask.title.IndexOf('.')), out var _x) ? _x : 0)
				: 0;
			if(itTaskNumber > 1)
			{
				if(entity.status == completeStatus)
				{
					var prevTask = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.TaskByRoleAccess.GetByNumber($"{itTaskNumber - 1}", warehouseId);
					if(prevTask?.status != (int)Enums.WarehouseMouvementStatus.Complete)
					{
						return ResponseModel<int>.FailureResponse($"This change is forbiden because previous task [{prevTask.title}] is not complete.");
					}
				}
				else
				{
					// - changing task from complete
					var nextTask = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.TaskByRoleAccess.GetByNumber($"{itTaskNumber + 1}", warehouseId);
					if(taskBeforeChange.status == (int)Enums.WarehouseMouvementStatus.Complete && nextTask?.status == (int)Enums.WarehouseMouvementStatus.Complete)
					{
						return ResponseModel<int>.FailureResponse($"This change is forbiden because next task [{nextTask.title}] is complete.");
					}
				}
			}

			return ResponseModel<int>.SuccessResponse();
		}
		private void SendReports(int warehouseId, Infrastructure.Services.Utils.TransactionsManager transaction)
		{
			byte[] freigabeReport = GetReleaseReportPDF(warehouseId, transaction);

			try
			{
				var destinationAddress = new List<string>
				{
					"Sani.ChaibouSalaou@psz-electronic.com"
				};
				var inventoryStats = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.InventoryFaStatsAccess.GetByWarehouse(_data.lagerId ?? 0);
				var requester = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(inventoryStats?.WarehouseValidatorId ?? -2);
				var HQValidatorProfiles = Infrastructure.Data.Access.Tables.Logistics.AccessProfileAccess.GetInventoryHqValidationActive();
				if(HQValidatorProfiles?.Count>0)
				{
					var hqValidators = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(
						Infrastructure.Data.Access.Tables.Logistics.AccessProfileUsersAccess.GetByAccessProfileIds(
							HQValidatorProfiles.Select(x=> x.Id)?.ToList())?.Select(x=> x.UserId)?.ToList()
							)?.Select(x=> x.Email);
					if(hqValidators?.Count()>0)
					{
						destinationAddress.AddRange(hqValidators);
					}
				}
				if(!string.IsNullOrWhiteSpace( requester?.Email))
				{
					destinationAddress.Add(requester?.Email);
				}
				Module.EmailingService.SendEmailAsync(
					"Inventurfreigabe",
					$@"<div style='font-family: Arial, sans-serif; font-size: 14px; color: #333333; line-height: 1.5;'>
							  <div style='margin-bottom: 10px;'>Approval Required - Inventory Release</div>
							  <div style='margin-bottom: 10px;'>The following inventory release request requires validation:</div>
							  <table cellpadding='4' cellspacing='0' style='border-collapse: collapse; margin-top: 8px; margin-bottom: 16px;'>
								<tr>
								  <td style='font-weight: bold; padding-right: 8px;'>Requester:</td>
								  <td>{_user.Name}</td>
								</tr>
								<tr>
								  <td style='font-weight: bold; padding-right: 8px;'>Site:</td>
								  <td>{warehouseId}</td>
								</tr>
								<tr>
								  <td style='font-weight: bold; padding-right: 8px;'>Submitted At:</td>
								  <td>{DateTime.Now:dd.MM.yyyy HH:mm}</td>
								</tr>
							  </table>
							  <div style='margin-bottom: 10px;'>
								Please access the system to review and approve the request by using the link below:
							  </div>
							  <div style='margin-bottom: 10px;'>
								<a href='/inventory-stock' style='display: inline-block; text-decoration: none; padding: 8px 16px; border-radius: 4px; border: 1px solid #333333;'>
								  Open Inventory Release
								</a>
							  </div>
							  <div style='margin-top: 16px; font-size: 12px; color: #777777;'>
								This is an automated notification. Please do not reply to this email.
							  </div>
							</div>",
					destinationAddress?.Distinct()?.ToList(),
					new List<KeyValuePair<string, System.IO.Stream>> { new KeyValuePair<string, System.IO.Stream>("Inventurfreigabe.pdf", new System.IO.MemoryStream(freigabeReport)) });
			} catch
			{
				throw;
			}
		}
		public static byte[] GetReleaseReportPDF(int warehouseId, Infrastructure.Services.Utils.TransactionsManager transaction)
		{
			var warehouse = Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.GetWithTransaction(warehouseId, transaction.connection, transaction.transaction);
			var companyInfo = Infrastructure.Data.Access.Tables.STG.CompanyAccess.GetFirst();
			var inventoryStats = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.InventoryFaStatsAccess.Get(warehouseId, DateTime.Today.Year, transaction.connection, transaction.transaction);

			// -
			var freigabeReport = Infrastructure.Services.Reporting.IText.LGT.GenerateInventurfreigabeReport(
				companyName: companyInfo.Name,
				companyAddress: $"{companyInfo.Address}, {companyInfo.PostalCode} {companyInfo.City}, {companyInfo.Country}",
				logoBytes: companyInfo.Logo,
				title: "Inventurfreigabe",
				standort: warehouse.Lagerort,
				startDate: (inventoryStats?.StartTime ?? DateTime.Now).ToString("dd.MM.yyyy HH:mm"),
				closeDate: (inventoryStats?.StopTime ?? DateTime.Now).ToString("dd.MM.yyyy HH:mm"),
				jahr: (inventoryStats?.StartTime ?? DateTime.Now).Year.ToString(),
				responsibleValidateTime: (inventoryStats?.WarehouseValidatorValidateTime ?? DateTime.Now).ToString("dd.MM.yyyy HH:mm"),
				responsible: $"Logistics [{warehouse.Lagerort}] | {(inventoryStats?.WarehouseValidatorName ?? "Inventory Responsible")}",
				examiner: $"Logistics [VOH] | {(inventoryStats?.HqValidatorName ?? "Inventory Examiner")}",
				examinerValidateTime: (inventoryStats?.HqValidatorValidateTime ?? DateTime.Now).ToString("dd.MM.yyyy HH:mm"),
				remarksHL: inventoryStats.WarehouseNotesHL,
				remarksPL: inventoryStats.WarehouseNotesPL);
			return freigabeReport;
		}
		void sendApprovalRequestEmail()
		{
			try
			{
				var destinationAddress = new List<string>
				{
					"Sani.ChaibouSalaou@psz-electronic.com"
				};
				var inventoryStats = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.InventoryFaStatsAccess.GetByWarehouse(_data.lagerId ?? 0);
				var requester = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(inventoryStats?.WarehouseValidatorId ?? -2);
				var HQValidatorProfiles = Infrastructure.Data.Access.Tables.Logistics.AccessProfileAccess.GetInventoryHqValidationActive();
				if(HQValidatorProfiles?.Count > 0)
				{
					var hqValidators = Infrastructure.Data.Access.Tables.COR.UserAccess.Get(
						Infrastructure.Data.Access.Tables.Logistics.AccessProfileUsersAccess.GetByAccessProfileIds(
							HQValidatorProfiles.Select(x => x.Id)?.ToList())?.Select(x => x.UserId)?.ToList()
							)?.Select(x => x.Email);
					if(hqValidators?.Count() > 0)
					{
						destinationAddress.AddRange(hqValidators);
					}
				}
				if(!string.IsNullOrWhiteSpace(requester?.Email))
				{
					destinationAddress.Add(requester?.Email);
				}

				var warehouse = Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.Get(_data.lagerId ?? 0);
				var title = "[EBAS] Inventory Release Approval Request";
				var content = "";
				content += $"<div style='font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;max-width:600px;'>{DateTime.Now.ToString("D", new System.Globalization.CultureInfo("en-US"))}<br/>";
				content += $"<span style='font-size:1.5em;'>Good {(DateTime.Now.Hour <= 12 ? "morning" : "afternoon")},</span><br/><br/>";
				content += $"</div>";
				content += $"<div style='font-family: Arial, sans-serif; font-size: 14px; color: #333333; line-height: 1.5;'>";
				//content += $"<div style='margin-bottom: 10px;'>Approval Required - Inventory Release</div>";
				content += $"<div style='margin-bottom: 10px;'>The following inventory release request requires validation:</div>";
				content += $"<table cellpadding='4' cellspacing='0' style='border-collapse: collapse; margin-top: 8px; margin-bottom: 16px;'>";
				content += $"<tr>";
				content += $"<td style='font-weight: bold; padding-right: 8px;'>Requester:</td>";
				content += $"<td>{_user.Name}</td>";
				content += $"</tr>";
				content += $"<tr>";
				content += $"<td style='font-weight: bold; padding-right: 8px;'>Site:</td>";
				content += $"<td>{warehouse?.Lagerort}</td>";
				content += $"</tr>";
				content += $"<tr>";
				content += $"<td style='font-weight: bold; padding-right: 8px;'>Submitted At:</td>";
				content += $"<td>{DateTime.Now:dd.MM.yyyy HH:mm}</td>";
				content += $"</tr>";
				content += $"</table>";
				content += $"<div style='margin-bottom: 10px;'>";
				content += $"Please access the system to review and approve the request.";
				content += $"</div>";
				//content += $"<div style='margin-bottom: 10px;'>";
				//content += $"<a href='/inventory-stock' style='display: inline-block; text-decoration: none; padding: 8px 16px; border-radius: 4px; border: 1px solid #333333;'>";
				//content += $"Open Inventory Release";
				//content += $"</a>";
				//content += $"</div>";
				content += $"<div style='margin-top: 16px; font-size: 12px; color: #777777;'>";
				content += $"This is an automated notification. Please do not reply to this email.";
				content += $"</div>";
				content += $"</div>";

				Module.EmailingService.SendEmailAsync(title, content, destinationAddress);
			} catch
			{
				throw;
			}
		}
	}
}