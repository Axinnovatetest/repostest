using Psz.Core.MaterialManagement.Helpers;
using Psz.Core.MaterialManagement.Orders.Models.DashBoard;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.DashBoard
{
	public class GetNotNeededOrderDetailsAllHandler: IHandle<NotNeededOrderDetailsAllRequestModel, ResponseModel<IPaginatedResponseModel<NotNeededOrderDetailsAllResponseModel>>>
	{

		private NotNeededOrderDetailsAllRequestModel data { get; set; }
		private UserModel user { get; set; }

		public GetNotNeededOrderDetailsAllHandler(UserModel user, NotNeededOrderDetailsAllRequestModel data)
		{
			this.data = data;
			this.user = user;
		}

		public ResponseModel<IPaginatedResponseModel<NotNeededOrderDetailsAllResponseModel>> Handle()
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
		public ResponseModel<IPaginatedResponseModel<NotNeededOrderDetailsAllResponseModel>> Perform()
		{
			var result = new List<Infrastructure.Data.Entities.Joins.MTM.Order.NotNeededOrderDetailsAllEntity>();
			var startDate = SpecialHelper.FirstDateOfWeekISO8601(data.Year, data.Week);
			var endDate = startDate.AddDays(6);
			lock(CRP.Locks.OrdersReadLock)
			{
				result = Infrastructure.Data.Access.Joins.MTM.Order.NotNeededOrdersAccess.GetNotNeededOrdersDetailsAll(SpecialHelper.GetFertigungLager(data.AreaLager), data.AreaLager, startDate, endDate, data.PageSize, data.RequestedPage);
			}
			return ResponseModel<IPaginatedResponseModel<NotNeededOrderDetailsAllResponseModel>>.SuccessResponse(new IPaginatedResponseModel<NotNeededOrderDetailsAllResponseModel>
			{
				Items = result.Select(x => new NotNeededOrderDetailsAllResponseModel(x)).ToList(),
				PageRequested = this.data.RequestedPage,
				PageSize = this.data.PageSize,
				TotalCount = result[0].TotlaCount,
				TotalPageCount = this.data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(result[0].TotlaCount > 0 ? result[0].TotlaCount : 0) / this.data.PageSize)) : 0
			});
		}


		public ResponseModel<IPaginatedResponseModel<NotNeededOrderDetailsAllResponseModel>> Validate()
		{
			if(this.user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<IPaginatedResponseModel<NotNeededOrderDetailsAllResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<IPaginatedResponseModel<NotNeededOrderDetailsAllResponseModel>>.SuccessResponse();
		}
	}
}
