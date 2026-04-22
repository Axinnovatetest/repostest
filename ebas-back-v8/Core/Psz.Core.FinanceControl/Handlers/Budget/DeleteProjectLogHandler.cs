using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class DeleteProjectLogHandler: IHandle<Models.Budget.GetProjects_LogModel, ResponseModel<int>>
	{

		private Models.Budget.GetProjects_LogModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public DeleteProjectLogHandler(Models.Budget.GetProjects_LogModel data, Identity.Models.UserModel user)
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

				// -
				if(Infrastructure.Data.Access.Tables.FNC.Log_State_BudgetAccess.Insert(new Infrastructure.Data.Entities.Tables.FNC.Log_State_BudgetEntity
				{
					Id_proj = this._data.Id_proj,
					Action_date = DateTime.Now,
					Id_LS = 0,
					Id_state = _data.Id_State,
					Id_user = _user.Id
				}) > 0)
				{
					//-
					return ResponseModel<int>.SuccessResponse(-1/*Infrastructure.Data.Access.Tables.FNC.Project_BudgetAccess.Delete(this._data.Id_proj)*/);
				}

				return ResponseModel<int>.SuccessResponse(-1);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null /*|| !this._user.Access.Budget.ConfigDeleteArtikel*/)
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
