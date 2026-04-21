using System.Collections.Generic;

namespace Psz.Core.CRP.Models.FA
{
	public class FAWerkUpdateReportModel
	{
		public List<FAUpdatedFromExcelModel> Updated { get; set; }
		public List<FANotUpdatedFromExcelModel> NotUpdated { get; set; }

		public FAWerkUpdateReportModel()
		{

		}
	}
}
