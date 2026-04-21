using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.Settings.SuppliersGroup
{
	public class DeleteSupplierGroupHandler: IHandle<Identity.Models.UserModel, ResponseModel<int>>
	{
		private Identity.Models.UserModel _user { get; set; }
		private int _data { get; set; }

		public DeleteSupplierGroupHandler(Identity.Models.UserModel user, int data)
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
				var supplierGroupEntity = Infrastructure.Data.Access.Tables.BSD.PszLieferantengruppenAccess.Get(this._data);
				var logType = Enums.ObjectLogEnums.LogType.Delete;
				var log = ObjectLogHelper.getLog(this._user, this._data, "Supplier Group", supplierGroupEntity.Lieferantengruppe, null, Enums.ObjectLogEnums.Objects.Supplier_group.GetDescription(), logType);
				Infrastructure.Data.Access.Tables.PRS.ObjectLogAccess.Insert(log);
				var response = Infrastructure.Data.Access.Tables.BSD.PszLieferantengruppenAccess.Delete(this._data);
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
			var supplierGroupEntity = Infrastructure.Data.Access.Tables.BSD.PszLieferantengruppenAccess.Get(this._data);
			if(supplierGroupEntity == null)
			{
				return new ResponseModel<int>()
				{
					Errors = new List<ResponseModel<int>.ResponseError>{
							new ResponseModel<int>.ResponseError() { Key = "", Value = "Supplier Group not found" }
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
							new ResponseModel<int>.ResponseError() { Key = "", Value = $"Cannot delete Group. The following supplier(s) use this Supplier Group. [{string.Join(" | ", addressEntities?.Take(5).Select(x => $"{x.Lieferantennummer} - {x.Name1}")?.ToList()) }]" }
						}
				};
			}
			return ResponseModel<int>.SuccessResponse();
		}
	}
}
