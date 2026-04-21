using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.CustomersGroup
{
	public class UpdateCustomerGroupHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.CustomersGroup.CustomerGroupModel _data { get; set; }

		public UpdateCustomerGroupHandler(Identity.Models.UserModel user, Models.CustomersGroup.CustomerGroupModel data)
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
				var response = Infrastructure.Data.Access.Tables.BSD.PSZ_KundengruppenAccess.Update(_entity);
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
			var customerGroupEntity = Infrastructure.Data.Access.Tables.BSD.PSZ_KundengruppenAccess.Get(this._data.Id);
			if(customerGroupEntity == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Customer Group not found" }
						}
				};
			}
			if(string.IsNullOrEmpty(this._data.GroupName))
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Customer Group name should not be empty" }
						}
				};
			}
			var CustomerGroupEntities = Infrastructure.Data.Access.Tables.BSD.PSZ_KundengruppenAccess.Get();
			var theRest = CustomerGroupEntities.Where(x => x.ID != this._data.Id);
			var check = theRest.Where(x => x.Kundengruppe == this._data.GroupName);
			if(check != null && check.Count() > 0)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "A Customer group with the Name already exsists" }
						}
				};
			}

			var exsist1 = Infrastructure.Data.Access.Tables.PRS.KundenAccess.GetByCustomerGroup(this._data.Id);
			if(exsist1 != null && exsist1.Count > 0)
			{
				var addressEntities = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(exsist1.Select(x => x.Nummer ?? -1)?.Distinct().ToList());
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = $"Cannot update Group. The following customer(s) use this Customer Group. [{string.Join(" | ", addressEntities?.Take(5).Select(x => $"{x.Kundennummer} - {x.Name1}")?.ToList()) }]" }
						}
				};
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
