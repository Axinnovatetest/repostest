using System.Collections.Generic;

namespace Infrastructure.Services.Reporting.Models.CTS
{
	public class FAWunshUpdateReporUsertModel
	{
		public int IdUpdate { get; set; }
		public List<FAUpdatedFromExcelModel> Updated { get; set; }
		public List<FANotUpdatedwunshUserModel> NotUpdated { get; set; }
		public FAWunshUpdateReporUsertModel()
		{

		}
	}
}