using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.ManagementOverview.Production.Models
{
	public class GeplantStundenByTypModel
	{
		public int id { get; set; }
		public int  typ { get; set; }
		public decimal stunden { get; set; }
		public int jahr { get; set; }
		public int KW { get; set; }
		public DateTime? datum { get; set; }
		public GeplantStundenByTypModel(Infrastructure.Data.Entities.Tables.MGO.GeplantStundenByTypEntity geplantStundenEntity)
		{
			if(geplantStundenEntity == null)
			{
				return;
			}

			id = geplantStundenEntity.id;
			typ = geplantStundenEntity.typ;
			stunden = geplantStundenEntity.stunden;
			jahr = geplantStundenEntity.jahr;
			KW = geplantStundenEntity.KW;
			datum = geplantStundenEntity.datum;


		}
		public GeplantStundenByTypModel(int typ, int jahr, int KW, decimal stunden)
		{
			this.typ = typ;
			this.jahr = jahr;
			this.KW = KW;
			this.stunden = stunden;
			this.datum= datum;


		}
	}
}
