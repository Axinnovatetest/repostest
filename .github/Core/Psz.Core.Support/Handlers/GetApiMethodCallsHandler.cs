using Psz.Core.Support.Helpers;

namespace Psz.Core.Support.Handlers;

public  class GetApiMethodCallsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<ApiMethodCallsModel>>>
{
	private Identity.Models.UserModel _user { get; set; }
	private GetApiMethodCallsRequestModel _data { get; set; }

	public GetApiMethodCallsHandler(Identity.Models.UserModel user, GetApiMethodCallsRequestModel data)
	{
		this._user = user;
		this._data = data;
	}

	public ResponseModel<List<ApiMethodCallsModel>> Handle()
	{
		try
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}
			var fetchedData = Infrastructure.Data.Access.Tables.SPR.ApiCallsAccess.GetApiMethodCounts(_data.NumberOfDays, _data.Area.ToLower(), _data.Controller.ToLower(),_data.ApiMethod);

			if(fetchedData is null || fetchedData.Count() == 0)
				return ResponseModel<List<ApiMethodCallsModel>>.SuccessResponse(new List<ApiMethodCallsModel>());

			var result = fetchedData.Select(x => new ApiMethodCallsModel(x)).ToList();

			return ResponseModel<List<ApiMethodCallsModel>>.SuccessResponse(result);
		} catch(Exception e)
		{
			Infrastructure.Services.Logging.Logger.Log(e);
			throw;
		}
	}
	public ResponseModel<List<ApiMethodCallsModel>> Validate()
	{
		if(!UserValidationHelper.IsValidUser(this._user))
		{
			return ResponseModel<List<ApiMethodCallsModel>>.AccessDeniedResponse();
		}

		return ResponseModel<List<ApiMethodCallsModel>>.SuccessResponse();
	}
}

