using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using Psz.Core.Purchase.Models.StockWarnings;

namespace Psz.Core.Purchase.Handlers.StockWarnings
{
	public partial class PRSService
	{
		public ResponseModel<List<ExtraOrdersNeedsInOtherPlantsModel>> GetExtraOrdersNeedsInOtherPlants(UserModel user, ExtraOrdersNeedsInOtherPlantsRequestModel data)
		{
			if(user == null)
				return ResponseModel<List<ExtraOrdersNeedsInOtherPlantsModel>>.AccessDeniedResponse();

			try
			{
				var faWarehouses = GetFertigungLager(data.Lager);
				var entities = Infrastructure.Data.Access.Joins.PRS.PRSStockWarningsAccess.GetNeedsInOtherPlantsExtraOrders(data.Lager, faWarehouses, data.ArtikelNr);

				var response = entities?.Select(e => new ExtraOrdersNeedsInOtherPlantsModel(e)).ToList();

				return ResponseModel<List<ExtraOrdersNeedsInOtherPlantsModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		private static List<int> GetFertigungLager(int hauptLager)
		{
			switch(hauptLager)
			{
				case 4:
					return new List<int>() { 7 };
				case 41:
					return new List<int>() { 42 };
				case 24:
					return new List<int>() { 26 };
				case 3:
					return new List<int>() { 6, 21 };
				case 58:
					return new List<int>() { 60 };
				default:
					return new List<int>() { 0 };
			}
		}
	}
}