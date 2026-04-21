
using Psz.Core.Logistics.Enums;
using Psz.Core.Logistics.Handlers.PlantBookings.SubHandlers;

namespace Psz.Core.Logistics.Handlers.PlantBookings;
public class CopyWithVerpackungNrHandler: IHandle<Identity.Models.UserModel, ResponseModel<PlantBookingDetailsResponseModel>>
{
	private int _verpackungNr;
	private int _lagerId;
	private Core.Identity.Models.UserModel _user;
	public CopyWithVerpackungNrHandler(int VerpackungNr, int lager, Core.Identity.Models.UserModel user)
	{
		_verpackungNr = VerpackungNr;
		_lagerId = lager;
		_user = user;
	}
	public ResponseModel<PlantBookingDetailsResponseModel> Handle()
	{
		var botransaction = new Infrastructure.Services.Utils.TransactionsManager();

		try
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}
				return _lagerId switch
				{
					(int)LagerEnum.Albanien => new CopyWithVerpackungNrAlbaniaHandler(_verpackungNr, _lagerId, _user).Handle(),
					(int)LagerEnum.Eigenfertigung => new CopyWithVerpackungNrCZHandler(_verpackungNr, _lagerId, _user).Handle(),
					_ => new CopyWithVerpackungNrTNHandler(_verpackungNr, _lagerId, _user).Handle(),
				};

		} catch(Exception ex)
		{
			Infrastructure.Services.Logging.Logger.Log(ex);
			throw;
		}
	}

	public ResponseModel<PlantBookingDetailsResponseModel> Validate()
	{

		if(this._user == null/*|| this._user.Access.____*/)
		{
			return ResponseModel<PlantBookingDetailsResponseModel>.AccessDeniedResponse();
		}

		return ResponseModel<PlantBookingDetailsResponseModel>.SuccessResponse();
	}
}
