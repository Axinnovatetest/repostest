using Psz.Core.MaterialManagement.CRP.Models.Capacity;
using Psz.Core.SharedKernel.Interfaces;
using System.Linq;

namespace Psz.Core.MaterialManagement.CRP.Handlers.Capacity
{
	public class GetArticleFaTimeDiffHandler: IHandle<Identity.Models.UserModel, ResponseModel<List<ArticleFaTimeDiffModel>>>
	{
		private Identity.Models.UserModel user { get; set; }
		private int? lager { get; set; }

		public GetArticleFaTimeDiffHandler(Identity.Models.UserModel user, int? lager)
		{
			this.user = user;
			this.lager = lager;
		}

		public ResponseModel<List<ArticleFaTimeDiffModel>> Handle()
		{
			lock(Locks.CapacityLock)
			{
				try
				{
					if(user == null)
					{
						throw new SharedKernel.Exceptions.UnauthorizedException();
					}

					return Perform(this.user, this.lager);
				} catch(Exception e)
				{
					Infrastructure.Services.Logging.Logger.Log(e);
					throw;
				}
			}
		}

		public static ResponseModel<List<ArticleFaTimeDiffModel>> Perform(Identity.Models.UserModel user, int? lager)
		{
			var articleEntities = Infrastructure.Data.Access.Joins.MTM.CRP.ArticleFaTimeAccess.GetArticlesFaTimeDiff(lager)
				?? new List<Infrastructure.Data.Entities.Joins.MTM.CRP.ArticleFaTimeEntity>();

			return ResponseModel<List<ArticleFaTimeDiffModel>>.SuccessResponse(articleEntities.Select(x => new ArticleFaTimeDiffModel(x))?.ToList());
		}
		public ResponseModel<List<ArticleFaTimeDiffModel>> Validate()
		{
			throw new NotImplementedException();
		}
	}
}
