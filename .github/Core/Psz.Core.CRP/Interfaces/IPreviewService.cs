using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.Preview;

namespace Psz.Core.CRP.Interfaces
{
	public interface IPreviewService
	{
		ResponseModel<int> GetHorizon1(Identity.Models.UserModel user);
		ResponseModel<List<PreviewHeaderWeekResponseModel>> GetHeadersHandler(Identity.Models.UserModel user);
		ResponseModel<PreviewArticleResponseModel> GetArticleHandler(Identity.Models.UserModel user, int articleId);
		ResponseModel<List<PreviewWeekResponseModel>> GetEntitiesByArticleYearWeekHandler(Identity.Models.UserModel user, string entityType, int articleId, int year, int week);
		ResponseModel<List<KeyValuePair<int, string>>> GetArticleNumbersHandler(Identity.Models.UserModel user, string searchTerm, int page, int pageSize);
		ResponseModel<int> UpdateSnapshot(Identity.Models.UserModel user);
	}
}
