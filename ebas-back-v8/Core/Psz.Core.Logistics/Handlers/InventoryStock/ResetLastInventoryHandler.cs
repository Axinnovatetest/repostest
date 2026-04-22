
using Psz.Core.Identity.Models;

namespace Psz.Core.Logistics.Handlers.InventoryStock
{
	public class ResetLastInventoryHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private readonly UserModel _user;
		private readonly int _data;
		public ResetLastInventoryHandler(UserModel user, int data)
		{
			_user = user;
			_data = data;
		}

		public ResponseModel<int> Handle()
		{
			var validationResponse = Validate();
			if(!validationResponse.Success)
				return validationResponse;

			try
			{
				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Joins.Logistics.InventoryAccess.ResetLastInventory(_data));
			} catch(Exception ex)
			{
				Infrastructure.Services.Logging.Logger.Log(ex);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(_user == null || _user.SuperAdministrator != true)
				return ResponseModel<int>.AccessDeniedResponse();

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
