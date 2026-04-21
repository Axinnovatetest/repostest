using System.Collections.Generic;

namespace Psz.Core.Reporting.Models
{
	public class DelforRepotModel
	{
		public DLFModels.HeaderModel Header { get; set; }
		public DLFModels.LineItemModel LineItem { get; set; }
		public List<DLFModels.LineItemPlanModel> LineItemPlan { get; set; }
		public DLFModels.I18N.HeaderModel HeaderLabel { get; set; }
		public DLFModels.I18N.LineItemModel LineItemLabel { get; set; }
		public DLFModels.I18N.LineItemPlanModel LineItemPlanLabel { get; set; }
	}
	public class DelforRepotFooterModel
	{
		public string FooterLine1 { get; set; }
		public string FooterLine2 { get; set; }
		public string FooterLine3 { get; set; }
	}
}