using System.Collections.Generic;

namespace Psz.Core.Logistics.Models.Verpackung
{
	public class KundenKurtzModel
	{
		public string lVornameNameFirma { get; set; }
		public KundenKurtzModel(Infrastructure.Data.Entities.Joins.Logistics.KundenKurtzEntity kundenEntity)
		{

			if(kundenEntity != null)
			{
				lVornameNameFirma = kundenEntity.lVornameNameFirma;
			}
		}
	}

	public class ListKundenKurtzModel
	{
		public List<string> ListelVornameNameFirma { get; set; }

	}
}
