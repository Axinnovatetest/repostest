using System;
using System.Linq;

namespace Psz.Core.FinanceControl.Handlers.Accounting;

public class PSZ_BH_Kontenrahmen_LogHandler: IHandle<PSZ_BH_Kontenrahmen_LogRequestModel, ResponseModel<IPaginatedResponseModel<PSZ_BH_Kontenrahmen_LogModel>>>
{

	private PSZ_BH_Kontenrahmen_LogRequestModel _data { get; set; }
	private UserModel _user { get; set; }
	public PSZ_BH_Kontenrahmen_LogHandler(UserModel user, PSZ_BH_Kontenrahmen_LogRequestModel data)
	{
		this._user = user;
		this._data = data;
	}
	public ResponseModel<IPaginatedResponseModel<PSZ_BH_Kontenrahmen_LogModel>> Handle()
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

	private ResponseModel<IPaginatedResponseModel<PSZ_BH_Kontenrahmen_LogModel>> Perform(UserModel user, PSZ_BH_Kontenrahmen_LogRequestModel data)
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
					"LogType" => "LogType",
					"LogText" => "LogText",
					_ => "DateTime"
				};
				dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
				{
					SortFieldName = sortFieldName,
					SortDesc = this._data.SortDesc,
				};
			}
			var fetchedData = Infrastructure.Data.Access.Tables.FNC.PSZ_BH_Kontenrahmen_FNC_LogAccess.GetWithPagination(dataPaging, dataSorting, 0);
			var restoreturn = fetchedData.Select(x => new PSZ_BH_Kontenrahmen_LogModel(x)).ToList();

			int TotalCount = 0;
			if(fetchedData is not null && fetchedData.Count > 0)
			{
				TotalCount = fetchedData.FirstOrDefault().TotalCount;
			}
			return ResponseModel<IPaginatedResponseModel<PSZ_BH_Kontenrahmen_LogModel>>.SuccessResponse(
		 new IPaginatedResponseModel<PSZ_BH_Kontenrahmen_LogModel>
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
	public ResponseModel<IPaginatedResponseModel<PSZ_BH_Kontenrahmen_LogModel>> Validate()
	{
		if(_user is null)
		{
			return ResponseModel<IPaginatedResponseModel<PSZ_BH_Kontenrahmen_LogModel>>.AccessDeniedResponse();
		}
		return ResponseModel<IPaginatedResponseModel<PSZ_BH_Kontenrahmen_LogModel>>.SuccessResponse();
	}
}
