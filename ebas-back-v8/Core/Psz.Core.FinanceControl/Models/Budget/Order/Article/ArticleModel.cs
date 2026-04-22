using System;
using System.Collections.Generic;

namespace Psz.Core.FinanceControl.Models.Budget.Order.Article
{
	public class ArticleModel
	{
		public string Currency_Article { get; set; }
		public int Id_AO { get; set; }
		public int Id_Article { get; set; }
		public string Name_Article { get; set; }
		public int? Id_Currency_Article { get; set; }
		public int Id_Order { get; set; }
		public decimal? Quantity { get; set; }
		public decimal? TotalCost_Article { get; set; }
		public decimal? Unit_Price { get; set; }
		public int? Location_Id { get; set; }
		public string Location_Name { get; set; }
		public int? Account_Id { get; set; }
		public string Account_Name { get; set; }
		public DateTime? Delivery_Date { get; set; }
		public DateTime? Confirmation_Date { get; set; }
		public string Description { get; set; }
		public string Internal_Contact { get; set; }
		public decimal? VAT { get; set; }

		public int? Position { get; set; }

		public int? Artikel_Nr { get; set; }
		public string BestellNummer { get; set; }
		public string Bezeichnung_1 { get; set; }
		public string Bezeichnung_2 { get; set; }

		public int? DefaultCurrencyDecimals { get; set; }
		public int? DefaultCurrencyId { get; set; }
		public string DefaultCurrencyName { get; set; }
		public decimal? DefaultCurrencyRate { get; set; }
		public decimal? UnitPriceDefaultCurrency { get; set; }
		public decimal? TotalCostDefaultCurrency { get; set; }

		public string SupplierOrderNumber { get; set; }
		public DateTime? SupplierDeliveryDate { get; set; }


		public ArticleModel() { }
		public ArticleModel(Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity artikelOrderEntity,
			Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity bestellteArtikelEntity,
			List<Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity> artikel_BudgetEntities)
			: this(artikelOrderEntity, bestellteArtikelEntity)
		{
			var artikel = artikel_BudgetEntities?.Find(x => x.Artikel_Nr == artikelOrderEntity.ArticleId);
			if(artikel != null)
			{
				Name_Article = artikel.Artikelnummer;
			}
		}
		public ArticleModel(Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity artikelOrderEntity,
			Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity bestellteArtikelEntity)
		{
			if(artikelOrderEntity == null)
				return;

			Id_AO = artikelOrderEntity.Id;
			Id_Order = artikelOrderEntity.OrderId;
			Id_Article = artikelOrderEntity.ArticleId;

			Id_Currency_Article = artikelOrderEntity.CurrencyId;
			Currency_Article = artikelOrderEntity.CurrencyName;
			Quantity = artikelOrderEntity.Quantity;
			Unit_Price = artikelOrderEntity.UnitPrice;
			TotalCost_Article = artikelOrderEntity.TotalCost;
			Account_Id = artikelOrderEntity.AccountId;

			Confirmation_Date = artikelOrderEntity.ConfirmationDate;
			Delivery_Date = artikelOrderEntity.DeliveryDate;
			Description = artikelOrderEntity.Description;
			Internal_Contact = artikelOrderEntity.InternalContact;
			Location_Id = artikelOrderEntity.LocationId;
			Account_Name = artikelOrderEntity.AccountName;
			Location_Name = artikelOrderEntity.LocationName;
			//Location_Name = lagerorteEntity.Lagerort;

			VAT = artikelOrderEntity.VAT;

			Position = bestellteArtikelEntity?.Position;
			Artikel_Nr = bestellteArtikelEntity?.Artikel_Nr;
			BestellNummer = bestellteArtikelEntity?.Bestellnummer;
			Bezeichnung_1 = bestellteArtikelEntity?.Bezeichnung_1;
			Bezeichnung_2 = bestellteArtikelEntity?.Bezeichnung_2;

			DefaultCurrencyId = artikelOrderEntity.DefaultCurrencyId;
			DefaultCurrencyName = artikelOrderEntity.DefaultCurrencyName;
			DefaultCurrencyRate = artikelOrderEntity.DefaultCurrencyRate;
			DefaultCurrencyDecimals = artikelOrderEntity.DefaultCurrencyDecimals;
			UnitPriceDefaultCurrency = artikelOrderEntity.UnitPriceDefaultCurrency;
			TotalCostDefaultCurrency = artikelOrderEntity.TotalCostDefaultCurrency;


			SupplierOrderNumber = artikelOrderEntity.SupplierOrderNumber;
			SupplierDeliveryDate = artikelOrderEntity.SupplierDeliveryDate;
		}


