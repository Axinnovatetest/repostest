using Psz.Core.Logistics.Enums;

namespace Psz.Core.Logistics.Handlers.PlantBookings
{
	public class GetPlantbookingDetailsHandler:IHandle<int,ResponseModel<PlantBookingDetailsResponseModel>>
	{
		private Core.Identity.Models.UserModel _user;
		private PlantBookingRequestPrintModel _data;
		public GetPlantbookingDetailsHandler(Core.Identity.Models.UserModel user, PlantBookingRequestPrintModel data)
		{
			this._user = user;
		    this._data= data;
	}

		public ResponseModel<PlantBookingDetailsResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				Infrastructure.Data.Entities.Joins.Logistics.PSZ_Eingangskontrolle_TNEntity fetchedData = new Infrastructure.Data.Entities.Joins.Logistics.PSZ_Eingangskontrolle_TNEntity();
				Infrastructure.Data.Entities.Joins.Logistics.PSZ_Eingangskontrolle_TNEntity Previous = new Infrastructure.Data.Entities.Joins.Logistics.PSZ_Eingangskontrolle_TNEntity();
				Infrastructure.Data.Entities.Joins.Logistics.PSZ_Eingangskontrolle_TNEntity Next= new Infrastructure.Data.Entities.Joins.Logistics.PSZ_Eingangskontrolle_TNEntity();
				PlantBookingDetailsResponseModel resToReturn = new PlantBookingDetailsResponseModel();

				switch(_data.LagerId)
				{
					case (int)LagerEnum.BETN:
					case (int)LagerEnum.GZTN:
					case (int)LagerEnum.TN:
					case (int)LagerEnum.WS:

						fetchedData = Infrastructure.Data.Access.Tables.Logistics.PlantBookingAccess.GetByVerpackungNrTn(this._data.NummerVerpackung);
						resToReturn = new PlantBookingDetailsResponseModel(fetchedData);
						Previous = Infrastructure.Data.Access.Tables.Logistics.PlantBookingAccess.GetPlantBookingsPrecedentByLagerID(fetchedData.LagerortID ?? -1, fetchedData.Nummer_Verpackung);
						 Next = Infrastructure.Data.Access.Tables.Logistics.PlantBookingAccess.GetPlantBookingsSuivantByLagerID(fetchedData.LagerortID ?? -1, fetchedData.Nummer_Verpackung);
							resToReturn.Previous = Previous == null ? 0 : Previous.Nummer_Verpackung;
							resToReturn.Next = Next == null ? 0 : Next.Nummer_Verpackung;
						break;
					
					case (int)LagerEnum.Eigenfertigung:
						fetchedData = Infrastructure.Data.Access.Tables.CTS.PSZ_Eingangskontrolle_CZAccess.GetByVerpackungNrCz(this._data.NummerVerpackung);
						resToReturn = new PlantBookingDetailsResponseModel(fetchedData);
						Previous = Infrastructure.Data.Access.Tables.CTS.PSZ_Eingangskontrolle_CZAccess.GetPlantBookingsPrecedentByLagerIDCZ(fetchedData.LagerortID ?? -1, fetchedData.Verpackungsnr ?? -1);
						Next = Infrastructure.Data.Access.Tables.CTS.PSZ_Eingangskontrolle_CZAccess.GetPlantBookingsSuivantByLagerIDCZ(fetchedData.LagerortID ?? -1, fetchedData.Verpackungsnr ?? -1);
							resToReturn.Previous = Previous == null ? 0 : Previous.Verpackungsnr;
							resToReturn.Next = Next == null ? 0 : Next.Verpackungsnr;
						break;

					case (int)LagerEnum.Albanien:
						fetchedData = Infrastructure.Data.Access.Tables.CTS.PSZ_Eingangskontrolle_ALAccess.GetByVerpackungNrAl(this._data.NummerVerpackung);
						resToReturn = new PlantBookingDetailsResponseModel(fetchedData);
						Previous = Infrastructure.Data.Access.Tables.CTS.PSZ_Eingangskontrolle_ALAccess.GetPlantBookingsPrecedentByLagerIDAL(fetchedData.LagerortID ?? -1, fetchedData.Verpackungsnr ?? -1);
						Next = Infrastructure.Data.Access.Tables.CTS.PSZ_Eingangskontrolle_ALAccess.GetPlantBookingsSuivantByLagerIDAL(fetchedData.LagerortID ?? -1, fetchedData.Verpackungsnr ?? -1);
							resToReturn.Previous = Previous == null ? 0 : Previous.Verpackungsnr;
							resToReturn.Next = Next == null ? 0: Next.Verpackungsnr;

						break;
					default:
						break;
				}
				return ResponseModel<PlantBookingDetailsResponseModel>.SuccessResponse(resToReturn);

			} catch(Exception ex) {
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
}
