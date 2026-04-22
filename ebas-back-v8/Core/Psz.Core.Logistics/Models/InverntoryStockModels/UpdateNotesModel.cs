using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Logistics.Models.InverntoryStockModels
{
	public class UpdateNotesRequestModel
	{
		public int WarehouseId { get; set; }
		public string RemarksHL { get; set; }
		public string RemarksPL { get; set; }
	}
}
