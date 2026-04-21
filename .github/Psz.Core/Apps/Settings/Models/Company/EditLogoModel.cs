namespace Psz.Core.Apps.Settings.Models.Company
{
	public class EditLogoModel
	{
		public int CompanyId { get; set; }
		public string FileExtension { get; set; }
		public byte[] FileData { get; set; }
	}
}
