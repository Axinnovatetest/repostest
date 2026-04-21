using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.ContactPersonSettings.SalutationContactPerson
{
	public class AddSalutationContactPersonHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public Models.Settings.SalutationContactPerson.SalutationContactPersonModel _data { get; set; }

		public AddSalutationContactPersonHandler(Identity.Models.UserModel user, Models.Settings.SalutationContactPerson.SalutationContactPersonModel data)
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
				var response = Infrastructure.Data.Access.Tables.BSD.Adressen_briefanredenAccess.Insert(_entity);
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

			if(string.IsNullOrEmpty(this._data.Salutation))
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Salutation value should not be empty" }
						}
				};
			}
			var saluationContactPersonEntities = Infrastructure.Data.Access.Tables.BSD.Adressen_briefanredenAccess.Get();
			var check = saluationContactPersonEntities.Where(x => x.Anrede == this._data.Salutation);
			if(check != null && check.Count() > 0)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "A salutation with the same name exsists" }
						}
				};
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
