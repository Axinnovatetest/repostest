using Psz.Core.Common.Models;
using System;
using System.Collections.Generic;

namespace Psz.Core.CustomerService.Models.Statistics.INS
{
	public class INSOverviewVK_Summe_ungebuchte_ABDetailsModel
	{
		public int? AngeboteNr { get; set; }
		public int? Nr { get; set; }
		public int? Position { get; set; }
		public int? ArtikelNr { get; set; }
		public string Artikelnummer { get; set; }
		public decimal? Anzahl { get; set; }
		public decimal? Gesamtpreis { get; set; }
		public INSOverviewVK_Summe_ungebuchte_ABDetailsModel()
		{

		}
		public INSOverviewVK_Summe_ungebuchte_ABDetailsModel(Infrastructure.Data.Entities.Joins.CTS.INSOverview.INSOverviewVK_Summe_ungebuchte_ABDetailsEntity entity)
		{
			AngeboteNr = entity.AngeboteNr;
			Nr = entity.Nr;
			Position = entity.Position;
			ArtikelNr = entity.ArtikelNr;
			Artikelnummer = entity.Artikelnummer;
			Anzahl = entity.Anzahl;
			Gesamtpreis = entity.Gesamtpreis;
		}
	}
	public class INSOverviewVK_Summe_ungebuchte_ABDetailsRequestModel: IPaginatedRequestModel
	{
		public bool OlderDate { get; set; }
		public string Date { get; set; }
		public int? CustomerNumber { get; set; }
		public int? userId { get; set; }
		public string SearchText { get; set; }
	}
	public class INSOverviewVK_Summe_ungebuchte_ABDetailsResponseModel: IPaginatedResponseModel<INSOverviewVK_Summe_ungebuchte_ABDetailsModel> { }
}
