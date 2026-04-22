using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.EdiCustomerConcern
{
	public class DeleteHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public DeleteHandler(Identity.Models.UserModel user, int data)
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
				var entity = Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernAccess.GetWithTransaction(this._data, botransaction.connection, botransaction.transaction);
				var responseBody = Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernAccess.DeleteWithTransaction(this._data, botransaction.connection, botransaction.transaction);

				// -
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(ObjectLogHelper.getLog(this._user, responseBody,
						  $"",
						  $"{entity.ConcernNumber} | {entity.ConcernName}",
						  $"", Enums.ObjectLogEnums.Objects.ArticleConfig_EdiConcerns.GetDescription(),
						  Enums.ObjectLogEnums.LogType.Delete),
						  botransaction.connection, botransaction.transaction);

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

			var entity = Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernAccess.Get(this._data);
			if(entity == null)
			{
				return ResponseModel<int>.FailureResponse("Item not found");
			}

			var concernCustomerEntities = Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernItemsAccess.GetByConcernId(entity.Id);
			if(concernCustomerEntities != null && concernCustomerEntities.Count > 0)
			{
				return ResponseModel<int>.FailureResponse($"Concern [{(entity.ConcernNumber)} | {entity.ConcernName}] has customers [{(string.Join(", ", concernCustomerEntities.Take(5).Select(x => x.CustomerNumber)))}]. Please remove the customers from concern first.");
			}
			// -
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
