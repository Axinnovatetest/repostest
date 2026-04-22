using System;

namespace Psz.Core.CustomerService.Models.Blanket
{
	public class PosBestellLinkBlanketModel
	{
		public string ArticleNumber { get; set; }
		public int? ArtikelNr { get; set; }
		public int? OrderNumber { get; set; }
		public string Description { get; set; }
		public DateTime? DeliveryDate { get; set; }
		public decimal? Quantity { get; set; }
		public decimal? UnitPrice { get; set; }
		public decimal? TotalPrice { get; set; }
		public string BlanketNumber { get; set; }
		public int? BlanketNr { get; set; }
		public decimal? OpenQuantity { get; set; }

		public PosBestellLinkBlanketModel()
		{

		}

		public PosBestellLinkBlanketModel(Infrastructure.Data.Entities.Tables.MTM.Bestellte_ArtikelEntity entity,
			Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity artikelEntity,
			Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity raEntity,
			Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity raPosEntity
			)
		{
			ArticleNumber = artikelEntity?.ArtikelNummer;
			ArtikelNr = entity.Artikel_Nr;
			OrderNumber = entity.Bestellung_Nr;
			Description = entity.Bezeichnung_1;
			DeliveryDate = entity.Liefertermin;
			Quantity = entity.Start_Anzahl;
			OpenQuantity = entity.Anzahl;
			UnitPrice = entity.Einzelpreis;
			TotalPrice = entity.Einzelpreis * entity.Start_Anzahl;
			BlanketNr = raEntity?.Nr;
			BlanketNumber = raEntity is not null && raPosEntity is not null ? $"{raEntity.Bezug} | Pos {raPosEntity.Position}" : "";
		}
	}
}
