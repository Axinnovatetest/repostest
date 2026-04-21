using Psz.Core.BaseData.Models.Article.ROH.OfferRequests;
using Psz.Core.Common.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Handlers.ROH.OfferRequests;

public class VerifySelectedSupplierInformationsHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<VerifySupplierInformationModel>>>
{
	private Identity.Models.UserModel _user { get; set; }
	private List<int> _data { get; set; }

	public VerifySelectedSupplierInformationsHandler(Identity.Models.UserModel user, List<int> data)
	{
		this._user = user;
		this._data = data;
	}

	public ResponseModel<List<VerifySupplierInformationModel>> Handle()
	{

		try
		{
			if(!Validate().Success)
			{
				return ResponseModel<List<VerifySupplierInformationModel>>.AccessDeniedResponse();
			}

			var restoreturn = new List<VerifySupplierInformationModel>();

			if(_data.Count == 0)
			{
				restoreturn.Add(new VerifySupplierInformationModel() { IsValid = false, Message = "At least one Supplier Needed !" });
			}
			foreach(var item in _data)
			{
				var supplier = Infrastructure.Data.Access.Tables.PRS.AdressenAccess.Get(item);

				var valid = Infrastructure.Services.Helpers.EmailHelper.IsValidEmailAdress(supplier.EMail) && ValidName(supplier.Name1);
				if(!valid)
				{
					restoreturn.Add(new VerifySupplierInformationModel() { IsValid = valid, Message = $"the Supplier {supplier.Name1} does not has valid adresses and informations !" });
				}
			}

			return ResponseModel<List<VerifySupplierInformationModel>>.SuccessResponse(restoreturn);


		} catch(Exception e)
		{
			Infrastructure.Services.Logging.Logger.Log(e);
			throw;
		}

	}

	public ResponseModel<List<VerifySupplierInformationModel>> Validate()
	{
		if(this._user == null /*|| this._user.Access.____*/)
		{
			return ResponseModel<List<VerifySupplierInformationModel>>.AccessDeniedResponse();
		}
		return ResponseModel<List<VerifySupplierInformationModel>>.SuccessResponse();
	}
	public bool ValidName(string name)
	{
		return !(string.IsNullOrEmpty(name) && string.IsNullOrWhiteSpace(name));
	}
}
