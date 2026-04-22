using Psz.Core.CustomerService.Models.InsideSalesChecks;
using Psz.Core.CustomerService.Models.InsideSalesChecksArchive;
using Psz.Core.Identity.Models;


namespace Psz.Core.CustomerService.Interfaces
{
	using Common.Models;
	public interface IInsideSalesChecksArchive
	{
		ResponseModel<GetInsideSalesHistoryResponseModel> GetInsideSalesChecksHistories(UserModel user, GetInsideSalesHistoryRequestModel data);

		ResponseModel<int> SendInstructionBack(UserModel user, int instructionId);
		ResponseModel<byte[]> GetInsideSalesChecksHistories_XLS(UserModel user, GetInsideSalesHistoryRequestModel data);

	}
}
