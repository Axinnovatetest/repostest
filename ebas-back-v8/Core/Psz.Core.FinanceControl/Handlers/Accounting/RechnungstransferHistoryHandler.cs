using System;
using System.Linq;

namespace Psz.Core.FinanceControl.Handlers.Accounting;

public class RechnungstransferHistoryHandler: IHandle<UserModel, ResponseModel<IPaginatedResponseModel<RechnungstransferModel>>>
{

	private UserModel _user { get; set; }
	private RechnungstransferHistoryRequestModel _data { get; set; }
	public RechnungstransferHistoryHandler(UserModel user, RechnungstransferHistoryRequestModel data)
	{
		this._user = user;
		this._data = data;
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
			return Perform();
		} catch(Exception e)
		{
			Infrastructure.Services.Logging.Logger.Log(e);
			throw;
		}
	}

	private ResponseModel<IPaginatedResponseModel<RechnungstransferModel>> Perform()
	{
		try
		{



			var fetchedData = Infrastructure.Data.Access.Joins.FNC.Accounting.AccountingAccess.GetRechnungstransferHistory(this._data.DateFrom ?? DateTime.Today, this._data.DateTill ?? DateTime.Today);
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
