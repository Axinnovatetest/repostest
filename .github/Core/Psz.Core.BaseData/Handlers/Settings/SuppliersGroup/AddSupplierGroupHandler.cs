using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.SuppliersGroup
{
	public class AddSupplierGroupHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private Models.SuppliersGroup.SuppliersGroupModel _data { get; set; }

		public AddSupplierGroupHandler(Identity.Models.UserModel user, Models.SuppliersGroup.SuppliersGroupModel data)
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
				var response = Infrastructure.Data.Access.Tables.BSD.PszLieferantengruppenAccess.Insert(_entity);
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
			if(string.IsNullOrEmpty(this._data.GroupName))
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Group name should not be empty" }
						}
				};
			}
			var SupplierGroupEntities = Infrastructure.Data.Access.Tables.BSD.PszLieferantengruppenAccess.Get();
			var check = SupplierGroupEntities.Where(x => x.Lieferantengruppe == this._data.GroupName);
			if(check != null && check.Count() > 0)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "A group with the Name already exsists" }
						}
				};
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
