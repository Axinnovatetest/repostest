using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CRP.Models.FA
{
	public class ArticleWarehouseAndCustomerForFACreationModel
	{
		public int Warehouse { get; set; }
		public KeyValuePair<int, string> CutomerNumber { get; set; }
	}
}
