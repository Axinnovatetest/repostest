using System.Collections.Generic;

namespace Infrastructure.Services.Reporting.Models.CTS
{
	public class FAWerkUpdateReportModel
	{
		public int IdUpdate { get; set; }
		public List<FAUpdatedFromExcelModel> Updated { get; set; }
		public List<FANotUpdatedFromExcelModel> NotUpdated { get; set; }
		public FAWerkUpdateReportModel()
		{

		}
	}
}