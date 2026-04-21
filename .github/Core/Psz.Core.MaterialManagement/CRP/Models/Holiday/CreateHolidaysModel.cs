namespace Psz.Core.MaterialManagement.CRP.Models.Holiday
{
	public class CreateHolidaysModel: WorkLocationBaseModel
	{
		public List<Item> Items { get; set; } = new List<Item>();

		public class Item
		{
			public DateTime Day { get; set; }
			public string Name { get; set; }
		}
	}
}
