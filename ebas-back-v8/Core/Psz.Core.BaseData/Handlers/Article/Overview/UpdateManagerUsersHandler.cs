using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Article.Overview
{
	public class UpdateManagerUsersHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Article.ManagerUser.ManagerUserModel _data { get; set; }
		public UpdateManagerUsersHandler(Identity.Models.UserModel user, Models.Article.ManagerUser.ManagerUserModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<int> Handle()
		{
			try
			{
				lock((Locks.ArticleEditLock.GetOrAdd(this._data.ArtikelNr, new object())))
				{
					var validationResponse = this.Validate();
					if(!validationResponse.Success)
					{
						return validationResponse;
					}

					var logs = LogChanges();
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(logs);
					var response = 0;
					var managerEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelManagerUserAccess.GetSingleByArtikelNr(this._data.ArtikelNr);
					if(managerEntity != null)
					{
						response = Infrastructure.Data.Access.Tables.PRS.ArtikelManagerUserAccess.UpdateByArticleNr(this._data.ToEntity());
					}
					else
					{
						response = Infrastructure.Data.Access.Tables.PRS.ArtikelManagerUserAccess.Insert(this._data.ToEntity());
					}

					// - 2022-03-30
					CreateHandler.generateFileDAT(this._data.ArtikelNr);
					return ResponseModel<int>.SuccessResponse(response);
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			var managerEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelManagerUserAccess.GetByUserId(this._data.UserId);
			if(managerEntity == null || managerEntity.Count <= 0)
			{
				return new ResponseModel<int>()
				{
					Success = false,
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "1", Value = "Manager not found"}
					}
				};
			}

			return ResponseModel<int>.SuccessResponse();
		}
		internal List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity> LogChanges()
		{
			if(string.IsNullOrWhiteSpace(this._data.UserFullName))
				return null;

			var logs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
			var managersEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelManagerUserAccess.GetSingleByArtikelNr(this._data.ArtikelNr);
			if(managersEntity != null)
			{
				if(managersEntity.UserFullName?.ToLower()?.Trim() != this._data.UserFullName?.ToLower()?.Trim())
				{
					logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "Article Manager", managersEntity.UserFullName, this._data.UserFullName, Enums.ObjectLogEnums.Objects.Article.GetDescription(), Enums.ObjectLogEnums.LogType.Edit));
				}
			}
			else
			{
				logs.Add(ObjectLogHelper.getLog(this._user, this._data.ArtikelNr, "Article Manager", null, this._data.UserFullName, Enums.ObjectLogEnums.Objects.Article.GetDescription(), Enums.ObjectLogEnums.LogType.Add));
			}
			//
			return logs;
		}
	}
}
