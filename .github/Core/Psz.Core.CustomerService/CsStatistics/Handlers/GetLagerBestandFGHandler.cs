using Psz.Core.Common.Models;
using Psz.Core.CustomerService.CsStatistics.Models;
using Psz.Core.SharedKernel.Interfaces;
using System;
using System.Linq;

namespace Psz.Core.CustomerService.CsStatistics.Handlers
{
	public class GetLagerBestandFGHandler: IHandle<Identity.Models.UserModel, ResponseModel<LagerBestandFGResponseModel>>
	{
		private LagerBestandFGRequestModel _data { get; set; }
		private Identity.Models.UserModel _user { get; set; }
		public GetLagerBestandFGHandler(Identity.Models.UserModel user, LagerBestandFGRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<LagerBestandFGResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}

				var response = new LagerBestandFGResponseModel();
				var _sorting = new Infrastructure.Data.Access.Settings.SortingModel
				{
					SortDesc = this._data.SortDesc,
					SortFieldName = this._data.SortField
				};
				var _paging = new Infrastructure.Data.Access.Settings.PaginModel
				{
					FirstRowNumber = this._data.RequestedPage * this._data.PageSize,
					RequestRows = this._data.PageSize
				};
				var LagerBetandFGEntity = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetLagerBestandFG(_sorting, _paging, this._data.Artikelnummer, this._data.Kunde);
				var _total_count = Infrastructure.Data.Access.Joins.CTS.CSStatisticsAccess.GetLagerBestandFG_Count(this._data.Artikelnummer, this._data.Kunde);

				if(LagerBetandFGEntity != null && LagerBetandFGEntity.Count > 0)
					response = new LagerBestandFGResponseModel
					{
						Items = LagerBetandFGEntity?.Select(x => new LagerBestandFGModel(x))?.ToList(),
						PageSize = this._data.PageSize,
						PageRequested = this._data.RequestedPage,
						TotalCount = _total_count,
						TotalPageCount = (int)Math.Ceiling((decimal)_total_count / this._data.PageSize)
					};

				return ResponseModel<LagerBestandFGResponseModel>.SuccessResponse(response);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<LagerBestandFGResponseModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<LagerBestandFGResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<LagerBestandFGResponseModel>.SuccessResponse();
		}
	}
}
