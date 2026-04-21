namespace Psz.Core.BaseData.Models.Article
{
	public class ArtikelMinimalModel
	{
		public int ArtikelNr { get; set; }
		public string ArtikelNummer { get; set; }
		public ArtikelMinimalModel()
		{

		}
		public ArtikelMinimalModel(Infrastructure.Data.Entities.Tables.PRS.ArtikelNrsOnlyEntity artikelrohEntity)
		{
			if(artikelrohEntity == null)
				return;
			ArtikelNr = artikelrohEntity.ArtikelNr;
			ArtikelNummer = artikelrohEntity.ArtikelNummer;
		}
	}
}
