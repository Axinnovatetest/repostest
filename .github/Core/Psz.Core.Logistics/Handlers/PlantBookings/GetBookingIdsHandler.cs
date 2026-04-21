namespace Psz.Core.Logistics.Handlers.PlantBookings;
public class GetBookingIdsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, WeVohModel>>>>
{
	private int _filter;
	private Core.Identity.Models.UserModel _user;
	public GetBookingIdsHandler(int filter, Core.Identity.Models.UserModel user)
	{
		_filter = filter;
		_user = user;

	}
	public ResponseModel<List<KeyValuePair<int, WeVohModel>>> Handle()
	{
		try
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success || _filter < 0)
			{
				return validationResponse;
			}
			var data = Infrastructure.Data.Access.Joins.Logistics.WeVOHIncomingAccess.GetWohBezeichungList(_filter);

			return ResponseModel<List<KeyValuePair<int, WeVohModel>>>.SuccessResponse(
			(data ?? new List<WEArtikelEntity>())
			?.Select(x => new KeyValuePair<int, WeVohModel>(x.weId, new WeVohModel(x)))
			.ToList());
		} catch(Exception e)
		{
			Infrastructure.Services.Logging.Logger.Log(e);
			throw;
		}
	}

	public ResponseModel<List<KeyValuePair<int, WeVohModel>>> Validate()
	{
		if(this._user == null/*|| this._user.Access.____*/)
		{
			return ResponseModel<List<KeyValuePair<int, WeVohModel>>>.AccessDeniedResponse();
		}

		return ResponseModel<List<KeyValuePair<int, WeVohModel>>>.SuccessResponse();
	}
}

