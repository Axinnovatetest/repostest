using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.Forecasts;
using Psz.Core.Identity.Models;


namespace Psz.Core.CRP.Handlers.Forecasts
{
	public partial class CrpForecastsService
	{
		public ResponseModel<ForecastHeaderModel> GetForecastHeader(UserModel user, int id)
		{
			if(user == null)
				return ResponseModel<ForecastHeaderModel>.AccessDeniedResponse();
			if(Infrastructure.Data.Access.Tables.CRP.ForecastsAccess.Get(id) == null)
				return ResponseModel<ForecastHeaderModel>.FailureResponse("Forecast not found.");
			try
			{
				var entity = Infrastructure.Data.Access.Tables.CRP.ForecastsAccess.Get(id);
				var versions = Infrastructure.Data.Access.Tables.CRP.ForecastsAccess.GetByKundenAndType(entity.kundennummer ?? -1, entity.TypeId ?? -1);
				versions = versions.OrderBy(v => v.Datum).ToList();

				var currentForecastIndex = versions.FindIndex(x => x.Id == entity.Id);
				var responseBody = new ForecastHeaderModel(entity);
				responseBody.IdNextVersion = currentForecastIndex == versions.Count - 1 ? null : versions[currentForecastIndex + 1].Id;
				responseBody.IdPreviousVersion = currentForecastIndex == 0 ? null : versions[currentForecastIndex - 1].Id;
				return ResponseModel<ForecastHeaderModel>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}