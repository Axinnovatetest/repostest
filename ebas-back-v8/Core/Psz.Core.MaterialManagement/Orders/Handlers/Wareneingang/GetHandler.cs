using Infrastructure.Data.Access.Joins.MTM.Order;
using Psz.Core.MaterialManagement.Orders.Models.Wareneingang;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Wareneingang
{
	public class GetHandler: IHandle<GetRequestModel, ResponseModel<List<GetResponseModel>>>
	{

		private GetRequestModel data { get; set; }
		private UserModel user { get; set; }

		public GetHandler(UserModel user, GetRequestModel data)
		{
			this.data = data;
			this.user = user;
		}
		public ResponseModel<List<GetResponseModel>> Handle()
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

		private ResponseModel<List<GetResponseModel>> Perform(UserModel user, GetRequestModel data)
		{

			var WareneingangData = WareneingangAccess.GetWareneingangEntities(data.Bestellung_Nr);

			return ResponseModel<List<GetResponseModel>>.SuccessResponse(WareneingangData.Select(x => new GetResponseModel(x)).ToList());
		}

		public ResponseModel<List<GetResponseModel>> Validate()
		{
			if(user == null)
			{
				return ResponseModel<List<GetResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<GetResponseModel>>.SuccessResponse();
		}
	}
}
