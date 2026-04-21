using Psz.Core.SharedKernel.Interfaces;
using System;
using Psz.Core.Common.Models;
using System.Linq;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Settings.EdiCustomerConcern
{
	public class AddCustomersHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Settings.EdiCustomerConcern.EdiConcernAddWithCustomersRequestModel _data { get; set; }
		public AddCustomersHandler(Identity.Models.UserModel user, Models.Settings.EdiCustomerConcern.EdiConcernAddWithCustomersRequestModel data)
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

				var logs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
				#region // -- transaction-based logic -- //
				// -
				if(this._data.Customers?.Count > 0)
				{
					var insertedCustomer = Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernAccess.GetWithTransaction(this._data.Id, botransaction.connection, botransaction.transaction);
					Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernItemsAccess.InsertWithTransaction(
						this._data.Customers.Select(
							 x => new Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernItemsEntity
							 {
								 ConcernId = insertedCustomer.Id,
								 ConcernNumber = insertedCustomer.ConcernNumber,
								 CustomerDUNS = x.CustomerDUNS,
								 CustomerNumber = x.CustomerNumber,
							 }).ToList(), botransaction.connection, botransaction.transaction);
					// -
					logs.AddRange(this._data.Customers.Select(x => ObjectLogHelper.getLog(this._user, insertedCustomer.Id,
						Enums.ObjectLogEnums.Objects.ArticleConfig_EdiConcerns.GetDescription(),
						null,
						$"{insertedCustomer.ConcernName} | {x.CustomerNumber}", Enums.ObjectLogEnums.Objects.ArticleConfig_EdiConcernCustomers.GetDescription(), Enums.ObjectLogEnums.LogType.Add)));
				}

				// -
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(logs, botransaction.connection, botransaction.transaction);
				#endregion // -- transaction-based logic -- //


				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(this._data.Id);
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

			var entity = Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernAccess.Get(this._data.Id);
			if(entity == null)
			{
				return ResponseModel<int>.FailureResponse("Item not found");
			}

			if(this._data.Customers?.Count > 0)
			{
				var customerInConcern = Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernItemsAccess.GetByCustomerNumbers(this._data.Customers.Select(x => x.CustomerNumber));
				if(customerInConcern?.Count > 0)
				{
					return ResponseModel<int>.FailureResponse($"Customers [{string.Join(",", customerInConcern.Take(5).Select(x => x.CustomerNumber))}] already in concerns");
				}
			}
			else
			{
				return ResponseModel<int>.FailureResponse("No customers added");
			}

			// -
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
