namespace Psz.Core.MaterialManagement.CRP.Models.Holiday
{
	public class DeleteHolidayModel: WorkLocationBaseModel
	{
		public int Id { get; set; }
		public bool? EditSimilar { get; set; } = false;
	}
}
