using Infrastructure.Data.Access.Tables.Logistics.InventoryStock;
using Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities;
using Psz.Core.Logistics.Enums;
using Psz.Core.Logistics.Handlers.InventoryStock;
using Psz.Core.Logistics.Models.InverntoryStockModels;

namespace Psz.Core.Logistics.Handlers.InventoryStockHandlers
{
	public class GetWarehouseInventoryStatsHandler: IHandle<Identity.Models.UserModel, ResponseModel<InventoryFaStatsModel>>
	{
		private Core.Identity.Models.UserModel _user;
		private int? _data;
		public GetWarehouseInventoryStatsHandler(Core.Identity.Models.UserModel user, int? data)
		{
			_user = user;
			_data = data;
		}
		public ResponseModel<InventoryFaStatsModel> Handle()
		{
			try
			{
				var validationResponse = Validate();
				if(!validationResponse.Success)
					return validationResponse;
				// -
				return ResponseModel<InventoryFaStatsModel>.SuccessResponse(new InventoryFaStatsModel(
					Infrastructure.Data.Access.Tables.Logistics.InventoryStock.InventoryFaStatsAccess.GetByWarehouse(_data ?? -1)));

			} catch(Exception e)
			{
				throw;
			}
		}
		public ResponseModel<InventoryFaStatsModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<InventoryFaStatsModel>.AccessDeniedResponse();
			}

			return ResponseModel<InventoryFaStatsModel>.SuccessResponse();
		}
	}

}
