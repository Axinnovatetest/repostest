using Psz.Core.Common.Models;

namespace Psz.Core.CustomerService.CsStatistics.Models
{
	public class LagerBestandFGRequestModel: IPaginatedRequestModel
	{
		public string Artikelnummer { get; set; }
		public string Kunde { get; set; }
	}
	public class LagerBestandFGResponseModel: IPaginatedResponseModel<LagerBestandFGModel>
	{
	}
}
