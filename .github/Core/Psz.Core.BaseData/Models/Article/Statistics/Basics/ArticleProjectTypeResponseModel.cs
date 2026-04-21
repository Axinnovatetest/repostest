using System.Collections.Generic;

namespace Psz.Core.BaseData.Models.Article.Statistics.Basics
{
	public class ArticleProjectTypeResponseModel
	{
		public List<Projecktart> Projectarts { get; set; }
		// - paginate data
		public int AllCount { get; set; }
		public int AllPagesCount { get; set; }
		public int RequestedPage { get; set; }
		public int ItemsPerPage { get; set; }

		public class Projecktart
		{
			public string artikelklassifizierung { get; set; }
			public string Artikelnummer { get; set; }
			public int? ArtikelNr { get; set; }
			public string Freigabestatus { get; set; }
			public string Warengruppe { get; set; }
			public string Zeichnungsnummer { get; set; }

			public Projecktart(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_ProjektartArtikel entity)
			{
				if(entity == null)
					return;
				ArtikelNr = entity.ArtikelNr;
				artikelklassifizierung = entity.artikelklassifizierung;
				Artikelnummer = entity.Artikelnummer;
				Freigabestatus = entity.Freigabestatus;
				Warengruppe = entity.Warengruppe;
				Zeichnungsnummer = entity.Zeichnungsnummer;
			}
		}
	}
}
