using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;


namespace Psz.Core.CapitalRequests.Handlers
{
	public partial class CapitalRequestsService
	{
		public ResponseModel<string> ValidateGetFAArtikelnummer(UserModel user, int fa)
		{
			if(user == null)
				return ResponseModel<string>.AccessDeniedResponse();
			var fertingung = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(fa);
			if(fertingung == null)
				return ResponseModel<string>.FailureResponse("FA not found.");
			var werke = Infrastructure.Data.Access.Joins.CapitalRequestsJointsAccess.GetWerkeId(user.CompanyId);
			var companyLager = Infrastructure.Data.Access.Tables.BSD.LagerorteAccess.GetByWerke(werke);
			var companyLagerIds = companyLager?.Select(x => x.Lagerort_id).ToList();
			if(!companyLagerIds.Contains(fertingung.Lagerort_id ?? -1))
				return ResponseModel<string>.FailureResponse("Order is not in user company.");
			return ResponseModel<string>.SuccessResponse();
		}
		public ResponseModel<string> GetFAArtikelnummer(UserModel user, int fa)
		{
			var validationReponse = ValidateGetFAArtikelnummer(user, fa);
			if(!validationReponse.Success)
				return validationReponse;

			var _fa = Infrastructure.Data.Access.Tables.PRS.FertigungAccess.GetByFertigungsnummer(fa);
			var response = Infrastructure.Data.Access.Tables.PRS.ArtikelAccess.Get(_fa.Artikel_Nr ?? -1).ArtikelNummer;

			return ResponseModel<string>.SuccessResponse(response);
		}
	}
}
