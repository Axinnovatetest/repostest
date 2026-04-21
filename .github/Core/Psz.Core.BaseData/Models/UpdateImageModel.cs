namespace Psz.Core.BaseData.Models
{
	public class UpdateImageModel
	{
		public int Nr { get; set; }
		public byte[] ImageFile { get; set; }
		public string ImageFileExtension { get; set; }
	}
}
