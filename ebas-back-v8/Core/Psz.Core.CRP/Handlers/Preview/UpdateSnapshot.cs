using Psz.Core.Common.Models;
using Psz.Core.CRP.Interfaces;

namespace Psz.Core.CRP.Handlers.Preview
{
	public partial class PreviewService: IPreviewService
	{
		public ResponseModel<int> UpdateSnapshot(Identity.Models.UserModel user)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();
			// -
			return ResponseModel<int>.SuccessResponse(Infrastructure.Data.Access.Joins.CRP.PreviewAccess.UpdateSnapshot());
		}
	}
}