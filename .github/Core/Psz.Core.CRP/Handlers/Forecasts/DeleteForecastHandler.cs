using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;


namespace Psz.Core.CRP.Handlers.Forecasts
{
	public partial class CrpForecastsService
	{
		public ResponseModel<int> DeleteForecast(UserModel user, int data)
		{
			var transaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				var validationRespnse = ValidateDeleteForecast(user, data);
				if(!validationRespnse.Success)
					return validationRespnse;
				var forecast = Infrastructure.Data.Access.Tables.CRP.ForecastsAccess.Get(data);
				transaction.beginTransaction();
				Infrastructure.Data.Access.Tables.CRP.ForecastsAccess.DeleteWithTransaction(data, transaction.connection, transaction.transaction);
				var positions = Infrastructure.Data.Access.Tables.CRP.ForecastsPositionAccess.GetByForecastId(data);
				if(positions != null && positions.Count > 0)
				{
					Infrastructure.Data.Access.Tables.CRP.ForecastsPositionAccess.DeleteWithTransaction(positions.Select(p => p.Id).ToList(),
						transaction.connection, transaction.transaction);
				}
				var log = new Infrastructure.Data.Entities.Tables.CTS.OrderProcesssing_LogEntity
				{
					Nr = data,
					AngebotNr = 0,
					ProjektNr = 0,
					LogType = Helpers.LogHelper.LogType.DELETIONOBJECT.GetDescription(),
					LogObject = "Forecast",
					DateTime = DateTime.Now,
					UserId = user.Id,
					Origin = "CRP",
					LogText = $"[Forecast] for customer number [{forecast.kundennummer}] Version [{forecast.Version}] deleted",
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
		public ResponseModel<int> ValidateDeleteForecast(UserModel user, int data)
		{
			if(user == null)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			var forecast = Infrastructure.Data.Access.Tables.CRP.ForecastsAccess.Get(data);
			if(forecast == null)
				return ResponseModel<int>.FailureResponse("Forecast not found.");
			return ResponseModel<int>.SuccessResponse();
		}
	}
}