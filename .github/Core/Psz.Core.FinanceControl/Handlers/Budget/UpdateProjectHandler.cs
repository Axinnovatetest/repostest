using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class UpdateProjectHandler: IHandle<Models.Budget.GetProjectsModel, ResponseModel<int>>
	{
		private Models.Budget.GetProjectsModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public UpdateProjectHandler(Models.Budget.GetProjectsModel data, Identity.Models.UserModel user)
		{
			this._data = data;
			this._user = user;
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

				var Projectentity = new Infrastructure.Data.Entities.Tables.FNC.BudgetProjectEntity()
				{
					Id = _data.Id_proj,
					ProjectName = _data.Name_proj,
					CustomerId = _data.Id_Customer,
					CustomerNr = _data.Nr_Customer,
					ProjectBudget = _data.Proj_Budget,
					ResponsableId = _data.Id_Responsable,
					Id_State = _data.Id_State,
					CurrencyId = _data.Id_Currency,
					CompanyId = _data.Id_Land,
					DepartmentId = _data.Id_Dept,
					Id_Type = _data.Id_Type,
					Description = _data.Description,

				};
				var updatedProject = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Update(Projectentity);
				return ResponseModel<int>.SuccessResponse(updatedProject);
			} catch(Exception e)
			{

				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null
				|| !this._user.Access.Financial.Budget.ConfigEditArtikel)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			var ProjID = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(this._data.Id_proj);
			var errors = new List<ResponseModel<int>.ResponseError>();
			if(ProjID == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "", Value = "Project not found"}
					}
				};
			}
			if(errors.Count > 0)
			{
				return new ResponseModel<int>() { Errors = errors };
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
