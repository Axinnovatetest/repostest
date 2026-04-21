using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.BaseData.Handlers.Address
{
	public class DeleteHandler: IHandle<Models.Supplier.UpdateModel, ResponseModel<int>>
	{
		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public DeleteHandler(Identity.Models.UserModel user, int data)
		{
			this._data = data;
			this._user = user;
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
				var addressEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(this._data, botransaction.connection, botransaction.transaction);
				Infrastructure.Data.Access.Tables.PRS.AdressenAccess.DeleteWithTransaction(this._data, botransaction.connection, botransaction.transaction);

				//logging
				var log = ObjectLogHelper.getLog(this._user, this._data, "Address", null, addressEntity.Name1, Enums.ObjectLogEnums.Objects.Supplier.GetDescription(), Enums.ObjectLogEnums.LogType.Delete);
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(log, botransaction.connection, botransaction.transaction);
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
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			var addressEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(this._data);
			if(addressEntity == null)
			{
				return ResponseModel<int>.FailureResponse("Address not found");
			}

			//var Lieferantennummer_exsist = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetCountLieferantennummer((int)this._data.Lieferantennummer);
			//if(Lieferantennummer_exsist)
			//{
			//	return ResponseModel<int>.FailureResponse($"Lieferantennummer [{this._data.Lieferantennummer}] already exsists.");
			//}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
