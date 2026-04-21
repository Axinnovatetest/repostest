using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.CustomerService.Handlers.OrderProcessing
{
	public class UpdateOrderCustomerUstIdHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public UpdateOrderCustomerUstIdHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<int> Handle()
		{
			var botransaction = new Infrastructure.Services.Utils.TransactionsManager();
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			try
			{
				botransaction.beginTransaction();

				var orderDb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetWithTransaction(_data, botransaction.connection, botransaction.transaction);
				var customerDb = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer((int)orderDb.Kunden_Nr, botransaction.connection, botransaction.transaction);

				//logging
				var _toLog = new Helpers.LogHelper(orderDb.Nr, (int)orderDb.Angebot_Nr, int.TryParse(orderDb.Projekt_Nr, out var val) ? val : 0, orderDb.Typ, Helpers.LogHelper.LogType.MODIFICATIONOBJECT, "CTS", this._user);
				Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.InsertWithTransaction(_toLog.LogCTS("EG___Identifikationsnummer", orderDb.Freitext, customerDb.EG___Identifikationsnummer.ToString(), this._data),
					botransaction.connection, botransaction.transaction);

				var result = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.UpdateCustomerUstIdWithTransaction(this._data, $"USt-ID-Nr.: {customerDb.EG___Identifikationsnummer}", botransaction.connection, botransaction.transaction);

				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(result);
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
			if(_user == null || (!_user.Access.CustomerService.ModuleActivated && !_user.Access.Purchase.ModuleActivated))
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			var orderDb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data);
			if(orderDb == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Order not found");
			if(Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByNummer(orderDb.Kunden_Nr ?? -1) == null)
				return ResponseModel<int>.FailureResponse(key: "1", value: $"Customer not found");

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
