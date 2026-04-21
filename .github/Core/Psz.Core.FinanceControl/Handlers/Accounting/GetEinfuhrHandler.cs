using System;
using System.Linq;

namespace Psz.Core.FinanceControl.Handlers.Accounting;

public class GetEinfuhrHandler: IHandle<EinfuhrRequestModel, ResponseModel<IPaginatedResponseModel<EinfuhrModel>>>
{

	private EinfuhrRequestModel _data { get; set; }
	private UserModel _user { get; set; }
	public GetEinfuhrHandler(UserModel user, EinfuhrRequestModel data)
	{
		this._user = user;
		this._data = data;
	}
	public ResponseModel<IPaginatedResponseModel<EinfuhrModel>> Handle()
	{
		try
		{
			var validation = Validate();
			if(!validation.Success)
			{
				return validation;
			}
			return Perform(this._user, this._data);
		} catch(Exception e)
		{
			Infrastructure.Services.Logging.Logger.Log(e);
			throw;
		}
	}

	private ResponseModel<IPaginatedResponseModel<EinfuhrModel>> Perform(UserModel user, EinfuhrRequestModel data)
	{
		try
		{
			var fetchedData = Infrastructure.Data.Access.Joins.FNC.Accounting.AccountingAccess.GetEinfuhr(data.fromdate, data.todate, data.gruppe, null, null, 0);
			var restoreturn = fetchedData.Select(x => new EinfuhrModel(x)).ToList();

			int TotalCount = 0;
			if(fetchedData is not null && fetchedData.Count > 0)
			{
				TotalCount = fetchedData.FirstOrDefault().TotalCount;
			}
			return ResponseModel<IPaginatedResponseModel<EinfuhrModel>>.SuccessResponse(
		 new IPaginatedResponseModel<EinfuhrModel>
		 {
			 Items = restoreturn,
			 TotalCount = TotalCount,
		 });
		} catch(Exception ex)
		{
			Infrastructure.Services.Logging.Logger.Log(ex);
			throw;
		}
	}
	public ResponseModel<IPaginatedResponseModel<EinfuhrModel>> Validate()
	{
		if(_user is null)
		{
			return ResponseModel<IPaginatedResponseModel<EinfuhrModel>>.AccessDeniedResponse();
		}
		return ResponseModel<IPaginatedResponseModel<EinfuhrModel>>.SuccessResponse();
	}
}
