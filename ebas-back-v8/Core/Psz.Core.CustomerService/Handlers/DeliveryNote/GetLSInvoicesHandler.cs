using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.DeliveryNote;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.DeliveryNote
{
	public class GetLSInvoicesHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<LSInvoiceResponseModel>>>
	{
		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetLSInvoicesHandler(Identity.Models.UserModel user, int nr)
		{
			this._user = user;
			this._data = nr;
		}

		public ResponseModel<List<LSInvoiceResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// 
				return ResponseModel<List<LSInvoiceResponseModel>>.SuccessResponse(Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetInvoiceByLieferschein(this._data)
					?.Select(x => new LSInvoiceResponseModel(x))?.ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<LSInvoiceResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<LSInvoiceResponseModel>>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(this._data) == null)
				return ResponseModel<List<LSInvoiceResponseModel>>.FailureResponse($"Delivery note not found");

			return ResponseModel<List<LSInvoiceResponseModel>>.SuccessResponse();
		}
	}
}
