using System.Data.SqlClient;
using Infrastructure.Data.Entities.Joins.Logistics;
using iText.Forms.Fields;
using Psz.Core.Logistics.Enums;
using Psz.Core.Logistics.Interfaces;
using Psz.Core.Logistics.Models.Lagebewegung;

namespace Psz.Core.Logistics.Handlers.PlantBookings
{
	public class PostPlantBookingWithAllDataHandler: IHandle<Identity.Models.UserModel, ResponseModel<PlantBookingDetailsResponseModel>>
	{

		private Core.Identity.Models.UserModel _user;
		private CreatePlantBookingRequestModel _data;
		private readonly IPlantBookingPostArtikel _strategy;


		public PostPlantBookingWithAllDataHandler(Core.Identity.Models.UserModel user, CreatePlantBookingRequestModel data)
		{
			_user = user;
			this._data = data;
			_strategy = _data.LagerId switch
			{
				(int)LagerEnum.Albanien => new AlbanienStrategy(),
				(int)LagerEnum.Eigenfertigung => new EigenfertigungStrategy(),
				_ => new TnStrategy()
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
				if(_data.Choice && (this._data.ArtikelNummer is null || this._data.ArtikelNummer.ToLower().Contains("Archi".ToLower())))
					return ResponseModel<PlantBookingDetailsResponseModel>.FailureResponse(key: "1", value: "Invalid Article");
				botransaction.beginTransaction();

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

			if(this._data.Choice)
			{
				var article = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByNumber(this._data.ArtikelNummer);
				if(article is null)
				{
					return ResponseModel<PlantBookingDetailsResponseModel>.FailureResponse($"Article [{this._data.ArtikelNummer}] does not exist");
				}
				if(article.MHD == true && (this._data.MHDDatum == null || this._data.MHDDatum.Value <= DateTime.Today))
				{
					return ResponseModel<PlantBookingDetailsResponseModel>.FailureResponse($"MHD Datum [{this._data.MHDDatum?.ToString("dd/MM/yyyy")}] is not a valid date");
				}
			}

			return ResponseModel<PlantBookingDetailsResponseModel>.SuccessResponse();
		}
	}


	public class AlbanienStrategy: IPlantBookingPostArtikel
	{

		public int InsertData(CreatePlantBookingRequestModel data, SqlConnection connection, SqlTransaction transaction)
		{
			return data.Choice
				? Infrastructure.Data.Access.Tables.CTS.PSZ_Eingangskontrolle_ALAccess.InsertWithTransactionUsingArtikelNummerAl(data.LagerId, data.ArtikelNummer, data.MHDDatum, data.Inspector, data.Remarks, connection, transaction)
				: Infrastructure.Data.Access.Tables.CTS.PSZ_Eingangskontrolle_ALAccess.InsertWithTransactionBestellteArtikelAl(data.LagerId, data.NrBestellteArtikel, data.Inspector, data.Remarks, connection, transaction);
		}

		public PSZ_Eingangskontrolle_TNEntity FetchData(int result)
		{
			return Infrastructure.Data.Access.Tables.CTS.PSZ_Eingangskontrolle_ALAccess.GetByVerpackungNrAl(result);
		}


	}

	public class EigenfertigungStrategy: IPlantBookingPostArtikel
	{

		public int InsertData(CreatePlantBookingRequestModel data, SqlConnection connection, SqlTransaction transaction)
		{
			return data.Choice
				? Infrastructure.Data.Access.Tables.CTS.PSZ_Eingangskontrolle_CZAccess.InsertWithArtikelNummerCz(data.LagerId, data.ArtikelNummer, data.MHDDatum, data.Inspector, data.Remarks, connection, transaction)
				: Infrastructure.Data.Access.Tables.CTS.PSZ_Eingangskontrolle_CZAccess.InsertWithTransactionBestellteArtikelCz(data.LagerId, data.NrBestellteArtikel, data.Inspector, data.Remarks, connection, transaction);
		}

		public PSZ_Eingangskontrolle_TNEntity FetchData(int result)
		{
			return Infrastructure.Data.Access.Tables.CTS.PSZ_Eingangskontrolle_CZAccess.GetByVerpackungNrCz(result);
		}
	}

	public class TnStrategy: IPlantBookingPostArtikel
	{

		public int InsertData(CreatePlantBookingRequestModel data, SqlConnection connection, SqlTransaction transaction)
		{
			return data.Choice
				? Infrastructure.Data.Access.Tables.Logistics.PlantBookingAccess.InsertWithArtikelNummerTn(data.LagerId, data.ArtikelNummer, data.MHDDatum, data.Inspector, data.Remarks, connection, transaction)
				: Infrastructure.Data.Access.Tables.Logistics.PlantBookingAccess.InsertWithTransactionBestellteArtikelTn(data.LagerId, data.NrBestellteArtikel, data.Inspector, data.Remarks, connection, transaction);
		}

		public PSZ_Eingangskontrolle_TNEntity FetchData(int result)
		{
			return Infrastructure.Data.Access.Tables.Logistics.PlantBookingAccess.GetByVerpackungNrTn(result);
		}
	}


}
