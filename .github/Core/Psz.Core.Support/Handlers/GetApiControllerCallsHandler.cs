
using Psz.Core.Support.Helpers;

namespace Psz.Core.Support.Handlers;

public  class GetApiControllerCallsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<ApiAllControllerCallsModel>>>
{
	private Identity.Models.UserModel _user { get; set; }
	private GetApiControllersCallsRequestModel _data { get; set; }

	public GetApiControllerCallsHandler(Identity.Models.UserModel user, GetApiControllersCallsRequestModel data)
	{
		this._user = user;
		this._data = data;
	}

	public ResponseModel<List<ApiAllControllerCallsModel>> Handle()
	{
		try
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}
			var fetchedData = Infrastructure.Data.Access.Tables.SPR.ApiCallsAccess.GetApiAllControllersCounts(_data.NumberOfDays,_data.Area.ToLower());
			
			if(fetchedData is null || fetchedData.Count() == 0)
				return ResponseModel<List<ApiAllControllerCallsModel>>.SuccessResponse(new List<ApiAllControllerCallsModel>());

			var result = fetchedData.Select(x => new ApiAllControllerCallsModel(x)).ToList();

		


			return ResponseModel<List<ApiAllControllerCallsModel>>.SuccessResponse(result);
		} catch(Exception e)
		{
			Infrastructure.Services.Logging.Logger.Log(e);
			throw;
		}
	}

	public ResponseModel<List<ApiAllControllerCallsModel>> Validate()
	{
		if(!UserValidationHelper.IsValidUser(this._user))
		{
			return ResponseModel<List<ApiAllControllerCallsModel>>.AccessDeniedResponse();
		}

		return ResponseModel<List<ApiAllControllerCallsModel>>.SuccessResponse();
	}
}
