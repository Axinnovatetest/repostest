using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.Supplier
{
	public class GetSuppliersDropdownHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<Models.Supplier.SupplierDropdownModel>>>
	{
		private Identity.Models.UserModel _user { get; set; }

		public GetSuppliersDropdownHandler(Identity.Models.UserModel user)
		{
			this._user = user;
		}
		public ResponseModel<List<Models.Supplier.SupplierDropdownModel>> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var suplliersDropdownEntity = Infrastructure.Data.Access.Joins.Kunden_LieferantenAccess.GetSupplierDropdown();
				var response = new List<Models.Supplier.SupplierDropdownModel>();
				if(suplliersDropdownEntity != null && suplliersDropdownEntity.Count > 0)
				{
					foreach(var item in suplliersDropdownEntity)
					{
						response.Add(new Models.Supplier.SupplierDropdownModel(item));
					}
				}
				return ResponseModel<List<Models.Supplier.SupplierDropdownModel>>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<List<Models.Supplier.SupplierDropdownModel>> Validate()
		{
			if(this._user == null/*
                || this._user.Access.____*/)
			{
				return ResponseModel<List<Models.Supplier.SupplierDropdownModel>>.AccessDeniedResponse();
			}

			return ResponseModel<List<Models.Supplier.SupplierDropdownModel>>.SuccessResponse();
		}
	}
}
