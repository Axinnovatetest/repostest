using System;

namespace Psz.Core.CustomerService.Models.Blanket
{
	public class LinkToABPositionModel
	{
		public int? AngeboteNr { get; set; }
		public int? KundunNr { get; set; }
		public string ArtikelNummer { get; set; }
		public int? Anzahl { get; set; }
		public int? Nr { get; set; }
		public int? NrRA { get; set; }
		public int? Position { get; set; }
		public DateTime? DateDebut { get; set; }
		public DateTime? DateFin { get; set; }
		public LinkToABPositionModel()
		{

		}
		public LinkToABPositionModel(Infrastructure.Data.Entities.Joins.CTS.RahmenLinkToAbPosEntity entity)
		{
			AngeboteNr = entity.AngeboteNr;
			KundunNr = entity.KundunNr;
			ArtikelNummer = entity.ArtikelNummer;
			Anzahl = entity.Anzahl;
			Nr = entity.Nr;
			NrRA = entity.NrRA;
			Position = entity.Position;
			DateDebut = entity.DateDebut;
			DateFin = entity.DateFin;
		}


	}
}
