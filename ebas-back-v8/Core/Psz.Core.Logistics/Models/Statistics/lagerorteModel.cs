namespace Psz.Core.Logistics.Models.Statistics
{
	public class lagerorteModel
	{
		public int Lagerort_id { get; set; }
		public string Lagerort { get; set; }
		public lagerorteModel(Infrastructure.Data.Entities.Joins.Logistics.lagerorte lagerorte)
		{

			if(lagerorte == null)
				return;

			Lagerort_id = lagerorte.Lagerort_id;
			Lagerort = lagerorte.Lagerort_id + " || " + lagerorte.Lagerort;



		}
	}
}
