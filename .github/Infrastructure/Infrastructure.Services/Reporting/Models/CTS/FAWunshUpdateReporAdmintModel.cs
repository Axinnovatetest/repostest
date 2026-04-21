using System.Collections.Generic;

namespace Infrastructure.Services.Reporting.Models.CTS
{
	public class FAWunshUpdateReporAdmintModel
	{
		public int IdUpdate { get; set; }
		public List<FAUpdatedFromExcelModel> Updated { get; set; }
		public List<FAUpdatedFromExcelModel> NotUpdated { get; set; }
		public FAWunshUpdateReporAdmintModel()
		{

		}
	}
}