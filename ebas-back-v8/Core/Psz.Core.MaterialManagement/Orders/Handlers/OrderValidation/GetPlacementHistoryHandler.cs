using Psz.Core.MaterialManagement.Orders.Models.OrderValidation;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.OrderValidation
{
	public class GetPlacementHistoryHandler: IHandle<int, ResponseModel<List<GetPlacementHistoryResponseModel>>>
	{

		private int data { get; set; }
		private UserModel user { get; set; }

		public GetPlacementHistoryHandler(UserModel user, int data)
		{
			this.data = data;
			this.user = user;
		}

		public ResponseModel<List<GetPlacementHistoryResponseModel>> Handle()
		{

			try
			{
				var validation = Validate();
				if(!validation.Success)
				{
					return validation;
				}

				return Perform();
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		private ResponseModel<List<GetPlacementHistoryResponseModel>> Perform()
		{
			try
			{
				var placementHistory = Infrastructure.Data.Access.Tables.PRS.OrderPlacementHistoryAccess.GetByOrderId(this.data);

				return ResponseModel<List<GetPlacementHistoryResponseModel>>.SuccessResponse(placementHistory.Select(x => new GetPlacementHistoryResponseModel(x)).ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<List<GetPlacementHistoryResponseModel>> Validate()
		{
			if(user == null)
			{
				return ResponseModel<List<GetPlacementHistoryResponseModel>>.AccessDeniedResponse();
			}
			var bestellungen = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.Get(this.data);
			if(bestellungen is null)
				return ResponseModel<List<GetPlacementHistoryResponseModel>>.FailureResponse("Order doesn't exist");



			return ResponseModel<List<GetPlacementHistoryResponseModel>>.SuccessResponse();
		}
	}
}