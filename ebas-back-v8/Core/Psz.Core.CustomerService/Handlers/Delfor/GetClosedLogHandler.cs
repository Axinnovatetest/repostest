using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.Delfor
{
	public class GetClosedLogHandler: IHandle<Identity.Models.UserModel, ResponseModel<IEnumerable<CTSLoggingModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public GetClosedLogHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<IEnumerable<CTSLoggingModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// -
				return ResponseModel<IEnumerable<CTSLoggingModel>>.SuccessResponse(
					Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.GetDLFArchives()
					?.Select(x => new CTSLoggingModel(x)));

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<IEnumerable<CTSLoggingModel>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<IEnumerable<CTSLoggingModel>>.AccessDeniedResponse();
			}

			return ResponseModel<IEnumerable<CTSLoggingModel>>.SuccessResponse();
		}

	}
}
