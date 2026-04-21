using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.BaseData.Handlers.Address
{
	public class CopyHandler: IHandle<Models.Supplier.UpdateModel, ResponseModel<int>>
	{
		private Models.Address.CopyRequestModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public CopyHandler(Identity.Models.UserModel user, Models.Address.CopyRequestModel data)
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

				var copyAddressEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(this._data.CopyId, botransaction.connection, botransaction.transaction);
				copyAddressEntity.Adresstyp = this._data.NewAddressType;
				copyAddressEntity.Kundennummer = null;
				copyAddressEntity.Lieferantennummer = null;
				copyAddressEntity.Erfasst = DateTime.Now;

				var insertedAddressId = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.InsertWithTransaction(copyAddressEntity, botransaction.connection, botransaction.transaction);
				//logging
				var log = ObjectLogHelper.getLog(this._user, insertedAddressId, "Address", null, copyAddressEntity.Name1, Enums.ObjectLogEnums.Objects.Supplier.GetDescription(), Enums.ObjectLogEnums.LogType.Add);
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
			var copyAddressEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(this._data.CopyId);
			if(copyAddressEntity == null)
			{
				return ResponseModel<int>.FailureResponse("Copy Address not found");
			}

			var addressEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByName1StreetPostCodeCityCountry(
				  copyAddressEntity.Name1?.Trim()
				, copyAddressEntity.StraBe?.Trim()
				, copyAddressEntity.PLZ_StraBe?.Trim()
				, copyAddressEntity.Ort?.Trim()
				, copyAddressEntity.Land?.Trim()
				, this._data.NewAddressType);


			var kundenByNummer = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByName1(copyAddressEntity.Name1.Trim(), this._data.NewAddressType);

			if(kundenByNummer != null &&
				kundenByNummer.Adresstyp == (int)Infrastructure.Data.Entities.Tables.PRS.AdressenTypEnum.Standard &&
				this._data.NewAddressType == (int)Infrastructure.Data.Entities.Tables.PRS.AdressenTypEnum.Standard)
			{
				return ResponseModel<int>.FailureResponse($"Kunde [{copyAddressEntity.Name1}] can have only one standard address");
			}


			if(addressEntity != null)
			{
				return ResponseModel<int>.FailureResponse("Address [Name 1] with the same Street/City/Country and Address Type is already used");
			}

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
