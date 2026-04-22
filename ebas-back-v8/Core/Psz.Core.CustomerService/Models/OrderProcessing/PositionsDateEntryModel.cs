using System;

namespace Psz.Core.CustomerService.Models.OrderProcessing
{
	public class PositionsDateEntryModel
	{
		public int Id { get; set; }
		public DateTime? Wunshtermin { get; set; }
		public DateTime? Liefertermin { get; set; }

		public PositionsDateEntryModel()
		{

		}
	}
}
