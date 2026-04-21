using Psz.Core.SharedKernel.Interfaces;
using Psz.Core.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.CocType
{
	public class EditHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Settings.CoCType.CoCTypeRequestModel _data { get; set; }

		public EditHandler(Identity.Models.UserModel user, Models.Settings.CoCType.CoCTypeRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<int> Handle()
		{
			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				//TODO: - insert process here
				botransaction.beginTransaction();

				var entity = Infrastructure.Data.Access.Tables.BSD.CocTypeAccess.Get(this._data.Id);
				var data = this._data.ToEntity();
				data.LastEditUserId = this._user.Id;
				data.LastEditUserName = this._user.Username;
				var responseBody = Infrastructure.Data.Access.Tables.BSD.CocTypeAccess.UpdateWithTransaction(data, botransaction.connection, botransaction.transaction);

				// -
				var logs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
				if(entity.Version?.ToLower()?.Trim() != this._data.Version?.ToLower()?.Trim())
				{
					logs.Add(ObjectLogHelper.getLog(this._user, responseBody,
						  $"CoC: {this._data.Version} - Version",
						  $"{entity.Version}",
						  $"{this._data.Version}", Enums.ObjectLogEnums.Objects.ArticleConfig_CoCTypes.GetDescription(),
						  Enums.ObjectLogEnums.LogType.Edit));
				}
				if(entity.Name?.ToLower()?.Trim() != this._data.Name?.ToLower()?.Trim())
				{
					logs.Add(ObjectLogHelper.getLog(this._user, responseBody,
						  $"CoC: {this._data.Name} - Name",
						  $"{entity.Name}",
						  $"{this._data.Name}", Enums.ObjectLogEnums.Objects.ArticleConfig_CoCTypes.GetDescription(),
						  Enums.ObjectLogEnums.LogType.Edit));
				}

				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(logs, botransaction.connection, botransaction.transaction);

				if(botransaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(responseBody);
				}
				else
				{
					return ResponseModel<int>.FailureResponse(key: "1", value: "Transaction error");
				}
			} catch(Exception e)
			{
				botransaction.rollback();
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

			if(string.IsNullOrEmpty(this._data.Version))
			{
				return ResponseModel<int>.FailureResponse($"[Version] invalid data [{_data.Version}]");
			}
			if(string.IsNullOrEmpty(this._data.Name))
			{
				return ResponseModel<int>.FailureResponse($"[Type (Art)] invalid data [{_data.Name}]");
			}

			var entity = Infrastructure.Data.Access.Tables.BSD.CocTypeAccess.Get(this._data.Id);
			if(entity == null)
			{
				return ResponseModel<int>.FailureResponse("Item not found");
			}

			// - changing version
			if(entity.Version?.ToLower()?.Trim() != this._data.Version?.ToLower()?.Trim())
			{
				var articles = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByCoCVersion(this._data.Version);
				if(articles?.Count > 0)
				{
					return ResponseModel<int>.FailureResponse($"Error: articles [{string.Join(", ", articles.Take(5).Select(x => x.ArtikelNummer))}] with Version [{_data.Version}]");
				}

				var newCustomer = Infrastructure.Data.Access.Tables.BSD.CocTypeAccess.GetByVersion(this._data.Version)
					?.Where(x => x.Id != this._data.Id)?.ToList();
				if(newCustomer != null && newCustomer.Count > 0)
				{
					return ResponseModel<int>.FailureResponse($"Version [{(this._data.Version)}] already exists");
				}
			}
			// - changing name
			if(entity.Name?.ToLower()?.Trim() != this._data.Name?.ToLower()?.Trim())
			{
				var sameVersion = Infrastructure.Data.Access.Tables.BSD.CocTypeAccess.GetByVersionName(this._data.Version, this._data.Name)
					?.Where(x => x.Id != this._data.Id)?.ToList();
				if(sameVersion != null && sameVersion.Count > 0)
				{
					return ResponseModel<int>.FailureResponse($"Type [{this._data.Name}] already exists for Version [{this._data.Version}]");
				}
			}

			// -
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