		public Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity ToBestellteArtikelEntity(/*Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity artikel_BudgetEntity*/)
		{
			return new Infrastructure.Data.Entities.Tables.FNC.Bestellte_ArtikelEntity
			{
				//AB_Nr_Lieferant = artikel_BudgetEntity.ab
				Aktuelle_Anzahl = Quantity,
				//AnfangLagerBestand = (dataRow["AnfangLagerBestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["AnfangLagerBestand"]);
				Anzahl = Quantity,
				Artikel_Nr = Artikel_Nr,
				//Bemerkung_Pos = (dataRow["Bemerkung_Pos"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bemerkung_Pos"]);
				//Bemerkung_Pos_ID = (dataRow["Bemerkung_Pos_ID"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Bemerkung_Pos_ID"]);
				Bestatigter_Termin = SupplierDeliveryDate,
				Bestellnummer = BestellNummer,
				Bestellung_Nr = Id_Order,
				Bezeichnung_1 = Bezeichnung_1,
				Bezeichnung_2 = Bezeichnung_2,
				//BP_zu_RBposition = (dataRow["BP zu RBposition"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["BP zu RBposition"]);
				//COC_bestatigung = (dataRow["COC_bestatigung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["COC_bestatigung"]);
				//CUPreis = (dataRow["CUPreis"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["CUPreis"]);
				Einheit = "",
				Einzelpreis = Unit_Price,
				//EMPB_Bestatigung = (dataRow["EMPB_Bestatigung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["EMPB_Bestatigung"]);
				//EndeLagerBestand = (dataRow["EndeLagerBestand"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["EndeLagerBestand"]);
				Erhalten = 0,
				erledigt_pos = false,
				Gesamtpreis = (Quantity * Unit_Price),
				//In_Bearbeitung = (dataRow["In Bearbeitung"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["In Bearbeitung"]);
				//InfoRahmennummer = (dataRow["InfoRahmennummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["InfoRahmennummer"]);
				//Kanban = (dataRow["Kanban"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Kanban"]);
				Lagerort_id = Location_Id,
				Liefertermin = Delivery_Date,
				//Löschen = (dataRow["Löschen"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["Löschen"]);
				//MhdDatumArtikel = (dataRow["MhdDatumArtikel"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["MhdDatumArtikel"]);
				Position = Position,
				Position_erledigt = false,
				Preiseinheit = 1,
				//Preisgruppe = (dataRow["Preisgruppe"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Preisgruppe"]);
				//Produktionsort = (dataRow["Produktionsort"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Produktionsort"]);
				Rabatt = 0,
				Rabatt1 = 0,
				//RB_Abgerufen = (dataRow["RB_Abgerufen"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["RB_Abgerufen"]);
				//RB_Offen = (dataRow["RB_Offen"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["RB_Offen"]);
				//RB_OriginalAnzahl = (dataRow["RB_OriginalAnzahl"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["RB_OriginalAnzahl"]);
				//schriftart = (dataRow["schriftart"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["schriftart"]);
				//sortierung = (dataRow["sortierung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["sortierung"]);
				Start_Anzahl = Quantity,
				Umsatzsteuer = VAT,
				//WE_Pos_zu_Bestellposition =
			};
		}
		public Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity ToBestellteExtensionEntity(/*Infrastructure.Data.Entities.Tables.FNC.Artikel_BudgetEntity artikel_BudgetEntity*/)
		{
			return new Infrastructure.Data.Entities.Tables.FNC.BestellteArtikelExtensionEntity
			{
				Id = Id_AO,
				OrderId = Id_Order,
				ArticleId = Id_Article,
				CurrencyId = Id_Currency_Article,
				CurrencyName = Currency_Article,
				Quantity = Quantity,
				UnitPrice = (decimal?)Unit_Price,
				TotalCost = (decimal?)TotalCost_Article,
				AccountId = Account_Id,
				ConfirmationDate = Confirmation_Date,
				DeliveryDate = Delivery_Date,
				Description = Description,
				InternalContact = Internal_Contact,
				LocationId = Location_Id,
				AccountName = Account_Name,
				LocationName = Location_Name,
				VAT = VAT,
				BestellteArtikelNr = -1,

				DefaultCurrencyId = DefaultCurrencyId,
				DefaultCurrencyName = DefaultCurrencyName,
				DefaultCurrencyRate = DefaultCurrencyRate,
				DefaultCurrencyDecimals = DefaultCurrencyDecimals,
				UnitPriceDefaultCurrency = UnitPriceDefaultCurrency,
				TotalCostDefaultCurrency = TotalCostDefaultCurrency,

				SupplierDeliveryDate = SupplierDeliveryDate,
				SupplierOrderNumber = SupplierOrderNumber
			};
		}
	}
}
