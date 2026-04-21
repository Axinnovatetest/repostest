using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.Logistics.Models.Statistics
{
	public class LieferantMitAnzahModel
	{
		public string name1 { get; set; }
		public int anzahlVonWareneingang { get; set; }

		public LieferantMitAnzahModel(Infrastructure.Data.Entities.Joins.Logistics.LieferantMitAnzahEntity LieferantEntity)
		{

			if(LieferantEntity != null)
			{
				name1 = LieferantEntity.name1;
				anzahlVonWareneingang = LieferantEntity.anzahlVonWareneingang;
			}
		}
	}
}
