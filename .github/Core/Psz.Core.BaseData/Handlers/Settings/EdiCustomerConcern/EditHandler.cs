using Psz.Core.SharedKernel.Interfaces;
using Psz.Core.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.EdiCustomerConcern
{
	public class EditHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Settings.EdiCustomerConcern.EdiConcernAddRequestModel _data { get; set; }

		public EditHandler(Identity.Models.UserModel user, Models.Settings.EdiCustomerConcern.EdiConcernAddRequestModel data)
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

				var entity = Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernAccess.Get(this._data.Id);
				var data = this._data.ToEntity();
				data.LastEditUserId = this._user.Id;
				data.LastEditUserName = this._user.Username;
				var responseBody = Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernAccess.UpdateWithTransaction(data, botransaction.connection, botransaction.transaction);

				// -
				var logs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
				if(entity.IncludeDescription != this._data.IncludeDescription)
				{
					logs.Add(ObjectLogHelper.getLog(this._user, responseBody,
						  $"Concern: {this._data.IncludeDescription}",
						  $"{entity.IncludeDescription}",
						  $"{this._data.IncludeDescription}", Enums.ObjectLogEnums.Objects.ArticleConfig_EdiConcerns.GetDescription(),
						  Enums.ObjectLogEnums.LogType.Edit));
				}
				if(entity.TrimLeadingZeros != this._data.TrimLeadingZeros)
				{
					logs.Add(ObjectLogHelper.getLog(this._user, responseBody,
						  $"Concern: {this._data.TrimLeadingZeros}",
						  $"{entity.TrimLeadingZeros}",
						  $"{this._data.TrimLeadingZeros}", Enums.ObjectLogEnums.Objects.ArticleConfig_EdiConcerns.GetDescription(),
						  Enums.ObjectLogEnums.LogType.Edit));
				}
				if(entity.ConcernName?.ToLower()?.Trim() != this._data.ConcernName?.ToLower()?.Trim())
				{
					logs.Add(ObjectLogHelper.getLog(this._user, responseBody,
						  $"Concern: {this._data.ConcernName} - Name",
						  $"{entity.ConcernName}",
						  $"{this._data.ConcernName}", Enums.ObjectLogEnums.Objects.ArticleConfig_EdiConcerns.GetDescription(),
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

			if(string.IsNullOrEmpty(this._data.ConcernName))
			{
				return ResponseModel<int>.FailureResponse($"[Name] invalid data [{_data.ConcernName}]");
			}

			var entity = Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernAccess.Get(this._data.Id);
			if(entity == null)
			{
				return ResponseModel<int>.FailureResponse("Item not found");
			}

			// - changing name
			if(entity.ConcernName?.ToLower()?.Trim() != this._data.ConcernName?.ToLower()?.Trim())
			{
				var sameName = Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernAccess.GetSameName(this._data.ConcernName)
					?.Where(x => x.Id != this._data.Id)?.ToList();
				if(sameName != null && sameName.Count > 0)
				{
					return ResponseModel<int>.FailureResponse($"Concern [{this._data.ConcernName}] already exists");
				}
			}

			// -
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
