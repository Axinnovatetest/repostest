using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.CustomerService.Handlers.OrderProcessing
{
	public class ToggleBookedHandler: IHandle<Identity.Models.UserModel, ResponseModel<object>>
	{
		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public ToggleBookedHandler(int data, Identity.Models.UserModel user)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<object> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}

			try
			{
				lock(Locks.Locks.OrdersLock)
				{
					var orderDb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data);
					orderDb.Gebucht = orderDb.Gebucht.HasValue ? !orderDb.Gebucht.Value : true;
					Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Update(orderDb);
					//logging
					var _log = new Helpers.LogHelper(orderDb.Nr, (int)orderDb.Angebot_Nr, int.TryParse(orderDb.Projekt_Nr, out var val) ? val : 0, orderDb.Typ, Helpers.LogHelper.LogType.MODIFICATIONOBJECT, "CTS", _user)
						.LogCTS("Gebucht", (!orderDb.Gebucht).ToString(), orderDb.Gebucht.ToString(), 0);
					Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(_log);

					return ResponseModel<object>.SuccessResponse();
				}
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				Infrastructure.Services.Logging.Logger.Log(Infrastructure.Services.Logging.Logger.Levels.Trace, $"INPUT DATA: _data:{_data}");
				Infrastructure.Services.Logging.Logger.Log(e.StackTrace);
				throw;
			}
		}
		public ResponseModel<object> Validate()
		{
			if(_user == null || (!_user.Access.CustomerService.ModuleActivated && !_user.Access.Purchase.ModuleActivated))
			{
				return ResponseModel<object>.AccessDeniedResponse();
			}
			var orderDb = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data);
			if(orderDb == null)
				return ResponseModel<object>.FailureResponse(key: "1", value: $"Order not found");

			// - 2022-07-04 - block changes for LS w/ Rechnung
			if(/*orderDb.Erledigt == true &&*/ orderDb.Typ == Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_DELIVERY)
			{
				var invoiceEntities = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetInvoiceByLieferschein(orderDb.Nr);
				if(invoiceEntities != null && invoiceEntities.Count > 0)
				{
					return ResponseModel<object>.FailureResponse(new List<string> { "ACHTUNG: Lieferschein ist erledigt.", "Buchung rückgängig nicht möglich!", "Rechnung stornieren notwendig?" });
				}
			}

			return ResponseModel<object>.SuccessResponse();
		}
	}
}
