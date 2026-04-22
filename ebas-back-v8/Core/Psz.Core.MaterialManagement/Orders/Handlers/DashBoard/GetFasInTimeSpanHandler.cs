using Psz.Core.MaterialManagement.Orders.Models.DashBoard;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.DashBoard
{
	public class GetFasInTimeSpanHandler: IHandle<GetFasInTimeSpanRequestModel, ResponseModel<List<GetFasInTimeSpanResponseModel>>>
	{
		private GetFasInTimeSpanRequestModel data { get; set; }
		private UserModel user { get; set; }

		public GetFasInTimeSpanHandler(UserModel user, GetFasInTimeSpanRequestModel data)
		{
			this.data = data;
			this.user = user;
		}
		public ResponseModel<List<GetFasInTimeSpanResponseModel>> Handle()
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
		private ResponseModel<List<GetFasInTimeSpanResponseModel>> Perform(UserModel user, GetFasInTimeSpanRequestModel data)
		{
			var response = new List<GetFasInTimeSpanResponseModel>();

			var fetchedOrders = Infrastructure.Data.Access.Joins.MTM.Order.NeededQuantityInFasAccess.GetFasInTimeSpan(Psz.Core.MaterialManagement.Helpers.SpecialHelper.AdditionalLagers(data.CountryId, data.UnitId), data.ArtikleNr, data.SpanStart, data.SpanEnd);
			if(fetchedOrders.Count() == 0 || fetchedOrders is null)
				return ResponseModel<List<GetFasInTimeSpanResponseModel>>.NotFoundResponse();

			foreach(var item in fetchedOrders)
			{
				response.Add(new GetFasInTimeSpanResponseModel(item));
			}


			return ResponseModel<List<GetFasInTimeSpanResponseModel>>.SuccessResponse(response);

		}
		public ResponseModel<List<GetFasInTimeSpanResponseModel>> Validate()
		{
			if(user is null)
			{
				return ResponseModel<List<GetFasInTimeSpanResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<GetFasInTimeSpanResponseModel>>.SuccessResponse();
		}



	}
}
