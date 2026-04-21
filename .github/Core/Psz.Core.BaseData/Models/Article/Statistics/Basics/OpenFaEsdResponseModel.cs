using System;

namespace Psz.Core.BaseData.Models.Article.Statistics.Basics
{
	public class OpenFaEsdResponseModel
	{
		public string Artikelnummer { get; set; }
		public string ESD_ARTIKEL { get; set; }
		public int? Fertigungsnummer { get; set; }
		public int? ArtikelNr { get; set; }
		public string Kennzeichen { get; set; }
		public int? Lagerort_id { get; set; }
		public DateTime? Termin_Bestatigt1 { get; set; }
		public OpenFaEsdResponseModel(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_OFFeneFaEsd entity)
		{
			if(entity == null)
				return;

			ArtikelNr = entity.ArtikelNr;
			Artikelnummer = entity.Artikelnummer;
			ESD_ARTIKEL = entity.ESD_ARTIKEL;
			Fertigungsnummer = entity.Fertigungsnummer;
			Kennzeichen = entity.Kennzeichen;
			Lagerort_id = entity.Lagerort_id;
			Termin_Bestatigt1 = entity.Termin_Bestatigt1;
		}
	}
}
