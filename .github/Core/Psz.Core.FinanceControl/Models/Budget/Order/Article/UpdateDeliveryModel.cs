using System;

namespace Psz.Core.FinanceControl.Models.Budget.Order.Article
{
	public class UpdateDeliveryModel
	{
		public int OrderId { get; set; }
		public int ArticleId { get; set; }
		public string SupplierOrderNumber { get; set; }
		public DateTime? SupplierDeliveryDate { get; set; }

		public Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity ToEntity(
			Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity artikelEntity)
		{
			if(artikelEntity != null)
			{
				artikelEntity.Bestatigter_Termin = SupplierDeliveryDate;
			}

			return artikelEntity;
		}
		public Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity ToExtensionEntity(
			Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity artikelEntity)
		{
			if(artikelEntity != null)
			{
				artikelEntity.SupplierDeliveryDate = SupplierDeliveryDate;
				artikelEntity.SupplierOrderNumber = SupplierOrderNumber;
			}

			return artikelEntity;
		}
	}
}
