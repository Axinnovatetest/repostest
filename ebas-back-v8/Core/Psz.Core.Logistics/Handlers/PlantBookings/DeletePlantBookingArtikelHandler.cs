
using Psz.Core.Logistics.Enums;

namespace Psz.Core.Logistics.Handlers.PlantBookings;
public class DeletePlantBookingArtikelHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private int _ID;
	    private int _lagerId;
		private Core.Identity.Models.UserModel _user;
	public DeletePlantBookingArtikelHandler(Core.Identity.Models.UserModel user,int ID,int lagerId)
		{
		_user = user;
		_ID = ID;
		_lagerId = lagerId;
		}

	public ResponseModel<int> Handle() {


		var botransaction = new Infrastructure.Services.Utils.TransactionsManager();

		try
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success || _ID < 0)
			{
				return validationResponse;
			}

			#region Deleting Script
			int  data ;

			data = _lagerId switch
			{
				(int)LagerEnum.Albanien => Infrastructure.Data.Access.Tables.CTS.PSZ_Eingangskontrolle_ALAccess.DeleteWithArtikelAl(_ID, _lagerId),
				(int)LagerEnum.Eigenfertigung => Infrastructure.Data.Access.Tables.CTS.PSZ_Eingangskontrolle_CZAccess.DeleteWithArtikelCZ(_ID, _lagerId),
				_ => Infrastructure.Data.Access.Tables.Logistics.PlantBookingAccess.DeleteWithArtikelTn(_ID, _lagerId),
			};
			#endregion

			#region Adding Delete Log
			var logs = PlantBookingLogHelper.GenerateLogForDelete(_user, _lagerId, _ID);
			botransaction.beginTransaction();
			var InsertLogsResult = Infrastructure.Data.Access.Tables.Logistics.PlantBookingsLogsAccess.InsertWithTransaction(logs, botransaction.connection, botransaction.transaction);
			#endregion
			if(botransaction.commit())
			{
				return ResponseModel<int>.SuccessResponse(data * InsertLogsResult);
			}
			else
			{
				return ResponseModel<int>.FailureResponse(key: "0", value: "Transaction error");
			}
		} catch(Exception e)
		{
			Infrastructure.Services.Logging.Logger.Log(e);
			throw;
		}
	}
	public ResponseModel<int> Validate()
	{
		if(this._user == null/*|| this._user.Access.____*/)
		{
			return ResponseModel<int>.AccessDeniedResponse();
		}

		return ResponseModel<int>.SuccessResponse();
	}

}
