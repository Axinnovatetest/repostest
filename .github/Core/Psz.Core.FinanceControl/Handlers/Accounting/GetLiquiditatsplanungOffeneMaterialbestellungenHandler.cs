using System;
using System.Linq;

namespace Psz.Core.FinanceControl.Handlers.Accounting;

public class GetLiquiditatsplanungOffeneMaterialbestellungenHandler: IHandle<LiquiditatsplanungOffeneMaterialbestellungenRequestModel, ResponseModel<IPaginatedResponseModel<LiquiditatsplanungOffeneMaterialbestellungenModel>>>
{

	private LiquiditatsplanungOffeneMaterialbestellungenRequestModel _data { get; set; }
	private UserModel _user { get; set; }
	public GetLiquiditatsplanungOffeneMaterialbestellungenHandler(UserModel user, LiquiditatsplanungOffeneMaterialbestellungenRequestModel data)
	{
		this._user = user;
		this._data = data;
	}
	public ResponseModel<IPaginatedResponseModel<LiquiditatsplanungOffeneMaterialbestellungenModel>> Handle()
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

	private ResponseModel<IPaginatedResponseModel<LiquiditatsplanungOffeneMaterialbestellungenModel>> Perform(UserModel user, LiquiditatsplanungOffeneMaterialbestellungenRequestModel data)
	{
		try
		{
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
					"Benutzer" => "Benutzer",
					"Lieferantennr" => "Lieferantennr",
					"Lieferant" => "Lieferant",
					"Anzahl" => "Anzahl",
					"Mindestbestellmenge" => "Mindestbestellmenge",
					"Einzelpreis" => "Einzelpreis",
					"Gesamtpreis" => "Gesamtpreis",
					"Bestellnummer" => "Bestellnummer",
					_ => "Benutzer"
				};
				dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
				{
					SortFieldName = sortFieldName,
					SortDesc = this._data.SortDesc,
				};
			}
			var fetchedData = Infrastructure.Data.Access.Joins.FNC.Accounting.AccountingAccess.GetLiquiditatsplanungOffeneMaterialbestellungen(data.fromdate, data.todate, dataPaging, dataSorting, 0);
			var restoreturn = fetchedData.Select(x => new LiquiditatsplanungOffeneMaterialbestellungenModel(x)).ToList();

			int TotalCount = 0;
			if(restoreturn is not null && fetchedData.Count > 0)
			{
				TotalCount = fetchedData.FirstOrDefault().TotalCount;
			}
			return ResponseModel<IPaginatedResponseModel<LiquiditatsplanungOffeneMaterialbestellungenModel>>.SuccessResponse(
		 new IPaginatedResponseModel<LiquiditatsplanungOffeneMaterialbestellungenModel>
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
	public ResponseModel<IPaginatedResponseModel<LiquiditatsplanungOffeneMaterialbestellungenModel>> Validate()
	{
		if(_user is null)
		{
			return ResponseModel<IPaginatedResponseModel<LiquiditatsplanungOffeneMaterialbestellungenModel>>.AccessDeniedResponse();
		}
		return ResponseModel<IPaginatedResponseModel<LiquiditatsplanungOffeneMaterialbestellungenModel>>.SuccessResponse();
	}
}
