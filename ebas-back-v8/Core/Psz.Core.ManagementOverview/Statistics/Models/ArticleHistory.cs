using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.ManagementOverview.Statistics.Models
{
	public record ArticleHistoryResponseModel
	{
		public string Log { get; set; }
		public string ChangeUser { get; set; }
		public string ChangeTime { get; set; }
	}
}
