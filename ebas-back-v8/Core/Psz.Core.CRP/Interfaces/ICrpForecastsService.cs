using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.Forecasts;
using Psz.Core.Identity.Models;


namespace Psz.Core.CRP.Interfaces
{
	public interface ICrpForecastsService
	{
		ResponseModel<int> AddForecast(UserModel user, ForecastModel data);
		ResponseModel<int> DeleteForecast(UserModel user, int id);
		ResponseModel<IEnumerable<ForecastPositionModel>> ImportForecastFromXLS(UserModel user, ImportFileModel data);
		ResponseModel<IEnumerable<ForecastHeaderModel>> GetForecasts(UserModel user);
		ResponseModel<byte[]> GetForecastsExcel(UserModel user, CRPExcelRequestModel data);
		ResponseModel<byte[]> GetForecastsDraft(UserModel user, int type);
		ResponseModel<int> ToggleForecastPositionOrdered(UserModel user, int data);
		ResponseModel<ForecastHeaderModel> GetForecastHeader(UserModel user, int id);
		ResponseModel<ForecastPositionsResponseModel> GetForecastPositions(UserModel user, ForecastPositonsRequestModel model);
	}
}