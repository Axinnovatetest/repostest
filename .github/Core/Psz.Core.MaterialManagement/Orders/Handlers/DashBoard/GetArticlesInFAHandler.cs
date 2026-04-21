using Psz.Core.MaterialManagement.Orders.Models.DashBoard;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.Orders.Handlers.DashBoard
{
	public class GetArticlesInFAHandler: IHandle<GetArticleInFaRequestModel, ResponseModel<IPaginatedResponseModel<GetArticleInFaResponseModel>>>
	{
		private GetArticleInFaRequestModel data { get; set; }
		private UserModel user { get; set; }
		public GetArticlesInFAHandler(UserModel user, GetArticleInFaRequestModel data)
		{
			this.data = data;
			this.user = user;
		}
		public ResponseModel<IPaginatedResponseModel<GetArticleInFaResponseModel>> Handle()
		{
			try
			{
				var validation = Validate();
				if(!validation.Success)
				{
					return validation;
				}

				return Perform(this.user, this.data);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		private ResponseModel<IPaginatedResponseModel<GetArticleInFaResponseModel>> Perform(UserModel user, GetArticleInFaRequestModel data)
		{
			// - 2023-02-09 - avoid Error when api gets called before Country/Unit init in frontend
			if(data.CountryId <= 0 || data.UnitId <= 0)
			{
				return ResponseModel<IPaginatedResponseModel<GetArticleInFaResponseModel>>.SuccessResponse(null);
			}
			//  - 
			lock(CRP.Locks.ArticlesReadLock)
			{
				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = this.data.PageSize > 0 ? (this.data.RequestedPage * this.data.PageSize) : 0,
					RequestRows = this.data.PageSize
				};

				var articlesFiltered = Infrastructure.Data.Access.Joins.MTM.Order.ArticlesInOpenFaAccess.getArticlesFiltered(
									MaterialManagement.Helpers.SpecialHelper.AdditionalLagers(data.CountryId, data.UnitId),
									MaterialManagement.Helpers.SpecialHelper.GetMainAndProductionLagers(data.CountryId, data.UnitId).Item2,
									MaterialManagement.Helpers.SpecialHelper.GetMainAndProductionLagers(data.CountryId, data.UnitId).Item1,
									dataPaging,
									data.Months <= 0 ? 1 : data.Months,
									data.ArtikelNr
									);


				if(articlesFiltered != null && articlesFiltered.Count > 0)
				{
					var totalCount = articlesFiltered != null ? articlesFiltered.First().TotalCount : 0;
					return ResponseModel<IPaginatedResponseModel<GetArticleInFaResponseModel>>.SuccessResponse(new IPaginatedResponseModel<GetArticleInFaResponseModel>
					{
						Items = articlesFiltered.Select(x => new GetArticleInFaResponseModel(x)).ToList(),
						PageRequested = this.data.RequestedPage,
						PageSize = this.data.PageSize,
						TotalCount = totalCount,
						TotalPageCount = this.data.PageSize > 0 ? (int)Math.Ceiling(((decimal)totalCount / this.data.PageSize)) : 0
					});
				}
				return ResponseModel<IPaginatedResponseModel<GetArticleInFaResponseModel>>.NotFoundResponse();

			}
		}



		public ResponseModel<IPaginatedResponseModel<GetArticleInFaResponseModel>> Validate()
		{
			if(user is null)
			{
				return ResponseModel<IPaginatedResponseModel<GetArticleInFaResponseModel>>.AccessDeniedResponse();
			}
			//if(data.CountryId <= 0 || data.UnitId <= 0)
			//{
			//	return ResponseModel<IPaginatedResponseModel<GetArticleInFaResponseModel>>.AccessDeniedResponse();
			//}

			return ResponseModel<IPaginatedResponseModel<GetArticleInFaResponseModel>>.SuccessResponse();
		}
	}
}
