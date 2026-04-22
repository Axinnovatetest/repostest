namespace Psz.Core.Apps.Purchase.Models.CustomerService.OrderReport
{
	public class LogoImageModel
	{
		public int LanguageId { get; set; }
		public int OrderTypeId { get; set; }
		// 
		public byte[] LogoImage { get; set; }
		public string LogoImageExtension { get; set; }
	}
}
