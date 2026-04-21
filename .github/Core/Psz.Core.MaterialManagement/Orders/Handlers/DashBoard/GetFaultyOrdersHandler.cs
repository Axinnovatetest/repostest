using Psz.Core.MaterialManagement.Orders.Models.DashBoard;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.DashBoard
{
	public class GetFaultyOrdersHandler: IHandle<FaultyOrdersRequestModel, ResponseModel<IPaginatedResponseModel<FaultyOrdersResponseModel>>>
	{

		private FaultyOrdersRequestModel data { get; set; }
		private UserModel user { get; set; }

		public GetFaultyOrdersHandler(UserModel user, FaultyOrdersRequestModel data)
		{
			this.data = data;
			this.user = user;
		}
		public ResponseModel<IPaginatedResponseModel<FaultyOrdersResponseModel>> Handle()
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
		public ResponseModel<IPaginatedResponseModel<FaultyOrdersResponseModel>> Perform(UserModel user, FaultyOrdersRequestModel data)
		{

			var response = new IPaginatedResponseModel<FaultyOrdersResponseModel>();
			var fetchedOrders = Infrastructure.Data.Access.Joins.MTM.Order.OrderedArticlesAccess.GetFaultyOrders(Psz.Core.MaterialManagement.Helpers.SpecialHelper.AdditionalLagers(data.CountryId, data.UnitId), data.ArtikelNr, data.RequestedPage, data.PageSize);
			response.Items = new List<FaultyOrdersResponseModel>();
			int TotalCount = default;
			if(fetchedOrders is null || fetchedOrders.Count == 0)
				return ResponseModel<IPaginatedResponseModel<FaultyOrdersResponseModel>>.NotFoundResponse();

			foreach(var item in fetchedOrders)
			{
				response.Items.Add(new FaultyOrdersResponseModel(item));
			}
			TotalCount = fetchedOrders.ElementAtOrDefault(0).TotalCount;
			return ResponseModel<IPaginatedResponseModel<FaultyOrdersResponseModel>>.SuccessResponse(
				new IPaginatedResponseModel<FaultyOrdersResponseModel>
				{
					Items = response.Items,
					PageRequested = this.data.RequestedPage,
					PageSize = this.data.PageSize,
					TotalCount = TotalCount,
					TotalPageCount = this.data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(TotalCount > 0 ? TotalCount : 0) / this.data.PageSize)) : 0
				});
		}

		public ResponseModel<IPaginatedResponseModel<FaultyOrdersResponseModel>> Validate()
		{
			if(user is null)
			{
				return ResponseModel<IPaginatedResponseModel<FaultyOrdersResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<IPaginatedResponseModel<FaultyOrdersResponseModel>>.SuccessResponse();
		}
	}
}
