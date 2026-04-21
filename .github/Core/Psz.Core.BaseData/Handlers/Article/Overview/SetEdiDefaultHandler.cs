using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Article.Overview
{
	public class SetEdiDefaultHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }
		public SetEdiDefaultHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<int> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			try
			{
				botransaction.beginTransaction();

				#region // -- transaction-based logic -- //

				var logs = new List<Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity>();
				var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetWithTransaction(this._data, botransaction.connection, botransaction.transaction);
				var articleSiblings = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.GetByCustomerItemNumber(articleEntity.CustomerNumber ?? -1, articleEntity.ArtikelNummer?.Substring(0, articleEntity.ArtikelNummer?.IndexOf('-') ?? 0), articleEntity.CustomerItemNumber, botransaction.connection, botransaction.transaction);
				Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.ResetCustomerEdiDefaultWithTransaction(articleEntity.CustomerNumber ?? -1, articleEntity.CustomerItemNumber, botransaction.connection, botransaction.transaction);
				Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.SetCustomerEdiDefaultWithTransaction(this._data, botransaction.connection, botransaction.transaction);

				if(articleSiblings != null && articleSiblings.Count > 0)
				{
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(articleSiblings.Where(x => x.ArtikelNr != this._data)
						.Select(x => new Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity
						{
							LastUpdateTime = DateTime.Now,
							LastUpdateUserFullName = this._user.Name,
							LastUpdateUserId = this._user.Id,
							LastUpdateUsername = this._user.Username,
							LogDescription = $"Remove EDI default for [{articleEntity.CustomerItemNumber}]",
							LogObject = $"Article",
							LogObjectId = x.ArtikelNr,
						}).ToList(), botransaction.connection, botransaction.transaction);
				}
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(new Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity
				{
					LastUpdateTime = DateTime.Now,
					LastUpdateUserFullName = this._user.Name,
					LastUpdateUserId = this._user.Id,
					LastUpdateUsername = this._user.Username,
					LogDescription = $"Set to EDI default for [{articleEntity.CustomerItemNumber}]",
					LogObject = $"Article",
					LogObjectId = this._data,
				}, botransaction.connection, botransaction.transaction);

				#endregion // -- transaction-based logic -- //

				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(this._data);
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
			if(this._user == null /*this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			// -
			var articleEntity = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(this._data);
			if(articleEntity == null)
			{
				return ResponseModel<int>.FailureResponse("Article not found");
			}

			if(string.IsNullOrWhiteSpace(articleEntity.CustomerItemNumber))
			{
				return ResponseModel<int>.FailureResponse("Article Customer Item Number can not be empty");
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
