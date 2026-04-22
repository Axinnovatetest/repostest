using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.BaseData.Handlers.Settings.CustomerItemNumber
{
	public class AddHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Settings.CustomerItemNumber.CustomerItemNumberResponseModel _data { get; set; }

		public AddHandler(Identity.Models.UserModel user, Models.Settings.CustomerItemNumber.CustomerItemNumberResponseModel data)
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
				var responseBody = Infrastructure.Data.Access.Tables.CTS.PSZ_Nummerschlüssel_KundeAccess.InsertWithTransaction(entity, botransaction.connection, botransaction.transaction);

				// -
				var log = ObjectLogHelper.getLog(this._user, responseBody,
						Enums.ObjectLogEnums.Objects.ArticleConfig_CustomerItemNumber.GetDescription(),
						null,
						entity.Nummerschlüssel, Enums.ObjectLogEnums.Objects.ArticleConfig_CustomerItemNumber.GetDescription(), Enums.ObjectLogEnums.LogType.Add);
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

			if(string.IsNullOrEmpty(this._data.Kunde))
			{
				return ResponseModel<int>.FailureResponse("Kunde Name should not be empty");
			}
			if(!this._data.Kundennummer.HasValue || this._data.Kundennummer.Value <= 0)
			{
				return ResponseModel<int>.FailureResponse("Kundennummer should not be empty");
			}
			if(string.IsNullOrEmpty(this._data.Nummerschlussel))
			{
				return ResponseModel<int>.FailureResponse("Nummerschlüssel should not be empty");
			}

			// -
			var sameKundeNumber = Infrastructure.Data.Access.Tables.CTS.PSZ_Nummerschlüssel_KundeAccess.GetByCustomerAndKreis(this._data.Kundennummer ?? 0, this._data.Nummerschlussel);
			if(sameKundeNumber != null && sameKundeNumber.Count > 0)
			{
				return ResponseModel<int>.FailureResponse($"Kunde [{(this._data.Kundennummer ?? 0)} | {this._data.Kunde}] with Nummerschlüssel [{this._data.Nummerschlussel}] already exists");
			}
			var sameKreis = Infrastructure.Data.Access.Tables.CTS.PSZ_Nummerschlüssel_KundeAccess.GetByKundeKreis(this._data.Nummerschlussel);
			if(sameKreis != null && sameKreis.Count > 0)
			{
				return ResponseModel<int>.FailureResponse($"Nummerschlüssel [{this._data.Nummerschlussel}] already exists");
			}

			// -
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
