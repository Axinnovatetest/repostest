namespace Psz.Core.Logistics.Handlers.InventoryStock
{
		public class TaskByRoleHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Psz.Core.Logistics.Models.InverntoryStockModels.UpdateTaskRequestModel>>>
		{

			private Core.Identity.Models.UserModel _user;
			private int _lagerId;
			public TaskByRoleHandler(Core.Identity.Models.UserModel user,int lagerId)
			{
				_user = user;
				_lagerId = lagerId;
			}
			public ResponseModel<List<Psz.Core.Logistics.Models.InverntoryStockModels.UpdateTaskRequestModel>> Handle()
			{
				try
				{
					var validationResponse = Validate();
					if(!validationResponse.Success)
						return validationResponse;


					var ListTaskByRole = Infrastructure.Data.Access.Tables.Logistics.InventoryStock.TaskByRoleAccess.GetWithLager(_lagerId);

				   var model = ListTaskByRole.Select(x => new Psz.Core.Logistics.Models.InverntoryStockModels.UpdateTaskRequestModel(x)).ToList();



				return ResponseModel<List<Psz.Core.Logistics.Models.InverntoryStockModels.UpdateTaskRequestModel>>.SuccessResponse(model);

				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
					throw;
				}
			}
			public ResponseModel<List<Psz.Core.Logistics.Models.InverntoryStockModels.UpdateTaskRequestModel>> Validate()
			{
				if(this._user == null/*|| this._user.Access.____*/)
				{
					return ResponseModel<List<Psz.Core.Logistics.Models.InverntoryStockModels.UpdateTaskRequestModel>>.AccessDeniedResponse();
				}

				return ResponseModel<List<Psz.Core.Logistics.Models.InverntoryStockModels.UpdateTaskRequestModel>>.SuccessResponse();
			}
		}
	}
