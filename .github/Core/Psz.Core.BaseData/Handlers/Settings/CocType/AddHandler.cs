using Psz.Core.SharedKernel.Interfaces;
using Psz.Core.Common.Models;
using System;

namespace Psz.Core.BaseData.Handlers.Settings.CocType
{
	public class AddHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Settings.CoCType.CoCTypeRequestModel _data { get; set; }
		public AddHandler(Identity.Models.UserModel user, Models.Settings.CoCType.CoCTypeRequestModel data)
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

				botransaction.beginTransaction();

				#region // -- transaction-based logic -- //
				// -
				var entity = this._data.ToEntity();
				entity.CreationTime = DateTime.Now;
				entity.CreationUserId = this._user.Id;
				entity.CreationUserName = this._user.Username;
				var responseBody = Infrastructure.Data.Access.Tables.BSD.CocTypeAccess.InsertWithTransaction(entity, botransaction.connection, botransaction.transaction);

				// -
				var log = ObjectLogHelper.getLog(this._user, responseBody,
						Enums.ObjectLogEnums.Objects.ArticleConfig_CoCTypes.GetDescription(),
						null,
						entity.Version, Enums.ObjectLogEnums.Objects.ArticleConfig_CoCTypes.GetDescription(), Enums.ObjectLogEnums.LogType.Add);
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(log, botransaction.connection, botransaction.transaction);
				#endregion // -- transaction-based logic -- //


				//TODO: handle transaction state (success or failure)
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

			// - 
			var sameVersion = Infrastructure.Data.Access.Tables.BSD.CocTypeAccess.GetByVersion(this._data.Version);
			if(sameVersion?.Count > 0)
			{
				return ResponseModel<int>.FailureResponse($"Version [{this._data.Version}] already exists");
			}
			var sameName = Infrastructure.Data.Access.Tables.BSD.CocTypeAccess.GetByName(this._data.Name);
			if(sameName?.Count > 0)
			{
				return ResponseModel<int>.FailureResponse($"Type [{this._data.Name}] already exists");
			}

			// -
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
