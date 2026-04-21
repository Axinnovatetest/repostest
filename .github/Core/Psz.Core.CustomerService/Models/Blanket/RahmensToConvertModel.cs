using System;

namespace Psz.Core.CustomerService.Models.Blanket
{
	public class RahmensToConvertModel
	{
		public string Artikelnummer { get; set; }
		public int? Artikel_Nr { get; set; }
		public bool? Rahmen { get; set; }
		public string Rahmen_Nr { get; set; }
		public decimal? Rahmenmenge { get; set; }
		public DateTime? Rahmenauslauf { get; set; }
		public bool? Rahmen2 { get; set; }
		public string Rahmen_Nr2 { get; set; }
		public decimal? Rahmenmenge2 { get; set; }
		public DateTime? Rahmenauslauf2 { get; set; }
		public decimal? Einkaufspreis { get; set; }
		public string Name1 { get; set; }
		public RahmensToConvertModel()
		{

		}
		public RahmensToConvertModel(Infrastructure.Data.Entities.Joins.CTS.RahmensToConvertEntity entity)
		{
			Artikelnummer = entity.Artikelnummer;
			Artikel_Nr = entity.Artikel_Nr;
			Rahmen = entity.Rahmen;
			Rahmen_Nr = entity.Rahmen_Nr;
			Rahmenmenge = entity.Rahmenmenge;
			Rahmenauslauf = entity.Rahmenauslauf;
			Rahmen2 = entity.Rahmen2;
			Rahmen_Nr2 = entity.Rahmen_Nr2;
			Rahmenmenge2 = entity.Rahmenmenge2;
			Rahmenauslauf2 = entity.Rahmenauslauf2;
			Einkaufspreis = entity.Einkaufspreis;
			Name1 = entity.Name1;
		}
	}
}
