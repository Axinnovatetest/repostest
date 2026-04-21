using Psz.Core.Common.Models;
using Psz.Core.CRP.Interfaces;

namespace Psz.Core.CRP.Handlers.Preview
{
	public partial class PreviewService: IPreviewService
	{
		public ResponseModel<List<KeyValuePair<int, string>>> GetArticleNumbersHandler(Identity.Models.UserModel user, string searchTerm, int page, int pageSize)
		{
			if(user == null)
				return ResponseModel<List<KeyValuePair<int, string>>>.AccessDeniedResponse();
			// -
			return ResponseModel<List<KeyValuePair<int, string>>>.SuccessResponse(Infrastructure.Data.Access.Joins.CRP.PreviewAccess.GetArticleNumbers(searchTerm, page, pageSize));
		}
	}
}
