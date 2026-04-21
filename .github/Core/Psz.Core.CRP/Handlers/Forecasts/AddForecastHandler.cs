using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.Forecasts;
using Psz.Core.Identity.Models;
using System.DirectoryServices.ActiveDirectory;

namespace Psz.Core.CRP.Handlers.Forecasts
{
	public partial class CrpForecastsService
	{
		public ResponseModel<int> AddForecast(UserModel user, ForecastModel data)
		{
			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				botransaction.beginTransaction();

				#region // -- transaction-based logic -- //
				var validationRespnse = ValidateAddForcast(user, data);
				if(!validationRespnse.Success)
					return validationRespnse;

				var kunden = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetWithTransaction(data.Header.KundenNr, botransaction.connection, botransaction.transaction);
				var adressen = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(kunden.Nummer ?? -1, botransaction.connection, botransaction.transaction);
				var idHeader = Infrastructure.Data.Access.Tables.CRP.ForecastsAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.CRP.ForecastsEntity
				{
					DateCreation = DateTime.Now,
					Datum = data.Header.Datum,
					Type = ((Enums.CRPEnums.ForcastType)data.Header.ForcastTypeId).ToString(),
					TypeId = data.Header.ForcastTypeId,
					kundennummer = adressen.Kundennummer,
					kunden = adressen.Name1,
					UserId = user.Id,
					Version = Helpers.CRPHelper.GetForecastVersion(adressen.Kundennummer ?? -1, data.Header.ForcastTypeId)
				}, botransaction.connection, botransaction.transaction);

				var response = Infrastructure.Data.Access.Tables.CRP.ForecastsPositionAccess.InsertWithTransaction(data.Positions.Select(p => new Infrastructure.Data.Entities.Tables.CRP.ForecastsPositionEntity
				{
					ArtikelNr = p.ArtikelNr,
					Artikelnummer = p.Artikelnummer,
					Datum = p.Datum,
					GesamtPreis = p.Gesampreis,
					IdForcast = idHeader,
					Jahr = p.Jahr,
					KW = p.KW,
					Material = p.Material,
					Menge = p.Menge,
					VKE = p.VKE,
				}).ToList(), botransaction.connection, botransaction.transaction);
				//logging
				var forecast = Infrastructure.Data.Access.Tables.CRP.ForecastsAccess.GetWithTransaction(idHeader, botransaction.connection, botransaction.transaction);
				var log = new Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity
				{
					Nr = idHeader,
					AngebotNr = 0,
					ProjektNr = 0,
					LogType = Helpers.LogHelper.LogType.CREATIONOBJECT.GetDescription(),
					LogObject = "Forecast",
					DateTime = DateTime.Now,
					UserId = user.Id,
					Origin = "CRP",
					LogText = $"[Forecast] For custommer number [{forecast.kundennummer}] Version [{forecast.Version}] created",
					Username = user.Name,
				};
				Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.InsertWithTransaction(log, botransaction.connection, botransaction.transaction);

				#endregion // -- transaction-based logic -- //


				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(response);
				}
				else
				{
					return ResponseModel<int>.FailureResponse("Transaction error");
				}
			} catch(Exception e)
			{
				botransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> ValidateAddForcast(UserModel user, ForecastModel data)
		{
			if(user == null/*|| this._user.Access.____*/)
				return ResponseModel<int>.AccessDeniedResponse();
			var forecast = Infrastructure.Data.Access.Tables.CRP.ForecastsAccess.GetByKundenAndTypeAndDate(data.Header.KundenNummer, data.Header.ForcastTypeId, data.Header.Datum);
			if(forecast != null || forecast?.Count > 0)
				return ResponseModel<int>.FailureResponse("A Forecast with the same customer and type and date already exsists .");
			return ResponseModel<int>.SuccessResponse();
		}
	}
}