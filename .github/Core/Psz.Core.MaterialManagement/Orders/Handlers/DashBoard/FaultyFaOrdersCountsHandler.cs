using Psz.Core.MaterialManagement.Orders.Models.DashBoard;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.MaterialManagement.Orders.Handlers.DashBoard
{
	public class FaultyFaOrdersCountsHandler: IHandle<FaultyOrdersAndFasRequestModel, ResponseModel<FaultyOrdersAndFasResponseModel>>
	{
		private FaultyOrdersAndFasRequestModel data { get; set; }
		private UserModel user { get; set; }

		public FaultyFaOrdersCountsHandler(UserModel user, FaultyOrdersAndFasRequestModel data)
		{
			this.data = data;
			this.user = user;
		}
		public ResponseModel<FaultyOrdersAndFasResponseModel> Handle()
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
		private ResponseModel<FaultyOrdersAndFasResponseModel> Perform(UserModel user, FaultyOrdersAndFasRequestModel data)
		{
			var fetchedfaultyOrders = Infrastructure.Data.Access.Joins.MTM.Order.OrderedArticlesAccess.GetFaultyOrdersCount(Psz.Core.MaterialManagement.Helpers.SpecialHelper.AdditionalLagers(data.countryId, data.UnitId), data.ArtikelNr);
			var fetchedFaultedFas = Infrastructure.Data.Access.Joins.MTM.Order.ArticlesInOpenFaAccess.GetFaultyFertigungsCount(Psz.Core.MaterialManagement.Helpers.SpecialHelper.AdditionalLagers(data.countryId, data.UnitId), data.ArtikelNr);
			if(fetchedFaultedFas is null && fetchedfaultyOrders is null)
				return ResponseModel<FaultyOrdersAndFasResponseModel>.NotFoundResponse();
			var result = new FaultyOrdersAndFasResponseModel(fetchedFaultedFas, fetchedfaultyOrders) { countryId = data.countryId, UnitId = data.UnitId };

			return ResponseModel<FaultyOrdersAndFasResponseModel>.SuccessResponse(result);
		}
		public ResponseModel<FaultyOrdersAndFasResponseModel> Validate()
		{
			if(user is null)
			{
				return ResponseModel<FaultyOrdersAndFasResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<FaultyOrdersAndFasResponseModel>.SuccessResponse();
		}

	}
}
