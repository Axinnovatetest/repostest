using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FAPlanning;
using Psz.Core.Identity.Models;


namespace Psz.Core.CRP.Handlers.FAPlannung
{
	public partial class CrpFAPlannung
	{
		public ResponseModel<ArticleKwDetailResponseModel> GetKwDetailHandler(UserModel user, ArticleKwDetailRequestModel data)
		{
			if(user == null)
				return ResponseModel<ArticleKwDetailResponseModel>.AccessDeniedResponse();
			if(data == null)
				return ResponseModel<ArticleKwDetailResponseModel>.FailureResponse($"Wrong data");

			var currentYear = DateTime.Today.Year;
			var currentWeek = Common.Helpers.DateHelpers.ExtractIsoWeek(DateTime.Today);

			DateTime? date =
				(data.Year > currentYear || (data.Year == currentYear && data.Kw >= currentWeek))
					? Helpers.DatesHelper.FirstDateOfWeek(data.Year, data.Kw)
					: null;
			// - 
			return ResponseModel<ArticleKwDetailResponseModel>.SuccessResponse(
				new ArticleKwDetailResponseModel
				{
					ArticleId = data.ArticleId,
					Kw = data.Kw,
					ABs = Infrastructure.Data.Access.Joins.CRP.FAPlannungAccess.GetArticleKwData_delayed(data.ArticleId, date, "AB")
					?.Select(x => new ArticleKwDetailResponseModel.Item(x))?.ToList(),
					FAs = Infrastructure.Data.Access.Joins.CRP.FAPlannungAccess.GetArticleKwData_delayed(data.ArticleId, date, "FA")
					?.Select(x => new ArticleKwDetailResponseModel.Item(x))?.ToList(),
					FCs = Infrastructure.Data.Access.Joins.CRP.FAPlannungAccess.GetArticleKwData_delayed(data.ArticleId, date, "FC")
					?.Select(x => new ArticleKwDetailResponseModel.Item(x))?.ToList(),
					LPs = Infrastructure.Data.Access.Joins.CRP.FAPlannungAccess.GetArticleKwData_delayed(data.ArticleId, date, "LP")
					?.Select(x => new ArticleKwDetailResponseModel.Item(x))?.ToList(),
				});
		}
	}
}
