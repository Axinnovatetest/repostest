using System;

namespace Psz.Core.BaseData.Models.Article.Statistics.ControllingAnalysis
{
	public class AverageMaterialContentRequestModel
	{
		public string CustomerNumber { get; set; }
		public string CustomerName { get; set; }
		public DateTime DateFrom { get; set; }
		public DateTime DateTill { get; set; }
		public decimal SurchargeOnUBG { get; set; }
	}
}
