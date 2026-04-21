using System;
using System.Linq;

namespace Psz.Core.FinanceControl.Handlers.Accounting;

public class RechnungstransferHandler: IHandle<UserModel, ResponseModel<IPaginatedResponseModel<RechnungstransferModel>>>
{

	private UserModel _user { get; set; }
	public RechnungstransferHandler(UserModel user)
	{
		this._user = user;
	}
	public ResponseModel<IPaginatedResponseModel<RechnungstransferModel>> Handle()
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

	private ResponseModel<IPaginatedResponseModel<RechnungstransferModel>> Perform(UserModel user)
	{
		try
		{



			var fetchedData = Infrastructure.Data.Access.Joins.FNC.Accounting.AccountingAccess.GetRechnungstransfer(user?.Id ?? -1, null, null, 0);
			var restoreturn = fetchedData.Select(x => new RechnungstransferModel(x)).ToList();

			int TotalCount = 0;
			if(fetchedData is not null && fetchedData.Count > 0)
			{
				TotalCount = fetchedData.FirstOrDefault().TotalCount;
			}
			return ResponseModel<IPaginatedResponseModel<RechnungstransferModel>>.SuccessResponse(
		 new IPaginatedResponseModel<RechnungstransferModel>
		 {
			 Items = restoreturn,
			 TotalCount = TotalCount
		 });
			;
		} catch(Exception ex)
		{
			Infrastructure.Services.Logging.Logger.Log(ex);
			throw;
		}
	}
	public ResponseModel<IPaginatedResponseModel<RechnungstransferModel>> Validate()
	{
		if(_user is null)
		{
			return ResponseModel<IPaginatedResponseModel<RechnungstransferModel>>.AccessDeniedResponse();
		}
		return ResponseModel<IPaginatedResponseModel<RechnungstransferModel>>.SuccessResponse();
	}
}
