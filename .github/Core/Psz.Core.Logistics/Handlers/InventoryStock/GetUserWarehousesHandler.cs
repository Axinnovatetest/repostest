namespace Psz.Core.Logistics.Handlers.InventoryStockHandlers
{
	public class GetUserWarehousesHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<KeyValuePair<int, string>>>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public GetUserWarehousesHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<KeyValuePair<int, string>>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var mainWarehouseIds = Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.GetProductionLagers(Module.BSD.ProductionLagerIds)?.Select(x=> x.Lagerort_id)?.ToList();
				if(!_user.IsAdministrator && !_user.IsGlobalDirector)
				{
					var userAccessProfiles = Infrastructure.Data.Access.Tables.Logistics.AccessProfileUsersAccess.GetByUserId(_user.Id);
					// - filter mainWarehouseIds by user access profiles
					var lagersEntities = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.AccessProfileWarehouseAccess.GetByAccessProfiles(userAccessProfiles?.Select(x=> x.AccessProfileId));
					mainWarehouseIds = mainWarehouseIds.Where(x => lagersEntities.Exists(y=> y.WarehouseId == x)).ToList();
				}

				// - 
				return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(
					Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.Get(mainWarehouseIds)
					?.Select(x=> new KeyValuePair<int, string>(x.Lagerort_id, x.Lagerort))
					?.ToList());
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<List<KeyValuePair<int, string>>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
			}

			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
		}
	}
}
