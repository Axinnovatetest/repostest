using Psz.Core.Common.Models;
using Psz.Core.CRP.Interfaces;
using Psz.Core.CRP.Models.Preview;

namespace Psz.Core.CRP.Handlers.Preview
{
	public partial class PreviewService: IPreviewService
	{
		public ResponseModel<List<PreviewWeekResponseModel>> GetEntitiesByArticleYearWeekHandler(Identity.Models.UserModel user, string entityType, int articleId, int year, int week)
		{
			if (user == null) return ResponseModel<List<PreviewWeekResponseModel>>.AccessDeniedResponse();
			// -
			return ResponseModel<List<PreviewWeekResponseModel>>.SuccessResponse(
				(Common.Helpers.DateHelpers.IsBeforeCurrentWeek(year, week)
				? Infrastructure.Data.Access.Joins.CRP.PreviewAccess.GetEntitiesByArticleIdBacklog(entityType, articleId)
				: Infrastructure.Data.Access.Joins.CRP.PreviewAccess.GetEntitiesByArticleIdAndWeek(entityType, articleId, year, week))
				?.Select(x => new PreviewWeekResponseModel(x))?.ToList());
		}
	}
}
