using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.ObjectLog
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class GetHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.ObjectLog.ObjectLogModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public GetHandler(Identity.Models.UserModel user, int artikelNr)
		{
			this._user = user;
			this._data = artikelNr;
		}

		public ResponseModel<List<Models.ObjectLog.ObjectLogModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				List<Models.ObjectLog.ObjectLogModel> results = null;
				var logEntities = Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.GetByObjetAndId(Enums.ObjectLogEnums.Objects.Article.GetDescription(), this._data, 5);

				if(logEntities != null && logEntities.Count > 0)
				{
					results = new List<Models.ObjectLog.ObjectLogModel>();
					foreach(var logEntity in logEntities)
					{
						results.Add(new Models.ObjectLog.ObjectLogModel(logEntity));
					}
				}

				return ResponseModel<List<Models.ObjectLog.ObjectLogModel>>.SuccessResponse(results);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<Models.ObjectLog.ObjectLogModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.ObjectLog.ObjectLogModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.ObjectLog.ObjectLogModel>>.SuccessResponse();
		}
	}

}
