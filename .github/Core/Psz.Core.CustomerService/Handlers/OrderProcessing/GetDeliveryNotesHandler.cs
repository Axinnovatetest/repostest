using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.OrderProcessing;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.OrderProcessing
{
	public class GetDeliveryNotesHandler: IHandle<Identity.Models.UserModel, ResponseModel<DeliveryNoteResponseModel>>
	{
		private DeliveryNoteRequestModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetDeliveryNotesHandler(DeliveryNoteRequestModel data, Identity.Models.UserModel user)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<DeliveryNoteResponseModel> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			try
			{
				var deliveries = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetDeliveryNotesLastByCustomer(this._data.CustomerNumber, this._data.DateFrom ?? DateTime.Today.AddMonths(-1));
				var positons = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(deliveries?.Select(x => x.Nr)?.ToList());
				var articles = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(positons?.Select(x => x.ArtikelNr ?? -1)?.ToList());

				return ResponseModel<DeliveryNoteResponseModel>.SuccessResponse(new DeliveryNoteResponseModel(deliveries, positons, articles));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<DeliveryNoteResponseModel> Validate()
		{
			if(_user == null || (!_user.Access.CustomerService.ModuleActivated && !_user.Access.Purchase.ModuleActivated))
			{
				return ResponseModel<DeliveryNoteResponseModel>.AccessDeniedResponse();
			}

			// -
			return ResponseModel<DeliveryNoteResponseModel>.SuccessResponse();
		}
	}
}
