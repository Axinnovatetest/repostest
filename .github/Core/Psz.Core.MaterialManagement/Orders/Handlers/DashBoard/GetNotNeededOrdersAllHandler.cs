using Psz.Core.MaterialManagement.Helpers;
using Psz.Core.MaterialManagement.Orders.Models.DashBoard;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.DashBoard
{
	public class GetNotNeededOrdersAllHandler: IHandle<NotNeededOrdersAllRequestModel, ResponseModel<List<NotNeededOrdersAllResponseModel>>>
	{
		private NotNeededOrdersAllRequestModel data { get; set; }
		private UserModel user { get; set; }

		public GetNotNeededOrdersAllHandler(UserModel user, NotNeededOrdersAllRequestModel data)
		{
			this.data = data;
			this.user = user;
		}
		public ResponseModel<List<NotNeededOrdersAllResponseModel>> Handle()
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
		public ResponseModel<List<NotNeededOrdersAllResponseModel>> Perform()
		{
			var result = new List<Infrastructure.Data.Entities.Joins.MTM.Order.NotNeededOrdersAllEntity>();
			lock(CRP.Locks.OrdersAllReadLock)
			{
				result = Infrastructure.Data.Access.Joins.MTM.Order.NotNeededOrdersAccess.GetNotNeededOrdersAll(SpecialHelper.GetFertigungLager(data.Area), data.Area);
			}

			return ResponseModel<List<NotNeededOrdersAllResponseModel>>.SuccessResponse(result.Select(x => new NotNeededOrdersAllResponseModel(x)).ToList());
		}
		public ResponseModel<List<NotNeededOrdersAllResponseModel>> Validate()
		{
			if(user is null)
			{
				return ResponseModel<List<NotNeededOrdersAllResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<NotNeededOrdersAllResponseModel>>.SuccessResponse();
		}
	}
}
