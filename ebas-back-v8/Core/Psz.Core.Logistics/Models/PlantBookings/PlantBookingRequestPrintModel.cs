using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Logistics.Models.PlantBookings
{
	public class PlantBookingRequestPrintModel
	{
			public int NummerVerpackung { get; set; }
			public int LagerId { get; set; }
		    public int? Order { get; set; }

	}
}
