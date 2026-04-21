using Psz.Core.SharedKernel.Interfaces;
using System;
using Psz.Core.Common.Models;

namespace Psz.Core.BaseData.Handlers.Settings.EdiCustomerConcern
{
	public class AddHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Settings.EdiCustomerConcern.EdiConcernAddRequestModel _data { get; set; }
		public AddHandler(Identity.Models.UserModel user, Models.Settings.EdiCustomerConcern.EdiConcernAddRequestModel data)
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
				var responseBody = Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernAccess.InsertAutoNumber(entity, botransaction.connection, botransaction.transaction);

				// -
				var log = ObjectLogHelper.getLog(this._user, responseBody,
						Enums.ObjectLogEnums.Objects.ArticleConfig_EdiConcerns.GetDescription(),
						null,
						entity.ConcernName, Enums.ObjectLogEnums.Objects.ArticleConfig_EdiConcerns.GetDescription(), Enums.ObjectLogEnums.LogType.Add);
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

			if(string.IsNullOrEmpty(this._data.ConcernName))
			{
				return ResponseModel<int>.FailureResponse($"[Concern Name] invalid data [{_data.ConcernName}]");
			}

			// - 
			var sameVersion = Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernAccess.GetSameName(this._data.ConcernName);
			if(sameVersion?.Count > 0)
			{
				return ResponseModel<int>.FailureResponse($"Concern [{this._data.ConcernName}] already exists");
			}

			// -
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
