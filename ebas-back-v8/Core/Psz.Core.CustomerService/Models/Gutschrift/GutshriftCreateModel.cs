using Psz.Core.CustomerService.Models.OrderProcessing;

namespace Psz.Core.CustomerService.Models.Gutshrift
{
	public class GutshriftCreateModel: CreateOrderModel
	{
		public int RechnungId { get; set; }
	}
}
