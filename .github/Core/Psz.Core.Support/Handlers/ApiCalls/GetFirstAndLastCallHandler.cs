using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Support.Handlers.ApiCalls
{
	public class GetFirstAndLastCallHandler: IHandle<Core.Identity.Models.UserModel, ResponseModel<KeyValuePair<DateTime, DateTime>>>
	{
		private readonly UserModel _user;
		private readonly string _data;

		public GetFirstAndLastCallHandler(Core.Identity.Models.UserModel user, string data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<KeyValuePair<DateTime, DateTime>> Handle()
		{
			var validationResponse = Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			try
			{
				var dates = Infrastructure.Data.Access.Tables.NLogs.ERP_Nlog_ExceptionsAccess.GetFristAndLastCall(_data);
				return ResponseModel<KeyValuePair<DateTime, DateTime>>.SuccessResponse(new KeyValuePair<DateTime, DateTime>(dates.firstDate ?? DateTime.MinValue, dates.lastDate ?? DateTime.MinValue));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<KeyValuePair<DateTime, DateTime>> Validate()
		{
			if(_user == null)
			{
				return ResponseModel<KeyValuePair<DateTime, DateTime>>.AccessDeniedResponse();
			}
			return ResponseModel<KeyValuePair<DateTime, DateTime>>.SuccessResponse();
		}
	}
}