using Psz.Core.MaterialManagement.Orders.Models.Orders;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers
{
	public partial class OrderService
	{
		public ResponseModel<ROHArticleRahmenNeedsResponseModel> GetROHRahmenNeeds(UserModel user, int artikelNr)
		{
			if(user == null)
			{
				return ResponseModel<ROHArticleRahmenNeedsResponseModel>.AccessDeniedResponse();
			}

			try
			{
				var fgNeeds = Infrastructure.Data.Access.Joins.MTM.Order.DashboardAccess.GetFGThatNeedsROHArticle(artikelNr);
				var needsPurchase = Infrastructure.Data.Access.Joins.MTM.Order.DashboardAccess.GetRahmenPurchaseNeeds(artikelNr);
				var needsSale = Infrastructure.Data.Access.Joins.MTM.Order.DashboardAccess.GetRahmenSaleNeeds(fgNeeds, artikelNr);
				//var rahmenSalesNeeded = Infrastructure.Data.Access.Joins.MTM.Order.DashboardAccess.GetSaleRahmenNeeded(fgNeeds, artikelNr);

				return ResponseModel<ROHArticleRahmenNeedsResponseModel>.SuccessResponse(new ROHArticleRahmenNeedsResponseModel
				{
					NeedsRahmenPurchase = needsPurchase,
					NeedsRahmenSale = needsSale,
				});
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}