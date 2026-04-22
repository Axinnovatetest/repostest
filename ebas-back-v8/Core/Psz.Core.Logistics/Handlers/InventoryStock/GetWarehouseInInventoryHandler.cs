using Psz.Core.Logistics.Models.InverntoryStockModels;

namespace Psz.Core.Logistics.Handlers.InventoryStock
{
	public class GetWarehouseInInventoryHandler: IHandle<Identity.Models.UserModel, ResponseModel<IEnumerable<int>>>
	{
		private Core.Identity.Models.UserModel _user;
		public GetWarehouseInInventoryHandler(Core.Identity.Models.UserModel user)
		{
			_user = user;
		}
		public ResponseModel<IEnumerable<int>> Handle()
		{
			try
			{
				var validationResponse = Validate();
				if(!validationResponse.Success)
					return validationResponse;

				var data = Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.GetInInventory();

				return ResponseModel<IEnumerable<int>>.SuccessResponse(data);

			} catch(Exception e)
			{
				throw e;
			}
		}
		public ResponseModel<IEnumerable<int>> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<IEnumerable<int>>.AccessDeniedResponse();
			}

			return ResponseModel<IEnumerable<int>>.SuccessResponse();
		}
	}
}