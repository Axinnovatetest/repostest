using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Xml.Serialization;

namespace Psz.Core.Apps.EDI.Handlers
{
	public partial class Order
	{
		public static Core.Models.ResponseModel<object> ValidateUpdate(int orderId, Core.Identity.Models.UserModel user, bool UpdateChoice = false)
		{

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				botransaction.beginTransaction();

				var response = new Core.Models.ResponseModel<object>();
				#region // -- transaction-based logic -- //
				//-
				lock(Locks.OrdersLock)
				{
					#region > Validation
					if(user == null || !user.Access.CustomerService.EDIOrderEdit)
					{
						return Core.Models.ResponseModel<object>.AccessDeniedResponse();
					}

					var orderDb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetWithTransaction(orderId, botransaction.connection, botransaction.transaction);
					if(orderDb == null)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Success = false,
							Errors = new List<string>()
						{
							"Order not found"
						}
						};
					}

					if(!(orderDb.Neu_Order ?? false))
					{
						return new Core.Models.ResponseModel<object>()
						{
							Success = false,
							Errors = new List<string>()
						{
							"Order already validated"
						}
						};
					}

					// - 2022-11-24 - prevent validation of Orders imported from P3000
					if(!string.IsNullOrEmpty(orderDb.EDI_Dateiname_CSV) && orderDb.EDI_Dateiname_CSV.Trim().ToLower().Substring(orderDb.EDI_Dateiname_CSV.Trim().Length - 4, 4) == ".csv")
					{
						return Core.Models.ResponseModel<object>.FailureResponse("Cannot validate CSV file order, please try it from P3000");
					}

					// Positions validation
					var positionErrors = new List<string> { };
					var postionsDb = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(orderId, botransaction.connection, botransaction.transaction, false);
					if(postionsDb != null || postionsDb.Count > 0)
					{
						for(int i = 0; i < postionsDb.Count; i++)
						{
							if(postionsDb[i].Anzahl == 0)
							{
								continue;
							}

							if(UpdateChoice && (postionsDb[i].Anzahl < 0))
							{
								positionErrors.Add($"Position {postionsDb[i].Position}: invalid value '{postionsDb[i].Anzahl}' for quantity.");
								continue;
							}
							if(UpdateChoice && (postionsDb[i].Wunschtermin == null || DateTime.TryParse(postionsDb[i].Wunschtermin.ToString(), out DateTime w) != true))
							{
								positionErrors.Add($"Position {postionsDb[i].Position}: invalid value '{postionsDb[i].Wunschtermin}' for Wunschtermin.");
								continue;
							}
							if(UpdateChoice && (postionsDb[i].Liefertermin == null || DateTime.TryParse(postionsDb[i].Liefertermin.ToString(), out DateTime d) != true))
							{
								postionsDb[i].Liefertermin = postionsDb[i].Wunschtermin;
							}
							if(UpdateChoice && (postionsDb[i].Lagerort_id == null || postionsDb[i].Lagerort_id <= 0))
							{
								var articleProductionExtensionEntity = Infrastructure.Data.Access.Tables.BSD.ArtikelProductionExtensionAccess.GetByArticleId(postionsDb[i].ArtikelNr ?? -1);
								var articleProductionPlace = articleProductionExtensionEntity?.ProductionPlace1_Id;
								var storageLocation = Enums.OrderEnums.GetArticleHauplager((Psz.Core.Apps.EDI.Enums.OrderEnums.ArticleProductionPlace__)articleProductionPlace);

								if(articleProductionPlace == (int)Psz.Core.Common.Enums.ArticleEnums.ArticleProductionPlace.TN)
								{
									storageLocation = Enums.OrderEnums.GetArticleHauplager(Psz.Core.Apps.EDI.Enums.OrderEnums.ArticleProductionPlace__.WS);
								}
								postionsDb[i].Lagerort_id = storageLocation.Key;
							}
						}
						Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Update(postionsDb);
					}
					if(positionErrors.Count > 0)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Success = false,
							Errors = positionErrors
						};
					}
					#endregion

					Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.UpdateNeuOrder(orderDb.Nr, false, botransaction.connection, botransaction.transaction);

					// if order was already assigned a project value; DO NOT change it!
					var nextAngeboteNr = (string.IsNullOrEmpty(orderDb.Projekt_Nr) || string.IsNullOrWhiteSpace(orderDb.Projekt_Nr)) ?
						$"{Convert.ToInt32(Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.MaxAngebotNrByTyp(Apps.Purchase.Enums.OrderEnums.TypeToData(Purchase.Enums.OrderEnums.Types.Confirmation), botransaction.connection, botransaction.transaction)) + 1}"
						: orderDb.Projekt_Nr;

					var nextProjektNr = nextAngeboteNr;
					Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.UpdateWithTransaction(orderDb.Nr,
						nextAngeboteNr,
						nextProjektNr,
						$"Gebucht, {user.Username}, {DateTime.Now}",
						true, botransaction.connection, botransaction.transaction);

					var orderExtensionDb = Infrastructure.Data.Access.Tables.PRS.OrderExtensionAccess.GetByOrderId(orderDb.Nr, botransaction.connection, botransaction.transaction);
					if(orderExtensionDb == null)
					{
						Infrastructure.Data.Access.Tables.PRS.OrderExtensionAccess.InsertWithTRansaction(new Infrastructure.Data.Entities.Tables.PRS.OrderExtensionEntity()
						{
							Id = -1,
							OrderId = orderDb.Nr,
							EdiValidationTime = DateTime.Now,
							EdiValidationUserId = user.Id,
							LastUpdateTime = DateTime.Now,
							LastUpdateUserId = user.Id,
							LastUpdateUsername = user.Username,
							RecipientId = null,
							SenderDuns = null,
						}, botransaction.connection, botransaction.transaction);
					}
					else
					{
						orderExtensionDb.EdiValidationUserId = user.Id;
						orderExtensionDb.EdiValidationTime = DateTime.Now; // Convert Berlin Time
						Infrastructure.Data.Access.Tables.PRS.OrderExtensionAccess.UpdateWithTRansaction(orderExtensionDb, botransaction.connection, botransaction.transaction);
					}

					// > Notify
					// > Notify

					Core.Program.Notifier.PushEdiImportedOrdersNotification(new Core.Apps.EDI.Models.HubMessage.ImportedOrdersNotificationModel()
					{
						Type = "Success",
						Payload = Order.CountUnvalidated(null, botransaction).ToString()
					});

					// - 2025-03-14 logs
					var logs = new CustomerService.Helpers.LogHelper(orderDb.Nr, Convert.ToInt32( nextAngeboteNr), int.TryParse(nextProjektNr, out var v) ? v : 0, orderDb.Typ, CustomerService.Helpers.LogHelper.LogType.VALIDATIONORDER, "EDI", user)
							.LogCTS(null, null, null, 0);
					if(UpdateChoice)
					{
						logs.LogText += " [Confirmed as ordered]";
					}
					Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.InsertWithTransaction(logs, botransaction.connection, botransaction.transaction);

					// > Order Response
					response = GenerateOrderResponse(Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetWithTransaction(orderDb.Nr, botransaction.connection, botransaction.transaction), botransaction);
				}

				#endregion // -- transaction-based logic -- //

				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					return response;
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


		}
	}
}
