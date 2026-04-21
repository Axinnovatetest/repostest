using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Settings
{
	public class CheckLPExsistanceHandler: IHandle<Identity.Models.UserModel, ResponseModel<KeyValuePair<bool, List<Models.LPCheckResponseModel>>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public CheckLPExsistanceHandler(Identity.Models.UserModel user, int nr)
		{
			this._user = user;
			this._data = nr;
		}
		public ResponseModel<KeyValuePair<bool, List<Models.LPCheckResponseModel>>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var LPEntity = Infrastructure.Data.Access.Joins.Kunden_LieferantenAccess.GetLP(this._data);
				var LPList = new List<Models.LPCheckResponseModel>();
				if(LPEntity == null || LPEntity.Count == 0)
					return ResponseModel<KeyValuePair<bool, List<Models.LPCheckResponseModel>>>.SuccessResponse(new KeyValuePair<bool, List<Models.LPCheckResponseModel>>(false, null));

				foreach(var item in LPEntity)
				{
					LPList.Add(new Models.LPCheckResponseModel(item));
				}
				return ResponseModel<KeyValuePair<bool, List<Models.LPCheckResponseModel>>>.SuccessResponse(new KeyValuePair<bool, List<Models.LPCheckResponseModel>>(true, LPList));

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<KeyValuePair<bool, List<Models.LPCheckResponseModel>>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<KeyValuePair<bool, List<Models.LPCheckResponseModel>>>.AccessDeniedResponse();
			}

			return ResponseModel<KeyValuePair<bool, List<Models.LPCheckResponseModel>>>.SuccessResponse();
		}
	}
}
