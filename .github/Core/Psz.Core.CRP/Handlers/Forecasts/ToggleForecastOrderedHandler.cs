using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
namespace Psz.Core.CRP.Handlers.Forecasts
{
	public partial class CrpForecastsService
	{
		public ResponseModel<int> ToggleForecastPositionOrdered(UserModel user, int data)
		{
			var transaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				var validationRespnse = ValidateToggleForecastPositionOrdered(user, data);
				if(!validationRespnse.Success)
					return validationRespnse;
				var forecastPos = Infrastructure.Data.Access.Tables.CRP.ForecastsPositionAccess.Get(data);
				var forecast = Infrastructure.Data.Access.Tables.CRP.ForecastsAccess.Get(forecastPos.IdForcast ?? -1);

				transaction.beginTransaction();
				Infrastructure.Data.Access.Tables.CRP.ForecastsPositionAccess.ToggleOrdered(data, transaction.connection, transaction.transaction);
				//logging
				var log = new Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity
				{
					Nr = forecast.Id,
					AngebotNr = -1,
					ProjektNr = -1,
					LogType = Helpers.LogHelper.LogType.MODIFICATIONPOS.GetDescription(),
					LogObject = "Forecast",
					DateTime = DateTime.Now,
					UserId = user.Id,
					Origin = "CRP",
					LogText = $"[Forecast] for customer [{forecast.kundennummer}] Version [{forecast.Version}] Position with Article [{forecastPos.Artikelnummer}] set as Ordered",
					Username = user.Name,
				};
				Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.InsertWithTransaction(log, transaction.connection, transaction.transaction);

				if(transaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(1);
				}
				else
					return ResponseModel<int>.FailureResponse("Error Deleteting Forecast.");

			} catch(Exception e)
			{
				transaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> ValidateToggleForecastPositionOrdered(UserModel user, int data)
		{
			if(user == null)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			var forecastPos = Infrastructure.Data.Access.Tables.CRP.ForecastsPositionAccess.Get(data);
			if(forecastPos == null)
				return ResponseModel<int>.FailureResponse("Forecast Position not found.");

			var forecast = Infrastructure.Data.Access.Tables.CRP.ForecastsAccess.Get(forecastPos.IdForcast ?? -1);
			if(forecastPos == null)
				return ResponseModel<int>.FailureResponse("Forecast not found.");

			var maxDate = Infrastructure.Data.Access.Tables.CRP.ForecastsAccess.GetMAxVersionByKundenAndType(forecast.kundennummer ?? -1, forecast.TypeId ?? -1);
			if(maxDate is not null && forecast.Datum != maxDate)
				return ResponseModel<int>.FailureResponse("Edit forecast is only allowed for the last version.");
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
