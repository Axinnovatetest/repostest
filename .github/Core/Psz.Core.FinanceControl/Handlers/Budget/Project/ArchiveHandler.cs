using System;

namespace Psz.Core.FinanceControl.Handlers.Budget.Project
{
	using Psz.Core.Common.Models;
	using Psz.Core.SharedKernel.Interfaces;

	public class ArchiveHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public ArchiveHandler(Identity.Models.UserModel user, int id)
		{
			this._user = user;
			this._data = id;
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

				var projectEntity = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(this._data);

				/// archive orders
				var orderEntites = Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.GetByProject(projectEntity.Id);
				if(orderEntites != null && orderEntites.Count > 0)
				{
					foreach(var orderEntity in orderEntites)
					{
						orderEntity.Archived = true;
						orderEntity.ArchiveTime = DateTime.Now;
						orderEntity.ArchiveUserId = this._user.Id;
						Infrastructure.Data.Access.Tables.FNC.BestellungenExtensionAccess.Update(orderEntity);
					}

				}

				/// archive project
				projectEntity.Archived = true;
				projectEntity.ArchiveTime = DateTime.Now;
				projectEntity.ArchiveUserId = this._user.Id;

				// - History
				Helpers.Processings.Budget.Project.SaveProjectHistory(projectEntity, Enums.BudgetEnums.ProjectWorkflowActions.Archive, this._user);
				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Update(projectEntity));


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

			var projectEntity = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(this._data);
			if(projectEntity == null)
				return ResponseModel<int>.FailureResponse("Project not found");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
