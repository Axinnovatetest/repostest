using Infrastructure.Data.Entities.Tables.BSD;
using Psz.Core.BaseData.Models.LifeCycle;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.BaseData.Handlers.ArticleLifeCycle
{
	public partial class ArticleLifeCycleService
	{
		public ResponseModel<List<ArticleLifeCyclesModel>> GetArticleLifeCycle(UserModel user, int articleId)
		{
			if(user == null)
				return ResponseModel<List<ArticleLifeCyclesModel>>.AccessDeniedResponse();

			try
			{
				var entities = Infrastructure.Data.Access.Tables.BSD.ArticleLifeCyclesAccess.GetByArtilceId(articleId);
				return ResponseModel<List<ArticleLifeCyclesModel>>.SuccessResponse(
					entities.Select(x => new ArticleLifeCyclesModel(x)).OrderBy(x=>x.PhaseOrderInCycle).ToList()
					);
			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> AddArticleLifeCycle(UserModel user, AddArticleLifeCycleRequestModel data)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();

			try
			{
				var phase = Infrastructure.Data.Access.Tables.BSD.ArticleLifeCyclePhasesAccess.Get(data.LifeCyclePhaseId);
				var entity = new ArticleLifeCyclesEntity
				{
					ArticleId = data.ArtileId,
					PhaseId = data.LifeCyclePhaseId,
					PhaseName = phase.PhaseName,
					PhaseOrderInCycle = data.PhaseOrderInCycle,
					CreateTime = System.DateTime.Now,
					CreateUserId = user.Id,
					CreateUserName = user.Username
				};

				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.BSD.ArticleLifeCyclesAccess.Insert(entity));
			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> UpdateArticleLifeCycle(UserModel user, AddArticleLifeCycleRequestModel data)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();

			try
			{
				var articlePhase=Infrastructure.Data.Access.Tables.BSD.ArticleLifeCyclesAccess.Get(data.Id);
				var phase = Infrastructure.Data.Access.Tables.BSD.ArticleLifeCyclePhasesAccess.Get(data.LifeCyclePhaseId);

				articlePhase.PhaseId = phase.Id;
				articlePhase.PhaseName = phase.PhaseName;
				articlePhase.PhaseOrderInCycle = data.PhaseOrderInCycle;
				articlePhase.UpdateTime = System.DateTime.Now;
				articlePhase.UpdateUserId = user.Id;
				articlePhase.UpdateUserName = user.Username;

				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.BSD.ArticleLifeCyclesAccess.Update(articlePhase));
			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
		public ResponseModel<int> DeleteArticleLifeCycle(UserModel user, int id)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();

			try
			{
				return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Tables.BSD.ArticleLifeCyclesAccess.Delete(id));
			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}