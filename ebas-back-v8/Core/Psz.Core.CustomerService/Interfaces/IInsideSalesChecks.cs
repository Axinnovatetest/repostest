using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.InsideSalesChecks;
using Psz.Core.Identity.Models;
using System.Collections.Generic;


namespace Psz.Core.CustomerService.Interfaces
{
	public interface IInsideSalesChecks
	{
		ResponseModel<int> UpdateInstructions(UserModel user, InsideSalesChecksUpdateRequestModel data);

		ResponseModel<SearchInsideSaleResponseModel> GetInsideSalesChecks(InsideSalesChecksSearchRequestModel data, UserModel user);
		public ResponseModel<InsideSalesChecksUpdateLogResponseModel> GetLogs(UserModel user, InsideSalesChecksUpdateLogRequestModel data);
	}
}
