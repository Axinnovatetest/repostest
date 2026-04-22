using System;

namespace Psz.Core.BaseData.Handlers.Article.Configuration.ArticleSample
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class AddHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Configuration.ArticleSampleModel _data { get; set; }
		public AddHandler(UserModel user, Models.Article.Configuration.ArticleSampleModel projectType)
		{
			_user = user;
			_data = projectType;
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
				var vdEntity = this._data.ToEntity();
				var insertedId = Infrastructure.Data.Access.Tables.BSD.ArticleSampleAccess.Insert(vdEntity);
				if(insertedId > 0)
				{
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, insertedId, "Name", "",
						vdEntity.Name,
						Enums.ObjectLogEnums.Objects.ArticleConfig_ArticleSample.GetDescription(),
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
				return ResponseModel<int>.FailureResponse($"Name should have a value");

			var projectTypeEntities = Infrastructure.Data.Access.Tables.BSD.ArticleSampleAccess.GetByName(this._data.Name);
			if(projectTypeEntities != null && projectTypeEntities.Count > 0)
				return ResponseModel<int>.FailureResponse($"Sample [{this._data.Name}] exists");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
