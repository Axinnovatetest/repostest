using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Logging
{
	public class FADetailsLoggingHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<CTSLoggingModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }
		public FADetailsLoggingHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<List<CTSLoggingModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				return ResponseModel<List<CTSLoggingModel>>.SuccessResponse(
					 Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.GetByNr(this._data)
					 ?.OrderByDescending(y => y.DateTime)
					 ?.Select(x => new CTSLoggingModel(x))
					 ?.ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: _data:{_data}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<List<CTSLoggingModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<CTSLoggingModel>>.AccessDeniedResponse();
			}
			return ResponseModel<List<CTSLoggingModel>>.SuccessResponse();
		}
	}
}
