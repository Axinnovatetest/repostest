using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Models.Budget.Reception
{
	public class BookModel
	{
		public int Nr { get; set; }
		public string DeliveryNoteNumber { get; set; }
		public string InvoiceNumber { get; set; }
		public string CFNumber { get; set; }
		public List<Article.UpdateModel> Articles { get; set; }

		public static bool ToInvoiceItems(
			int invoiceId, List<Article.UpdateModel> articles, List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity> articleEntities,
			List<Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity> bestellteArtikelExtensions,
			out List<Tuple<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity, Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity>> results)
		{
			results = new List<Tuple<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity, Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity>>();

			var items = new List<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity>();
			var itemsExt = new List<Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity>();
			foreach(var item in articles)
			{
				var idx = articleEntities.FindIndex(x => x.Nr == item.Nr);
				var idy = bestellteArtikelExtensions.FindIndex(x => x.BestellteArtikelNr == item.Nr);
				if(idx >= 0)
				{
					if(item.ReceivingQuantiy > 0)
					{
						var articleItem = articleEntities[idx];
						articleItem.Bestellung_Nr = invoiceId;
						articleItem.Liefertermin = DateTime.Now;
						articleItem.Lagerort_id = item.Lagerort_id;
						articleItem.Anzahl = item.ReceivingQuantiy;
						articleItem.Gesamtpreis = item.ReceivingQuantiy * item.Einzelpreis / item.Preiseinheit;
						articleItem.erledigt_pos = true;
						items.Add(articleItem);

						// - 
						if(idy >= 0)
						{
							var articleItemExt = bestellteArtikelExtensions[idy];
							articleItemExt.TotalCost = (item.ReceivingQuantiy / articleItemExt.Quantity) * articleItemExt.TotalCost;
							articleItemExt.TotalCostDefaultCurrency = (item.ReceivingQuantiy / articleItemExt.Quantity) * articleItemExt.TotalCostDefaultCurrency;
							articleItemExt.VAT = (item.ReceivingQuantiy / articleItemExt.Quantity) * articleItemExt.VAT;
							articleItemExt.Quantity = item.ReceivingQuantiy;
							itemsExt.Add(articleItemExt);

							// - 
							results.Add(new Tuple<Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity, Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity>
							(articleItem, articleItemExt));
						}
					}
				}
			}

			return results.Count > 0;
		}
	}
}

