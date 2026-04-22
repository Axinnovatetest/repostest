using Psz.Core.MaterialManagement.Helpers;
using Psz.Core.MaterialManagement.Orders.Models.DashBoard;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.DashBoard
{
	public class GetNotNeededOrdersArticleHandler: IHandle<NotNeededOrdersArticleRequestModel, ResponseModel<List<NotNeededOrdersArticleResponseModel>>>
	{
		private NotNeededOrdersArticleRequestModel data { get; set; }
		private UserModel user { get; set; }

		public GetNotNeededOrdersArticleHandler(UserModel user, NotNeededOrdersArticleRequestModel data)
		{
			this.data = data;
			this.user = user;
		}
		public ResponseModel<List<NotNeededOrdersArticleResponseModel>> Handle()
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
		public ResponseModel<List<NotNeededOrdersArticleResponseModel>> Perform()
		{
			lock(CRP.Locks.OrdersArticleReadLock)
			{
				var result = Infrastructure.Data.Access.Joins.MTM.Order.NotNeededOrdersAccess.GetNotNeededOrdersArtikel(SpecialHelper.GetFertigungLager(data.Area), data.Area, data.ArtikelNr);
				return ResponseModel<List<NotNeededOrdersArticleResponseModel>>.SuccessResponse(result.Select(x => new NotNeededOrdersArticleResponseModel(x)).ToList());
			}
		}
		public ResponseModel<List<NotNeededOrdersArticleResponseModel>> Validate()
		{
			if(user is null)
			{
				return ResponseModel<List<NotNeededOrdersArticleResponseModel>>.AccessDeniedResponse();
			}
			if(data.ArtikelNr == 0)
			{
				return ResponseModel<List<NotNeededOrdersArticleResponseModel>>.FailureResponse("");

			}

			return ResponseModel<List<NotNeededOrdersArticleResponseModel>>.SuccessResponse();
		}
	}
}
