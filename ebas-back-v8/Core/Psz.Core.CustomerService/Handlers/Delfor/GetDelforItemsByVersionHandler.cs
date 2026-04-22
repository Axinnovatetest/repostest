using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Delfor;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Delfor
{
	public class GetDelforItemsByVersionHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<DeflorItemsResponseModel>>>
	{

		private DelforItemsRequsetModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetDelforItemsByVersionHandler(Identity.Models.UserModel user, DelforItemsRequsetModel data)
		{
			this._user = user;
			this._data = data;
		}


		public ResponseModel<List<DeflorItemsResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new List<DeflorItemsResponseModel>();
				var header = Infrastructure.Data.Access.Tables.CTS.HeaderAccess.GetByDocumentNumberAndCustomerNumber(_data.DocumentNumber, _data.CustomerNumber, _data.Version);
				var lineItems = Infrastructure.Data.Access.Tables.CTS.LineItemAccess.GetByHeaderId((int)header.Id);
				var firstLineItemPlanWithOpenQty = Infrastructure.Data.Access.Tables.CTS.LineItemPlanAccess.GetFirstWithOpenQtyByLineItems(lineItems?.Select(x => x.Id)?.ToList());

				if(lineItems != null && lineItems.Count > 0)
				{
					foreach(var item in lineItems)
					{
						var fWopenQty = firstLineItemPlanWithOpenQty.FirstOrDefault(x => x.LineItemId == item.Id);
						response.Add(new DeflorItemsResponseModel
						{
							Artikelnummer = item.SuppliersItemMaterialNumber,
							ArticleNr = item.ArticleId,//Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(item.SuppliersItemMaterialNumber)?.ArtikelNr,
							LastReceivedQuantity = item.LastReceivedQuantity,
							PositionNumber = item.PositionNumber,
							RecivedQuantity = item.CumulativeReceivedQuantity,
							ScheduledQuantity = item.CumulativeReceivedQuantity,
							HeaderId = (int)header.Id,
							Id = (int)item.Id,
							Done = header.Done ?? false,
							Customernumber = header.PSZCustomernumber,
							kundenIndex = item.DrawingRevisionNumber,
							LastDeliverydate = item.LastASNDate,
							LastDeliveryNumer = item.LastASNNumber,
							FirstLineItemPlanWithOpenQuantity = fWopenQty?.PlanningQuantityQuantity,
							FirstLineItemPlanWithOpenQuantityRSD = fWopenQty?.PlanningQuantityRequestedShipmentDate,
							ABNumber = Infrastructure.Data.Access.Tables.CTS.LineItemPlanAccess.GetByLineItems(new List<long> { item.Id }).ToList()?
						.Where(x => x.OrderId.HasValue).ToList().Count ?? 0,
						});
					}
				}

				return ResponseModel<List<DeflorItemsResponseModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<DeflorItemsResponseModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<DeflorItemsResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<DeflorItemsResponseModel>>.SuccessResponse();
		}

	}
}
