using System.Collections.Generic;

namespace Psz.Core.Logistics.Models.Lagebewegung.PDFReports
{
	public class ReportLagerbewegungModel
	{
		public List<HeaderReportLagerbewegungModel> Headers { get; set; }
		public List<DetailsReportLagerbewegungModel> Details { get; set; }

		public ReportLagerbewegungModel()
		{

		}
	}

	public class ReportPlantBookingLagerbewegungModel
	{
		public List<HeaderReportLagerbewegungModel> Headers { get; set; }
		public List<DetailsReportPlantBookingLagerbewegungModel> Details { get; set; }
		public ReportPlantBookingLagerbewegungModel()
		{

		}
	}
}
