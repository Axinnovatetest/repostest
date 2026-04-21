using Psz.Core.Common.Models;
using Psz.Core.CRP.Interfaces;
using Psz.Core.CRP.Models.Forecasts;
using Psz.Core.Identity.Models;


namespace Psz.Core.CRP.Handlers.Forecasts
{
	public partial class CrpForecastsService: ICrpForecastsService
	{
		public ResponseModel<IEnumerable<ForecastPositionModel>> ImportForecastFromXLS(UserModel user, ImportFileModel data)
		{
			try
			{
				var validationRespnse = ValidateImportForcastFromXLS(user);
				if(!validationRespnse.Success)
					return validationRespnse;

				var errors = new List<string>();
				var response = data.ForcastDraftTypeId == (int)Enums.CRPEnums.ForcastDraftType.Hoch
					? Helpers.CRPHelper.ReadFromExcelHoch(data.FilePath, out errors)
					: Helpers.CRPHelper.ReadFromExcelQuer(data.FilePath, out errors);

				if(errors != null && errors.Count > 0)
					return ResponseModel<IEnumerable<ForecastPositionModel>>.FailureResponse(errors);

				return ResponseModel<IEnumerable<ForecastPositionModel>>.SuccessResponse(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<IEnumerable<ForecastPositionModel>> ValidateImportForcastFromXLS(UserModel user)
		{
			if(user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<IEnumerable<ForecastPositionModel>>.AccessDeniedResponse();
			}

			return ResponseModel<IEnumerable<ForecastPositionModel>>.SuccessResponse();
		}
	}
}