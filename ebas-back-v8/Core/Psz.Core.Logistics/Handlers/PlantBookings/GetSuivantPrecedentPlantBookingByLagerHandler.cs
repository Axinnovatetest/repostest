
using Psz.Core.Logistics.Enums;

namespace Psz.Core.Logistics.Handlers.PlantBookings
{
	public class GetSuivantPrecedentPlantBookingByLagerHandler: IHandle<Identity.Models.UserModel, ResponseModel<PlantBookingDetailsResponseModel>>
	{
		private Core.Identity.Models.UserModel _user;
		private SelectPlantBookingMaxMinModel _data;



		public GetSuivantPrecedentPlantBookingByLagerHandler(Core.Identity.Models.UserModel user, SelectPlantBookingMaxMinModel data)
		{
			_user = user;
			_data = data;
		}

		public ResponseModel<PlantBookingDetailsResponseModel> Handle()
		{
			PlantBookingDetailsResponseModel Datamodel = new();
			var fetchedData = _data.LagerID switch
			{
				(int)LagerEnum.Albanien => Infrastructure.Data.Access.Tables.CTS.PSZ_Eingangskontrolle_ALAccess.GetByVerpackungNrAl(this._data.PassedNmrVpckng),
				(int)LagerEnum.Eigenfertigung => Infrastructure.Data.Access.Tables.CTS.PSZ_Eingangskontrolle_CZAccess.GetByVerpackungNrCz(this._data.PassedNmrVpckng),
				_ => Infrastructure.Data.Access.Tables.Logistics.PlantBookingAccess.GetByVerpackungNrTn(this._data.PassedNmrVpckng),
			};
			var resToReturn = new PlantBookingDetailsResponseModel(fetchedData);

		return ResponseModel<PlantBookingDetailsResponseModel>.SuccessResponse(resToReturn);

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
}
