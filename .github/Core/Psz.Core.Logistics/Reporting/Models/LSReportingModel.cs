using Infrastructure.Services.Reporting.Models.Logistics;
using System.Collections.Generic;

namespace Psz.Core.Logistics.Reporting.Models
{
	public class LSReportingModel
	{
		public List<LSDruckHeaderReportModel> Headers { get; set; }
		public List<LSDruckDetailsReportModel> Details { get; set; }
		public List<LSDruckFooterReportModel> InvoiceFields { get; set; }
		public LSReportingModel()
		{

		}
	}


}
