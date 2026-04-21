using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.BaseData.Models.Article
{
	public class ArtikelOutilCossModel
	{
		public int ArtikelNRFG { get; set; }
		public string ArtikelnummerFG { get; set; }
		public int ArtikelNRROH { get; set; }
		public string ArtikelnummerROH { get; set; }
		public string Outil { get; set; }
		public bool IsOk { get; set; }
		public ArtikelOutilCossModel() { }

		public ArtikelOutilCossModel(Infrastructure.Data.Entities.Tables.BSD.ArtikelOutilCossEntity artikelrohEntity, bool isOk)
		{
			if(artikelrohEntity == null)
				return;
			ArtikelNRFG = artikelrohEntity.ArtikelNRFG;
			ArtikelnummerFG = artikelrohEntity.ArtikelnummerFG;
			ArtikelNRROH = artikelrohEntity.ArtikelNRROH;
			ArtikelnummerROH = artikelrohEntity.ArtikelnummerROH;
			Outil = artikelrohEntity.Outil;
			IsOk = isOk;
		}
	}
}
