using Infrastructure.Data.Entities.Joins.MTM.CRP;
using Psz.Core.Support.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Support.Handlers;

public  class GetSingleApiAreaCallsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<ApiAreaCallsModel>>>
{
	private Identity.Models.UserModel _user { get; set; }
	private GetSingleAreaRequestModel _data { get; set; }

	public GetSingleApiAreaCallsHandler(Identity.Models.UserModel user, GetSingleAreaRequestModel data)
	{
		this._user = user;
		this._data = data;
	}
	public ResponseModel<List<ApiAreaCallsModel>> Handle()
	{
		try
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}
			var fetchedData = Infrastructure.Data.Access.Tables.SPR.ApiCallsAccess.GetSingleApiAreaCounts(_data.NumberOfDays,_data.Area.ToLower());

			if(fetchedData is null || fetchedData.Count() == 0)
				return ResponseModel<List<ApiAreaCallsModel>>.SuccessResponse(new List<ApiAreaCallsModel>());


			var data = fetchedData.Select(x=> new ApiAreaCallsModel(x)).ToList();

			return ResponseModel<List<ApiAreaCallsModel>>.SuccessResponse(data);

		} catch(Exception e)
		{
			Infrastructure.Services.Logging.Logger.Log(e);
			throw;
		}
	}

	public ResponseModel<List<ApiAreaCallsModel>> Validate()
	{
		if(!UserValidationHelper.IsValidUser(this._user))
		{
			return ResponseModel<List<ApiAreaCallsModel>>.AccessDeniedResponse();
		}

		return ResponseModel<List<ApiAreaCallsModel>>.SuccessResponse();
	}
}

