using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Models.Supplier
{
	public class GetFilteredSuppliersListRequestModel
	{
		public GetFilteredSuppliersListRequestModel()
		{
			SuppliersToExcludes = new List<int>();
		}
		public bool? includeLieferAddress { get; set; }
		public List<int> SuppliersToExcludes { get; set; }
	}
}
