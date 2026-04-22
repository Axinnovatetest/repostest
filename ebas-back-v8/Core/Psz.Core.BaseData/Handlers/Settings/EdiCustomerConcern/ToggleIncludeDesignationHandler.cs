using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.EdiCustomerConcern
{
	public class ToggleIncludeDesignationHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public ToggleIncludeDesignationHandler(Identity.Models.UserModel user, int data)
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

				var entity = Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernAccess.GetWithTransaction(this._data, botransaction.connection, botransaction.transaction);
				// -
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(ObjectLogHelper.getLog(this._user, this._data,
						  $"Concern: IncludeDesignation",
						  $"{entity.IncludeDescription}",
						  $"{!entity.IncludeDescription}", Enums.ObjectLogEnums.Objects.ArticleConfig_EdiConcerns.GetDescription(),
						  Enums.ObjectLogEnums.LogType.Edit), botransaction.connection, botransaction.transaction);
				// -
				if(botransaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(
						Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernAccess.ToggeIncludeDesignation(this._data, this._user.Id, this._user.Name, botransaction.connection, botransaction.transaction));
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

			if(Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernAccess.Get(this._data) == null)
			{
				return ResponseModel<int>.FailureResponse("Concern Item not found");
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
