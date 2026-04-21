using Psz.Core.Support.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Support.Handlers;

public  class GetFirstAndLastCallsHandler: IHandle<Identity.Models.UserModel, ResponseModel<GetFirstAndLastCallResponseModel>>
{
	private Identity.Models.UserModel _user { get; set; }
	private GetFirstAndLastCallRequestModel _data { get; set; }

	public GetFirstAndLastCallsHandler(Identity.Models.UserModel user, GetFirstAndLastCallRequestModel data)
	{
		this._user = user;
		this._data = data;
	}

	public ResponseModel<GetFirstAndLastCallResponseModel> Handle()
	{
		try
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}
			var fetchedData = Infrastructure.Data.Access.Tables.SPR.ApiCallsAccess.GetFirstAndLastCalls(_data.NumberOfDays,_data.ApiArea,_data.ApiController,_data.ApiMethod);

			
				return ResponseModel<GetFirstAndLastCallResponseModel>.SuccessResponse(new GetFirstAndLastCallResponseModel(fetchedData) );

		} catch(Exception e)
		{
			Infrastructure.Services.Logging.Logger.Log(e);
			throw;
		}
	}

	public ResponseModel<GetFirstAndLastCallResponseModel> Validate()
	{
		if(!UserValidationHelper.IsValidUser(this._user))
		{
			return ResponseModel<GetFirstAndLastCallResponseModel>.AccessDeniedResponse();
		}

		return ResponseModel<GetFirstAndLastCallResponseModel>.SuccessResponse();
	}
}
