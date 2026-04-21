using System;

namespace Psz.Core.FinanceControl.Handlers.Budget
{
	public class DeleteProjectHandler: IHandle<Models.Budget.GetProjectsModel, ResponseModel<int>>
	{
		private int _ProjectID { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public DeleteProjectHandler(int projectid, Identity.Models.UserModel user)
		{
			this._ProjectID = projectid;
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

				var projectentity = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(_ProjectID);
				if(projectentity == null)
				{
					return ResponseModel<int>.SuccessResponse();
				}


				projectentity.Deleted = true;
				projectentity.DeleteTime = DateTime.Now;
				projectentity.DeleteUserId = this._user.Id;

				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Update(projectentity));
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null
				|| !this._user.Access.Financial.Budget.ConfigDeleteArtikel)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
