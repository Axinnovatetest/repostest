using Psz.Core.MaterialManagement.Orders.Models.DashBoard;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.DashBoard
{
	public class GetOrdersInTimeSpanHandler: IHandle<GetOrdersInTimeSpanRequestModel, ResponseModel<List<GetOrdersInTimeSpanResponseModel>>>
	{
		private GetOrdersInTimeSpanRequestModel data { get; set; }
		private UserModel user { get; set; }

		public GetOrdersInTimeSpanHandler(UserModel user, GetOrdersInTimeSpanRequestModel data)
		{
			this.data = data;
			this.user = user;
		}
		public ResponseModel<List<GetOrdersInTimeSpanResponseModel>> Handle()
		{
			try
			{
				var validation = Validate();
				if(!validation.Success)
				{
					return validation;
				}

				return Perform(this.user, this.data);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		private ResponseModel<List<GetOrdersInTimeSpanResponseModel>> Perform(UserModel user, GetOrdersInTimeSpanRequestModel data)
		{
			var response = new List<GetOrdersInTimeSpanResponseModel>();

			var fetchedOrders = Infrastructure.Data.Access.Joins.MTM.Order.OrderedArticlesAccess.GetOrdersInTimeSpan(Psz.Core.MaterialManagement.Helpers.SpecialHelper.AdditionalLagers(data.CountryId, data.UnitId), data.ArtikleNr, data.SpanStart, data.SpanEnd);
			if(fetchedOrders.Count() == 0 || fetchedOrders is null)
				return ResponseModel<List<GetOrdersInTimeSpanResponseModel>>.NotFoundResponse();

			foreach(var item in fetchedOrders)
			{
				response.Add(new GetOrdersInTimeSpanResponseModel(item));
			}


			return ResponseModel<List<GetOrdersInTimeSpanResponseModel>>.SuccessResponse(response);

		}
		public ResponseModel<List<GetOrdersInTimeSpanResponseModel>> Validate()
		{
			if(user is null)
			{
				return ResponseModel<List<GetOrdersInTimeSpanResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<GetOrdersInTimeSpanResponseModel>>.SuccessResponse();
		}



	}
}
