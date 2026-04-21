namespace Psz.Core.Logistics.Models.Statistics
{
	public class PSZ_Disposition_Nettobedarfsermittlung_Umbuchung_Details_II_Model
	{
		public PSZ_Disposition_Nettobedarfsermittlung_Umbuchung_Details_II_Model(Infrastructure.Data.Entities.Joins.Logistics.PSZ_Disposition_Nettobedarfsermittlung_Umbuchung_Details_II_Entity _data)
		{
			PSZ = _data.PSZ;
			Bestand = _data.Bestand;
			Lagerort = _data.Lagerort;
			Lagerort_id = _data.Lagerort_id;
		}
		public string PSZ { get; set; }
		public decimal Bestand { get; set; }
		public string Lagerort { get; set; }
		public int? Lagerort_id { get; set; }
	}
}
