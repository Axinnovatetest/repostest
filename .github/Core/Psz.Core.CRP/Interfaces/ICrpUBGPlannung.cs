using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FAPlanning;
using Psz.Core.Identity.Models;

namespace Psz.Core.CRP.Interfaces
{
	public interface ICrpUBGPlannung
	{
		ResponseModel<ArticlesResponseModel> GetArticles(UserModel user, ArticlesRequestModel data);
		ResponseModel<FASystemModel> GetFaSystem(UserModel user, FASystemRequestModel data);
	}
}