using System.Collections.Generic;

namespace Psz.Core.CustomerService.Models.Blanket
{
	public class ArticleRahmenCheckModel
	{
		public Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity ArticleEntity { get; set; }
		public Infrastructure.Data.Entities.Tables.MTM.BestellnummernEntity BestellnummernEntity { get; set; }
		public Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity lieferantenEntity { get; set; }
		public Infrastructure.Data.Entities.Tables.PRS.AdressenEntity AdressenEntity { get; set; }
		public List<string> Errors { get; set; }
		public ArticleRahmenCheckModel()
		{

		}
	}
}
