namespace Psz.Core.Logistics.Models.Statistics
{
	public class LagerOrt_IdModel
	{
		public int Lagerort_id { get; set; }
		public LagerOrt_IdModel(Infrastructure.Data.Entities.Joins.Logistics.LagerOrt_Id lagerOrt_Id)
		{

			if(lagerOrt_Id == null)
				return;

			Lagerort_id = lagerOrt_Id.Lagerort_id;




		}
	}
}
