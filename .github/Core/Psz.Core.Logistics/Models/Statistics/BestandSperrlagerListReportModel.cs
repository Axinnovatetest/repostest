using System.Collections.Generic;

namespace Psz.Core.Logistics.Models.Statistics
{
	public class BestandSperrlagerListReportModel
	{
		public BestandSperrlagerListReportModel(Infrastructure.Data.Entities.Joins.Logistics.BestandSperrlagerListReportEntity BestandSperrlagerListReportEntity)
		{
			if(BestandSperrlagerListReportEntity == null)
			{
				return;
			}
			Artikelnummer = BestandSperrlagerListReportEntity.Artikelnummer;
			Bezeichnung1 = BestandSperrlagerListReportEntity.Bezeichnung1;
			Bestand = BestandSperrlagerListReportEntity.Bestand;
			Lagerort_id = BestandSperrlagerListReportEntity.Lagerort_id;
		}
		public string Artikelnummer { get; set; }
		public string Bezeichnung1 { get; set; }
		public decimal Bestand { get; set; }
		public int Lagerort_id { get; set; }

	}
	public class BestandSperrlagerListReportDetails
	{
		public List<BestandSperrlagerListReportModel> Details { get; set; } = new List<BestandSperrlagerListReportModel>();
	}
}
