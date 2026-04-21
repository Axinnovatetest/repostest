using Psz.Core.MaterialManagement.CRP.Models.Capacity;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.CRP.Handlers.Capacity
{
	public class GetArticlesWoWPLHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<ArticlesWoWPLResponseModel>>>
	{
		private Identity.Models.UserModel user { get; set; }
		private ArticlesWoWPLRequestModel _data { get; set; }

		public GetArticlesWoWPLHandler(Identity.Models.UserModel user, ArticlesWoWPLRequestModel data)
		{
			this.user = user;
			this._data = data;
		}

		public ResponseModel<List<ArticlesWoWPLResponseModel>> Handle()
		{
			lock(Locks.CapacityLock)
			{
				try
				{
					if(user == null)
					{
						throw new SharedKernel.Exceptions.UnauthorizedException();
					}

					return Perform(this.user, this._data);
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}

		public static ResponseModel<List<ArticlesWoWPLResponseModel>> Perform(Identity.Models.UserModel user, ArticlesWoWPLRequestModel _data)
		{
			var articleEntities = Infrastructure.Data.Access.Joins.MTM.CRP.ArticleAccess.GetArticlesWoWPL(_data.warengruppeEF, _data.wStuckliste, _data.wFa, _data.wOpenFa, _data.lager, _data.faDateFrom, _data.faDateTill)
				?? new List<Infrastructure.Data.Entities.Joins.MTM.CRP.ArticleWpl>();

			return ResponseModel<List<ArticlesWoWPLResponseModel>>.SuccessResponse(articleEntities.Select(x => new ArticlesWoWPLResponseModel(x))?.ToList());
		}
		public ResponseModel<List<ArticlesWoWPLResponseModel>> Validate()
		{
			throw new NotImplementedException();
		}
	}
}
