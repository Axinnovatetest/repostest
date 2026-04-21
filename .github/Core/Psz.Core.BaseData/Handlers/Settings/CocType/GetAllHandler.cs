using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.CocType
{
	public class GetAllHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Settings.CoCType.CoCTypeResponseModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetAllHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<Models.Settings.CoCType.CoCTypeResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// -
				return ResponseModel<List<Models.Settings.CoCType.CoCTypeResponseModel>>.SuccessResponse(
					Infrastructure.Data.Access.Tables.BSD.CocTypeAccess.Get()?.Select(x => new Models.Settings.CoCType.CoCTypeResponseModel(x))?.ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.Settings.CoCType.CoCTypeResponseModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Settings.CoCType.CoCTypeResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Settings.CoCType.CoCTypeResponseModel>>.SuccessResponse();
		}
	}
}
