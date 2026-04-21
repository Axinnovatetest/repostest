
using Psz.Core.Support.Helpers;
using Psz.Core.Support.Models;

namespace Psz.Core.Support.Handlers;

public  class GetApiMethodsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<string>>>
{
	private Identity.Models.UserModel _user { get; set; }
	private GetMethodsRequestModel _data { get; set; }


	public GetApiMethodsHandler(Identity.Models.UserModel user, GetMethodsRequestModel data)
	{
		this._user = user;
		this._data = data;
	}

	public ResponseModel<List<string>> Handle()
	{
		try
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}
			var fetchedData = Infrastructure.Data.Access.Tables.SPR.ApiCallsAccess.GetApiMethods(_data.NumberOfDays, _data.Area.ToLower(), _data.Controller.ToLower());

			if(fetchedData is null || fetchedData.Count() == 0)
				return ResponseModel<List<string>>.SuccessResponse(new List<string>());

			fetchedData = fetchedData.Select(x=>x.ToLower()).ToList();

			return ResponseModel<List<string>>.SuccessResponse(fetchedData);
		} catch(Exception e)
		{
			Infrastructure.Services.Logging.Logger.Log(e);
			throw;
		}
	}

	public ResponseModel<List<string>> Validate()
	{
		if(!UserValidationHelper.IsValidUser(this._user))
		{
			return ResponseModel<List<string>>.AccessDeniedResponse();
		}

		return ResponseModel<List<string>>.SuccessResponse();
	}
}

