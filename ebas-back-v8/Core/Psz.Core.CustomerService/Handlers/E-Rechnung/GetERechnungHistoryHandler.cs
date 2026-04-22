using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.E_Rechnung;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.E_Rechnung
{
	public class GetERechnungHistoryHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<ERechnungReprintModel>>>
	{

		private Identity.Models.UserModel _user { get; set; }
		public GetERechnungHistoryHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}


		public ResponseModel<List<ERechnungReprintModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var entities = Infrastructure.Data.Access.Joins.CTS.Divers.GetErechnungHistory();
				var response = entities?.Select(x => new ERechnungReprintModel(x)).ToList();

				return ResponseModel<List<ERechnungReprintModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<ERechnungReprintModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<ERechnungReprintModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<ERechnungReprintModel>>.SuccessResponse();
		}

	}
}
