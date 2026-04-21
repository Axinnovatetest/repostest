using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.Preview;
using Psz.Core.CRP.Interfaces;

namespace Psz.Core.CRP.Handlers.Preview
{
	public partial class PreviewService: IPreviewService
	{
		public ResponseModel<int> GetHorizon1(Identity.Models.UserModel user)
		{
			if(user == null)
				return ResponseModel<int>.AccessDeniedResponse();

			return ResponseModel<int>.SuccessResponse(Module.CTS?.FAHorizons?.H1LengthInDays ?? 42);
		}
	}
}
