using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Support.Models
{
	public  class GetFirstAndLastCallRequestModel
	{
		public int NumberOfDays { get; set; }
		public string  ApiArea { get; set; }
		public string  ApiController { get; set; }
		public string  ApiMethod { get; set; }
	}

	public  class GetFirstAndLastCallResponseModel
	{
		public string FirstCallTime { get; set; }
		public string LastCallTime { get; set; }
		public GetFirstAndLastCallResponseModel(GetFirstAndLastCall entity)
		{
			FirstCallTime = entity.FirstCallTime;
			LastCallTime = entity.LastCallTime;
		}
	}

}
