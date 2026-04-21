using System.Linq;
using static Psz.Core.MaterialManagement.Orders.Models.OrderValidation.ClientModel;

namespace Psz.Core.MaterialManagement.Orders.Handlers.OrderValidation
{
	public class GetClientHandler
	{

		private ClientRequestModel data { get; set; }
		private UserModel user { get; set; }

		public GetClientHandler(UserModel user, ClientRequestModel data)
		{
			this.data = data;
			this.user = user;
		}

		public ResponseModel<List<ClientResponseModel>> Handle()
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

		private ResponseModel<List<ClientResponseModel>> Perform(UserModel user, ClientRequestModel data)
		{
			try
			{
				var ClientList = Infrastructure.Data.Access.Tables.MTM.PSZ_MandantenAccess.Get();

				return ResponseModel<List<ClientResponseModel>>.SuccessResponse(ClientList?.Select(x => new ClientResponseModel(x))?.ToList());


			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}



		public ResponseModel<List<ClientResponseModel>> Validate()
		{
			if(user == null)
			{
				return ResponseModel<List<ClientResponseModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<ClientResponseModel>>.SuccessResponse();
		}
	}
}
