using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.MaterialManagement.Orders.Models.Orders
{
	public class ConfirmedDateAndCommentModel
	{
		public int PositionId { get; set; }
		public DateTime? ConfirmedDate { get; set; }
		public string Comment { get; set; }
		public string ABNummer { get; set; }
	}
}
