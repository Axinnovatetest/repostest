
namespace Psz.Core.Logistics.Handlers.Administration.AccessProfiles
{
	public class AddInventoryWarehouseHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Administration.AccessProfiles.AddInventoryWarehouseRequestModel _data { get; set; }

		public AddInventoryWarehouseHandler(Identity.Models.UserModel user, Models.Administration.AccessProfiles.AddInventoryWarehouseRequestModel data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<int> Handle()
		{
			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				botransaction.beginTransaction();
				var accessProfile = Infrastructure.Data.Access.Tables.Logistics.AccessProfileAccess.GetWithTransaction(this._data.AccessProfileId, botransaction.connection, botransaction.transaction);
				var warehouses = Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.GetWithTransaction(this._data.WarehouseIds, botransaction.connection, botransaction.transaction);
				var id = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.AccessProfileWarehouseAccess.InsertWithTransaction(
					this._data.WarehouseIds?.Select(x=> 
					new Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.AccessProfileWarehouseEntity
					{
						AccessProfileId = this._data.AccessProfileId,
						WarehouseId = x,
						LastEditTime = DateTime.Now,
						LastEditUser = this._user.Username
					}).ToList(),
					botransaction.connection,
					botransaction.transaction
				);

				// - 
				Infrastructure.Data.Access.Tables.Logistics.Logistics_LogAccess.Insert(
					new Infrastructure.Data.Entities.Tables.Logistics.Logistics_LogEntity
					{
						AngebotNr = id,
						DateTime = DateTime.Now,
						LogObject = "AccessProfileWarehouse",
						LogText = $"Warehouse [{string.Join(", ", warehouses?.Select(x => x.Lagerort))}] added to Access Profile [{accessProfile.AccessProfileName}] by User [{this._user.Username}]",
						LogType = "Info",
						Origin	= "LGT",
						ProjektNr = null,
						UserId = this._user.Id,
						Username= this._user.Username
					},
					botransaction.connection,
					botransaction.transaction
				);

				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(id);
				}
				else
				{
					return ResponseModel<int>.FailureResponse("Transaction error");
				}
			} catch(Exception e)
			{
				botransaction.rollback();
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			// - 
			var userEntity = Infrastructure.Data.Access.Tables.Logistics.AccessProfileAccess.Get(this._data.AccessProfileId);
			if(userEntity == null)
				return ResponseModel<int>.FailureResponse("Access Profile not found");

			if(Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.Get(this._data.WarehouseIds) == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Warehouse not found.");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
