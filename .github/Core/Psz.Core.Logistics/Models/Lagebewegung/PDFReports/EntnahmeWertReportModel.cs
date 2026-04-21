using System.Collections.Generic;

namespace Psz.Core.Logistics.Models.Lagebewegung.PDFReports
{
	public class EntnahmeWertReportModel
	{
		public List<HeaderEntnahmeWertReportModel> Headers { get; set; }
		public List<HeaderGroupEntnahmeWertReportModel> HeadersGroup { get; set; }
		public List<DetailEntnahmeWertReportModel> Details { get; set; }

		public List<HeaderEntnahmeWertReportModel> HeadersWEK { get; set; }
		public List<HeaderGroupEntnahmeWertReportModel> HeadersGroupWEK { get; set; }
		public List<DetailEntnahmeWertReportModel> DetailsWEK { get; set; }

		public EntnahmeWertReportModel()
		{

		}
	}
}
