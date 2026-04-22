using Infrastructure.Data.Entities.Tables.CTS;

namespace Psz.Core.Logistics.Handlers.PlantBookings
{
	public class PostBookingHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		
		private Core.Identity.Models.UserModel _user;
		private CreatePlantBookingRequestModel _data;

		public PostBookingHandler( Core.Identity.Models.UserModel user, CreatePlantBookingRequestModel data)
		{
			_user = user;
			this._data = data;
		}
				public ResponseModel<int> Handle()
		{
			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();

			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
					
				var plantbookingvalue = new PSZ_Eingangskontrolle_TNEntity()
				{
					Datum = DateTime.Now,
					Artikelnummer =this._data.ArtikelNummer,
					Eingangslieferscheinnr=this._data.NrBestellteArtikel,
					LagerortID =this._data.LagerId
				};

				if(this._data.ArtikelNummer is null || this._data.ArtikelNummer.Length > 20 || this._data.ArtikelNummer.ToLower().Contains("Archi".ToLower()))
					return  ResponseModel<int>.FailureResponse(key: "1", value: "Invalid Article"); 
				#region Create Script
				var data = Infrastructure.Data.Access.Tables.CTS.PSZ_Eingangskontrolle_TNAccess.Insert(plantbookingvalue);
				#endregion

				#region Adding Create Log
				var logs = PlantBookingLogHelper.GenerateLogForCreate(_user, data, this._data.ArtikelNummer,this._data.LagerId);
				botransaction.beginTransaction();
				var InsertLogsResult = Infrastructure.Data.Access.Tables.Logistics.PlantBookingsLogsAccess.InsertWithTransaction(logs, botransaction.connection, botransaction.transaction);
				#endregion

				if(botransaction.commit())
				{
					return ResponseModel<int>.SuccessResponse( data * InsertLogsResult);
				}
				else
				{
					return ResponseModel<int>.FailureResponse(key: "1", value: "Transaction error");
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

}
