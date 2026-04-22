using MoreLinq;
using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.Forecasts;
using Psz.Core.Identity.Models;

namespace Psz.Core.CRP.Handlers.Forecasts
{
	public partial class CrpForecastsService
	{
		public ResponseModel<IEnumerable<ForecastHeaderModel>> GetForecasts(UserModel user)
		{
			try
			{
				var validationRespnse = ValidateGetForecasts(user);
				if(!validationRespnse.Success)
					return validationRespnse;

				var response = new List<ForecastHeaderModel>();
				var entities = Infrastructure.Data.Access.Tables.CRP.ForecastsAccess.Get();
				if(entities != null && entities.Count > 0)
				{
					var customers = entities.Select(c => c.kundennummer).Distinct().ToList();
					foreach(var customer in customers)
					{
						var types = entities.Where(x => x.kundennummer == customer).Select(y => y.TypeId).Distinct().ToList();
						foreach(var type in types)
						{
							var forcastInfo = entities.Where(x => x.kundennummer == customer && x.TypeId == type).Distinct().OrderByDescending(y => y.Datum).ToList();
							response.Add(new ForecastHeaderModel
							{
								Datum = forcastInfo[0].Datum ?? DateTime.Today,
								Kunden = forcastInfo[0].kunden,
								KundenNummer = forcastInfo[0].kundennummer ?? -1,
								Type = ((Enums.CRPEnums.ForcastType)type).ToString(),
								Versions = forcastInfo.Count,
								IdLastVersion = forcastInfo[0].Id
							});
						}
					}
				}
				return ResponseModel<IEnumerable<ForecastHeaderModel>>.SuccessResponse(response.AsEnumerable());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<IEnumerable<ForecastHeaderModel>> ValidateGetForecasts(UserModel user)
		{
			if(user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<IEnumerable<ForecastHeaderModel>>.AccessDeniedResponse();
			}

			return ResponseModel<IEnumerable<ForecastHeaderModel>>.SuccessResponse();
		}
	}
}