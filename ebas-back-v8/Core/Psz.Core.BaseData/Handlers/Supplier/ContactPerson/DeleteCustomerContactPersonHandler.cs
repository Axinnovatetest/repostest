using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Supplier.ContactPerson
{
	public class DeleteCustomerContactPersonHandler: IHandle<int, ResponseModel<int>>
	{
		private int _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }

		public DeleteCustomerContactPersonHandler(Identity.Models.UserModel user, int Id)
		{
			this._data = Id;
			this._user = user;
		}

		public ResponseModel<int> Handle()
		{
			var validationResponse = this.Validate();
			if(!validationResponse.Success)
			{
				return validationResponse;
			}
			var contactPersonEntity = Infrastructure.Data.Access.Tables.BSD.AnsprechpartnerAccess.Get(this._data);
			var adressenEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(contactPersonEntity.Nummer ?? -1);
			var lieferantenId = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.GetByNummer(adressenEntity.Nr).Nr;
			//log
			var logTypeEdit = Enums.ObjectLogEnums.LogType.Delete;
			var log = ObjectLogHelper.getContactPersonLog(this._user, lieferantenId, "Supplier", contactPersonEntity.Ansprechpartner, null, Enums.ObjectLogEnums.Objects.Supplier_ContcatPerson.GetDescription(), logTypeEdit);
			Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(log);
			var response = Infrastructure.Data.Access.Tables.BSD.AnsprechpartnerAccess.Delete(this._data);
			return ResponseModel<int>.SuccessResponse(response);
		}

		public ResponseModel<int> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<int>.AccessDeniedResponse();
			}
			var contactPersonEntity = Infrastructure.Data.Access.Tables.BSD.AnsprechpartnerAccess.Get(this._data);
			if(contactPersonEntity == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>() {
						new ResponseModel<int>.ResponseError {Key = "1", Value = "Contact Person not found"}
					}
				};
			}

			return ResponseModel<int>.SuccessResponse();
		}
	}
}
