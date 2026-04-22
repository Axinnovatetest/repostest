using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.ManagementOverview.Production.Models
{
	public class PackGeplanteStundenModel
	{
		public List<GeplantStundenModel> listeGeplantStunden { get; set; }
		public List<string> listeKunden { get; set; }
		public List<string> listeJahrKW { get; set; }
		public  DateTime? lastUpdate { get; set; }

	}
}
