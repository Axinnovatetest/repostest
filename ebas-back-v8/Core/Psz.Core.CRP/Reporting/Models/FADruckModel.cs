using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CRP.Reporting.Models
{
	public class FADruckModel
	{
		public FADruckHeaderReportModel Header { get; set; }
		public List<FADruckPositionsReportModel> Positions { get; set; }
		public List<FADruckPlannungReportModel> Plannung { get; set; }
		public bool IsTechnick { get; set; }
	}
}