using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Administration.AccessProfiles
{
	public class GetLagerListHandler: IHandle<UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private UserModel _user { get; set; }
		public GetLagerListHandler(UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<List<KeyValuePair<int, string>>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var bsdLagerEntity = Enum.GetValues(typeof(Identity.Enums.BaseDataEmuns.BaseDataLager)).Cast<Identity.Enums.BaseDataEmuns.BaseDataLager>().ToList();
				if(bsdLagerEntity != null && bsdLagerEntity.Count > 0)
				{
					return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(bsdLagerEntity
							.Select(x => new KeyValuePair<int, string>((int)x, $"{x.GetDescription()}".Trim())).Distinct().ToList());
				}

				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
			} catch(Exception exception)
			{
				Infrastructure.Services.Logging.Logger.Log(exception);
				throw;
			}
		}

		public ResponseModel<List<KeyValuePair<int, string>>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
			}

			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
		}
	}
}
