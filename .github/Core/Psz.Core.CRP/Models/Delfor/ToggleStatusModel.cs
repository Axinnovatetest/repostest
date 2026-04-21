namespace Psz.Core.CRP.Models.Delfor
{
	public class ToggleStatusRequestModel
	{
		public int CustomerId { get; set; }
		public string DocumentNumber { get; set; }
		public bool Done { get; set; }
	}
}
