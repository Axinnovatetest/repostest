using Psz.Core.Logistics.Models.InverntoryStockModels;
using Psz.Core.MaterialManagement.Settings;
using static Psz.Core.CustomerService.Enums.InsideSalesEnums;

namespace Psz.Core.Logistics.Handlers.InventoryStock
{
	public class StartInventoryHandler
	{
		private UploadChoiceModel _data { get; }
		private Identity.Models.UserModel _user { get; }
		List<string> warnings = new List<string>();

		public StartInventoryHandler(Identity.Models.UserModel user, UploadChoiceModel? data)
		{
			_user = user;
			_data = data;
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

				// -
				int lagerId = this._data.LagerId ?? -1;
				var prodWarehouseIds = Psz.Core.Logistics.Helpers.LagerHelperHandler.GetProductionWarehouseIds(lagerId);
				switch(_data.Choice)
				{
					case 0:
						var warehouseIds = new List<int> { lagerId };
						warehouseIds.AddRange(prodWarehouseIds);
						Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.SetInventoryStatus(warehouseIds, true, transaction.connection, transaction.transaction);
						Infrastructure.Data.Access.Joins.Logistics.InventoryAccess.InitializeInventory(LagerHelperHandler.GetWarehouseIds(lagerId), prodWarehouseIds, transaction.connection, transaction.transaction);
						Infrastructure.Data.Access.Tables.Logistics.InventoryStock.InventoryFaStatsAccess.Initialize(_user.Username, LagerHelperHandler.GetWarehouseIds(lagerId), prodWarehouseIds, transaction.connection, transaction.transaction);
						var resultOne = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.ReportOneTblAccess.InitOpenFaReport(LagerHelperHandler.GetWarehouseIds(lagerId), transaction.connection, transaction.transaction);
						var resultTwo = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.ReportTwoTblAccess.InitStartedFaReport(LagerHelperHandler.GetWarehouseIds(lagerId), prodWarehouseIds, transaction.connection, transaction.transaction);
						Infrastructure.Data.Access.Tables.Logistics.InventoryStock.TaskByRoleAccess.InitITTask(lagerId, transaction.connection, transaction.transaction);
						break;
					default:
						break;
				}

				#region add logs
				var logs = Psz.Core.Logistics.Helpers.InventoryStockLogHelper.GenerateLogForStartInventory(_user, lagerId);
				var InsertLogsResult = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.LogsAccess.InsertWithTransaction(logs, transaction.connection, transaction.transaction);
				#endregion add logs

				return transaction.commit()
					? ResponseModel<int>.SuccessResponse(1,warnings)
					: ResponseModel<int>.FailureResponse(key: "1", value: "Transaction error");
			} catch(Exception e)
			{
				transaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(_user == null /*|| _user.Access.____*/)
				return ResponseModel<int>.AccessDeniedResponse();

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
