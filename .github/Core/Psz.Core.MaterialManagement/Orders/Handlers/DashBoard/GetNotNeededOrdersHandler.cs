using Psz.Core.MaterialManagement.Helpers;
using Psz.Core.MaterialManagement.Orders.Models.DashBoard;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.DashBoard
{
	public class GetNotNeededOrdersHandler: IHandle<NotNeededOrdersRequestModel, ResponseModel<IPaginatedResponseModel<NotNeededOrdersResponseModel>>>
	{
		private NotNeededOrdersRequestModel data { get; set; }
		private UserModel user { get; set; }

		public GetNotNeededOrdersHandler(UserModel user, NotNeededOrdersRequestModel data)
		{
			this.data = data;
			this.user = user;
		}
		public ResponseModel<IPaginatedResponseModel<NotNeededOrdersResponseModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				return Perform();
			} catch(Exception e)
			{
				throw;
			}
		}
		public ResponseModel<IPaginatedResponseModel<NotNeededOrdersResponseModel>> Perform()
		{
			var result = new List<Infrastructure.Data.Entities.Joins.MTM.Order.NotNeededOrdersEntity>();
			lock(CRP.Locks.OrdersReadLock)
			{
				result = Infrastructure.Data.Access.Joins.MTM.Order.NotNeededOrdersAccess.GetNotNeededOrders(SpecialHelper.GetFertigungLager(data.AreaLager), data.AreaLager, data.Artikelnummer, data.ProjectOrders, data.OnlyUnconfirmed, data.DateConfirmationBefore, data.RequestedPage, data.PageSize);
			}
			return ResponseModel<IPaginatedResponseModel<NotNeededOrdersResponseModel>>.SuccessResponse(new IPaginatedResponseModel<NotNeededOrdersResponseModel>
			{
				Items = result.Select(x => new NotNeededOrdersResponseModel(x)).ToList(),
				PageRequested = this.data.RequestedPage,
				PageSize = this.data.PageSize,
				TotalCount = result[0].TotlaCount,
				TotalPageCount = this.data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(result[0].TotlaCount > 0 ? result[0].TotlaCount : 0) / this.data.PageSize)) : 0
			});
		}
		public ResponseModel<IPaginatedResponseModel<NotNeededOrdersResponseModel>> Validate()
		{
			if(user is null)
			{
				return ResponseModel<IPaginatedResponseModel<NotNeededOrdersResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<IPaginatedResponseModel<NotNeededOrdersResponseModel>>.SuccessResponse();
		}
	}
}
