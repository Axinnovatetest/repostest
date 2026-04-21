
using Psz.Core.Support.Helpers;

namespace Psz.Core.Support.Handlers;

public  class GetApiAreasHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<string>>>
{
	private Identity.Models.UserModel _user { get; set; }
	private int _data { get; set; }

	public GetApiAreasHandler(Identity.Models.UserModel user, int NumberOfDays)
	{
		this._user = user;
		this._data = NumberOfDays;
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
			var fetchedData = Infrastructure.Data.Access.Tables.SPR.ApiCallsAccess.GetApiAreas(_data);

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
