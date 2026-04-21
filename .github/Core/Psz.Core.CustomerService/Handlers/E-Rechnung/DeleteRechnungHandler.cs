using Infrastructure.Services.Utils;
using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Helpers;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Linq;

namespace Psz.Core.CustomerService.Handlers.E_Rechnung
{
	public class DeleteRechnungHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public DeleteRechnungHandler(Identity.Models.UserModel user, int data)
		{
			this._user = user;
			this._data = data;
		}


		public ResponseModel<int> Handle()
		{
			var DBTransaction = new TransactionsManager();
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var rechnung = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data);
				var rechnungItems = Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.GetByAngeboteNr(_data);
				DBTransaction.beginTransaction();
				Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.DeleteWithTransaction(_data, DBTransaction.connection, DBTransaction.transaction);
				Infrastructure.Data.Access.Tables.PRS.AngeboteneArtikelAccess.DeleteWithTransaction(rechnungItems?.Select(x => x.Nr).ToList(), DBTransaction.connection, DBTransaction.transaction);

				var deliveries = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.GetByNrRechnung(_data);
				deliveries.ForEach(x =>
				{
					x.Nr_rec = null;
					x.Erledigt = false;
				});
				Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.UpdateWithTransaction(deliveries, DBTransaction.connection, DBTransaction.transaction);
				if(DBTransaction.commit())
				{
					//Logging
					var _log = new LogHelper(rechnung.Nr, (int)rechnung.Angebot_Nr,
						int.TryParse(rechnung.Projekt_Nr, out var val) ? val : 0, rechnung.Typ, LogHelper.LogType.DELETIONOBJECT, "CTS", _user)
						.LogCTS(null, null, null, 0);
					Infrastructure.Data.Access.Tables.CTS.OrderProcesssing_LogAccess.Insert(_log);
					return ResponseModel<int>.SuccessResponse(1);
				}
				else
					return ResponseModel<int>.FailureResponse("Error in transaction");

			} catch(Exception e)
			{
				DBTransaction.rollback();
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			var rechnung = Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.Get(_data);
			if(rechnung == null)
				return ResponseModel<int>.FailureResponse("Invoice not found");
			if(rechnung.Gebucht.HasValue && rechnung.Gebucht.Value)
				return ResponseModel<int>.FailureResponse("Cannot delete validated invoice");
			return ResponseModel<int>.SuccessResponse();
		}

	}
}
