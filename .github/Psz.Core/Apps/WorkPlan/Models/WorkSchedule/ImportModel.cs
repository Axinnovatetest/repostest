namespace Psz.Core.Apps.WorkPlan.Models.WorkSchedule
{
	public class ImportModel
	{
		public int Id { get; set; }
		// 
		public byte[] WPLFile { get; set; }
		public string WPLFileExtension { get; set; }
	}
}
