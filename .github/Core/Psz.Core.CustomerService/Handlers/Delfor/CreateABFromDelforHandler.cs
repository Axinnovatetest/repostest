using PdfSharp.Charting;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Delfor;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Delfor
{
	public class CreateABFromDelforHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{

		private CreateABFromDelforModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public const string MANUAL_DOCUMENT_PREFIX = "AB-";
		public CreateABFromDelforHandler(Identity.Models.UserModel user, CreateABFromDelforModel data)
		{
			this._user = user;
			this._data = data;
		}


		public ResponseModel<int> Handle()
		{


			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}
			var transactionManager = new Infrastructure.Services.Utils.TransactionsManager();

			try
			{
				var lineItemPlan = Infrastructure.Data.Access.Tables.CTS.LineItemPlanAccess.Get(_data.LineItemPlan);
				var itemPlan = Infrastructure.Data.Access.Tables.CTS.LineItemAccess.Get(lineItemPlan.LineItemId);
				var headerPlan = Infrastructure.Data.Access.Tables.CTS.HeaderAccess.Get(itemPlan.HeaderId);

				var customer = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer(_data.CustomerNumber);

				//AB creation
				var createABModel = new Models.OrderProcessing.CreateOrderModel
				{
					CustomerId = customer.Nr,
				};

				transactionManager.beginTransaction();
				var creationAB = new OrderProcessing.CreateOrderHandler(createABModel, _user).Perform(transactionManager, true);
				if(!creationAB.Success)
				{
					transactionManager.rollback();
					return ResponseModel<int>.FailureResponse(creationAB.Errors.Select(x => x.Value).ToList());
				}

				var ABid = creationAB.Body;
				var AB = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetWithTransaction(ABid, transactionManager.connection, transactionManager.transaction);
				AB.nr_dlf = (int)lineItemPlan.Id;
				AB.Bezug = headerPlan.DocumentNumber;

				// - 2023-04-12 - update Delivery Address if different form DLF's
				// if(headerPlan?.ManualCreation != true) // - 2023-04-25 - apply this only if not Manual DLF - Heidenreich
				{
					var laddress = $"{headerPlan?.ConsigneePostCode} {headerPlan?.ConsigneeCity}, {headerPlan?.ConsigneeCountryName}".Trim();
					if(!string.IsNullOrWhiteSpace(headerPlan?.ConsigneePartyName) || !string.IsNullOrWhiteSpace(headerPlan?.ConsigneeStreet) ||
						!string.IsNullOrWhiteSpace(laddress))
					{
						var sameAddress = true;
						if(AB.LVorname_NameFirma?.ToLower()?.Trim() != headerPlan?.ConsigneePartyName?.ToLower()?.Trim())
						{
							AB.LVorname_NameFirma = headerPlan?.ConsigneePartyName;
							sameAddress = false;
						}
						if(AB.LStraße_Postfach?.ToLower()?.Trim() != headerPlan?.ConsigneeStreet?.ToLower()?.Trim())
						{
							AB.LStraße_Postfach = headerPlan?.ConsigneeStreet;
							sameAddress = false;
						}
						if(AB.LLand_PLZ_Ort?.ToLower()?.Trim() != laddress.ToLower())
						{
							AB.LLand_PLZ_Ort = laddress.Trim(new char[] { ',', ' ' });
							sameAddress = false;
						}

						// - no data coming from DLF
						if(!sameAddress)
						{
							AB.LAnrede = ""; // - lieferadressDb?.Anrede;
							AB.LName2 = ""; // - lieferadressDb?.Name2;
							AB.LName3 = ""; // - lieferadressDb?.Name3;
							AB.LAnsprechpartner = ""; // - lieferadressDb?.Abteilung;
							AB.LAbteilung = ""; // - lieferadressDb?.Abteilung;
							AB.LBriefanrede = ""; // - lieferadressDb?.Briefanrede;
							AB.UnloadingPoint = headerPlan.ConsigneeUnloadingPoint;
							AB.StorageLocation = headerPlan.ConsigneeStorageLocation;
						}
					}
				}

				// - 2023-08-17 - Groetsh - add Contact Person
				AB.LAnsprechpartner = headerPlan.BuyerContactName ?? AB.LAnsprechpartner;

				Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.UpdateDLF(AB, transactionManager.connection, transactionManager.transaction);

				//AB position Creation
				var createPositionModel = new Models.OrderProcessing.CreateOrderItemModel
				{
					CSInterneBemerkung = null,
					Headline = null,
					ItemNumber = itemPlan.SuppliersItemMaterialNumber,
					ItemTypeId = 0,
					OrderId = ABid,
					Position = 10,
					Quantity = _data.Quantity//lineItemPlan.PlanningQuantityQuantity
				};
				var creationPositionHandler = new OrderProcessing.CreateOrderItemHandler(createPositionModel, _user);
				creationPositionHandler._botransaction = transactionManager;
				var creationPosition = creationPositionHandler.Perform(true);
				if(!creationPosition.Success)
				{
					transactionManager.rollback();
					return ResponseModel<int>.FailureResponse(creationPosition.Errors.Select(x => x.Value).ToList());
				}



				//update position dates
				var updateDatesModel = new Models.OrderProcessing.PositionsDateEntryModel
				{
					Id = creationPosition.Body,
					Wunshtermin = lineItemPlan.PlanningQuantityRequestedShipmentDate
				};
				// - 2023-11-03 - Reil - accept position in the past
				if(lineItemPlan.PlanningQuantityRequestedShipmentDate >= DateTime.Today)
				{
					updateDatesModel.Liefertermin = lineItemPlan.PlanningQuantityRequestedShipmentDate;
				}
				var datesUpdateHandler = new OrderProcessing.UpdatePositionDatesHandler(_user, updateDatesModel);
				datesUpdateHandler._botransaction = transactionManager;
				var datesUpdate = datesUpdateHandler.Perform(true);
				if(!datesUpdate.Success)
				{
					transactionManager.rollback();
					return ResponseModel<int>.FailureResponse(datesUpdate.Errors.Select(x => x.Value).ToList());
				}

				//update delfor position order details
				//lineItemPlan.OrderId = ABid;
				//lineItemPlan.OrderUserId = _user.Id;
				//lineItemPlan.OrderItemId = creationPosition.Body;
				//lineItemPlan.OrderDate = DateTime.Now;
				//Infrastructure.Data.Access.Tables.CTS.LineItemPlanAccess.Update(lineItemPlan);
				//Infrastructure.Data.Access.Tables.CTS.LineItemPlan_OrdersAccess.Insert(new Infrastructure.Data.Entities.Tables.CTS.LineItemPlan_OrdersEntity
				//{
				//	CreationTime = DateTime.Now,
				//	CreationUserId = _user.Id,
				//	LineItemPlanId = lineItemPlan.Id,
				//	OrderId = ABid,
				//	Quantity = createPositionModel.Quantity
				//});

				if(transactionManager.commit())
				{
					return ResponseModel<int>.SuccessResponse(ABid);
				}
				else
				{
					return ResponseModel<int>.FailureResponse("Transaction error.");
				}


			} catch(Exception e)
			{
				transactionManager.rollback();
				Psz.Core.Common.Helpers.blockHelper.Block_UnblockCreation(Common.Helpers.blockHelper.BlockObject.AB, false);
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
			var lineItemPlan = Infrastructure.Data.Access.Tables.CTS.LineItemPlanAccess.Get(_data.LineItemPlan);
			var itemPlan = Infrastructure.Data.Access.Tables.CTS.LineItemAccess.Get(lineItemPlan.LineItemId);
			if(lineItemPlan == null)
				return ResponseModel<int>.FailureResponse($"item plan [{_data.LineItemPlan}] position not found.");
			var customer = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer(_data.CustomerNumber);
			if(customer == null)
				return ResponseModel<int>.FailureResponse($"customer [{_data.CustomerNumber}] not found.");

			var lineItemPlansOrders = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetByLineItemPlanIds(new List<long> { this._data.LineItemPlan });
			var lineItemPlansOrdersQty = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetTotalQuantityByAB(lineItemPlansOrders?.Select(x => x.Nr)?.ToList());
			var absTotalQty = lineItemPlansOrdersQty?.Sum(x => x.Value) ?? 0;
			if(lineItemPlan.PlanningQuantityQuantity - absTotalQty < _data.Quantity)
			{
				return ResponseModel<int>.FailureResponse($"AB Quantity [{_data.Quantity}] bigger than Line Item Rest Quantity [{((lineItemPlan.PlanningQuantityQuantity ?? 0) - absTotalQty).ToString("##.##")}].");
			}
			DateTime _newDate, _oldDate;
			_newDate = _oldDate = lineItemPlan.PlanningQuantityRequestedShipmentDate ?? new DateTime(1900, 1, 1);

			// - 2023-11-03 - Reil - accept position in the past
			if(_newDate < DateTime.Today)
			{
				_newDate = DateTime.Today;
			}
			// var technicArticles = Module.BSD.TechnicArticleIds;
			var itemPlanArticle = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(itemPlan.SuppliersItemMaterialNumber);
			var horrizonCheck = Psz.Core.CustomerService.Helpers.HorizonsHelper.userHasABPosHorizonRight(_newDate, _oldDate, _user, out List<string> messages);
			if(!horrizonCheck && !Helpers.HorizonsHelper.ArticleIsTechnic(itemPlanArticle.ArtikelNr))
				return ResponseModel<int>.FailureResponse(messages);


			return ResponseModel<int>.SuccessResponse();
		}
	}
}
