using Newtonsoft.Json;

namespace Psz.Core.Identity.Models
{
	public class EDIAccesModel
	{

		[JsonProperty("ModuleActivated")]
		public bool ModuleActivated;

		[JsonProperty("EDI")]
		public bool EDI;

		[JsonProperty("Access")]
		public bool Access;

		[JsonProperty("AccessUpdate")]
		public bool AccessUpdate;

		[JsonProperty("Customer")]
		public bool Customer;

		[JsonProperty("AllCustomers")]
		public bool AllCustomers;

		[JsonProperty("CustomerUpdate")]
		public bool CustomerUpdate;

		[JsonProperty("Order")]
		public bool Order;

		[JsonProperty("OrderUpdate")]
		public bool OrderUpdate;

		[JsonProperty("OrderValidate")]
		public bool OrderValidate;

		[JsonProperty("OrderHistory")]
		public bool OrderHistory;

		[JsonProperty("OrderError")]
		public bool OrderError;

		[JsonProperty("OrderErrorHistory")]
		public bool OrderErrorHistory;

		[JsonProperty("OrderErrorValidate")]
		public bool OrderErrorValidate;
	}
}
