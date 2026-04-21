using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FAPlanning;
using Psz.Core.CRP.Models.FAPlanning.Historie;
using Psz.Core.Identity.Models;


namespace Psz.Core.CRP.Interfaces
{
	public interface ICrpFAPlannung
	{
		ResponseModel<FaultyFAReponseModel> GetFaultyFA(UserModel user, FaultyFARequestModel model);
		ResponseModel<FaultyNeedsResponseModel> GetFaultyNeeds(UserModel user, FaultyNeedsResquestModel data);
		ResponseModel<ArticleInfoModel> GetArticleInformation(UserModel user, int articleNr);
		ResponseModel<FASystemModel> GetFaSystem(UserModel user, FASystemRequestModel data);
		ResponseModel<ArticlesResponseModel> GetArticles(UserModel user, ArticlesRequestModel data);
		ResponseModel<List<NumberKreisResponseModel>> GetNumberKreis(UserModel user);
		ResponseModel<List<KeyValuePair<int, string>>> GetArticleUBG(UserModel user, int articleNr);
		ResponseModel<List<KeyValuePair<int, string>>> GetUnits(UserModel user);
		ResponseModel<ArticleKwDetailResponseModel> GetKwDetailHandler(UserModel user, ArticleKwDetailRequestModel data);
		ResponseModel<int> ActivateFAPlannugAgent(UserModel user);
		ResponseModel<List<FaPlanningLogRequestModel>> GetLogs(UserModel user);
		ResponseModel<FaPlanningLogRequestModel> GetLastLog(UserModel user);

		#region Historie
		ResponseModel<List<Models.FAPlanning.Historie.HistorieFaPlannungDetailsModel>> GetHistorieFromExcel(UserModel user, Core.Common.Models.ImportFileModel data);
		ResponseModel<int> SaveFaPlannungHistorie(UserModel user, Models.FAPlanning.Historie.HistorieFaPlannungSaveRequestModel data);
		ResponseModel<FaPlannungHistorieHeadersResponseModel> GetHistorieFaPlannungHeaders(UserModel user, FaPlannungHistorieHeadersRequestModel data);
		ResponseModel<Models.FAPlanning.Historie.FaPlannungHistorieDetailsResponsetModel> GetHistorieFaPlannungDetails(UserModel user, Models.FAPlanning.Historie.FaPlannungHistorieDetailsRequestModel data);
		ResponseModel<byte[]> GetFaPlannungHistoryExcel(UserModel user, FAPlannungHistorieExcelRequestModel data);
		ResponseModel<int> FAPlannungHistorieRefreshData(UserModel user);
		ResponseModel<string> GetHistorieFaPlannungAgentLastExcutionTime(UserModel user);
		ResponseModel<List<FAPlannungHistorieAgentLogModel>> GetFaPlannungHistorieAgentFullLog(UserModel user);
		ResponseModel<byte[]> GetHsitorieFAPlannungDraft(UserModel user);
		ResponseModel<FaPlannungHistorieHeadersModel> GetHistorieFAPlannungheaderById(UserModel user, int id);
		#endregion
	}
}