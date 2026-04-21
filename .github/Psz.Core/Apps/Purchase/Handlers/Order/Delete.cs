using Psz.Core.CustomerService.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.Purchase.Handlers
{
	public partial class Order
	{
		public static Core.Models.ResponseModel<object> Delete(int id,
			Core.Identity.Models.UserModel user)
		{
			if(user == null || (!user.Access.CustomerService.ModuleActivated && !user.Access.Purchase.ModuleActivated))
			{
				throw new Core.Exceptions.UnauthorizedException();
			}

			lock(Locks.OrdersLock)
			{
				try
				{
					var orderDb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(id);
					if(orderDb == null)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = new List<string>() { "Order not found" }
						};
					}

					if(orderDb.Erledigt == true)
					{
						return Core.Models.ResponseModel<object>.FailureResponse($"Order Confirmation is done, cannot delete");
					}



					// - 2023-04-18 - transaction-based
					var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
					try
					{
						botransaction.beginTransaction();

						#region // -- transaction-based logic -- //
						//done: - insert process here

						//-
						if(orderDb.Typ == Enums.OrderEnums.TypeToData(Enums.OrderEnums.Types.Contract))
						{
							var errors = new List<string>();
							// - reset RA quantity, if any
							var raEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetWithTransaction(orderDb.Nr_RA ?? -1, botransaction.connection, botransaction.transaction);
							var abPositions = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(orderDb.Nr, botransaction.connection, botransaction.transaction);
							var raPositions = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetWithTransaction(abPositions?.Select(x => x.ABPoszuRAPos ?? -1)?.ToList(), botransaction.connection, botransaction.transaction);
							var raPosExtenions = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.GetByRahmenPositionNr(raPositions?.Select(x => x.Nr)?.ToList(), botransaction.connection, botransaction.transaction);
							var raToUpdate = new List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity>();
							if(raPositions != null && raPositions.Count > 0)
							{
								foreach(var raPosItem in raPositions)
								{
									var ext = raPosExtenions.FirstOrDefault(x => x.AngeboteArtikelNr == raPosItem.Nr);
									if(raPosItem.Anzahl <= 0 || (ext?.GultigBis ?? DateTime.MaxValue) < DateTime.Today)
									{
										errors.Add($"Position [{raPosItem.Position}]: rahmen position is closed.");
									}
								}
								foreach(var abPosItem in abPositions)
								{
									var raPos = raPositions.FirstOrDefault(x => x.Nr == abPosItem.ABPoszuRAPos);
									if(raPos != null)
									{
										raPos.Anzahl = (raPos.Anzahl ?? 0) + (abPosItem.Anzahl ?? 0);
										raPos.Geliefert = (raPos.Geliefert ?? 0) - (abPosItem.Anzahl ?? 0);
										raPos.Gesamtpreis = (raPos.Anzahl ?? 0) * (abPosItem.Einzelpreis ?? 0);
										raPos.erledigt_pos = (raPos.Geliefert ?? 0) != (abPosItem.OriginalAnzahl ?? 0);
										raToUpdate.Add(raPos);
									}
								}
							}
							if(errors.Count > 0)
							{
								return new Core.Models.ResponseModel<object>
								{
									Success = false,
									Errors = errors
								};
							}

							if(raToUpdate.Count > 0)
								Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.UpdateWithTransaction(raToUpdate, botransaction.connection, botransaction.transaction);

							// - 
							Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.DeleteByOrder(id, botransaction.connection, botransaction.transaction);
							Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.DeleteWithTransaction(id, botransaction.connection, botransaction.transaction);
							var _log = new LogHelper(orderDb.Nr, (int)orderDb.Angebot_Nr, int.TryParse(orderDb.Projekt_Nr, out var val) ? val : 0, orderDb.Typ, LogHelper.LogType.DELETIONOBJECT, "CTS", user)
								.LogCTS(null, null, null, 0);
							Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.InsertWithTransaction(_log, botransaction.connection, botransaction.transaction);
							// - return Core.Models.ResponseModel<object>.SuccessResponse();
						}
						else
						{
							if(orderDb.Typ == Enums.OrderEnums.TypeToData(Enums.OrderEnums.Types.Confirmation))
							{
								var errorMsg = "";
								if(Helpers.ItemHelper.CanDeleteOrder(id, orderDb.Angebot_Nr ?? -1, out errorMsg))
								{
									Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.DeleteByOrder(id, botransaction.connection, botransaction.transaction);
									Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.DeleteWithTransaction(id, botransaction.connection, botransaction.transaction);
									var _log = new LogHelper(orderDb.Nr, (int)orderDb.Angebot_Nr, int.TryParse(orderDb.Projekt_Nr, out var val) ? val : 0, orderDb.Typ, LogHelper.LogType.DELETIONOBJECT, "CTS", user)
										.LogCTS(null, null, null, 0);
									//delete delfor link if any
									var delforItemPlans = Infrastructure.Data.Access.Tables.CTS.LineItemPlanAccess.GetByOrder(id, botransaction.connection, botransaction.transaction);
									if(delforItemPlans != null && delforItemPlans.Count > 0)
									{
										foreach(var item in delforItemPlans)
										{
											item.OrderId = null;
											item.OrderItemId = null;
											item.OrderUserId = null;
											item.OrderDate = null;
										}
										Infrastructure.Data.Access.Tables.CTS.LineItemPlanAccess.UpdateWithTransaction(delforItemPlans, botransaction.connection, botransaction.transaction);
									}

									Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.InsertWithTransaction(_log, botransaction.connection, botransaction.transaction);
									// - return Core.Models.ResponseModel<object>.SuccessResponse();

								}
								else
								{
									return Core.Models.ResponseModel<object>.FailureResponse(errorMsg);
								}
							}
							else
							{
								Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.DeleteByOrder(id, botransaction.connection, botransaction.transaction);
								Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.DeleteWithTransaction(id, botransaction.connection, botransaction.transaction);
								var _log = new LogHelper(orderDb.Nr, (int)orderDb.Angebot_Nr, int.TryParse(orderDb.Projekt_Nr, out var val) ? val : 0, orderDb.Typ, LogHelper.LogType.DELETIONOBJECT, "CTS", user)
									.LogCTS(null, null, null, 0);
								Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.InsertWithTransaction(_log, botransaction.connection, botransaction.transaction);
								// - return Core.Models.ResponseModel<object>.SuccessResponse();
							}
						}
						#endregion // -- transaction-based logic -- //

						//done: handle transaction state (success or failure)
						if(botransaction.commit())
						{
							return Core.Models.ResponseModel<object>.SuccessResponse();
						}
						else
						{
							return Core.Models.ResponseModel<object>.FailureResponse("Transaction error");
						}
					} catch(Exception e)
					{
						botransaction.rollback();
						Infrastructure.Services.Logging.Logger.Log(e);
						throw;
					}

				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}
	}
}


