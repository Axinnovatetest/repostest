using System;
using System.Collections.Generic;
using System.Linq;

namespace Psz.Core.CustomerService.Models.OrderProcessing
{
	public class DeliveryNoteRequestModel
	{
		public int CustomerNumber { get; set; }
		public DateTime? DateFrom { get; set; }
		public DateTime? DateTo { get; set; }
	}
	public class DeliveryNoteResponseModel
	{
		public string CustomerName { get; set; }
		public string CustomerNumber { get; set; }
		public string ShippingMethod { get; set; }

		public List<DeliveryNoteItem> DeliveryNoteItems { get; set; }
		public DeliveryNoteResponseModel(
			List<Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity> angeboteEntities,
				List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity> angeboteneArtikelEntities,
				List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity> artikelEntities)
		{
			if(angeboteEntities?.Count > 0)
			{
				CustomerName = angeboteEntities[0].Vorname_NameFirma;
				CustomerNumber = angeboteEntities[0].Ihr_Zeichen;
				ShippingMethod = angeboteEntities[0].Versandart;

				// - 
				DeliveryNoteItems = new List<DeliveryNoteItem>();
				foreach(var item in angeboteEntities)
				{
					var positions = angeboteneArtikelEntities.Where(x => x.AngebotNr == item.Nr)?.ToList();
					var articles = artikelEntities.Where(x => positions?.Any(y => y.ArtikelNr == x.ArtikelNr) == true)?.ToList();
					DeliveryNoteItems.Add(new DeliveryNoteItem(item, positions, articles));
				}
			}
		}

		public class DeliveryNoteItem
		{
			public string DeliveryNoteNumber { get; set; }
			// - 
			public int DeliveryNotePosId { get; set; }
			public int DeliveryNotePosition { get; set; }
			public string ArticleNumber { get; set; }
			public string Designation1 { get; set; }
			public string Designation2 { get; set; }
			public decimal Quantity { get; set; }
			public decimal Weight { get; set; }
			public string Unit { get; set; }
			public decimal TotalWeight { get; set; }
			public DeliveryNoteItem(Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity angeboteEntity,
				List<Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity> angeboteneArtikelEntities,
				List<Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity> artikelEntities)
			{
				if(angeboteEntity is null)
				{
					return;
				}

				DeliveryNoteNumber = angeboteEntity.Angebot_Nr?.ToString();
				// -
				foreach(var item in angeboteneArtikelEntities)
				{
					var article = artikelEntities.FirstOrDefault(x => x.ArtikelNr == item.ArtikelNr);

					DeliveryNotePosId = item.Nr;
					DeliveryNotePosition = item.Position ?? 0;
					ArticleNumber = article?.ArtikelNummer;
					Designation1 = item.Bezeichnung1;
					Designation2 = item.Bezeichnung2;
					Quantity = item.Anzahl ?? 0;
					Weight = item.EinzelCuGewicht ?? 0;
					Unit = item.Einheit;
					TotalWeight = item.GesamtCuGewicht ?? 0;
				}
			}
		}
	}
}
