using Geocoding.Microsoft.Json;
using Infrastructure.Data.Entities.Tables.Logistics.InventroyStock;

namespace Psz.Core.Logistics.Handlers.InventoryStock
{
	public class StopInventoryHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{

		private Core.Identity.Models.UserModel _user;
		private int? _lagerId;
		public StopInventoryHandler(Core.Identity.Models.UserModel user, int? lagerId)
		{
			_user = user;
			_lagerId = lagerId;
		}
		public ResponseModel<int> Handle()
		{
			var transaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				var validationResponse = Validate();
				if(!validationResponse.Success)
					return validationResponse;


				transaction.beginTransaction();
				int lagerId = this._lagerId ?? -1;
				var prodWarehouseIds = Psz.Core.Logistics.Helpers.LagerHelperHandler.GetProductionWarehouseIds(lagerId);
				var warehouseIds = new List<int> { lagerId };
				warehouseIds.AddRange(prodWarehouseIds);

				Infrastructure.Data.Access.Tables.Logistics.InventoryStock.InventoryFaStatsAccess.CloseInventory(lagerId, _user.Username, transaction.connection, transaction.transaction);
				Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.SetInventoryStatus(warehouseIds, false, transaction.connection, transaction.transaction);
				
				#region add logs
				var InsertLogsResult = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.LogsAccess.InsertWithTransaction(new LogsEntity
				{
					LogTime = DateTime.Now,
					LogUserId = _user.Id,
					ObjectId = lagerId,
					ObjectName = "InventoryStock",
					LogDescription = $"The inventory stock has been stopped in Lager [{lagerId}] at [{DateTime.Now:yyyy-MM-dd HH:mm}] by [{_user.Name}]",
					LogsType = 2,
					LogUserName = _user.Name,
					LagerId = lagerId
				}, transaction.connection, transaction.transaction);
				#endregion add logs

				return transaction.commit()
					? ResponseModel<int>.SuccessResponse(1)
					: ResponseModel<int>.FailureResponse(key: "1", value: "Transaction error");

			} catch(Exception e)
			{
				transaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}

}
