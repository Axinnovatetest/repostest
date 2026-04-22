using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Support.Handlers.ApiCalls
{
	public class GetApiCallCountSixMonthPriorHandler: IHandle<Core.Identity.Models.UserModel, ResponseModel<List<Models.ApiCallCountSixMonthModel>>>
	{
		private readonly Core.Identity.Models.UserModel _user;
		private readonly string _data;

		public GetApiCallCountSixMonthPriorHandler(Core.Identity.Models.UserModel user, string data)
		{
			_user = user;
			_data = data;
		}
		public ResponseModel<List<ApiCallCountSixMonthModel>> Handle()
		{
			var validationResponse = Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			try
			{
				var ranges = Helpers.DatesHelper.GetDateRange(DateTime.Now);
				var entities = Infrastructure.Data.Access.Tables.NLogs.ERP_Nlog_ExceptionsAccess.GetApiCallsSixMonthPrior(ranges.firstDate.ToString("yyyy-MM-dd"), ranges.lastDate.ToString("yyyy-MM-dd"), _data);
				return ResponseModel<List<ApiCallCountSixMonthModel>>.SuccessResponse(entities?.Select(x => new ApiCallCountSixMonthModel(x)).ToList());

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<ApiCallCountSixMonthModel>> Validate()
		{
			if(_user == null)
			{
				return ResponseModel<List<ApiCallCountSixMonthModel>>.AccessDeniedResponse();
			}
			return ResponseModel<List<ApiCallCountSixMonthModel>>.SuccessResponse();
		}
	}
}