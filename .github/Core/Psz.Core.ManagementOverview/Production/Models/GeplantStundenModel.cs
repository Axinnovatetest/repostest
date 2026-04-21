using Infrastructure.Data.Entities.Joins.Logistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace Psz.Core.ManagementOverview.Production.Models
{
	public class GeplantStundenModel
	{
		public int id { get; set; }
		public string kunde { get; set; }
		public decimal stunden { get; set; }
		public decimal geschnittenStunden { get; set; }
		public decimal gestartetStunden { get; set; }
		public int jahr { get; set; }
		public int KW { get; set; }

		public GeplantStundenModel()
		{

		}
		public GeplantStundenModel(Infrastructure.Data.Entities.Tables.MGO.GeplantStundenEntity geplantStundenEntity)
		{
			if(geplantStundenEntity == null)
			{
				return;
			}
			
			id= geplantStundenEntity.id;
			kunde= geplantStundenEntity.kunde;
			stunden = geplantStundenEntity.stunden;
			geschnittenStunden = geplantStundenEntity.geschnittenStunden;
			gestartetStunden = geplantStundenEntity.gestartetStunden;
			jahr = geplantStundenEntity.jahr;
			KW = geplantStundenEntity.KW;
			

		}
		public GeplantStundenModel(string kunde, int jahr, int KW, decimal stunden)
		{
			this.kunde = kunde;
			this.jahr = jahr;
			this.KW = KW;
			this.stunden = stunden;


		}
	}
}
