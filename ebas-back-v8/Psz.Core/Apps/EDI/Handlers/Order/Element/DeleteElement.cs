using Psz.Core.CustomerService.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Apps.EDI.Handlers
{
	public partial class Order
	{
		public static Core.Models.ResponseModel<object> DeleteElement(Models.Order.Element.DeleteElementModel data,
			Core.Identity.Models.UserModel user)
		{
			lock(Locks.OrdersLock)
			{
				try
				{
					if(user == null || (!user.Access.Purchase.ModuleActivated && !user.Access.CustomerService.ModuleActivated))
					{
						throw new Core.Exceptions.UnauthorizedException();
					}

					var orderDb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(data.OrderId);
					if(orderDb == null)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = new List<string>() { "Order not found" }
						};
					}

					var orderElementDb = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(data.Id);
					if(orderElementDb == null)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = new List<string>() { "Order Element not found" }
						};
					}
					if(orderElementDb.Fertigungsnummer > 0)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = new List<string>() { "Element already in production" }
						};
					}

					if(orderElementDb.AngebotNr != orderDb.Nr)
					{
						return new Core.Models.ResponseModel<object>()
						{
							Errors = new List<string>() { "Element is not Element of Order" }
						};
					}
					//horizon check
					if(orderDb.Typ == Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_CONFIRMATION ||
						orderDb.Typ == Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_DELIVERY ||
						orderDb.Typ == Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_CREDIT ||
						orderDb.Typ == Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_INVOICE ||
						orderDb.Typ == Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_FORECAST)
					{
						//var technicArticles = Program.BSD.TechnicArticleIds;
						var orderArticleIsTechnic = CustomerService.Helpers.HorizonsHelper.ArticleIsTechnic(orderElementDb.ArtikelNr ?? -1);
						var horizonCheck = false;
						var horizonErrors = new List<string>();
						DateTime _oldDate, _newDate;
						_oldDate = _newDate = orderElementDb.Liefertermin ?? orderElementDb.Wunschtermin ?? new DateTime(1900, 1, 1);
						switch(orderDb.Typ)
						{
							case Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_CONFIRMATION:
								horizonCheck = Psz.Core.CustomerService.Helpers.HorizonsHelper.userHasABPosHorizonRight(_newDate, _oldDate, user, out List<string> msgs1);
								horizonErrors.AddRange(msgs1 ?? new List<string> { });
								break;
							case Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_DELIVERY:
								horizonCheck = Psz.Core.CustomerService.Helpers.HorizonsHelper.userHasLSPosHorizonRight(_newDate, _oldDate, user, out List<string> msgs2);
								horizonErrors.AddRange(msgs2 ?? new List<string> { });
								break;
							case Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_CREDIT:
								horizonCheck = Psz.Core.CustomerService.Helpers.HorizonsHelper.userHasGSPosHorizonRight(_oldDate, user, out List<string> msgs3);
								horizonErrors.AddRange(msgs3 ?? new List<string> { });
								break;
							case Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_INVOICE:
								horizonCheck = Psz.Core.CustomerService.Helpers.HorizonsHelper.userHasRGPosHorizonRight(_newDate, _oldDate, user, out List<string> msgs4);
								horizonErrors.AddRange(msgs4 ?? new List<string> { });
								break;
							case Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_FORECAST:
								horizonCheck = Psz.Core.CustomerService.Helpers.HorizonsHelper.userHasFRCPosHorizonRight(_newDate, _oldDate, user, out List<string> msgs5);
								horizonErrors.AddRange(msgs5 ?? new List<string> { });
								break;
						}
						if(horizonErrors != null && horizonErrors.Count > 0 && !orderArticleIsTechnic)
							return new Core.Models.ResponseModel<object>()
							{
								Errors = horizonErrors
							};
					}
					// - 2022-05-16 - soft/hard delete
					if(orderDb.Neu_Order.HasValue) // - EDI => soft-delete
					{
						Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.SoftDelete(orderElementDb.Nr);
						OrderElementExtension.SetStatus(orderElementDb.Nr);
					}
					else // - Manual AB => hard-delete
					{
						Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Delete(orderElementDb.Nr);
					}

					// - 2022-05-16 - check for sent delivery-notes on Position
					var deliveryNoteEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetDeliveryNotesByAB(orderDb.Nr);
					if(deliveryNoteEntities != null && deliveryNoteEntities.Count > 0)
					{
						var deliveredPositionEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetbyAngeboteNrs(deliveryNoteEntities.Select(x => x.Nr).ToList())
							?.Where(x => x.ArtikelNr == orderElementDb.ArtikelNr && x.Anzahl > 0 && x.LSPoszuABPos == data.Id)
							?.ToList();
						if(deliveredPositionEntities != null && deliveredPositionEntities.Count > 0)
						{
							return new Core.Models.ResponseModel<object>()
							{
								Errors = new List<string>() { $"Element has delivered quantity in LS" }
							};
						}
					}

					// - 2022-05-16 - soft/hard delete
					if(orderDb.Neu_Order.HasValue) // - EDI => soft-delete
					{
						Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.SoftDelete(orderElementDb.Nr);
						OrderElementExtension.SetStatus(orderElementDb.Nr);
					}
					else // - Manual AB => hard-delete
					{
						Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Delete(orderElementDb.Nr);
					}

					if(orderElementDb.ABPoszuRAPos.HasValue && orderElementDb.ABPoszuRAPos.Value != 0 && orderElementDb.ABPoszuRAPos.Value != -1)
					{
						var rahmenPosition = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(orderElementDb.ABPoszuRAPos.Value);
						var rahmenExtensionPosition = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.GetByAngeboteneArtikelNr(orderElementDb.ABPoszuRAPos.Value);

						rahmenPosition.Anzahl += orderElementDb.Anzahl;
						rahmenPosition.Geliefert -= orderElementDb.Anzahl;

						rahmenPosition.erledigt_pos = rahmenPosition.Geliefert != rahmenPosition.OriginalAnzahl ? false : true;

						Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Update(rahmenPosition);

						Common.Helpers.CTS.BlanketHelpers.CalculateRahmenGesamtPries(rahmenExtensionPosition?.RahmenNr ?? -1);
						if(orderDb.Neu_Order.HasValue)
						{
							orderElementDb.ABPoszuRAPos = 0;
							Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Update(orderElementDb);
						}
					}
					//delete delfor link if any
					var delforItemsPlans = Infrastructure.Data.Access.Tables.CTS.LineItemPlanAccess.GetByOrder(orderDb.Nr);
					if(delforItemsPlans != null && delforItemsPlans.Count > 0)
					{
						delforItemsPlans.ForEach(x => x.OrderItemId = null);
						Infrastructure.Data.Access.Tables.CTS.LineItemPlanAccess.Update(delforItemsPlans);
					}

					//logging
					var _log = new LogHelper(orderDb.Nr, (int)orderDb.Angebot_Nr, int.TryParse(orderDb.Projekt_Nr, out var val) ? val : 0, orderDb.Typ, LogHelper.LogType.DELETIONPOS, "CTS", user)
						.LogCTS(null, null, $"Article: {orderElementDb.ArtikelNr}| Quantity: {orderElementDb.Anzahl}", (int)orderElementDb.Position, orderElementDb.Nr);
					Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(_log);


					return Core.Models.ResponseModel<object>.SuccessResponse();
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}
	}
}
