using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CapitalRequests.Locks
{
	public class Locks
	{
		public static object TicketLock { get; set; } = new object();
	}
}