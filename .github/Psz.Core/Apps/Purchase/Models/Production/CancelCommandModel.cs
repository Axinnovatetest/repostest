namespace Psz.Core.Apps.Purchase.Models.Production
{
	public class CancelCommandModel
	{
		public int Id { get; set; }
		public string Notes { get; set; }
		public bool CancelUBG { get; set; } = false;
	}
}
