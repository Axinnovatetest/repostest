using Psz.Core.Common.Models;
using Psz.Core.CustomerService.CsStatistics.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.CsStatistics.Handlers
{
	public class GetE_rechnungHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<E_RechnungModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public GetE_rechnungHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<E_RechnungModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new List<E_RechnungModel>();

				var ERechnungEntity = Infrastructure.Data.Access.Tables.CTS.Tbl_E_Rechnung_KundendefinitionenAccess.Get();
				if(ERechnungEntity != null && ERechnungEntity.Count > 0)
					response = ERechnungEntity.Select(e => new E_RechnungModel(e)).ToList();

				return ResponseModel<List<E_RechnungModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<E_RechnungModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<E_RechnungModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<E_RechnungModel>>.SuccessResponse();
		}
	}
}
