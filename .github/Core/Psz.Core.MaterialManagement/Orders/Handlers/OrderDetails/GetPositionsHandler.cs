using Psz.Core.MaterialManagement.Orders.Models.OrderDetails;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.MaterialManagement.Orders.Handlers.OrderDetails
{
	public class GetPositionsHandler: IHandle<GetRequestModel, ResponseModel<List<KeyValuePair<int, int>>>>
	{

		private int data { get; set; }
		private UserModel user { get; set; }

		public GetPositionsHandler(UserModel user, int data)
		{
			this.data = data;
			this.user = user;
		}
		public ResponseModel<List<KeyValuePair<int, int>>> Handle()
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

		private ResponseModel<List<KeyValuePair<int, int>>> Perform()
		{
			// - add next Position with -1 Id
			var response = new List<KeyValuePair<int, int>> { new KeyValuePair<int, int>(-1, Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.GetOrderNextPosition(data)) };
			var positions = Infrastructure.Data.Access.Tables.MTM.Bestellte_ArtikelAccess.GetOrderPositions(data);
			if(positions != null && positions.Count > 0)
			{
				response.AddRange(positions);
			}
			return ResponseModel<List<KeyValuePair<int, int>>>.SuccessResponse(response);
		}

		public ResponseModel<List<KeyValuePair<int, int>>> Validate()
		{
			if(user == null)
			{
				return ResponseModel<List<KeyValuePair<int, int>>>.AccessDeniedResponse();
			}

			return ResponseModel<List<KeyValuePair<int, int>>>.SuccessResponse();
		}
	}
}
