using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Principal;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Principal
{
	public class GetListLagerBestand: IHandle<Identity.Models.UserModel, ResponseModel<List<LagerBestandModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public GetListLagerBestand(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<LagerBestandModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new List<LagerBestandModel>();

				var BestandListEntity = Infrastructure.Data.Access.Joins.Logistics.LagerBestandAccess.GetListeBestand();
				if(BestandListEntity != null && BestandListEntity.Count > 0)
					response = BestandListEntity.Select(k => new LagerBestandModel(k)).ToList();

				return ResponseModel<List<LagerBestandModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<LagerBestandModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<List<LagerBestandModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<LagerBestandModel>>.SuccessResponse();
		}
	}
}