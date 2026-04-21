using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Support.Models
{
	public  class GetControllersRequestModel : GetMetricsBaseRequestModel
	{
		public string Area { get; set; } = "";
	}

	public class GetMethodsRequestModel : GetMetricsBaseRequestModel
	{
		public string Area { get; set; } = "";
		public string Controller { get; set; } = "";
	}

	public class GetSingleAreaRequestModel: GetMetricsBaseRequestModel
	{
		public string Area { get; set; } = "";
	}
}
