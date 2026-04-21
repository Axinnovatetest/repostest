using System.Data.SqlClient;
using Psz.Core.Logistics.Enums;
using Psz.Core.Logistics.Interfaces;
using Infrastructure.Data.Entities.Joins.Logistics;

namespace Psz.Core.Logistics.Handlers.PlantBookings
{
	public class PostPlantBookingTransferWithAllDataHandler: IHandle<Identity.Models.UserModel, ResponseModel<PlantBookingDetailsResponseModel>>
	{

		private Core.Identity.Models.UserModel _user;
		private CreatePlantBookingRequestModel _data;
		private readonly IPlantBookingPostArtikel _strategy;


		public PostPlantBookingTransferWithAllDataHandler(Core.Identity.Models.UserModel user, CreatePlantBookingRequestModel data)
		{
			_user = user;
			this._data = data;
			_strategy = _data.LagerId switch
			{
				(int)LagerEnum.Albanien => new AlbanienTransferStrategy(),
				(int)LagerEnum.Eigenfertigung => new EigenfertigungTransferStrategy(),
				_ => new TnTransferStrategy()
			};
		}

		public ResponseModel<PlantBookingDetailsResponseModel> Handle()
		{
			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				PlantBookingDetailsResponseModel Datamodel = new();
				int result = -1;
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				if(_data.Choice && _data.ChoiceTransfer && (this._data.ArtikelNummer is null || this._data.ArtikelNummer.ToLower().Contains("Archi".ToLower())))
					return ResponseModel<PlantBookingDetailsResponseModel>.FailureResponse(key: "1", value: "Invalid Article");
				botransaction.beginTransaction();

				var AnzahNach = Infrastructure.Data.Access.Tables.Logistics.PlantBookingAccess.GetAnzahlNach(_data.LagerNach ?? 0 ,_data.NrBestellteArtikel ?? 0,_data.LagerbewegungenId ?? 0);
				_data.AnzahlNach = (int?)AnzahNach;
				result = _strategy.InsertData(_data, botransaction.connection, botransaction.transaction);
				if(result < 0)
					return ResponseModel<PlantBookingDetailsResponseModel>.FailureResponse("Problem occured during generating new VP NR");


				#region Adding Create Log
				var logs = PlantBookingLogHelper.GenerateLogForCreate(_user, result, this._data.ArtikelNummer, this._data.LagerId);
				var InsertLogsResult = Infrastructure.Data.Access.Tables.Logistics.PlantBookingsLogsAccess.InsertWithTransaction(logs, botransaction.connection, botransaction.transaction);
				#endregion

				if(botransaction.commit())
				{
					var fetchedData = _strategy.FetchData(result);

					if(fetchedData is not null)
					{
						Datamodel = new Core.Logistics.Models.PlantBookings.PlantBookingDetailsResponseModel(fetchedData);
						
					}
					else
					{
						throw new InvalidOperationException("Invalid Provided Data in the ticket model !");
					}
					return ResponseModel<PlantBookingDetailsResponseModel>.SuccessResponse(Datamodel);
				}
				else
				{
					return ResponseModel<PlantBookingDetailsResponseModel>.FailureResponse(key: "1", value: "Transaction error");
				}

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
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


	public class AlbanienTransferStrategy: IPlantBookingPostArtikel
	{

		public int InsertData(CreatePlantBookingRequestModel data, SqlConnection connection, SqlTransaction transaction)
		{
			return Infrastructure.Data.Access.Tables.CTS.PSZ_Eingangskontrolle_ALAccess.InsertWithTransactionTransferBestellteArtikelAl(data.AnzahlNach ?? 0,data.LagerId, data.NrBestellteArtikel,data.LagerbewegungenId, connection, transaction);
		}

		public PSZ_Eingangskontrolle_TNEntity FetchData(int result)
		{

			return Infrastructure.Data.Access.Tables.CTS.PSZ_Eingangskontrolle_ALAccess.GetByVerpackungNrAl(result);
		}


	}

	public class EigenfertigungTransferStrategy: IPlantBookingPostArtikel
	{

		public int InsertData(CreatePlantBookingRequestModel data, SqlConnection connection, SqlTransaction transaction)
		{
			return Infrastructure.Data.Access.Tables.CTS.PSZ_Eingangskontrolle_CZAccess.InsertWithTransactionTransferBestellteArtikelCZ(data.AnzahlNach ?? 0,data.LagerId, data.NrBestellteArtikel, data.LagerbewegungenId, connection, transaction);
		}

		public PSZ_Eingangskontrolle_TNEntity FetchData(int result)
		{
			return Infrastructure.Data.Access.Tables.CTS.PSZ_Eingangskontrolle_CZAccess.GetByVerpackungNrCz(result);
		}
	}

	public class TnTransferStrategy: IPlantBookingPostArtikel
	{

		public int InsertData(CreatePlantBookingRequestModel data, SqlConnection connection, SqlTransaction transaction)
		{
			return Infrastructure.Data.Access.Tables.Logistics.PlantBookingAccess.InsertWithTransactionTransferBestellteArtikelTn(data.AnzahlNach ?? 0,data.LagerId, data.NrBestellteArtikel, data.LagerbewegungenId, connection, transaction);
		}

		public PSZ_Eingangskontrolle_TNEntity FetchData(int result)
		{
			return Infrastructure.Data.Access.Tables.Logistics.PlantBookingAccess.GetByVerpackungNrTn(result);
		}
	}


}
