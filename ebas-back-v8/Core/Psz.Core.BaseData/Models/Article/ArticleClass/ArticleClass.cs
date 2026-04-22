namespace Psz.Core.BaseData.Models.Article.ArticleClass
{
	public class ArticleClass
	{
		public string Bezeichnung { get; set; }
		public string Gewerk { get; set; }
		public int ID { get; set; }
		public string Klassifizierung { get; set; }
		public string Kupferzahl { get; set; }
		public string Nummernkreis { get; set; }

		public ArticleClass(Infrastructure.Data.Entities.Tables.PRS.Artikelstamm_KlassifizierungEntity artikelstamm_KlassifizierungEntity)
		{
			if(artikelstamm_KlassifizierungEntity == null)
				return;

			ID = artikelstamm_KlassifizierungEntity.ID;
			Bezeichnung = artikelstamm_KlassifizierungEntity.Bezeichnung;
			Gewerk = artikelstamm_KlassifizierungEntity.Gewerk;
			Klassifizierung = artikelstamm_KlassifizierungEntity.Klassifizierung;
			Kupferzahl = artikelstamm_KlassifizierungEntity.Kupferzahl;
			Nummernkreis = artikelstamm_KlassifizierungEntity.Nummernkreis;

		}
		public Infrastructure.Data.Entities.Tables.PRS.Artikelstamm_KlassifizierungEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.PRS.Artikelstamm_KlassifizierungEntity
			{
				ID = ID,
				Bezeichnung = Bezeichnung,
				Gewerk = Gewerk,
				Klassifizierung = Klassifizierung,
				Kupferzahl = Kupferzahl,
				Nummernkreis = Nummernkreis
			};
		}
	}
}
