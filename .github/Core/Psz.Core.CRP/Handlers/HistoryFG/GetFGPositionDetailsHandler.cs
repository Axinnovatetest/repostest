using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.HistoryFG;
using Psz.Core.Identity.Models;
using Psz.Core.SharedKernel.Interfaces;

namespace Psz.Core.CRP.Handlers.HistoryFG
{
	public class GetFGPositionDetailsHandler: IHandle<UserModel, ResponseModel<HistoryFGDetailsResponseModel>>
	{
		private readonly HistoryDataFGDetailsRequestModel? _request;
		private readonly UserModel? _user;
		public GetFGPositionDetailsHandler(HistoryDataFGDetailsRequestModel request, Core.Identity.Models.UserModel user)
		{
			_request = request;
			_user = user;
		}

		public ResponseModel<HistoryFGDetailsResponseModel> Handle()
		{
			try
			{
				var validationResponse = this.Validate();
				if(!validationResponse.Success)
				{
					return validationResponse;
				}
				var HistoryFGHeaderList = new List<Infrastructure.Data.Entities.Tables.CRP.HistoryDetailsFGBestandEntity>();
				string searchValue = this._request.SearchValue;

				Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
				Infrastructure.Data.Access.Settings.PaginModel dataPaging = null;
				if(!string.IsNullOrWhiteSpace(this._request.SortField))
				{
					var sortFieldName = "";
					switch(this._request.SortField.ToLower())
					{
						default:
						case "id":
							sortFieldName = "[Id]";
							break;
						case "articledesignation1":
							sortFieldName = "[ArticleDesignation1]";
							break;
						case "articledesignation2":
							sortFieldName = "[ArticleDesignation2]";
							break;
						case "articlenumber":
							sortFieldName = "[ArticleNumber]";
							break;
						case "articlereleasestatus":
							sortFieldName = "[ArticleReleaseStatus]";
							break;
						case "cscontact":
							sortFieldName = "[CsContact]";
							break;
						case "edistandard":
							sortFieldName = "[EdiStandard]";
							break;
						case "headerid":
							sortFieldName = "[HeaderId]";
							break;
						case "stockquantity":
							sortFieldName = "[StockQuantity]";
							break;
						case "totalcostswithcu":
							sortFieldName = "[TotalCostsWithCu]";
							break;
						case "totalcostsWithoutCu":
							sortFieldName = "[TotalCostsWithoutCu]";
							break;
						case "totalsalesprice":
							sortFieldName = "[TotalSalesPrice]";
							break;
						case "ubg":
							sortFieldName = "[UBG]";
							break;
						case "unitsalesprice":
							sortFieldName = "[UnitSalesPrice]";
							break;
						case "warehouseid":
							sortFieldName = "[WarehouseId]";
							break;
						case "warehousename":
							sortFieldName = "[WarehouseName]";
							break;
					}

					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = this._request.SortDesc,
					};
				}
				int totalCount = Infrastructure.Data.Access.Tables.CRP.HistoryFG.HistoryDetailsFGBestandAccess.CountWithSearchPosition(_request.IdSearch ?? -1, _request.SearchValue);

				dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = this._request.PageSize > 0 ? (this._request.RequestedPage * this._request.PageSize) : 0,
					RequestRows = this._request.FullData ? totalCount : this._request.PageSize

				};


				HistoryFGHeaderList = Infrastructure.Data.Access.Tables.CRP.HistoryFG.HistoryDetailsFGBestandAccess.GetWithSearchPosition(_request.IdSearch ?? -1, _request.SearchValue, dataSorting, dataPaging);
				var r = this._request.PageSize > 0 ? totalCount / decimal.Parse((this._request.PageSize).ToString()) : 0;
				if(HistoryFGHeaderList is null || HistoryFGHeaderList.Count == 0)
					return ResponseModel<HistoryFGDetailsResponseModel>.SuccessResponse(new HistoryFGDetailsResponseModel
					{
						Items = new List<HistoryDataFGDetailsResponseModel>(),
						PageRequested = this._request.RequestedPage,
						PageSize = this._request.PageSize,
						TotalCount = totalCount,
						TotalPageCount = (int)Math.Ceiling(decimal.Parse(r.ToString())),
					});

				var result = new List<HistoryDataFGDetailsResponseModel>();

				foreach(var history in HistoryFGHeaderList)
				{
					string articleNr = Infrastructure.Data.Access.Tables.ArtikelAccess.GetArtikelId(history.ArticleNumber);
					result.Add(new HistoryDataFGDetailsResponseModel(history, Convert.ToInt32(articleNr)));
				}


				return ResponseModel<HistoryFGDetailsResponseModel>.SuccessResponse(new HistoryFGDetailsResponseModel
				{
					Items = result,
					PageRequested = this._request.RequestedPage,
					PageSize = this._request.PageSize,
					TotalCount = totalCount,
					TotalPageCount = (int)Math.Ceiling(decimal.Parse(r.ToString())),
				});


			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}

		public ResponseModel<HistoryFGDetailsResponseModel> Validate()
		{
			if(this._user == null/*|| this._user.Access.____*/)
			{
				return ResponseModel<HistoryFGDetailsResponseModel>.AccessDeniedResponse();
			}

			return ResponseModel<HistoryFGDetailsResponseModel>.SuccessResponse();
		}
	}
}
