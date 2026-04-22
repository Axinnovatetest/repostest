namespace Psz.Core.Common.Models
{
	public class ImportFileRequestBusinessModel
	{
		public int Nr { get; set; }
		public string FileExtension { get; set; }
		public byte[] FileByteArray { get; set; }
	}
}
