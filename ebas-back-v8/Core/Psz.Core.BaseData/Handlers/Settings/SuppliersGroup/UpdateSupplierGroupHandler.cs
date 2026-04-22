using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.SuppliersGroup
{
	public class UpdateSupplierGroupHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.SuppliersGroup.SuppliersGroupModel _data { get; set; }

		public UpdateSupplierGroupHandler(Identity.Models.UserModel user, Models.SuppliersGroup.SuppliersGroupModel data)
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
				var response = Infrastructure.Data.Access.Tables.BSD.PszLieferantengruppenAccess.Update(_entity);
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
			var supplierGroupEntity = Infrastructure.Data.Access.Tables.BSD.PszLieferantengruppenAccess.Get(this._data.Id);
			if(supplierGroupEntity == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Supplier Group not found" }
						}
				};
			}
			if(string.IsNullOrEmpty(this._data.GroupName))
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Supplier Group name should not be empty" }
						}
				};
			}
			var SupplierGroupEntities = Infrastructure.Data.Access.Tables.BSD.PszLieferantengruppenAccess.Get();
			var theRest = SupplierGroupEntities.Where(x => x.ID != this._data.Id);
			var check = theRest.Where(x => x.Lieferantengruppe == this._data.GroupName);
			if(check != null && check.Count() > 0)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "A Supplier group with the Name already exsists" }
						}
				};
			}

			var exsist1 = Infrastructure.Data.Access.Tables.BSD.LieferantenAccess.GetBySupplierGroup(supplierGroupEntity.Lieferantengruppe);
			if(exsist1 != null && exsist1.Count > 0)
			{
				var addressEntities = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(exsist1.Select(x => x.Nummer ?? -1)?.Distinct().ToList());
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = $"Cannot update Group. The following supplier(s) use this Supplier Group. [{string.Join(" | ", addressEntities?.Take(5).Select(x => $"{x.Lieferantennummer} - {x.Name1}")?.ToList()) }]" }
						}
				};
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
