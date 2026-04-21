using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.ContactPersonSettings.AddressContactPerson
{
	public class UpdateAddressContactPerson: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public Models.Settings.AddressContactPerson.AddressContactPersonModel _data { get; set; }
		public UpdateAddressContactPerson(Identity.Models.UserModel user, Models.Settings.AddressContactPerson.AddressContactPersonModel data)
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
				var _entity = this._data.ToEntity();
				var _oldEntity = Infrastructure.Data.Access.Tables.BSD.Adressen_anredenAccess.Get(_entity.ID);
				if(_oldEntity.Anrede != _entity.Anrede)
					Infrastructure.Data.Access.Tables.PRS.AdressenAccess.UpdateAnredeCascade(_oldEntity.Anrede, _entity.Anrede);

				var response = Infrastructure.Data.Access.Tables.BSD.Adressen_anredenAccess.Update(_entity);
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
			var AddressContactPersonEntity = Infrastructure.Data.Access.Tables.BSD.Adressen_anredenAccess.Get(this._data.Id);
			if(AddressContactPersonEntity == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Address not found" }
						}
				};
			}
			if(this._data.Description == null || this._data.Description == "")
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Address value should not be empty" }
						}
				};
			}

			var exsist1 = Infrastructure.Data.Access.Tables.BSD.AnsprechpartnerAccess.GetByAddress(AddressContactPersonEntity.Anrede);
			var exsist2 = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByAnrede(AddressContactPersonEntity.Anrede);
			if(exsist1 != null && exsist1.Count > 0 || exsist2 != null && exsist2.Count > 0)
			{
				var nr = exsist1?.Select(x => x.Nummer ?? -1)?.ToList() ?? new List<int>();
				nr.AddRange(exsist2?.Select(x => x.Nr)?.ToList() ?? new List<int>());
				var addressEntities = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(nr);
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = $"Cannot update Anrede. The following Customers/Suppliers/Contact Person(s) use this Anrede. [{string.Join(" | ", addressEntities?.Take(5).Select(x => $"{(x.Lieferantennummer+" "+ x.Kundennummer).Trim()} - {x.Name1}")?.ToList()) }]" }
						}
				};
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
