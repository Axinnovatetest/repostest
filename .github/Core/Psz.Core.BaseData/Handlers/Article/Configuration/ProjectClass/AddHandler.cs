using System;

namespace Psz.Core.BaseData.Handlers.Article.ProjectClass
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class AddHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		private Models.Article.ProjectClass.AddRequestModel _data { get; set; }
		public AddHandler(UserModel user, Models.Article.ProjectClass.AddRequestModel data)
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
				var insertedId = Infrastructure.Data.Access.Tables.BSD.ProjectClassAccess.Insert(this._data.ToEntity());
				if(insertedId > 0)
				{
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, insertedId, "Name", "",
						this._data.Name,
						Enums.ObjectLogEnums.Objects.ArticleConfig_ProjectClass.GetDescription(),
						Enums.ObjectLogEnums.LogType.Add));
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
			var similarEntities = Infrastructure.Data.Access.Tables.BSD.ProjectClassAccess.GetByName(this._data.Name);
			if(similarEntities != null && similarEntities.Count > 0)
				return ResponseModel<int>.FailureResponse($"Class [{this._data.Name}] already exists");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
