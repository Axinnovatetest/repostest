using Psz.Core.SharedKernel.Interfaces;
using System;
using Psz.Core.Common.Models;
using System.Linq;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Settings.EdiCustomerConcern
{
	public class AddWithCustomersHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Settings.EdiCustomerConcern.EdiConcernAddWithCustomersRequestModel _data { get; set; }
		public AddWithCustomersHandler(Identity.Models.UserModel user, Models.Settings.EdiCustomerConcern.EdiConcernAddWithCustomersRequestModel data)
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
				var entity = new Infrastructure.Data.Entities.Tables.CTS.EDI_CustomerConcernEntity
				{
					ConcernName = this._data.ConcernName,
					ConcernNumber = -1,
					Id = -1,
					IncludeDescription = this._data.IncludeDescription,
					TrimLeadingZeros = this._data.TrimLeadingZeros,
				};
				entity.CreationTime = DateTime.Now;
				entity.CreationUserId = this._user.Id;
				entity.CreationUserName = this._user.Username;
				var responseBody = Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernAccess.InsertAutoNumber(entity, botransaction.connection, botransaction.transaction);
				if(responseBody > 0 && this._data.Customers?.Count > 0)
				{
					var insertedCustomer = Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernAccess.GetWithTransaction(responseBody, botransaction.connection, botransaction.transaction);
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
					logs.AddRange(this._data.Customers.Select(x => ObjectLogHelper.getLog(this._user, responseBody,
						Enums.ObjectLogEnums.Objects.ArticleConfig_EdiConcerns.GetDescription(),
						null,
						$"{entity.ConcernName} | {x.CustomerNumber}", Enums.ObjectLogEnums.Objects.ArticleConfig_EdiConcernCustomers.GetDescription(), Enums.ObjectLogEnums.LogType.Add)));
				}

				// -
				logs.Add(ObjectLogHelper.getLog(this._user, responseBody,
						Enums.ObjectLogEnums.Objects.ArticleConfig_EdiConcerns.GetDescription(),
						null,
						entity.ConcernName, Enums.ObjectLogEnums.Objects.ArticleConfig_EdiConcerns.GetDescription(), Enums.ObjectLogEnums.LogType.Add));
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(logs, botransaction.connection, botransaction.transaction);
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
			var sameName = Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernAccess.GetSameName(this._data.ConcernName);
			if(sameName?.Count > 0)
			{
				return ResponseModel<int>.FailureResponse($"Concern [{this._data.ConcernName}] already exists");
			}
			if(this._data.Customers?.Count > 0)
			{
				var customerInConcern = Infrastructure.Data.Access.Tables.CTS.EDI_CustomerConcernItemsAccess.GetByCustomerNumbers(this._data.Customers.Select(x => x.CustomerNumber));
				if(customerInConcern?.Count > 0)
				{
					return ResponseModel<int>.FailureResponse($"Customers [{string.Join(",", customerInConcern.Take(5).Select(x => x.CustomerNumber))}] already in concerns");
				}
			}

			// -
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
