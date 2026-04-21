using Psz.Core.Common.Models;
using Psz.Core.CustomerService.Models.InsideSalesChecks;
using Psz.Core.Identity.Models;
using System.Collections.Generic;
using static Psz.Core.CustomerService.Models.InsideSalesCustomerSummary.InsideSalesCustomerSummaryModel;


namespace Psz.Core.CustomerService.Interfaces
{
	public interface IInsideSalesTotalDemandPlanning
	{
		ResponseModel<GetCustomerSummaryResponseModel> GetCustomerSummary(UserModel user, GetCustomerSummaryRequestModel data);
		ResponseModel<byte[]> GetCustomerSummary_XLS(UserModel user, GetCustomerSummaryRequestModel data);
		ResponseModel<List<KeyValuePair<int, int>>> GetNextNWeeks(UserModel user, int n);
	}
}
