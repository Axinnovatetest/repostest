using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Statistics.Suppliers
{
	public class RefreshOverviewHandler: IHandle<Identity.Models.UserModel, ResponseModel<KeyValuePair<int, DateTime>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public RefreshOverviewHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}

		public ResponseModel<KeyValuePair<int, DateTime>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// -
				return ResponseModel<KeyValuePair<int, DateTime>>.SuccessResponse(
					Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.SupplierOverview_Sync());

			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<KeyValuePair<int, DateTime>> Validate()
		{
			if(this._user is null/*|| this._user.Access.____*/)
			{
				return ResponseModel<KeyValuePair<int, DateTime>>.AccessDeniedResponse();
			}

			return ResponseModel<KeyValuePair<int, DateTime>>.SuccessResponse();
		}
	}
}
