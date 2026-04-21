using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psz.Core.MaterialManagement.Models
{
	public class DashboardResponseModel
	{
		public List<BlanketItem> RedPositions { get; set; } = new List<BlanketItem>();
		public List<BlanketItem> OrangePositions { get; set; } = new List<BlanketItem>();
		public List<BlanketItem> GreenPositions { get; set; } = new List<BlanketItem>();
	}
	public class BlanketItem
	{
		public int AngeboteArtiklNr { get; set; }
		public int AngeboteVorfallNr { get; set; }
		public string AngeboteProjektNr { get; set; }
		public int? Position { get; set; }
		public string Material { get; set; }
		public decimal? Zielmenge { get; set; }
		public string Bezeichnung { get; set; }
		public string KundenMatNummer { get; set; }
		public decimal? Preis { get; set; }
		public decimal? PreisDefault { get; set; }
		public string ME { get; set; }
		public DateTime? ValidFrom { get; set; }
		public DateTime? DateOfExpiry { get; set; }
		public DateTime? ExtensionDate { get; set; }
		public decimal? Gesamtpreis { get; set; }
		public decimal? GesamtpreisDefault { get; set; }
		public string WahrungName { get; set; }
		public string WahrungSymbole { get; set; }
		public int? WahrungId { get; set; }
		public int? AngebotNr { get; set; }
		public int? MaterialNr { get; set; }
		public decimal? DelivredQuantity { get; set; }
		public decimal? RestQuantity { get; set; }
		public bool? Done { get; set; }
		public bool? DateExpired { get; set; }
		public int LinkedToAB { get; set; }
		public decimal BasePrice { get; set; }
		public int? Lagerort { get; set; }

		public BlanketItem()
		{

		}
		public BlanketItem(
			Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity raEntity,
			Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity entity,
			Infrastructure.Data.Entities.Tables.CTS.AngeboteArticleBlanketExtensionEntity extensionEntity)
		{
			if(entity == null)
				return;
			AngeboteArtiklNr = entity.Nr;
			AngeboteVorfallNr = raEntity?.Angebot_Nr ?? -1;
			AngeboteProjektNr = raEntity?.Projekt_Nr;
			Position = entity.Position;
			Zielmenge = extensionEntity?.Zielmenge;
			Bezeichnung = entity?.Bezeichnung1;
			KundenMatNummer = entity?.Bezeichnung2;
			BasePrice = extensionEntity?.BasePrice ?? 0;
			Preis = extensionEntity?.Preis;
			PreisDefault = extensionEntity.PreisDefault;
			ME = extensionEntity?.ME;
			ValidFrom = extensionEntity?.GultigAb;
			DateOfExpiry = extensionEntity?.GultigBis;
			Gesamtpreis = extensionEntity?.Gesamtpreis;
			GesamtpreisDefault = extensionEntity.GesamtpreisDefault;
			MaterialNr = entity?.ArtikelNr;
			WahrungName = extensionEntity?.WahrungName;
			WahrungSymbole = extensionEntity?.WahrungSymbole;
			WahrungId = extensionEntity?.WahrungId;
			Material = extensionEntity?.Material;
			AngebotNr = entity?.AngebotNr;
			DelivredQuantity = entity.Geliefert;
			RestQuantity = entity.Anzahl;
			Done = entity.erledigt_pos;
			DateExpired = DateTime.Now > extensionEntity.GultigBis ? true : false;
			ExtensionDate = extensionEntity.GultigBis;
		}
	}
}
