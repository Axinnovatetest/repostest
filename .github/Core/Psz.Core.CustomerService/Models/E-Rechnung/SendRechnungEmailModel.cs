using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CustomerService.Models.E_Rechnung
{
	public class SendRechnungEmailModel
	{
		public bool Sent { get; set; }
		public string Subject { get; set; }
	}
}
