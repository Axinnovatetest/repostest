using System;
using System.Collections.Generic;

namespace Psz.Core.Apps.EDI.Handlers.Delfor
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Linq;
	public class GetForecastLineItemPlanDeliveryNotesHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Delfor.LineItemPlanDeliveryNoteModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetForecastLineItemPlanDeliveryNotesHandler(Identity.Models.UserModel user, int headerId)
		{
			this._user = user;
			this._data = headerId;
		}

		public ResponseModel<List<Models.Delfor.LineItemPlanDeliveryNoteModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				/// 
				var response = new List<Models.Delfor.LineItemPlanDeliveryNoteModel>();
				var lineItem = Infrastructure.Data.Access.Tables.CTS.LineItemAccess.Get(this._data);
				var header = Infrastructure.Data.Access.Tables.CTS.HeaderAccess.Get(lineItem.HeaderId);
				var minDate = DateTime.Today.AddDays(-7 * 2);
				var deliveryNotesEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetDeliveryNotesLastByCustomer(header.PSZCustomernumber ?? -1,
					minDate); // - prior 2 weeks
				var deliveryNotePosEntitites = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(deliveryNotesEntities?.Select(x => x.Nr)?.ToList());
				var articles = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(deliveryNotePosEntitites?.Select(x => x.ArtikelNr ?? -1)?.ToList());
				foreach(var item in deliveryNotesEntities)
				{
					var itemPositions = deliveryNotePosEntitites.Where(x => x.AngebotNr == item.Nr);
					foreach(var itemPos in itemPositions)
					{
						var article = articles?.FirstOrDefault(x => x.ArtikelNr == itemPos.ArtikelNr);
						response.Add(new Models.Delfor.LineItemPlanDeliveryNoteModel(this._data, 0, article?.ArtikelNummer, minDate, item, itemPos));
					}
				}

				return ResponseModel<List<Models.Delfor.LineItemPlanDeliveryNoteModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Delfor.LineItemPlanDeliveryNoteModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Delfor.LineItemPlanDeliveryNoteModel>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.CTS.LineItemAccess.Get(this._data) == null)
			{
				return ResponseModel<List<Models.Delfor.LineItemPlanDeliveryNoteModel>>.FailureResponse("Forecast not found");
			}

			return ResponseModel<List<Models.Delfor.LineItemPlanDeliveryNoteModel>>.SuccessResponse();
		}
	}
}