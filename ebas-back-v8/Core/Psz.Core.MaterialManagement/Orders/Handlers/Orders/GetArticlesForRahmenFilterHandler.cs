using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.MaterialManagement.Orders.Handlers
{
	public partial class OrderService
	{
		public ResponseModel<List<KeyValuePair<int, string>>> GetArticlesForRahmenFilter(UserModel user, string searchText)
		{
			if(user == null)
			{
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
			}

			try
			{
				var response = Infrastructure.Data.Access.Joins.MTM.Order.ArtikelFilterAccess.GetArticlesForRahmenFilter(searchText);
				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}