using Psz.Core.Common.Models;
using Psz.Core.CRP.Interfaces;
using Psz.Core.CRP.Models.FAPlanning;
using Psz.Core.Identity.Models;

namespace Psz.Core.CRP.Handlers.FAPlannung
{
	public partial class CrpFAPlannung: ICrpFAPlannung
	{
		public ResponseModel<FaultyFAReponseModel> ValidateGetFAPlanning(UserModel user)
		{
			if(user == null)
				return ResponseModel<FaultyFAReponseModel>.AccessDeniedResponse();
			return ResponseModel<FaultyFAReponseModel>.SuccessResponse();
		}
		public ResponseModel<FaultyFAReponseModel> GetFaultyFA(UserModel user, FaultyFARequestModel data)
		{
			try
			{
				var validationResponse = ValidateGetFAPlanning(user);
				if(!validationResponse.Success)
					return validationResponse;

				var response = new FaultyFAReponseModel();

				#region > Data sorting & paging
				var dataPaging = new Infrastructure.Data.Access.Settings.PaginModel()
				{
					FirstRowNumber = data.PageSize > 0 ? (data.RequestedPage * data.PageSize) : 0,
					RequestRows = data.PageSize
				};

				Infrastructure.Data.Access.Settings.SortingModel dataSorting = null;
				if(!string.IsNullOrWhiteSpace(data.SortField))
				{
					var sortFieldName = "";
					switch(data.SortField.ToLower())
					{
						default:
						case "Fertigungsauftrag":
							sortFieldName = "[Fertigungsnummer]";
							break;
						case "BemerkungExtern":
							sortFieldName = "[Bemerkung]";
							break;
						case "AktuellerTermin":
							sortFieldName = "[Termin_Bestätigt1]";
							break;
						case "Druckdatum":
							sortFieldName = "[FA_Druckdatum]";
							break;
						case "ManBemerkungPlannung":
							sortFieldName = "[Planungsstatus]";
							break;
						case "Bemerkung_Planung":
							sortFieldName = "[Bemerkung_Planung]";
							break;
					}
					dataSorting = new Infrastructure.Data.Access.Settings.SortingModel()
					{
						SortFieldName = sortFieldName,
						SortDesc = data.SortDesc,
					};
				}
				#endregion
				var faultyEntities = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetFaultyFA(data.SearchTerms, data.Ubg ?? false, dataSorting, dataPaging);
				var faultyCount = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetCountFaultyFA(data.SearchTerms, data.Ubg ?? false);
				if(faultyEntities != null && faultyEntities.Count > 0)
				{
					response = new FaultyFAReponseModel
					{
						Items = faultyEntities.Select(x => new FaultyFAModel(x)).ToList(),
						PageRequested = data.RequestedPage,
						PageSize = data.PageSize,
						TotalCount = faultyCount > 0 ? faultyCount : 0,
						TotalPageCount = data.PageSize > 0 ? (int)Math.Ceiling(((decimal)(faultyCount > 0 ? faultyCount : 0) / data.PageSize)) : 0,
					};
				}
				return ResponseModel<FaultyFAReponseModel>.SuccessResponse(response);
			} catch(System.Exception e)
			{
				Infrastructure.Services.Logging.Logger.Log(e);
				throw;
			}
		}
	}
}