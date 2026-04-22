using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Logging
{
	public class FALoggingHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<CTSLoggingModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public FALoggingHandler(Identity.Models.UserModel user)
		{
			this._user = user;
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
					Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.GetByObject("Fertigung")
					?.OrderByDescending(y => y.DateTime)
					?.Select(x => new CTSLoggingModel(x))
					?.ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
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
