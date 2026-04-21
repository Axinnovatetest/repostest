using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Statistics.CustomsModel;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Statistics.CustomsHandle;

public class LieferantenGruppeHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<LieferantenGruppeModel>>>
{

	private Identity.Models.UserModel _user { get; set; }
	public LieferantenGruppeHandler(Identity.Models.UserModel user)
	{
		this._user = user;
	}
	public ResponseModel<List<LieferantenGruppeModel>> Handle()
	{
		try
		{
			var validation = Validate();
			if(!validation.Success)
			{
				return validation;
			}
			return Perform(this._user);
		} catch(Exception e)
		{
			Infrastructure.Services.Logging.Logger.Log(e);
			throw;
		}
	}

	private ResponseModel<List<LieferantenGruppeModel>> Perform(Identity.Models.UserModel user)
	{
		try
		{
			var fetchedData = Infrastructure.Data.Access.Joins.Logistics.CustomsAccess.CustomsAccess.GetSupplierGroups();
			var restoreturn = fetchedData.Select(x => new LieferantenGruppeModel(x)).ToList();

			return ResponseModel<List<LieferantenGruppeModel>>.SuccessResponse(restoreturn);

		} catch(Exception ex)
		{
			Infrastructure.Services.Logging.Logger.Log(ex);
			throw;
		}
	}
	public ResponseModel<List<LieferantenGruppeModel>> Validate()
	{
		if(_user is null)
		{
			return ResponseModel<List<LieferantenGruppeModel>>.AccessDeniedResponse();
		}
		return ResponseModel<List<LieferantenGruppeModel>>.SuccessResponse();
	}
}
