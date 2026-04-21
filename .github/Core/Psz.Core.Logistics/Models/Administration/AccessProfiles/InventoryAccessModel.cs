using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Logistics.Models.Administration.AccessProfiles
{
	public class AddInventoryWarehouseRequestModel
	{
		public int AccessProfileId { get; set; }
		public List<int> WarehouseIds { get; set; }
	}
}
