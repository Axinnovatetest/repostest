using Psz.Core.Common.Models;
using Psz.Core.Logistics.Models.Statistics.CustomsModel;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Linq;

namespace Psz.Core.Logistics.Handlers.Statistics.CustomsHandle;

public class GetRMDCZHandler: IHandle<RMDCZRequestModel, ResponseModel<IPaginatedResponseModel<RMDCZModel>>>
{

	private RMDCZRequestModel _data { get; set; }
	private Identity.Models.UserModel _user { get; set; }
	public GetRMDCZHandler(Identity.Models.UserModel user, RMDCZRequestModel data)
	{
		this._user = user;
		this._data = data;
	}
	public ResponseModel<IPaginatedResponseModel<RMDCZModel>> Handle()
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

	private ResponseModel<IPaginatedResponseModel<RMDCZModel>> Perform(Identity.Models.UserModel user, RMDCZRequestModel data)
	{
		try
		{

			var fetchedData = Infrastructure.Data.Access.Joins.Logistics.CustomsAccess.CustomsAccess.GetRMDCZ(data.fromdate, data.todate, null, null, 0);
			var restoreturn = fetchedData.Select(x => new RMDCZModel(x)).ToList();

			int TotalCount = 0;
			if(fetchedData is not null && fetchedData.Count > 0)
			{
				TotalCount = fetchedData.FirstOrDefault().TotalCount;
			}
			return ResponseModel<IPaginatedResponseModel<RMDCZModel>>.SuccessResponse(
		 new IPaginatedResponseModel<RMDCZModel>
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
	public ResponseModel<IPaginatedResponseModel<RMDCZModel>> Validate()
	{
		if(_user is null)
		{
			return ResponseModel<IPaginatedResponseModel<RMDCZModel>>.AccessDeniedResponse();
		}
		return ResponseModel<IPaginatedResponseModel<RMDCZModel>>.SuccessResponse();
	}
}
