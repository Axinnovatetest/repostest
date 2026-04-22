namespace Psz.Core.MaterialManagement.CRP.Models.Holiday
{
	public class UpdateModel
	{
		public int Id { get; set; }
		public DateTime Day { get; set; }
		public string Name { get; set; }
		public bool? EditSimilar { get; set; } = false;
	}
}
