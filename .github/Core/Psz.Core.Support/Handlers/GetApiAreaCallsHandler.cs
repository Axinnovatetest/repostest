
using Psz.Core.Support.Helpers;

namespace Psz.Core.Support.Handlers;

public  class GetApiAreaCallsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<ApiALLAreaCallsModel>>>
{
	private Identity.Models.UserModel _user { get; set; }
	private int _data { get; set; }

	public GetApiAreaCallsHandler(Identity.Models.UserModel user, int NumberOfDays)
	{
		this._user = user;
		this._data = NumberOfDays;
	}

	public ResponseModel<List<ApiALLAreaCallsModel>> Handle()
	{
		try
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}
			var fetchedData = Infrastructure.Data.Access.Tables.SPR.ApiCallsAccess.GetAllApiAreaCounts(_data);
			
			if(fetchedData is null || fetchedData.Count() == 0)
				return ResponseModel<List<ApiALLAreaCallsModel>>.SuccessResponse(new List<ApiALLAreaCallsModel>());

			var result = fetchedData.Select(x=> new ApiALLAreaCallsModel(x)).ToList();

			return ResponseModel<List<ApiALLAreaCallsModel>>.SuccessResponse(result);
		} catch(Exception e)
		{
			Infrastructure.Services.Logging.Logger.Log(e);
			throw;
		}
	}

	public ResponseModel<List<ApiALLAreaCallsModel>> Validate()
	{
		if(!UserValidationHelper.IsValidUser(this._user))
		{
			return ResponseModel<List<ApiALLAreaCallsModel>>.AccessDeniedResponse();
		}

		return ResponseModel<List<ApiALLAreaCallsModel>>.SuccessResponse();
	}
}
