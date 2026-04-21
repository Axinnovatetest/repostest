namespace Psz.Core.Support.Handlers.ApiCalls
{
	public class GetMostCalledApiHandler: IHandle<UserModel, ResponseModel<MostCalledApiModel>>
	{
		private readonly UserModel _user;
		public GetMostCalledApiHandler(UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<MostCalledApiModel> Handle()
		{
			var validationResponse = Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			try
			{
				var totalApisCalled = Infrastructure.Data.Access.Tables.NLogs.__ERP_Nlog_ApisCallsAccess.GetTotalApisCalled();
				var mostCalledApi = Infrastructure.Data.Access.Tables.NLogs.__ERP_Nlog_ApisCallsAccess.LeastOrLeastCalledApi(true);
				var leastCalledApi = Infrastructure.Data.Access.Tables.NLogs.__ERP_Nlog_ApisCallsAccess.LeastOrLeastCalledApi();

				return ResponseModel<MostCalledApiModel>.SuccessResponse(new MostCalledApiModel { Count = totalApisCalled, MostCalledApi = mostCalledApi, LeastCalledApi = leastCalledApi });

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<MostCalledApiModel> Validate()
		{
			if(_user == null)
			{
				return ResponseModel<MostCalledApiModel>.AccessDeniedResponse();
			}
			return ResponseModel<MostCalledApiModel>.SuccessResponse();
		}
	}
}