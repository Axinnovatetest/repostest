using System.Collections.Generic;

namespace Infrastructure.Services.Reporting.Models.CTS
{
	public class FAGeneralDruckModel
	{
		public FADruckHeaderReportModel Header { get; set; }
		public List<FADruckPositionsReportModel> Positions { get; set; }
		public List<FADruckPlannungReportModel> Plannung { get; set; }
		public FAGeneralDruckModel()
		{

		}
	}
}