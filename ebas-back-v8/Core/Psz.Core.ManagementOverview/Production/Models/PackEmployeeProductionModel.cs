using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.ManagementOverview.Production.Models
{
	public class PackEmployeeProductionModel
	{
		public List<Models.EmployeeProductionModel> listeEmployeeProduction { get; set; }
		public DateTime? lastUpdate { get; set; }
	}
}
