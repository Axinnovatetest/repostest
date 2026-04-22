using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Support.Models
{
	public  class GetApiSingleControllersCallsRequestModel : GetMetricsBaseRequestModel
	{
		public string  Area { get; set; }
		public string Controller { get; set; }
	}
	public class GetApiControllersCallsRequestModel: GetMetricsBaseRequestModel
	{
		public string Area { get; set; }
	}

	public class GetApiMethodCallsRequestModel : GetMetricsBaseRequestModel
	{
		public string ApiMethod { get; set; }
		public string Area { get; set; }
		public string Controller { get; set; }
	}
}
