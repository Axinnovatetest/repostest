using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Logistics.Models.PlantBookings
{
	public class PostTicketLogsModel
	{
		public string? Artikelnummer { get; set; }
		public DateTime BeginDate { get; set; }
		public DateTime EndDate { get; set; }

	}
}
