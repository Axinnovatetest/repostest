using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.Purchase.Models.StockWarnings;
using static Psz.Core.Purchase.Enums.StockWarningEnums;

namespace Psz.Core.Purchase.Handlers.StockWarnings
{
	public partial class PRSService
	{
		public ResponseModel<List<StcoWarningsNeedsInOtherPlantsModel>> GetNeedsInOtherPlants(UserModel user, ArtikelUnitRequestModel data)
		{
			if(user == null)
				return ResponseModel<List<StcoWarningsNeedsInOtherPlantsModel>>.AccessDeniedResponse();

			try
			{
				var unit = ((StockWarningsUnits)data.Unit).GetDescription();

				var needs = Infrastructure.Data.Access.Joins.PRS.PRSStockWarningsAccess.GetNeedsInOtherPlants(unit, data.ArtikelNr);
				var response = needs?.Select(n => new StcoWarningsNeedsInOtherPlantsModel(n)).ToList();

				return ResponseModel<List<StcoWarningsNeedsInOtherPlantsModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}