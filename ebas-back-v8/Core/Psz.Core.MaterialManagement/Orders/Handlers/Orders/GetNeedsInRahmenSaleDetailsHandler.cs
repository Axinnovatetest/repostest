using Psz.Core.MaterialManagement.Orders.Models.Orders;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers
{
	public partial class OrderService
	{
		public ResponseModel<List<NeedsInRahmenSaleModel>> GetNeedsInRahmenSaleDetails(UserModel user, int artikelNr)
		{
			if(user == null)
			{
				return ResponseModel<List<NeedsInRahmenSaleModel>>.AccessDeniedResponse();
			}

			try
			{
				var fgNeeds = Infrastructure.Data.Access.Joins.MTM.Order.DashboardAccess.GetFGThatNeedsROHArticle(artikelNr);
				var rahmenSalesNeeded = Infrastructure.Data.Access.Joins.MTM.Order.DashboardAccess.GetSaleRahmenNeeded(fgNeeds, artikelNr);

				return ResponseModel<List<NeedsInRahmenSaleModel>>.SuccessResponse(rahmenSalesNeeded?.Select(r => new NeedsInRahmenSaleModel(r)).ToList());

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}