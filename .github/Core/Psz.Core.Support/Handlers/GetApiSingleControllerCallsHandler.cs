using Psz.Core.Support.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Support.Handlers;

public  class GetApiSingleControllerCallsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<ApiSingleControllerCallsModel>>>
{
	private Identity.Models.UserModel _user { get; set; }
	private GetApiSingleControllersCallsRequestModel _data { get; set; }

	public GetApiSingleControllerCallsHandler(Identity.Models.UserModel user, GetApiSingleControllersCallsRequestModel data)
	{
		this._user = user;
		this._data = data;
	}

	public ResponseModel<List<ApiSingleControllerCallsModel>> Handle()
	{
		try
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}
			var fetchedData = Infrastructure.Data.Access.Tables.SPR.ApiCallsAccess.GetApiSingleControllersCounts(_data.NumberOfDays, _data.Area.ToLower(), _data.Controller.ToLower());

			if(fetchedData is null || fetchedData.Count() == 0)
				return ResponseModel<List<ApiSingleControllerCallsModel>>.SuccessResponse(new List<ApiSingleControllerCallsModel>());

			var result = fetchedData.Select(x => new ApiSingleControllerCallsModel(x)).ToList();

			return ResponseModel<List<ApiSingleControllerCallsModel>>.SuccessResponse(result);
		} catch(Exception e)
		{
			Infrastructure.Services.Logging.Logger.Log(e);
			throw;
		}
	}

	public ResponseModel<List<ApiSingleControllerCallsModel>> Validate()
	{
		if(!UserValidationHelper.IsValidUser(this._user))
		{
			return ResponseModel<List<ApiSingleControllerCallsModel>>.AccessDeniedResponse();
		}

		return ResponseModel<List<ApiSingleControllerCallsModel>>.SuccessResponse();
	}
}
