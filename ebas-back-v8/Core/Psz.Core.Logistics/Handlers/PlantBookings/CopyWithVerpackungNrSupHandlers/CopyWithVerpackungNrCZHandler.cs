using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Psz.Core.Logistics.Enums;

namespace Psz.Core.Logistics.Handlers.PlantBookings.SubHandlers;

internal class CopyWithVerpackungNrCZHandler: IHandle<Identity.Models.UserModel, ResponseModel<PlantBookingDetailsResponseModel>>
{
	private int _verpackungNr;
	private int _lagerId;
	private Core.Identity.Models.UserModel _user;
	public CopyWithVerpackungNrCZHandler(int VerpackungNr, int lager, Core.Identity.Models.UserModel user)
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
			int vernum = -1;
			botransaction.beginTransaction();

			var plantBookingByVerpackungNr = Infrastructure.Data.Access.Tables.CTS.PSZ_Eingangskontrolle_CZAccess.GetByVerpackungNrCz(_verpackungNr);

			if(plantBookingByVerpackungNr is null)
				return ResponseModel<PlantBookingDetailsResponseModel>.FailureResponse("No data available to perform the copy  !");

			plantBookingByVerpackungNr.Aktiv = 0;
			plantBookingByVerpackungNr.Datum = DateTime.Now;
			plantBookingByVerpackungNr.Restmenge_Rolle_PPS = null;
			plantBookingByVerpackungNr.Status_Rolle = null;
			vernum = Infrastructure.Data.Access.Tables.CTS.PSZ_Eingangskontrolle_CZAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.CTS.PSZ_Eingangskontrolle_CZEntity(plantBookingByVerpackungNr, null), botransaction.connection, botransaction.transaction);

			if(!botransaction.commit())
				return ResponseModel<PlantBookingDetailsResponseModel>.FailureResponse("Failed to copy the Data !");

			var newlyInsertedPlantBooking = Infrastructure.Data.Access.Tables.CTS.PSZ_Eingangskontrolle_CZAccess.GetBynummer_verpackungCz(vernum);

			if(newlyInsertedPlantBooking is null)
				return ResponseModel<PlantBookingDetailsResponseModel>.FailureResponse("Failed to copy the Data !");

			return ResponseModel<PlantBookingDetailsResponseModel>.SuccessResponse(new PlantBookingDetailsResponseModel(newlyInsertedPlantBooking));

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

