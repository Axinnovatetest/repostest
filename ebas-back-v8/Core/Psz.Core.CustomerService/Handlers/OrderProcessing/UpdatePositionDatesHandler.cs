using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.OrderProcessing;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.OrderProcessing
{
	public class UpdatePositionDatesHandler: IHandle<Identity.Models.UserModel, ResponseModel<KeyValuePair<DateTime?, DateTime?>>>
	{

		private PositionsDateEntryModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public Infrastructure.Services.Utils.TransactionsManager _botransaction { get; set; }
		public UpdatePositionDatesHandler(Identity.Models.UserModel user, PositionsDateEntryModel data)
		{
			this._user = user;
			this._data = data;
		}


		public ResponseModel<KeyValuePair<DateTime?, DateTime?>> Handle()
		{
			return this.Perform();
		}
		public ResponseModel<KeyValuePair<DateTime?, DateTime?>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<KeyValuePair<DateTime?, DateTime?>>.AccessDeniedResponse();
			}
			var positionEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetWithTransaction(_data.Id, _botransaction.connection, _botransaction.transaction);
			if(positionEntity is null)
				return ResponseModel<KeyValuePair<DateTime?, DateTime?>>.FailureResponse("Position not found");

			var orderData = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetWithTransaction(positionEntity.AngebotNr ?? -1, _botransaction.connection, _botransaction.transaction);
			if(orderData?.Erledigt == true)
			{
				return ResponseModel<KeyValuePair<DateTime?, DateTime?>>.FailureResponse("Order is done, change not possible");
			}
			if(orderData?.Gebucht == true)
			{
				return ResponseModel<KeyValuePair<DateTime?, DateTime?>>.FailureResponse("Order is booked, change not possible");
			}
			//  - 2023-10-31 - Schremmer Desired Date not required for BV
			if(orderData != null && orderData.Typ?.ToLower()?.Trim() != Enums.OrderEnums.Types.forecast.GetDescription()?.ToLower()?.Trim() && _data.Wunshtermin.HasValue && _data.Wunshtermin.Value <= new DateTime(1753, 1, 1))
				return ResponseModel<KeyValuePair<DateTime?, DateTime?>>.FailureResponse("Wunshtermin: value cannot be prior to 1/1/1753");
			if(_data.Liefertermin.HasValue && _data.Liefertermin.Value <= new DateTime(1753, 1, 1))
				return ResponseModel<KeyValuePair<DateTime?, DateTime?>>.FailureResponse("Liefertermin: value cannot be prior to 1/1/1753");
			//if (positionEntity != null && positionEntity.ABPoszuRAPos.HasValue && positionEntity.ABPoszuRAPos.Value != 0 && positionEntity.ABPoszuRAPos.Value != -1)
			//{
			//    var RAPositionExtension = Infrastructure.Data.Access.Tables.CTS.AngeboteArticleBlanketExtensionAccess.GetByAngeboteneArtikelNr(positionEntity.ABPoszuRAPos ?? -1, _botransaction.connection, _botransaction.transaction));
			//    if (RAPositionExtension != null && RAPositionExtension.GultigAb.HasValue && RAPositionExtension.GultigBis.HasValue)
			//    {
			//        if (_data.Liefertermin.HasValue && (_data.Liefertermin < RAPositionExtension.GultigAb || _data.Liefertermin > RAPositionExtension.GultigBis))
			//            return ResponseModel<KeyValuePair<DateTime?, DateTime?>>.FailureResponse($"Delivery date should be in Rahmen dates range between [{RAPositionExtension.GultigAb}] and [{RAPositionExtension.GultigBis}].");
			//    }
			//}
			//horizon check
			var horizonCheck = false;
			var horizonErrors = new List<string>();
			//var technicArticles = Module.BSD.TechnicArticleIds;
			var orderArticleIsTechnic = Helpers.HorizonsHelper.ArticleIsTechnic(positionEntity.ArtikelNr ?? -1);
			if(positionEntity.Liefertermin != _data.Liefertermin)
			{
				var _newDate = _data.Liefertermin ?? _data.Wunshtermin ?? new DateTime(1900, 1, 1);
				var _oldDate = positionEntity.Liefertermin ?? positionEntity.Wunschtermin ?? _newDate;
				if(orderData?.Typ?.Trim()?.ToLower() == Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_CONFIRMATION.Trim().ToLower())
				{
					horizonCheck = Helpers.HorizonsHelper.userHasABPosHorizonRight(_newDate, _oldDate, _user, out List<string> messages);
					if(!horizonCheck && !orderArticleIsTechnic)
						horizonErrors.AddRange(messages);
				}
				if(orderData?.Typ?.Trim()?.ToLower() == Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_DELIVERY.Trim().ToLower())
				{
					horizonCheck = Helpers.HorizonsHelper.userHasLSPosHorizonRight(_newDate, _oldDate, _user, out List<string> messages);
					if(!horizonCheck && !orderArticleIsTechnic)
						horizonErrors.AddRange(messages);
				}
				if(orderData?.Typ?.Trim()?.ToLower() == Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_INVOICE.Trim().ToLower())
				{
					horizonCheck = Helpers.HorizonsHelper.userHasRGPosHorizonRight(_newDate, _oldDate, _user, out List<string> messages);
					if(!horizonCheck && !orderArticleIsTechnic)
						horizonErrors.AddRange(messages);
				}
				if(orderData?.Typ?.Trim()?.ToLower() == Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_FORECAST.Trim().ToLower())
				{
					horizonCheck = Helpers.HorizonsHelper.userHasFRCPosHorizonRight(_newDate, _oldDate, _user, out List<string> messages);
					if(!horizonCheck && !orderArticleIsTechnic)
						horizonErrors.AddRange(messages);
				}
			}
			if(horizonErrors != null && horizonErrors.Count > 0)
				return ResponseModel<KeyValuePair<DateTime?, DateTime?>>.FailureResponse(horizonErrors);


			// - 2023-05-10 - Heidenreich - allow only ONCE to toggle Book for RG
			if(orderData?.Typ?.Trim()?.ToLower() == Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_INVOICE.Trim().ToLower())
			{
				if(orderData?.Datum < DateTime.Today || orderData.Gebucht == true)
				{
					return ResponseModel<KeyValuePair<DateTime?, DateTime?>>.FailureResponse("Invoice edit is not allowed");
				}
				//	if(positionEntity.Liefertermin != _data.Liefertermin)
				//	{

				//		var horizonCheck = Helpers.HorizonsHelper.checkHorizonRights(_data.Liefertermin ?? new DateTime(1900, 1, 1), Enums.FAEnums.horizonAction.rg_pos,
				//			  _user, out List<string> messages);
				//		if(!horizonCheck)
				//			return ResponseModel<KeyValuePair<DateTime?, DateTime?>>.FailureResponse(messages);
				//	}
				//}
				//if(orderData?.Typ?.Trim()?.ToLower() == Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_FORECAST.Trim().ToLower())
				//{
				//	if(positionEntity.Liefertermin != _data.Liefertermin)
				//	{
				//		var horizonCheck = Helpers.HorizonsHelper.checkHorizonRights(_data.Liefertermin ?? new DateTime(1900, 1, 1), Enums.FAEnums.horizonAction.frc_pos,
				//			  _user, out List<string> messages);
				//		if(!horizonCheck)
				//			return ResponseModel<KeyValuePair<DateTime?, DateTime?>>.FailureResponse(messages);
				//	}
			}

			return ResponseModel<KeyValuePair<DateTime?, DateTime?>>.SuccessResponse();
		}

		public ResponseModel<KeyValuePair<DateTime?, DateTime?>> Perform(bool sharedTransaction = false)
		{
			try
			{
				if(sharedTransaction == false)
				{
					_botransaction = new Infrastructure.Services.Utils.TransactionsManager();
					_botransaction.beginTransaction();
				}

				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var positionEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetWithTransaction(_data.Id, _botransaction.connection, _botransaction.transaction);
				var positionEntityAfterUpdate = new Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity();
				var _oldDeliveryDate = positionEntity?.Liefertermin;
				var warnings = new List<string>();
				var _log = new List<Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity>();

				if(_data.Id != -1)
				{
					var Order = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetWithTransaction((int)positionEntity.AngebotNr, _botransaction.connection, _botransaction.transaction);
					if(positionEntity.Liefertermin != _data.Liefertermin)
					{
						_log.Add(new Psz.Core.CustomerService.Helpers.LogHelper(Order.Nr, (int)Order.Angebot_Nr, int.TryParse(Order.Projekt_Nr, out var val) ? val : 0, Order.Typ,
							  Psz.Core.CustomerService.Helpers.LogHelper.LogType.MODIFICATIONPOS, "CTS", _user)
							.LogCTS("Liefertermin", $"{positionEntity.Liefertermin?.ToString("dd.MM.yyyy")}", $"{_data.Liefertermin?.ToString("dd.MM.yyyy")}", (int)positionEntity.Position, positionEntity.Nr));
					}
					if(positionEntity.Wunschtermin != _data.Wunshtermin)
					{
						_log.Add(new Psz.Core.CustomerService.Helpers.LogHelper(Order.Nr, (int)Order.Angebot_Nr, int.TryParse(Order.Projekt_Nr, out var val) ? val : 0, Order.Typ,
							  Psz.Core.CustomerService.Helpers.LogHelper.LogType.MODIFICATIONPOS, "CTS", _user)
							.LogCTS("Wunschtermin", $"{positionEntity.Wunschtermin?.ToString("dd.MM.yyyy")}", $"{_data.Wunshtermin?.ToString("dd.MM.yyyy")}", (int)positionEntity.Position, positionEntity.Nr));
					}


					positionEntity.Wunschtermin = _data.Wunshtermin;
					positionEntity.Liefertermin = _data.Liefertermin;
					Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.UpdateWithTransaction(positionEntity, _botransaction.connection, _botransaction.transaction);
					positionEntityAfterUpdate = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetWithTransaction(_data.Id, _botransaction.connection, _botransaction.transaction);
				}
				if(_log.Count > 0)
				{
					Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.InsertWithTransaction(_log, _botransaction.connection, _botransaction.transaction);
				}


				if(sharedTransaction == true)
				{
					return ResponseModel<KeyValuePair<DateTime?, DateTime?>>.SuccessResponse(new KeyValuePair<DateTime?, DateTime?>(positionEntityAfterUpdate?.Liefertermin, positionEntityAfterUpdate?.Wunschtermin), warnings);
				}
				else
				{
					if(_botransaction.commit())
					{
						return ResponseModel<KeyValuePair<DateTime?, DateTime?>>.SuccessResponse(new KeyValuePair<DateTime?, DateTime?>(positionEntityAfterUpdate?.Liefertermin, positionEntityAfterUpdate?.Wunschtermin), warnings);
					}
					else
					{
						return ResponseModel<KeyValuePair<DateTime?, DateTime?>>.FailureResponse("Transaction error.");
					}
				}
			} catch(Exception e)
			{
				_botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}
