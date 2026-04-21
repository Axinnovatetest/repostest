using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.FinanceControl.Handlers.Accounting;

public class LieferantenGruppeHandler: IHandle<UserModel, ResponseModel<List<LieferantenGruppeModel>>>
{

	private UserModel _user { get; set; }
	public LieferantenGruppeHandler(UserModel user)
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

	private ResponseModel<List<LieferantenGruppeModel>> Perform(UserModel user)
	{
		try
		{
			var fetchedData = Infrastructure.Data.Access.Joins.FNC.Accounting.AccountingAccess.GetSupplierGroups();
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
