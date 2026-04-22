using System;
using System.Linq;

namespace Psz.Core.FinanceControl.Handlers.Accounting;

public class GetAusfuhrHandler: IHandle<AusfuhrRequestModel, ResponseModel<IPaginatedResponseModel<AusfuhrModel>>>
{

	private AusfuhrRequestModel _data { get; set; }
	private UserModel _user { get; set; }
	public GetAusfuhrHandler(UserModel user, AusfuhrRequestModel data)
	{
		this._user = user;
		this._data = data;
	}
	public ResponseModel<IPaginatedResponseModel<AusfuhrModel>> Handle()
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

	private ResponseModel<IPaginatedResponseModel<AusfuhrModel>> Perform(UserModel user, AusfuhrRequestModel data)
	{
		try
		{


			var fetchedData = Infrastructure.Data.Access.Joins.FNC.Accounting.AccountingAccess.GetAusfuhr(data.fromdate, data.todate, null, null, 0);
			var restoreturn = fetchedData.Select(x => new AusfuhrModel(x)).ToList();

			int TotalCount = 0;
			if(fetchedData is not null && fetchedData.Count > 0)
			{
				TotalCount = fetchedData.FirstOrDefault().TotalCount;
			}
			return ResponseModel<IPaginatedResponseModel<AusfuhrModel>>.SuccessResponse(
		 new IPaginatedResponseModel<AusfuhrModel>
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
	public ResponseModel<IPaginatedResponseModel<AusfuhrModel>> Validate()
	{
		if(_user is null)
		{
			return ResponseModel<IPaginatedResponseModel<AusfuhrModel>>.AccessDeniedResponse();
		}
		return ResponseModel<IPaginatedResponseModel<AusfuhrModel>>.SuccessResponse();
	}
}
