using Psz.Core.MaterialManagement.CRP.Models.Capacity;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.CRP.Handlers.Capacity
{
	public class GetArticleWPLTimeDiffHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<ArticleWPLTimeDiffModel>>>
	{
		private Identity.Models.UserModel user { get; set; }
		private int? countryId { get; set; }
		private int? hallId { get; set; }
		private decimal? minDiff { get; set; }

		public GetArticleWPLTimeDiffHandler(Identity.Models.UserModel user, int? _countryId, int? _hallId, decimal? _minDiff)
		{
			this.user = user;

			countryId = _countryId;
			hallId = _hallId;
			minDiff = _minDiff;
		}

		public ResponseModel<List<ArticleWPLTimeDiffModel>> Handle()
		{
			lock(Locks.CapacityLock)
			{
				try
				{
					if(user == null)
					{
						throw new SharedKernel.Exceptions.UnauthorizedException();
					}

					return Perform(this.user, countryId, hallId, minDiff);
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}

		public static ResponseModel<List<ArticleWPLTimeDiffModel>> Perform(Identity.Models.UserModel user, int? _countryId, int? _hallId, decimal? _minDiff)
		{
			var articleEntities = Infrastructure.Data.Access.Joins.MTM.CRP.ArticleAccess.GetArticleWPLTimeDiff(countryId: _countryId, hallId: _hallId, minDiff: _minDiff)
				?? new List<Infrastructure.Data.Entities.Joins.MTM.CRP.ArticleEntity>();

			return ResponseModel<List<ArticleWPLTimeDiffModel>>.SuccessResponse(articleEntities.Select(x => new ArticleWPLTimeDiffModel(x))?.ToList());
		}
		public ResponseModel<List<ArticleWPLTimeDiffModel>> Validate()
		{
			throw new NotImplementedException();
		}
	}
}
