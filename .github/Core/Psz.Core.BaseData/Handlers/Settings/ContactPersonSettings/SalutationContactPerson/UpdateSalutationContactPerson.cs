using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.ContactPersonSettings.SalutationContactPerson
{
	public class UpdateSalutationContactPerson: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		public Models.Settings.SalutationContactPerson.SalutationContactPersonModel _data { get; set; }
		public UpdateSalutationContactPerson(Identity.Models.UserModel user, Models.Settings.SalutationContactPerson.SalutationContactPersonModel data)
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
				var _oldEntity = Infrastructure.Data.Access.Tables.BSD.Adressen_briefanredenAccess.Get(_entity.ID);
				if(_oldEntity.Anrede != _entity.Anrede)
					Infrastructure.Data.Access.Tables.PRS.AdressenAccess.UpdateSalutationCascade(_oldEntity.Anrede, _entity.Anrede);

				var response = Infrastructure.Data.Access.Tables.BSD.Adressen_briefanredenAccess.Update(_entity);
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
			var salutationContactPersonEntity = Infrastructure.Data.Access.Tables.BSD.Adressen_briefanredenAccess.Get(this._data.Id);
			if(salutationContactPersonEntity == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Salutation not found" }
						}
				};
			}
			if(this._data.Salutation == null || this._data.Salutation == "")
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Salutation value should not be empty" }
						}
				};
			}
			var salutationEntities = Infrastructure.Data.Access.Tables.BSD.Adressen_briefanredenAccess.Get();
			var theRest = salutationEntities.Where(x => x.ID != this._data.Id);
			var check = theRest.Where(x => x.Anrede == this._data.Salutation);
			if(check != null && check.Count() > 0)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "A salutation with the same Name already exsists" }
						}
				};
			}

			var exsist1 = Infrastructure.Data.Access.Tables.BSD.AnsprechpartnerAccess.GetBySalutation(salutationContactPersonEntity.Anrede);
			var exsist2 = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.GetByBriefanrede(salutationContactPersonEntity.Anrede);
			if(exsist1 != null && exsist1.Count > 0 || exsist2 != null && exsist2.Count > 0)
			{
				var nr = exsist1?.Select(x => x.Nummer ?? -1)?.ToList() ?? new List<int>();
				nr.AddRange(exsist2?.Select(x => x.Nr)?.ToList() ?? new List<int>());
				var addressEntities = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(nr);
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = $"Cannot update Briefanrede. The following Customers/Suppliers/Contact Person(s) use this Briefanrede. [{string.Join(" | ", addressEntities?.Take(5).Select(x => $"{(x.Lieferantennummer+" "+ x.Kundennummer).Trim()} - {x.Name1}")?.ToList()) }]" }
						}
				};
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
