using Psz.Core.CRP.Models.FA;
using Psz.Core.Common.Models;
namespace Psz.Core.CRP.Interfaces
{
	public interface ICrpFaChangesHistoryService
	{
		public ResponseModel<FASearchResponseModel> GetFaPlanningViolation(Identity.Models.UserModel user, FaPlanningViolationRequestModel request);

		ResponseModel<GetFaHoursChangesResponseModel> GetFaHoursMovement(Identity.Models.UserModel user, FaHoursChangesRequestModel data);

		ResponseModel<GetFaChangesHistoryResponseModel> GetFaDatesChangeHistory(Identity.Models.UserModel user, FaChangesHistoryRequestModel data);

		ResponseModel<byte[]> GetFaDatesChangesHistoryXLS(Identity.Models.UserModel user, FaChangesHistoryRequestModel data);

		ResponseModel<FaDataChartResponseModel> GetFaMovementChartData(Identity.Models.UserModel user, FaChartDataRequestModel data);
		
	}
}
