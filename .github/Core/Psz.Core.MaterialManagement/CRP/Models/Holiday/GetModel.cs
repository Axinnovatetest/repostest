namespace Psz.Core.MaterialManagement.CRP.Models.Holiday
{
	public class GetModel: WorkLocationBaseModel
	{
		public int Year { get; set; }
		public bool WeekEndsOnly { get; set; } = false;
	}
}
