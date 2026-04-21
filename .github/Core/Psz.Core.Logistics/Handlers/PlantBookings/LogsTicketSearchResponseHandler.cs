namespace Psz.Core.Logistics.Handlers.PlantBookings;
public class LogsTicketSearchResponseHandler: IHandle<Core.Identity.Models.UserModel, ResponseModel<List<TicketCountResponseModel>>>
{

	private Core.Identity.Models.UserModel _user;
	private PostTicketLogsModel _data;
	public LogsTicketSearchResponseHandler(Core.Identity.Models.UserModel user, PostTicketLogsModel data)
	{
		this._user = user;
		this._data = data;
	}
	public ResponseModel<List<TicketCountResponseModel>> Handle()
	{
		try
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}
			var fetchedData = Infrastructure.Data.Access.Joins.Logistics.PlantBookingsTicketLogsAccess.CountResponseTicket(this._data.BeginDate ,this._data.EndDate);
			return ResponseModel<List<TicketCountResponseModel>>.SuccessResponse(fetchedData?.Select(el => new TicketCountResponseModel(el)).OrderByDescending(x=>x.ticketscount).ToList());

		} catch(Exception ex)
		{
			Infrastructure.Services.Logging.Logger.Log(ex);
			throw;
		}
	}

	public ResponseModel<List<TicketCountResponseModel>> Validate()
	{
		if(this._user == null/*|| this._user.Access.____*/)
		{
			return ResponseModel<List<TicketCountResponseModel>>.AccessDeniedResponse();
		}

		return ResponseModel<List<TicketCountResponseModel>>.SuccessResponse();
	}

}
