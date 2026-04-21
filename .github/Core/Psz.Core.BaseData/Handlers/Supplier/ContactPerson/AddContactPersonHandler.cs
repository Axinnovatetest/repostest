using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;

namespace Psz.Core.BaseData.Handlers.Supplier.ContactPerson
{
	public class AddContactPersonHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.Supplier.ContactPersonModel _data { get; set; }

		public AddContactPersonHandler(Identity.Models.UserModel user, Models.Supplier.ContactPersonModel data)
		{
			this._user = user;
			this._data = data;
		}

		public ResponseModel<int> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var lieferantenEntity = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get(this._data.KundenId);
				var adressenEntity = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(lieferantenEntity.Nummer ?? -1);
				var contactPersonEntity = this._data.ToDataEntity(adressenEntity?.Nr);

				var response = Infrastructure.Data.Access.Tables.BSD.AnsprechpartnerAccess.Insert(contactPersonEntity);
				var insertedEntity = Infrastructure.Data.Access.Tables.BSD.AnsprechpartnerAccess.Get(response);
				//log
				var logTypeEdit = Enums.ObjectLogEnums.LogType.Add;
				var log = ObjectLogHelper.getContactPersonLog(this._user, lieferantenEntity.Nr, "Supplier", null, this._data.ContactPerson, Enums.ObjectLogEnums.Objects.Supplier_ContcatPerson.GetDescription(), logTypeEdit);
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(log);

				return ResponseModel<int>.SuccessResponse(response);
			} catch(Exception e)
			{
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
			var lieferantenEntity = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.Get(this._data.KundenId);
			if(lieferantenEntity == null)
			{
				return ResponseModel<int>.FailureResponse("Supplier Not found");
			}
			if(Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(lieferantenEntity.Nummer ?? -1) == null)
			{
				return ResponseModel<int>.FailureResponse("Adress Not found");
			}
			if(string.IsNullOrEmpty(this._data.PhoneNumber) || string.IsNullOrEmpty(this._data.ContactPerson) || string.IsNullOrEmpty(this._data.EmailAdress) || string.IsNullOrEmpty(this._data.Adress))
			{
				return ResponseModel<int>.FailureResponse("Please fill all the required fields");
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
