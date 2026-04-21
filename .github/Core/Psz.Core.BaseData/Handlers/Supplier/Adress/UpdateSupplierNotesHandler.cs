using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.BaseData.Handlers.Supplier.Adress
{
	public class UpdateSupplierNotesHandler: IHandle<Models.Supplier.AddressNotesRequestModel, ResponseModel<int>>
	{
		private Models.Supplier.AddressNotesRequestModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public UpdateSupplierNotesHandler(Identity.Models.UserModel user, Models.Supplier.AddressNotesRequestModel data)
		{
			this._data = data;
			this._user = user;
		}

		public ResponseModel<int> Handle()
		{
			lock(Locks.CostumerEditLock.GetOrAdd(this._data.AddressId, new object()))
			{
				try
				{
					var validationResponse = this.Validate();
					if(!validationResponse.Success)
					{
						return validationResponse;
					}

					var oldAddress = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(this._data.AddressId);
					var response = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.UpdateAdressNotes(_data.AddressId, _data.Notes);
					// save update logs
					Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(
						ObjectLogHelper.getLog(this._user, this._data.AddressId, "Bemerkungen", oldAddress.Bemerkungen, this._data.Notes, Enums.ObjectLogEnums.Objects.Supplier_Adress.GetDescription(), Enums.ObjectLogEnums.LogType.Edit));

					// -
					return ResponseModel<int>.SuccessResponse(response);
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}

			if(Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get(this._data.SupplierId) == null)
			{
				return ResponseModel<int>.FailureResponse("Supplier not found");
			}

			if(Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(this._data.AddressId) == null)
			{
				return ResponseModel<int>.FailureResponse("Address not found");
			}

			return ResponseModel<int>.SuccessResponse();
		}

	}
}
