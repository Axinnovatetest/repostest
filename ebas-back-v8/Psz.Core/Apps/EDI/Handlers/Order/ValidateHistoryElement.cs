using System;

namespace Psz.Core.Apps.EDI.Handlers
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class ValidateHistoryElementHandler: IHandle<Identity.Models.UserModel, ResponseModel<Models.Order.Element.OrderElementModel>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }


		public ValidateHistoryElementHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}

		public ResponseModel<Models.Order.Element.OrderElementModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// 
				var orderElementEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(this._data);
				if(orderElementEntity.AngebotNr.HasValue == false)
					return ResponseModel<Models.Order.Element.OrderElementModel>.FailureResponse("Angeboten Nr invalid");

				var messageEntity = Infrastructure.Data.Access.Tables.PRS.XML_MessageAccess.GetByOrderId(orderElementEntity.AngebotNr.Value);
				if(messageEntity == null)
					return ResponseModel<Models.Order.Element.OrderElementModel>.FailureResponse("XML file not found");

				if(orderElementEntity.Position.HasValue == false)
					return ResponseModel<Models.Order.Element.OrderElementModel>.FailureResponse($"Position Nr invalid");

				var ordersLineItemEntities = Infrastructure.Data.Access.Tables.PRS.XML_OrdersLineItemAccess.GetByFileIdAndPositionNr(messageEntity.IdFile, orderElementEntity.Position.Value);
				var ordersScheduleLineEntities = Infrastructure.Data.Access.Tables.PRS.XML_OrdersScheduleLineAccess.GetByOrderElementId(orderElementEntity.Nr);
				var deliveryLineItemEntities = Infrastructure.Data.Access.Tables.PRS.XML_DeliveryLineItemAccess.GetByFileIdAndPositionNr(messageEntity.IdFile, orderElementEntity.Position.Value);

				return ResponseModel<Models.Order.Element.OrderElementModel>.SuccessResponse(
					new Models.Order.Element.OrderElementModel(orderElementEntity, messageEntity, ordersLineItemEntities?[0], ordersScheduleLineEntities, deliveryLineItemEntities?[0]));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<Models.Order.Element.OrderElementModel> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<Models.Order.Element.OrderElementModel>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(this._data) == null)
			{
				return ResponseModel<Models.Order.Element.OrderElementModel>.FailureResponse("1", "Position not found");
			}

			return ResponseModel<Models.Order.Element.OrderElementModel>.SuccessResponse();
		}
	}
}
