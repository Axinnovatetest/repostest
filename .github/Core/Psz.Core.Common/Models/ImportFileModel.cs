namespace Psz.Core.Common.Models
{
	public class ImportFileModel
	{
		public int Id { get; set; }
		public string FilePath { get; set; }
		public bool Overwrite { get; set; }
		//delfor
		public string CommaSeperator { get; set; }
		public string CheckFrequency { get; set; }
		// forcasts
		public int ForcastDraftTypeId { get; set; }
	}
}
