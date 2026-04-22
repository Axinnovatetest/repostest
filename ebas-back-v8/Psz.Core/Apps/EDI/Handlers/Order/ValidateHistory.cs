using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.EDI.Handlers
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;

	public class ValidateHistoryHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Order.Element.OrderElementModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }


		public ValidateHistoryHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
		}

		public ResponseModel<List<Models.Order.Element.OrderElementModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// 
				var orderEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(this._data);
				var messageEntity = Infrastructure.Data.Access.Tables.PRS.XML_MessageAccess.GetByOrderId(this._data);
				if(messageEntity == null)
					return ResponseModel<List<Models.Order.Element.OrderElementModel>>.FailureResponse("XML file not found");

				List<Models.Order.Element.OrderElementModel> responseBody = null;
				var angeboteneEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(this._data);
				if(angeboteneEntities != null && angeboteneEntities.Count > 0)
				{
					responseBody = new List<Models.Order.Element.OrderElementModel> { };
					foreach(var item in angeboteneEntities)
					{
						responseBody.Add(Order.GetElements(new List<int>() { item.Nr }).FirstOrDefault());
						responseBody.Add(getElementHistory(item.Nr, messageEntity));
					}
				}

				return ResponseModel<List<Models.Order.Element.OrderElementModel>>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Order.Element.OrderElementModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Order.Element.OrderElementModel>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(this._data) == null)
				return ResponseModel<List<Models.Order.Element.OrderElementModel>>.FailureResponse("Order not found");

			return ResponseModel<List<Models.Order.Element.OrderElementModel>>.SuccessResponse();
		}

		Models.Order.Element.OrderElementModel getElementHistory(int id, Infrastructure.Data.Entities.Tables.PRS.XML_MessageEntity messageEntity)
		{
			var orderElementEntity = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.Get(id);
			if(orderElementEntity.Position.HasValue == false)
				return null;

			var ordersLineItemEntities = Infrastructure.Data.Access.Tables.PRS.XML_OrdersLineItemAccess.GetByFileIdAndPositionNr(messageEntity.IdFile, orderElementEntity.Position.Value);
			var ordersScheduleLineEntities = Infrastructure.Data.Access.Tables.PRS.XML_OrdersScheduleLineAccess.GetByOrderElementId(orderElementEntity.Nr);
			var deliveryLineItemEntities = Infrastructure.Data.Access.Tables.PRS.XML_DeliveryLineItemAccess.GetByFileIdAndPositionNr(messageEntity.IdFile, orderElementEntity.Position.Value);

			return
				new Models.Order.Element.OrderElementModel(orderElementEntity, messageEntity, ordersLineItemEntities?[0], ordersScheduleLineEntities, deliveryLineItemEntities?[0]);


		}
	}
}
