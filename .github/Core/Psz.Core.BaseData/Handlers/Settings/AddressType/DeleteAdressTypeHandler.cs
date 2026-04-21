using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.AddressType
{
	public class DeleteAdressTypeHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public DeleteAdressTypeHandler(Identity.Models.UserModel user, int data)
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
				var adressTypeEntity = Infrastructure.Data.Access.Tables.BSD.Adressen_typenAccess.Get(this._data);
				var logType = Enums.ObjectLogEnums.LogType.Delete;
				var log = ObjectLogHelper.getLog(this._user, this._data, "Adress types", adressTypeEntity.Bezeichnung, null, Enums.ObjectLogEnums.Objects.Adress_types.GetDescription(), logType);
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(log);
				var responseBody = Infrastructure.Data.Access.Tables.BSD.Adressen_typenAccess.Delete(this._data);
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
			var adressTypeEntity = Infrastructure.Data.Access.Tables.BSD.Adressen_typenAccess.Get(this._data);
			if(adressTypeEntity == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Adress type not found" }
						}
				};
			}
			var addressEntities = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByAddressType(this._data);
			if(addressEntities != null && addressEntities.Count > 0)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = $"Cannot delete Adress type. The following Customers/Suppliers Contact Person(s) use this Adress type. [{string.Join(" | ", addressEntities?.Take(5).Select(x => $"{(x.Kundennummer+ " "+x.Lieferantennummer).Trim()} - {x.Name1}")?.ToList()) }]" }
						}
				};
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
