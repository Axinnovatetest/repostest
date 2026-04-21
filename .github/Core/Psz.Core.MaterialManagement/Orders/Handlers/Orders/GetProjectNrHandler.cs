using Psz.Core.MaterialManagement.Orders.Models.Orders;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Orders
{
	public class GetProjectNrHandler: IHandle<GetOrderNrRequestModel, ResponseModel<List<GetOrderNrReponseModel>>>
	{
		private GetOrderNrRequestModel data { get; set; }
		private UserModel user { get; set; }

		public GetProjectNrHandler(UserModel user, GetOrderNrRequestModel data)
		{
			this.data = data;
			this.user = user;
		}
		public ResponseModel<List<GetOrderNrReponseModel>> Handle()
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

		private ResponseModel<List<GetOrderNrReponseModel>> Perform(UserModel user, GetOrderNrRequestModel data)
		{
			var orderNumbers = Infrastructure.Data.Access.Tables.MTM.BestellungenAccess.GetFiltered(data.Filter, false);

			return ResponseModel<List<GetOrderNrReponseModel>>.SuccessResponse(orderNumbers?.Select(x => new GetOrderNrReponseModel(x))?.ToList());
		}

		public ResponseModel<List<GetOrderNrReponseModel>> Validate()
		{
			if(user == null)
			{
				return ResponseModel<List<GetOrderNrReponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<GetOrderNrReponseModel>>.SuccessResponse();
		}
	}
}
