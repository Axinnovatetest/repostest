using System;

namespace Psz.Core.FinanceControl.Handlers.Budget.Project
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class VisibilityUserDeleteHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Budget.Validator.ValidatorModel _data { get; set; }

		public VisibilityUserDeleteHandler(Identity.Models.UserModel user, Models.Budget.Validator.ValidatorModel model)
		{
			this._user = user;
			this._data = model;
		}

		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				// visibilty user
				var addId = Infrastructure.Data.Access.Tables.FNC.FinanceProjectsVisibiltyUsersAccess.DeleteUserFromProject(this._data.Id_Project ?? 0,this._data.Id_Validator);

				// - 
				return ResponseModel<int>.SuccessResponse(addId);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			var projectEntity = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(this._data.Id_Project ?? 0);
			if(projectEntity == null)
			{
				return ResponseModel<int>.FailureResponse("Project not found");
			}
			if(projectEntity.Id_Type != (int)Enums.BudgetEnums.ProjectTypes.Finance)
			{
				return ResponseModel<int>.FailureResponse("Project is not a Finance project. Cannot add visibility user");
			}
			if(projectEntity.ResponsableId != this._user.Id && this._user.IsGlobalDirector != true && this._user.SuperAdministrator != true && this._user.IsCorporateDirector != true)
			{
				return ResponseModel<int>.FailureResponse("User cannot delete visibility user");
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
