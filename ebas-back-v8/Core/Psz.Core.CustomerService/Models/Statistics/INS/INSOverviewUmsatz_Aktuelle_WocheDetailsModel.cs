using Psz.Core.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.CustomerService.Models.Statistics.INS
{
	public class INSOverviewUmsatz_Aktuelle_WocheDetailsModel
	{
		public int? AngeboteNr { get; set; }
		public int? Nr { get; set; }
		public int? Position { get; set; }
		public int? ArtikelNr { get; set; }
		public string Artikelnummer { get; set; }
		public decimal? Anzahl { get; set; }
		public decimal? Gesamtpreis { get; set; }
		public INSOverviewUmsatz_Aktuelle_WocheDetailsModel()
		{

		}

		public INSOverviewUmsatz_Aktuelle_WocheDetailsModel(Infrastructure.Data.Entities.Joins.CTS.INSOverview.INSOverviewUmsatz_Aktuelle_WocheDetailsEntity entity)
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
	public class INSOverviewUmsatz_Aktuelle_WocheDetailsRequestModel: IPaginatedRequestModel
	{
		public DateTime Date { get; set; }
		public string SearchText { get; set; }
	}
	public class INSOverviewUmsatz_Aktuelle_WocheDetailsResponseModel: IPaginatedResponseModel<INSOverviewUmsatz_Aktuelle_WocheDetailsModel> { }

}
