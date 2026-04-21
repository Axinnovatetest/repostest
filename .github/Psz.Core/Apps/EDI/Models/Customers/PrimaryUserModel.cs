namespace Psz.Core.Apps.EDI.Models.Customers
{
	public class PrimaryUserModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int AccessProfileId { get; set; }
		public string AccessProfileName { get; set; }
		public int CustomerId { get; set; }
		public string CustomerName { get; set; }
	}
}
