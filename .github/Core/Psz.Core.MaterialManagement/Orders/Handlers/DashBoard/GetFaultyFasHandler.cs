using Psz.Core.MaterialManagement.Orders.Models.DashBoard;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.DashBoard
{
	public class GetFaultyFasHandler: IHandle<GetFaultyFasRequestModel, ResponseModel<IPaginatedResponseModel<GetFaultyFasResponseModel>>>
	{

		private GetFaultyFasRequestModel data { get; set; }
		private UserModel user { get; set; }

		public GetFaultyFasHandler(UserModel user, GetFaultyFasRequestModel data)
		{
			this.data = data;
			this.user = user;
		}
		public ResponseModel<IPaginatedResponseModel<GetFaultyFasResponseModel>> Handle()
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
		public ResponseModel<IPaginatedResponseModel<GetFaultyFasResponseModel>> Perform(UserModel user, GetFaultyFasRequestModel data)
		{

			var response = new IPaginatedResponseModel<GetFaultyFasResponseModel>();
			var fetchedFas = Infrastructure.Data.Access.Joins.MTM.Order.ArticlesInOpenFaAccess.GetFaultyFertigungs(Psz.Core.MaterialManagement.Helpers.SpecialHelper.AdditionalLagers(data.CountryId, data.UnitId), data.ArtikelNr, data.RequestedPage, data.PageSize);
			response.Items = new List<GetFaultyFasResponseModel>();
			int TotalCount = default;
			if(fetchedFas is null || fetchedFas.Count == 0)
				return ResponseModel<IPaginatedResponseModel<GetFaultyFasResponseModel>>.NotFoundResponse();

			foreach(var item in fetchedFas)
			{
				response.Items.Add(new GetFaultyFasResponseModel(item));
			}
			TotalCount = fetchedFas.ElementAtOrDefault(0).TotalCount;
			return ResponseModel<IPaginatedResponseModel<GetFaultyFasResponseModel>>.SuccessResponse(
				new IPaginatedResponseModel<GetFaultyFasResponseModel>
				{
					Items = response.Items,
					PageRequested = this.data.RequestedPage,
					PageSize = this.data.PageSize,
					TotalCount = TotalCount,
					TotalPageCount = this.data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(TotalCount > 0 ? TotalCount : 0) / this.data.PageSize)) : 0
				});
		}

		public ResponseModel<IPaginatedResponseModel<GetFaultyFasResponseModel>> Validate()
		{
			if(user is null)
			{
				return ResponseModel<IPaginatedResponseModel<GetFaultyFasResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<IPaginatedResponseModel<GetFaultyFasResponseModel>>.SuccessResponse();
		}
	}
}
