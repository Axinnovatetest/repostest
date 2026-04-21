using System;
using System.Linq;

namespace Psz.Core.FinanceControl.Handlers.Accounting;

public class StammdatenkontrolleWareneingangeHandler: IHandle<StammdatenkontrolleWareneingangeRequestModel, ResponseModel<IPaginatedResponseModel<StammdatenkontrolleWareneingangeModel>>>
{

	private StammdatenkontrolleWareneingangeRequestModel _data { get; set; }
	private UserModel _user { get; set; }
	public StammdatenkontrolleWareneingangeHandler(UserModel user, StammdatenkontrolleWareneingangeRequestModel data)
	{
		this._user = user;
		this._data = data;
	}
	public ResponseModel<IPaginatedResponseModel<StammdatenkontrolleWareneingangeModel>> Handle()
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

	private ResponseModel<IPaginatedResponseModel<StammdatenkontrolleWareneingangeModel>> Perform(UserModel user, StammdatenkontrolleWareneingangeRequestModel data)
	{
		try
		{

			if(data.todate.Year - data.fromdate.Year > 5)
			{
				//throw new InvalidOperationException("please provide a shorter periode of time !");
			}
			var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
			{
				FirstRowNumber = this._data.PageSize > 0 ? (this._data.RequestedPage * this._data.PageSize) : 0,
				RequestRows = this._data.PageSize
			};

			Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
			if(!string.IsNullOrWhiteSpace(this._data.SortField))
			{
				string sortFieldName = this._data.SortField switch
				{
					"Lieferantengruppe" => "Lieferantengruppe",
					"Name1" => "Name1",
					"Ursprungsland" => "Ursprungsland",
					_ => "Artikelnummer "
				};
				dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
				{
					SortFieldName = sortFieldName,
					SortDesc = this._data.SortDesc,
				};
			}
			var fetchedData = Infrastructure.Data.Access.Joins.FNC.Accounting.AccountingAccess.GetStammdatenkontrolleWareneingange(data.fromdate, data.todate, data.gruppe, dataPaging, dataSorting, 0);

			var restoreturn = fetchedData.Select(x => new StammdatenkontrolleWareneingangeModel(x)).ToList();

			int TotalCount = 0;
			if(fetchedData is not null && fetchedData.Count > 0)
			{
				TotalCount = fetchedData.FirstOrDefault().TotalCount;
			}
			return ResponseModel<IPaginatedResponseModel<StammdatenkontrolleWareneingangeModel>>.SuccessResponse(
		 new IPaginatedResponseModel<StammdatenkontrolleWareneingangeModel>
		 {
			 Items = restoreturn,
			 PageRequested = this._data.RequestedPage,
			 PageSize = this._data.PageSize,
			 TotalCount = TotalCount,
			 TotalPageCount = this._data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(TotalCount > 0 ? TotalCount : 0) / this._data.PageSize)) : 0
		 });
		} catch(Exception ex)
		{
			Infrastructure.Services.Logging.Logger.Log(ex);
			throw;
		}
	}
	public ResponseModel<IPaginatedResponseModel<StammdatenkontrolleWareneingangeModel>> Validate()
	{
		if(_user is null)
		{
			return ResponseModel<IPaginatedResponseModel<StammdatenkontrolleWareneingangeModel>>.AccessDeniedResponse();
		}
		return ResponseModel<IPaginatedResponseModel<StammdatenkontrolleWareneingangeModel>>.SuccessResponse();
	}
}
