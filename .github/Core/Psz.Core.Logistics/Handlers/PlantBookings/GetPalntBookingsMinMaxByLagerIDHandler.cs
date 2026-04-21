using Psz.Core.Logistics.Enums;

namespace Psz.Core.Logistics.Handlers.PlantBookings
{
	public class GetPalntBookingsMinMaxByLagerIDHandler : IHandle<Identity.Models.UserModel,  ResponseModel<PlantBookingDetailsResponseModel>>
	{
			private Core.Identity.Models.UserModel _user;
			private SelectPlantBookingMaxMinModel _data;



			public GetPalntBookingsMinMaxByLagerIDHandler(Core.Identity.Models.UserModel user, SelectPlantBookingMaxMinModel data)
			{
				_user = user;
				_data = data;

			}

		public ResponseModel<PlantBookingDetailsResponseModel> Handle()
			{

			PlantBookingDetailsResponseModel Datamodel = new();
	

			var fetchedMax = _data.LagerID switch
			{
				(int)LagerEnum.Albanien => Infrastructure.Data.Access.Tables.CTS.PSZ_Eingangskontrolle_ALAccess.GetPalntBookingsMinMaxAL(_data.LagerID , _data.Order),
				(int)LagerEnum.Eigenfertigung => Infrastructure.Data.Access.Tables.CTS.PSZ_Eingangskontrolle_CZAccess.GetPalntBookingsMinMaxCZ(_data.LagerID , _data.Order),
				_ => Infrastructure.Data.Access.Tables.Logistics.PlantBookingAccess.GetPalntBookingsMinMaxByLagerID(_data.LagerID, _data.Order),
			};
			var resToReturn = new PlantBookingDetailsResponseModel(fetchedMax);

			//if(_data.Order == 1 && fetchedMax is not null && fetchedMax.Nummer_Verpackung > 0)
			//{
			//		resToReturn.NextExist = true;
			//}
			//else
			//{
			//	    resToReturn.PrevExist = true;
			//}


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


