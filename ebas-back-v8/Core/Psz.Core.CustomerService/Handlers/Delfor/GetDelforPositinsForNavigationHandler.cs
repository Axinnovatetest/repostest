using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.Delfor;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Delfor
{
	public class GetDelforPositinsForNavigationHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<DeflorItemsResponseModel>>>
	{

		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetDelforPositinsForNavigationHandler(Identity.Models.UserModel user, int data)
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

				var actualPosition = Infrastructure.Data.Access.Tables.CTS.LineItemAccess.Get(_data);
				var header = Infrastructure.Data.Access.Tables.CTS.HeaderAccess.Get(actualPosition.HeaderId);
				var allPositions = Infrastructure.Data.Access.Tables.CTS.LineItemAccess.GetByHeaderId((int)actualPosition.HeaderId);

				if(allPositions != null && allPositions.Count > 0)
				{
					response = allPositions.Where(x => x.Id != _data).Select(item => new DeflorItemsResponseModel
					{
						Artikelnummer = item.SuppliersItemMaterialNumber,
						ArticleNr = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(item.SuppliersItemMaterialNumber)?.ArtikelNr,
						LastReceivedQuantity = item.LastReceivedQuantity,
						PositionNumber = item.PositionNumber,
						RecivedQuantity = item.CumulativeReceivedQuantity,
						ScheduledQuantity = item.CumulativeReceivedQuantity,
						HeaderId = (int)header.Id,
						Id = (int)item.Id,
						Customernumber = header.PSZCustomernumber,
						kundenIndex = item.DrawingRevisionNumber,
						LastDeliverydate = item.LastASNDate,
						LastDeliveryNumer = item.LastASNNumber,
						ABNumber = Infrastructure.Data.Access.Tables.CTS.LineItemPlanAccess.GetByLineItems(new List<long> { item.Id }).ToList()?
						.Where(x => x.OrderId.HasValue).ToList().Count ?? 0,
					}).ToList();
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
