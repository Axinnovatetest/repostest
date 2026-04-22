using Psz.Core.MaterialManagement.Orders.Models.DashBoard;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Psz.Core.MaterialManagement.Orders.Handlers.DashBoard
{
	public class GetFaultyArticlesInOpenFaByWeekHandler: IHandle<FaultyArticlesInOpenFasRequestModel, Task<ResponseModel<IPaginatedResponseModel<ArticleAndFaultyWeek>>>>
	{
		private FaultyArticlesInOpenFasRequestModel data { get; set; }
		private UserModel user { get; set; }

		public GetFaultyArticlesInOpenFaByWeekHandler(UserModel user, FaultyArticlesInOpenFasRequestModel data)
		{
			this.data = data;
			this.user = user;
		}
		public async Task<ResponseModel<IPaginatedResponseModel<ArticleAndFaultyWeek>>> Handle()
		{
			try
			{
				var validation = Validate();
				if(!validation.Success)
				{
					return validation;
				}

				return await Perform(this.user, this.data);
			} catch(Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		private async Task<ResponseModel<IPaginatedResponseModel<ArticleAndFaultyWeek>>> Perform(UserModel user, FaultyArticlesInOpenFasRequestModel data)
		{
			var response = new IPaginatedResponseModel<ArticleAndFaultyWeek>();
			var mydatatoreturn = new List<ArticleAndFaultyWeek>();
			var fetchedArticles = await Infrastructure.Data.Access.Joins.MTM.Order.ArticlesInOpenFaAccess.GetListOfArticlesInOpenFasByWeekAsync(Psz.Core.MaterialManagement.Helpers.SpecialHelper.AdditionalLagers(data.CountryId, data.UnitId), data.Family);
			int TotalCount = default;
			var articlesWithNeed = new List<FaultyArticlesInOpenFasPerWeekResponseModel>();
			var orders = new List<OrdersForArtikelByWeekModel>();

			//if (fetchedArticles.Count() == 0 || fetchedArticles is null)
			//	return ResponseModel<IPaginatedResponseModel<ArticleAndFaultyWeek>>.NotFoundResponse();

			var articlesNrsOnly = fetchedArticles.Select(x => x.ArtikelNr).Distinct().ToList();
			var FetchedOreders = await Infrastructure.Data.Access.Joins.MTM.Order.OrderedArticlesAccess.GetOrdersByArtikelNrAndWeekAsync(Psz.Core.MaterialManagement.Helpers.SpecialHelper.AdditionalLagers(data.CountryId, data.UnitId));

			articlesWithNeed = fetchedArticles?.Select(item => new FaultyArticlesInOpenFasPerWeekResponseModel(item)).ToList();



			orders = FetchedOreders?.Select(x => new OrdersForArtikelByWeekModel(x)).ToList();
			orders = orders.Where(x => articlesNrsOnly.Contains(x.artikelNr)).ToList();
			var mstocks = await Infrastructure.Data.Access.Tables.MTM.LagerorteAccess.GetActualStockInLagerForArticlesByTypeAsync(
						  Psz.Core.MaterialManagement.Helpers.SpecialHelper.AdditionalLagers(data.CountryId, data.UnitId)
						  ,
						  articlesNrsOnly
						  ,
						  MaterialManagement.Helpers.SpecialHelper.GetMainAndProductionLagers(data.CountryId, data.UnitId).Item1
						  , MaterialManagement.Helpers.SpecialHelper.GetMainAndProductionLagers(data.CountryId, data.UnitId).Item2
				  );
			foreach(var item in articlesNrsOnly)
			{
				var fasListOfSpeceficArtikelNr = articlesWithNeed.Where(x => x.Artikel_Nr == item).ToList();

				var fetchedStocknotused = await Infrastructure.Data.Access.Tables.MTM.LagerorteAccess.GetActualStockInLagerPerArticleByTypeAsync
					  (
							Psz.Core.MaterialManagement.Helpers.SpecialHelper.AdditionalLagers(data.CountryId, data.UnitId)
							,
							item
							,
							MaterialManagement.Helpers.SpecialHelper.GetMainAndProductionLagers(data.CountryId, data.UnitId).Item1
							, MaterialManagement.Helpers.SpecialHelper.GetMainAndProductionLagers(data.CountryId, data.UnitId).Item2
					);
				var iAmFaultyArticle = Psz.Core.MaterialManagement.Helpers.IdentifyFaultyArticlesHelper.Faulty(orders, fasListOfSpeceficArtikelNr, fetchedStocknotused.Stock.Value).ToList();
				foreach(var iterator in iAmFaultyArticle)
				{
					mydatatoreturn.Add(iterator);
				}
			}
			mydatatoreturn = mydatatoreturn.DistinctBy(x => x.artcileNr).ToList();
			var faultyarticles = mydatatoreturn.Select(data => new ArticleAndFaultyWeek(data.artcileNr, data.artikelnummer)).ToList();
			TotalCount = faultyarticles.Count();
			faultyarticles.OrderBy(x => x.artcileNr).ToList();
			var t = faultyarticles.Skip((data.RequestedPage) * data.PageSize).Take(data.PageSize).ToList();
			return ResponseModel<IPaginatedResponseModel<ArticleAndFaultyWeek>>.SuccessResponse(
				new IPaginatedResponseModel<ArticleAndFaultyWeek>
				{
					Items = t,
					PageRequested = this.data.RequestedPage,
					PageSize = this.data.PageSize,
					TotalCount = TotalCount,
					TotalPageCount = this.data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(TotalCount > 0 ? TotalCount : 0) / this.data.PageSize)) : 0
				});

		}



		public ResponseModel<IPaginatedResponseModel<ArticleAndFaultyWeek>> Validate()
		{
			if(user is null)
			{
				return ResponseModel<IPaginatedResponseModel<ArticleAndFaultyWeek>>.AccessDeniedResponse();
			}

			return ResponseModel<IPaginatedResponseModel<ArticleAndFaultyWeek>>.SuccessResponse();
		}

		// contract 
		Task<ResponseModel<IPaginatedResponseModel<ArticleAndFaultyWeek>>> IHandle<FaultyArticlesInOpenFasRequestModel, Task<ResponseModel<IPaginatedResponseModel<ArticleAndFaultyWeek>>>>.Validate()
		{
			throw new NotImplementedException();
		}
	}
}
