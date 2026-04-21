using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Models.Article.Statistics.Basics
{
	public class ProofOfUsageRequestModel
	{
		public int ArticleId { get; set; }
		public string CustomerNumber { get; set; }
	}
	public class ProofOfUsageResponseModel
	{
		public string CurrentDate { get; set; } = DateTime.Now.ToString("dddd d, MMM yyyy");
		public string ArticleNumber { get; set; }
		public string Designation1 { get; set; }
		public string Designation2 { get; set; }
		public string GoodsGroup { get; set; }
		public List<Item> Items { get; set; }
		public class Item
		{
			public Single? Anzahl { get; set; }
			public string Artikelnummer { get; set; }
			public int? ArtikelNr { get; set; }
			public string Bezeichnung_1 { get; set; }
			public string Bezeichnung_2 { get; set; }
			public string Freigabestatus { get; set; }
			public string Position { get; set; }
			public bool? Rahmen { get; set; }
			public DateTime? Rahmenauslauf { get; set; }
			public Single? Rahmenmenge { get; set; }
			public string Rahmen_Nr { get; set; }
			public string Sysmonummer { get; set; }
			public string Variante { get; set; }

			public Item(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.BS_ProofOfUsage entity)
			{
				if(entity == null)
					return;
				ArtikelNr = entity.ArtikelNr;
				Anzahl = entity.Anzahl;
				Artikelnummer = entity.Artikelnummer;
				Bezeichnung_1 = entity.Bezeichnung_1;
				Bezeichnung_2 = entity.Bezeichnung_2;
				Freigabestatus = entity.Freigabestatus;
				Position = entity.Position;
				Rahmen = entity.Rahmen;
				Rahmenauslauf = entity.Rahmenauslauf;
				Rahmenmenge = entity.Rahmenmenge;
				Rahmen_Nr = entity.Rahmen_Nr;
				Sysmonummer = entity.Sysmonummer;
				Variante = entity.Variante;
			}
		}
	}
}
