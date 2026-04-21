using Psz.Core.BaseData.Models.LifeCycle;
using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Interfaces.Article
{
	public interface IArticleLifeCycleService
	{
		ResponseModel<List<ArticleLifeCyclePhasesModel>> GetLifeCyclesPhases(UserModel user);
		ResponseModel<int> AddLifeCyclePhase(UserModel user, ArticleLifeCyclePhasesRequestModel data);
		ResponseModel<int> UpdateLifeCyclePhase(UserModel user, ArticleLifeCyclePhasesRequestModel data);
		ResponseModel<int> DeleteLifeCyclePhase(UserModel user,int id);
		ResponseModel<int> CheckLifeCycleBeforeEditOrDelete(UserModel user, int id);

		ResponseModel<List<ArticleLifeCyclesModel>> GetArticleLifeCycle(UserModel user,int articleId);
		ResponseModel<int> AddArticleLifeCycle(UserModel user, AddArticleLifeCycleRequestModel data);
		ResponseModel<int> UpdateArticleLifeCycle(UserModel user, AddArticleLifeCycleRequestModel data);
		ResponseModel<int> DeleteArticleLifeCycle(UserModel user,int id);
	}
}