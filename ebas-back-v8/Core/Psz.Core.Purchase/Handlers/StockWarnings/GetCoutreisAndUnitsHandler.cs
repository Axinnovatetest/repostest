using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using static Psz.Core.Purchase.Enums.StockWarningEnums;

namespace Psz.Core.Purchase.Handlers.StockWarnings
{
	public partial class PRSService
	{
		public ResponseModel<List<KeyValuePair<int, string>>> GetCountries(UserModel user)
		{
			if(user == null)
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
			try
			{
				var response = new List<KeyValuePair<int, string>>();
				var countries = Enum.GetValues(enumType: typeof(Enums.StockWarningEnums.StcoWarningCountries)).Cast<Enums.StockWarningEnums.StcoWarningCountries>().ToList();
				if(countries != null && countries.Count > 0)
					response = countries.Select(x => new KeyValuePair<int, string>((int)x, x.GetDescription())).Distinct().ToList();

				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(response);

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<KeyValuePair<int, string>>> GetUnits(UserModel user, int country)
		{
			if(user == null)
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
			try
			{
				var response = new List<KeyValuePair<int, string>>();
				var units = Enum.GetValues(enumType: typeof(Enums.StockWarningEnums.StockWarningsUnits)).Cast<Enums.StockWarningEnums.StockWarningsUnits>().ToList();
				if(units != null && units.Count > 0)
					response = units.Select(x => new KeyValuePair<int, string>((int)x, x.GetDescription())).Distinct().ToList();

				if(country == (int)Enums.StockWarningEnums.StcoWarningCountries.Tunisia)
					response = response.Where(r => r.Key == (int)StockWarningsUnits.WS_TN || r.Key == (int)StockWarningsUnits.GZ).ToList();
				if(country == (int)Enums.StockWarningEnums.StcoWarningCountries.Albania)
					response = response.Where(r => r.Key == (int)StockWarningsUnits.AL).ToList();
				if(country == (int)Enums.StockWarningEnums.StcoWarningCountries.Czech_Republic)
					response = response.Where(r => r.Key == (int)StockWarningsUnits.CZ).ToList();
				if(country == (int)Enums.StockWarningEnums.StcoWarningCountries.Germany)
					response = response.Where(r => r.Key == (int)StockWarningsUnits.DE).ToList();

				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}
