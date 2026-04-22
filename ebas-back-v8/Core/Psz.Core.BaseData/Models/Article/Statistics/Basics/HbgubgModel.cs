using System;

namespace Psz.Core.BaseData.Models.Article.Statistics.Basics
{
	public class HbgubgModel
	{
		public string HBG_Freigabestatus { get; set; }
		public string HBG_FG { get; set; }
		public int? Losgroesse_UBG { get; set; }
		public int? Losgroesse_HBG { get; set; }
		public Single? Menge_Stuckliste { get; set; }
		public bool? UBG { get; set; }
		public string UBG_Artikelnummer { get; set; }
		public int? UBG_ArtikelNr { get; set; }
		public string UBG_Warengruppe { get; set; }
		public HbgubgModel(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_HbgUbg entity)
		{
			if(entity == null)
			{
				return;
			}
			UBG_ArtikelNr = entity.UBG_ArtikelNr;
			HBG_Freigabestatus = entity.HBG_Freigabestatus;
			HBG_FG = entity.HBG_FG;
			Losgroesse_UBG = entity.Losgroesse_UBG;
			Losgroesse_HBG = entity.Losgroesse_HBG;
			Menge_Stuckliste = entity.Menge_Stuckliste;
			UBG = entity.UBG;
			UBG_Artikelnummer = entity.UBG_Artikelnummer;
			UBG_Warengruppe = entity.UBG_Warengruppe;
		}
	}
}
