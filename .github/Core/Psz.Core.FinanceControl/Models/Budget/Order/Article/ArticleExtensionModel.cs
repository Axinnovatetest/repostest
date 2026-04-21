using System;

namespace Psz.Core.FinanceControl.Models.Budget.Order.Article
{
	public class ArticleExtensionModel
	{
		public int? AccountId { get; set; }
		public string AccountName { get; set; }
		public int ArticleId { get; set; }
		public int BestellteArtikelNr { get; set; }
		public DateTime? ConfirmationDate { get; set; }
		public int? CurrencyId { get; set; }
		public string CurrencyName { get; set; }
		public DateTime? DeliveryDate { get; set; }
		public string Description { get; set; }
		public decimal? Discount { get; set; }
		public int Id { get; set; }
		public string InternalContact { get; set; }
		public int? LocationId { get; set; }
		public string LocationName { get; set; }
		public int OrderId { get; set; }
		public decimal? Quantity { get; set; }
		public decimal? TotalCost { get; set; }
		public decimal? UnitPrice { get; set; }
		public decimal? VAT { get; set; }

		public ArticleExtensionModel(Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity entity)
		{
			if(entity == null)
			{
				return;
			}

			AccountId = entity.AccountId;
			AccountName = entity.AccountName;
			ArticleId = entity.ArticleId;
			BestellteArtikelNr = entity.BestellteArtikelNr;
			ConfirmationDate = entity.ConfirmationDate;
			CurrencyId = entity.CurrencyId;
			CurrencyName = entity.CurrencyName;
			DeliveryDate = entity.DeliveryDate;
			Description = entity.Description;
			Discount = entity.Discount;
			Id = entity.Id;
			InternalContact = entity.InternalContact;
			LocationId = entity.LocationId;
			LocationName = entity.LocationName;
			OrderId = entity.OrderId;
			Quantity = entity.Quantity;
			TotalCost = entity.TotalCost;
			UnitPrice = entity.UnitPrice;
			VAT = entity.VAT;
		}

		public Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity
			{
				AccountId = AccountId,
				AccountName = AccountName,
				ArticleId = ArticleId,
				BestellteArtikelNr = BestellteArtikelNr,
				ConfirmationDate = ConfirmationDate,
				CurrencyId = CurrencyId,
				CurrencyName = CurrencyName,
				DeliveryDate = DeliveryDate,
				Description = Description,
				Discount = Discount,
				Id = Id,
				InternalContact = InternalContact,
				LocationId = LocationId,
				LocationName = LocationName,
				OrderId = OrderId,
				Quantity = Quantity,
				TotalCost = TotalCost,
				UnitPrice = UnitPrice,
				VAT = VAT,
			};
		}
	}
}
