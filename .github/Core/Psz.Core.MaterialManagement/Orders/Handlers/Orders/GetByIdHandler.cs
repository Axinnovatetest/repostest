using Infrastructure.Data.Access.Tables.MTM;
using Psz.Core.MaterialManagement.Orders.Models.Orders;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Orders
{
	public class GetByIdHandler: IHandle<GetByIdRequestModel, ResponseModel<GetByIdResponseModel>>
	{

		private GetByIdRequestModel data { get; set; }
		private UserModel user { get; set; }

		public GetByIdHandler(UserModel user, GetByIdRequestModel data)
		{
			this.data = data;
			this.user = user;
		}
		private ResponseModel<GetByIdResponseModel> Perform(UserModel user, GetByIdRequestModel data)
		{
			var order = Infrastructure.Data.Access.Joins.MTM.Order.BestellungenAccess.Get(data.Id);
			var kondition = KonditionszuordnungstabelleAccess.Get();
			var BearbeiterEntities = Infrastructure.Data.Access.Tables.COR.UserAccess.GetLegacyUsernameNotNull();
			var betreffEntities = BetreffAccess.Get();
			var orderPlacementHistrpycount = Infrastructure.Data.Access.Tables.PRS.OrderPlacementHistoryAccess.GetCountByOrderId(this.data.Id);
			var adress = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(order?.Lieferanten_Nr ?? -1);
			if(order == null)
				return ResponseModel<GetByIdResponseModel>.NotFoundResponse();

			return ResponseModel<GetByIdResponseModel>.SuccessResponse(new GetByIdResponseModel(order, BearbeiterEntities, kondition, betreffEntities, orderPlacementHistrpycount, adress));
		}

		public ResponseModel<GetByIdResponseModel> Handle()
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

		public ResponseModel<GetByIdResponseModel> Validate()
		{
			if(user == null)
			{
				return ResponseModel<GetByIdResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<GetByIdResponseModel>.SuccessResponse();
		}
	}
}
