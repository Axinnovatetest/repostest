using System;
using System.Linq;



namespace Psz.Core.FinanceControl.Handlers.Accounting;

public class LiquiditatsplanungSkontozahlerAccountingHandler: IHandle<UserModel, ResponseModel<IPaginatedResponseModel<LiquiditatsplanungSkontozahlerModel>>>
{

	private UserModel _user { get; set; }
	public LiquiditatsplanungSkontozahlerAccountingHandler(UserModel user)
	{
		this._user = user;
	}
	public ResponseModel<IPaginatedResponseModel<LiquiditatsplanungSkontozahlerModel>> Handle()
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

	private ResponseModel<IPaginatedResponseModel<LiquiditatsplanungSkontozahlerModel>> Perform(UserModel user)
	{
		try
		{
			Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;

			var fetchedData = Infrastructure.Data.Access.Joins.FNC.Accounting.AccountingAccess.GetLiquiditatsplanungSkontozahler(null, null, 0);
			var restoreturn = fetchedData.Select(x => new LiquiditatsplanungSkontozahlerModel(x)).ToList();

			int TotalCount = 0;
			if(fetchedData is not null && fetchedData.Count > 0)
			{
				TotalCount = fetchedData.FirstOrDefault().TotalCount;
			}
			return ResponseModel<IPaginatedResponseModel<LiquiditatsplanungSkontozahlerModel>>.SuccessResponse(
		 new IPaginatedResponseModel<LiquiditatsplanungSkontozahlerModel>
		 {
			 Items = restoreturn,
			 // PageRequested = this._data.RequestedPage,
			 // PageSize = this._data.PageSize,
			 TotalCount = TotalCount,
			 // TotalPageCount = this._data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(TotalCount > 0 ? TotalCount : 0) / this._data.PageSize)) : 0
		 });
		} catch(Exception ex)
		{
			Infrastructure.Services.Logging.Logger.Log(ex);
			throw;
		}
	}
	public ResponseModel<IPaginatedResponseModel<LiquiditatsplanungSkontozahlerModel>> Validate()
	{
		if(_user is null)
		{
			return ResponseModel<IPaginatedResponseModel<LiquiditatsplanungSkontozahlerModel>>.AccessDeniedResponse();
		}
		return ResponseModel<IPaginatedResponseModel<LiquiditatsplanungSkontozahlerModel>>.SuccessResponse();
	}
}
