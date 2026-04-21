using Psz.Core.Logistics.Models.ControlProcedure;

namespace Psz.Core.Logistics.Handlers.ControlProcedure;

public class GetControlProcedureHandler : IHandle<Core.Identity.Models.UserModel, ResponseModel<List<ControlProcedureResponseModel>>>
{

	private Core.Identity.Models.UserModel _user;
	private string _artNr;
	public GetControlProcedureHandler(Core.Identity.Models.UserModel user, string artNr)
	{
		this._user = user;
		this._artNr = artNr;
	}

	public ResponseModel<List<ControlProcedureResponseModel>> Handle()
	{
		try
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			if(this._artNr is null || string.IsNullOrEmpty(this._artNr) ||  this._artNr.Length < 4)
				return ResponseModel<List<ControlProcedureResponseModel>>.SuccessResponse(new List<ControlProcedureResponseModel>());


			var fetchedData = Infrastructure.Data.Access.Tables.Logistics.PlantBookingArticleControlProcedureAccess.GetByNr(this._artNr);
			
			return ResponseModel<List<ControlProcedureResponseModel>>.SuccessResponse(fetchedData?.Select(el=> new ControlProcedureResponseModel(el)).ToList());

		} catch(Exception ex)
		{
			Infrastructure.Services.Logging.Logger.Log(ex);
			throw;
		}
	}

	public ResponseModel<List<ControlProcedureResponseModel>> Validate()
	{
		if(this._user == null/*|| this._user.Access.____*/)
		{
			return ResponseModel<List<ControlProcedureResponseModel>>.AccessDeniedResponse();
		}

		return ResponseModel<List<ControlProcedureResponseModel>>.SuccessResponse();
	}

}
