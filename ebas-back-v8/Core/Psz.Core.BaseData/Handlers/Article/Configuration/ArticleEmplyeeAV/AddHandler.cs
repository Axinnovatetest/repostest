using System;

namespace Psz.Core.BaseData.Handlers.Article.Configuration.ArticleEmployeeAV
{
	using Psz.Core.Common.Models;
	using Psz.Core.Identity.Models;
	using Psz.Core.SharedKernel.Interfaces;
	public class AddHandler: IHandle<UserModel, ResponseModel<int>>
	{
		private UserModel _user { get; set; }
		private Models.Article.Configuration.ArticleContactAVModel _data { get; set; }
		public AddHandler(UserModel user, Models.Article.Configuration.ArticleContactAVModel projectType)
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
				var insertedId = Infrastructure.Data.Access.Tables.CTS.AV_Abteilung_MitarbeiterAccess.Insert(
					new Infrastructure.Data.Entities.Tables.CTS.AV_Abteilung_MitarbeiterEntity
					{
						ID = -1,
						AV_Mitarbeiter = this._data.Name,
						EMAIL = this._data.Email
					});
				if(insertedId > 0)
				{
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, insertedId, "Name", "",
						this._data.Name,
						Enums.ObjectLogEnums.Objects.ArticleConfig_ArticleEmployeeAV.GetDescription(),
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

			var projectTypeEntities = Infrastructure.Data.Access.Tables.CTS.AV_Abteilung_MitarbeiterAccess.Get(this._data.Name);
			if(projectTypeEntities != null)
				return ResponseModel<int>.FailureResponse($"Employee [{this._data.Name}] exists");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
