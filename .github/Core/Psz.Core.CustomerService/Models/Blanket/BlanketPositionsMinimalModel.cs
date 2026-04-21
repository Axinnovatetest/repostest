using System;

namespace Psz.Core.CustomerService.Models.Blanket
{
	public class BlanketPositionsMinimalModel
	{
		public int Id { get; set; }
		public int? Position { get; set; }
		public string Material { get; set; }
		public int? ArticleNr { get; set; }
		public string Bezeichung { get; set; }
		public string Bezeichung1 { get; set; }
		public string ME { get; set; }
		public decimal? Zeilmenge { get; set; }
		public decimal? Preis { get; set; }
		public decimal? PreisDefault { get; set; }
		public decimal? Gesamtpreis { get; set; }
		public decimal? GesamtpreisDefault { get; set; }

		public string Wahrung { get; set; }
		public string WarungSymbol { get; set; }
		public DateTime? Gultigab { get; set; }
		public DateTime? Gultigbis { get; set; }

		public decimal? DelivredQuantity { get; set; }
		public decimal? RestQuantity { get; set; }
		public bool? Done { get; set; }
		public bool? DateExpired { get; set; }
		public DateTime? ExtensionDate { get; set; }

		public BlanketPositionsMinimalModel(Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity angeboteEntity,
			Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity extensionEntity)
		{
			Id = extensionEntity.AngeboteArtikelNr;
			Position = angeboteEntity.Position;
			Material = extensionEntity.Material;
			ArticleNr = angeboteEntity.ArtikelNr;
			Bezeichung = angeboteEntity.Bezeichnung1;
			Bezeichung1 = angeboteEntity.Bezeichnung2;
			ME = extensionEntity.ME;
			Zeilmenge = extensionEntity.Zielmenge;
			Preis = extensionEntity.Preis;
			PreisDefault = extensionEntity.PreisDefault;
			Gesamtpreis = extensionEntity.Gesamtpreis;
			GesamtpreisDefault = extensionEntity.GesamtpreisDefault;
			Wahrung = extensionEntity.WahrungName;
			WarungSymbol = extensionEntity.WahrungSymbole;
			Gultigab = extensionEntity.GultigAb;
			Gultigbis = extensionEntity.GultigBis;
			DelivredQuantity = angeboteEntity.Geliefert;
			RestQuantity = angeboteEntity.Anzahl;
			Done = angeboteEntity.erledigt_pos;
			DateExpired = DateTime.Now > extensionEntity.GultigBis ? true : false;
			ExtensionDate = extensionEntity.GultigBis;
		}
	}
}
