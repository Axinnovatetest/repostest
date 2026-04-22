using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Logistics.Models.Statistics
{
	public class ListWareneingangDetailsByKundeUndDatumModel
	{
		public string name1 { get; set; }
		public string name1Lower { get { return name1.ToLower(); } }
		public int mois { get; set; }
		public int annee { get; set; }
		public string moisEnLettre { get; set; }
		public List<WareneingangLieferantDetailsModel> details { get; set; }
	}
}
