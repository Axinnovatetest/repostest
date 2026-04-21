using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.BaseData.Handlers.Address
{
	public class BlockHandler: IHandle<Models.Supplier.UpdateModel, ResponseModel<int>>
	{
		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public BlockHandler(Identity.Models.UserModel user, int data)
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
				addressEntity.Sperren = !(addressEntity.Sperren ?? false);

				var insertedAddressId = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.UpdateWithTransaction(addressEntity, botransaction.connection, botransaction.transaction);
				//logging
				var log = ObjectLogHelper.getLog(this._user, insertedAddressId, "Address", null, addressEntity.Name1, Enums.ObjectLogEnums.Objects.Supplier.GetDescription(), Enums.ObjectLogEnums.LogType.Edit);
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.InsertWithTransaction(log, botransaction.connection, botransaction.transaction);

				#endregion // -- transaction-based logic -- //

				//TODO: handle transaction state (success or failure)
				if(botransaction.commit())
				{
					return ResponseModel<int>.SuccessResponse(insertedAddressId);
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
				return ResponseModel<int>.FailureResponse("Address does not exist");
			}

			// - 2023-03-14 - check with PM/CS
			//if(this._data.SupplierNumber.HasValue == true && this._data.SupplierNumber.Value > 0)
			//{
			//	if(Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByLieferantennummer(this._data.SupplierNumber.Value, 1) == null)
			//	{
			//		return ResponseModel<int>.FailureResponse($"Lieferant [{this._data.SupplierNumber.Value}] not found.");
			//	}
			//	var Lieferantennummer_exsist = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByLieferantennummer(this._data.SupplierNumber.Value, this._data.AddressType);
			//	if(Lieferantennummer_exsist != null)
			//	{
			//		return ResponseModel<int>.FailureResponse($"Lieferant [{this._data.SupplierNumber.Value}] already exists with address [{Lieferantennummer_exsist.Name1}].");
			//	}
			//}
			//if(this._data.CustomerNumber.HasValue == true && this._data.CustomerNumber.Value > 0)
			//{
			//	if(Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByKundenNummer(this._data.CustomerNumber.Value) == null)
			//	{
			//		return ResponseModel<int>.FailureResponse($"Kunde [{this._data.CustomerNumber.Value}] not found.");
			//	}
			//	var Lieferantennummer_exsist = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByKundenNummer(this._data.CustomerNumber.Value, this._data.AddressType);
			//	if(Lieferantennummer_exsist != null)
			//	{
			//		return ResponseModel<int>.FailureResponse($"Kunde [{this._data.CustomerNumber.Value}] already exists with address [{Lieferantennummer_exsist.Name1}].");
			//	}
			//}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
