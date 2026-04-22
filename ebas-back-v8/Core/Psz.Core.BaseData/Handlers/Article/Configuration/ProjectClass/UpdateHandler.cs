using System;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.ProjectClass
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class UpdateHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		private Models.Article.ProjectClass.AddRequestModel _data { get; set; }
		public UpdateHandler(UserModel user, Models.Article.ProjectClass.AddRequestModel data)
		{
			_user = user;
			_data = data;
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

				// 
				var insertedId = Infrastructure.Data.Access.Tables.BSD.ProjectClassAccess.Update(this._data.ToEntity());
				if(insertedId > 0)
				{
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, insertedId, "Name", "",
						this._data.Name,
						Enums.ObjectLogEnums.Objects.ArticleConfig_ProjectClass.GetDescription(),
						Enums.ObjectLogEnums.LogType.Edit));
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
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			if(string.IsNullOrWhiteSpace(this._data.Name))
				return ResponseModel<int>.FailureResponse($"Class Name should have a value");

			// -
			if(Infrastructure.Data.Access.Tables.BSD.ProjectClassAccess.Get(this._data.Id) == null)
				return ResponseModel<int>.FailureResponse("Project Class not found");

			var similarEntities = Infrastructure.Data.Access.Tables.BSD.ProjectClassAccess.GetByName(this._data.Name)
				?.Where(x => x.Id != this._data.Id)?.ToList();
			if(similarEntities != null && similarEntities.Count > 0)
				return ResponseModel<int>.FailureResponse($"Classe [{this._data.Name}] already exists");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
