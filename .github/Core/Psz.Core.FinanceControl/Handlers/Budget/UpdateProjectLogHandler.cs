using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class UpdateProjectLogHandler: IHandle<Models.Budget.GetProjects_LogModel, ResponseModel<int>>
	{
		private Models.Budget.GetProjects_LogModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public UpdateProjectLogHandler(Models.Budget.GetProjects_LogModel data, Identity.Models.UserModel user)
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

				var Projectentity = new Infrastructure.Data.Entities.Tables.FNC.Project_Log_BudgetEntity()
				{
					Id_proj = _data.Id_proj,
					Name_proj = _data.Name_proj,
					Id_Customer = _data.Id_Customer,
					Nr_Customer = _data.Nr_Customer,
					Proj_Budget = _data.Proj_Budget,
					Id_Responsable = _data.Id_Responsable,
					Id_State = _data.Id_State,
					Id_Currency = _data.Id_Currency,
					Id_Land = _data.Id_Land,
					Id_Dept = _data.Id_Dept,
					Id_Type = _data.Id_Type,
					Description = _data.Description,
					Action_date = _data.Action_date,
					Id_LS = _data.Id_LS,
					Id_state = _data.Id_State,
					Id_user = _user.Id,

				};
				//var updatedProject = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.UpdateProjectLog(Projectentity);
				return ResponseModel<int>.SuccessResponse(/*updatedProject*/0);
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
