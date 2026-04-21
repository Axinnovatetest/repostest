using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.AddressType
{
	public class UpdateAdressTypeHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.AddressType.AdreesTypeModel _data { get; set; }

		public UpdateAdressTypeHandler(Identity.Models.UserModel user, Models.AddressType.AdreesTypeModel data)
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

				var _entity = this._data.ToTypenEntity();
				var responseBody = Infrastructure.Data.Access.Tables.BSD.Adressen_typenAccess.Update(_entity);
				return ResponseModel<int>.SuccessResponse(responseBody);
			} catch(Exception e)
			{
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
			var adressTypeEntity = Infrastructure.Data.Access.Tables.BSD.Adressen_typenAccess.Get(this._data.Id);
			if(adressTypeEntity == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Adress type not found" }
						}
				};
			}
			if(string.IsNullOrEmpty(this._data.Description))
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Description should not be empty" }
						}
				};
			}
			var adressTypesEntities = Infrastructure.Data.Access.Tables.BSD.Adressen_typenAccess.Get();
			var theRest = adressTypesEntities.Where(x => x.ID_typ != this._data.Id);
			var check = theRest.Where(x => x.Bezeichnung == this._data.Description);
			if(check != null && check.Count() > 0)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "An adress type with the same decription already exsists" }
						}
				};
			}

			var addressEntities = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByAddressType(this._data.Id);
			if(addressEntities != null && addressEntities.Count > 0)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = $"Cannot update Adress type. The following Customers/Suppliers Contact Person(s) use this Adress type. [{string.Join(" | ", addressEntities?.Take(5).Select(x => $"{(x.Kundennummer+ " "+x.Lieferantennummer).Trim()} - {x.Name1}")?.ToList()) }]" }
						}
				};
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
