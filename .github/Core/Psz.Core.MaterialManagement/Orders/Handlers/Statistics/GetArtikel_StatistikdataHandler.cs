using Psz.Core.MaterialManagement.Orders.Models.Statistics;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.Statistics
{
	public class GetArtikel_StatistikdataHandler: IHandle<GetArtikelStatisticsModelRequestModel, ResponseModel<IPaginatedResponseModel<GetArtikelStatisticsModel>>>
	{

		private GetArtikelStatisticsModelRequestModel _data { get; set; }
		private UserModel _user { get; set; }
		public GetArtikel_StatistikdataHandler(UserModel user, GetArtikelStatisticsModelRequestModel data)
		{
			this._user = user;
			this._data = data;
		}
		public ResponseModel<IPaginatedResponseModel<GetArtikelStatisticsModel>> Handle()
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

		private ResponseModel<IPaginatedResponseModel<GetArtikelStatisticsModel>> Perform(UserModel user, GetArtikelStatisticsModelRequestModel data)
		{
			try
			{
				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = this._data.PageSize > 0 ? (this._data.RequestedPage * this._data.PageSize) : 0,
					RequestRows = this._data.PageSize
				};

				if(data.prd <= 0)
					return ResponseModel<IPaginatedResponseModel<GetArtikelStatisticsModel>>.NotFoundResponse();

				var fetcheData = Infrastructure.Data.Access.Joins.MTM.Order.Statistics.Offene_mat_bst_access.GetArtikel_Statistik(data.prd, dataPaging, 0);

				if(fetcheData is null || fetcheData.Count == 0)
					return ResponseModel<IPaginatedResponseModel<GetArtikelStatisticsModel>>.NotFoundResponse();

				var res = Page(fetcheData, dataPaging.FirstRowNumber, dataPaging.RequestRows);
				var restoreturn = res.Select(x => new GetArtikelStatisticsModel(x)).ToList();
				int TotalCount = 0;
				if(restoreturn is not null && restoreturn.Count > 0)
				{
					TotalCount = restoreturn.FirstOrDefault().TotalCount;
				}
				return ResponseModel<IPaginatedResponseModel<GetArtikelStatisticsModel>>.SuccessResponse(
			 new IPaginatedResponseModel<GetArtikelStatisticsModel>
			 {
				 Items = restoreturn,
				 PageRequested = this._data.RequestedPage,
				 PageSize = this._data.PageSize,
				 TotalCount = TotalCount,
				 TotalPageCount = this._data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(TotalCount > 0 ? TotalCount : 0) / this._data.PageSize)) : 0
			 });
			} catch(Exception ex)
			{
				throw;
			}
		}
		public ResponseModel<IPaginatedResponseModel<GetArtikelStatisticsModel>> Validate()
		{
			if(_user is null)
			{
				return ResponseModel<IPaginatedResponseModel<GetArtikelStatisticsModel>>.AccessDeniedResponse();
			}
			return ResponseModel<IPaginatedResponseModel<GetArtikelStatisticsModel>>.SuccessResponse();
		}
		private static IEnumerable<GetArtikelStatisticsEntity> Page<GetArtikelStatisticsEntity>(IEnumerable<GetArtikelStatisticsEntity> source, int FirstRowNumber, int RequestRows)
		{
			return source.Skip(FirstRowNumber).Take(RequestRows);
		}
	}
}
