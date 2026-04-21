using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FA;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.CRP.Handlers.FA
{
	public class GetFAStucklistHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<FAStucklistModel>>>
	{
		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetFAStucklistHandler(int data, Identity.Models.UserModel user)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<FAStucklistModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var FAstucklisEntity = Infrastructure.Data.Access.Tables.PRS.FertigungPositionenAccess.GetByIdFertigung(this._data);
				List<FAStucklistModel> response = new List<FAStucklistModel>();
				if(FAstucklisEntity != null && FAstucklisEntity.Count > 0)
				{
					foreach(var item in FAstucklisEntity)
					{
						response.Add(new FAStucklistModel(item));
					}
				}
				return ResponseModel<List<FAStucklistModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: _data:{_data}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<List<FAStucklistModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<FAStucklistModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<FAStucklistModel>>.SuccessResponse();
		}
	}
}