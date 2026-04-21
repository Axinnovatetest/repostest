using Psz.Core.Common.Models;
using Psz.Core.CRP.Models.FA;
using Psz.Core.Identity.Models;

namespace Psz.Core.CRP.Interfaces
{
	public interface IFAService
	{
		ResponseModel<List<FACRPUpdateListModel>> GetCRPUpdateFaList(UserModel user, string artikelnummer);
		ResponseModel<List<KeyValuePair<int, string>>> GetArticlesForCRPFAUpdate(UserModel user, string artikelnummer);
		ResponseModel<List<FACRPUpdateResponseModel>> UpdateCRPFA(UserModel user, FACRPUpdateRequestModel data);
		ResponseModel<List<FACRPNotRequiredROHResponseModel>> GetNotRequiredROH(UserModel user, FACRPNotRequiredROHRequestModel date);
	}
}