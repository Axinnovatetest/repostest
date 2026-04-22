
using Infrastructure.Data.Entities.Tables.Logistics.InventroyStock;
using Psz.Core.Identity.Models;

namespace Psz.Core.Logistics.Handlers.InventoryStock
{
	public class HqUpdateNotesHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private readonly UserModel _user;
		private readonly Models.InverntoryStockModels.UpdateNotesRequestModel _data;
		public HqUpdateNotesHandler(UserModel user, Models.InverntoryStockModels.UpdateNotesRequestModel data)
		{
			_user = user;
			_data = data;
		}

		public ResponseModel<int> Handle()
		{
			var validationResponse = Validate();
			if(!validationResponse.Success)
				return validationResponse;

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				botransaction.beginTransaction();

				var id = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.InventoryFaStatsAccess.UpdateHQNotesInventory(_data.WarehouseId, _data.RemarksHL, _data.RemarksPL, botransaction.connection, botransaction.transaction);

				#region add logs
				var InsertLogsResult = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.LogsAccess.InsertWithTransaction(new LogsEntity
				{
					LogTime = DateTime.Now,
					LogUserId = _user.Id,
					ObjectId = _data.WarehouseId,
					ObjectName = "InventoryStock",
					LogDescription = $"The inventory stock [HQ] notes have been updated in Lager [{_data.WarehouseId}] at [{DateTime.Now:yyyy-MM-dd HH:mm}] by [{_user.Name}]",
					LogsType = 2,
					LogUserName = _user.Name,
					LagerId = _data.WarehouseId
				}, botransaction.connection, botransaction.transaction);
				#endregion add logs

				if(botransaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(id);
				}
				else
				{
					return ResponseModel<int>.FailureResponse("Transaction error");
				}
			} catch
			{
				botransaction.rollback();
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(_user == null || (!_user.SuperAdministrator && !_user.IsGlobalDirector && !_user.Access.Logistics.InventoryWarehouseValidate))
				return ResponseModel<int>.AccessDeniedResponse();

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
