
using Psz.Core.Support.Helpers;
using Psz.Core.Support.Models;

namespace Psz.Core.Support.Handlers;

public  class GetApiControllersHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<string>>>
{
	private Identity.Models.UserModel _user { get; set; }
	private GetControllersRequestModel _data { get; set; }
	

	public GetApiControllersHandler(Identity.Models.UserModel user, GetControllersRequestModel data)
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
			var fetchedData = Infrastructure.Data.Access.Tables.SPR.ApiCallsAccess.GetApiControllers(_data.NumberOfDays,_data.Area.ToLower());
			
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
