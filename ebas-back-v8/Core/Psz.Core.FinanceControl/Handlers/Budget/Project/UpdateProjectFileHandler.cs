using System;

namespace Psz.Core.FinanceControl.Handlers.Budget.Project
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	using System.Threading.Tasks;

	public class UpdateProjectFileHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Budget.Project.ProjectFileModel _data { get; set; }

		public UpdateProjectFileHandler(Identity.Models.UserModel user, Models.Budget.Project.ProjectFileModel model)
		{
			this._user = user;
			this._data = model;
		}

		public async Task<ResponseModel<int>> Handleasync()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var insertedId = 0;
				var fileEntity = Infrastructure.Data.Access.Tables.FNC.ProjectFileAccess.DeleteByProjectIdwExceptIds(this._data.ProjectId, this._data.ProjectFileIds);

				if(this._data.Files != null && this._data.Files.Count > 0)
				{
					foreach(var fileItem in this._data.Files)
					{
						if(fileItem != null)
						{
							fileItem.ProjectId = this._data.ProjectId;
							fileItem.CreationUserId = this._user.Id;
							fileItem.CreationUserName = this._user.Name;
							insertedId = Infrastructure.Data.Access.Tables.FNC.ProjectFileAccess.Insert(await fileItem.ToFileEntityAsync(_user.Id));
						}
					}
				}

				var projectEntity = Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(this._data.ProjectId);
				if(projectEntity.Id_Type == (int)Enums.BudgetEnums.ProjectTypes.External)
				{
					Helpers.Notifications.Email.SendProjectCreationNotification(projectEntity.Id, this._user);
				}


				return ResponseModel<int>.SuccessResponse(insertedId);
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

			if(Infrastructure.Data.Access.Tables.FNC.BudgetProjectAccess.Get(this._data.ProjectId) == null)
				return ResponseModel<int>.FailureResponse("Project not found");

			return ResponseModel<int>.SuccessResponse();
		}

		ResponseModel<int> IHandle<UserModel, ResponseModel<int>>.Handle()
		{
			throw new NotImplementedException();
		}
	}
}
