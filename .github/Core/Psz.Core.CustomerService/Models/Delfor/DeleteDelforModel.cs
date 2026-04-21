namespace Psz.Core.CustomerService.Models.Delfor
{
	public class DeleteDelforModel
	{
		public int HeaderId { get; set; }
		public bool DeleteAllVersions { get; set; }
	}

	public class DeleteDelforInListmodel
	{
		public int Customernumber { get; set; }
		public string DocumentNumber { get; set; }
	}
}
