using Psz.Core.Common.Models;
using Psz.Core.CustomerService.CsStatistics.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.CsStatistics.Handlers
{
	public class GetKundenListHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KundenModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public GetKundenListHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<KundenModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new List<KundenModel>();

				var kundenListEntity = Infrastructure.Data.Access.Tables.CTS.PSZ_Nummerschlüssel_KundeAccess.Get();
				if(kundenListEntity != null && kundenListEntity.Count > 0)
					response = kundenListEntity.Select(k => new KundenModel(k)).ToList();

				return ResponseModel<List<KundenModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<KundenModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<KundenModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<KundenModel>>.SuccessResponse();
		}
	}
}
