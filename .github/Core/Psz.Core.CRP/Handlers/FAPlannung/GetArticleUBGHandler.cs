using Psz.Core.Common.Models;
using Psz.Core.Identity.Models;


namespace Psz.Core.CRP.Handlers.FAPlannung
{
	public partial class CrpFAPlannung
	{
		public ResponseModel<List<KeyValuePair<int, string>>> ValidateGetArticleUBG(UserModel user)
		{
			if(user == null)
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse();
		}
		public ResponseModel<List<KeyValuePair<int, string>>> GetArticleUBG(UserModel user, int articleNr)
		{
			var validationResponse = ValidateGetArticleUBG(user);
			if(!validationResponse.Success)
				return validationResponse;

			var response = Infrastructure.Data.Access.Joins.CRP.FAPlannungAccess.GetArticlesUBGStucklist(articleNr);
			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(response);
		}
	}
}
