using Psz.Core.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Psz.Core.MaterialManagement.Orders.Models.Statistics.SupplierHistoryOrderResponseModel;

namespace Psz.Core.MaterialManagement.Orders.Models.Statistics
{
	public class SupplierStufeResponseModel
	{
		public int AdressenNr { get; set; }
		public string Name { get; set; }
		public string Stufe { get; set; }
		public int SupplierId { get; set; }
		public int SupplierNumber { get; set; }
		public SupplierStufeResponseModel(Infrastructure.Data.Entities.Tables.PRS.AdressenEntity entity,
			Infrastructure.Data.Entities.Tables.BSD.LieferantenEntity lieferanten)
		{
			if(entity == null)
			{
				return;
			}

			// - 
			AdressenNr = entity.Nr;
			Name = entity.Name1;
			Stufe = entity.Stufe;
			SupplierId = lieferanten?.Nr ?? -1;
			SupplierNumber = entity.Lieferantennummer ?? -1;
		}
	}
	public class SupplierOverviewResponseModel
	{
		public int SupplierId { get; set; }
		public int SupplierAddressNr { get; set; }
		public string SupplierName { get; set; }
		public bool SupplierBlockedForFurtherBe { get; set; }
		public bool SupplierAddressBlocked { get; set; }
		public string Stufe { get; set; }
		public int SyncId { get; set; }
		public DateTime SyncDate { get; set; }
		// -
		public int StandardActiveArticlesCount { get; set; }
		public int StandardArticlesCount { get; set; }
		public int AllActiveArticlesCount { get; set; }
		public int AllArticlesCount { get; set; }

		// - Last n Years data
		public SupplierYearItem CurrentYear { get; set; }
		public SupplierYearItem LastN1Year { get; set; }
		public SupplierYearItem LastN2Year { get; set; }
		public SupplierYearItem LastN3Year { get; set; }
		public SupplierYearItem LastN4Year { get; set; }

		// - last n Year data per KW
		public List<SupplierPuchaseItem> PurchaseTotalBeCountsCurrentYear { get; set; }
		public List<SupplierPuchaseItem> PurchaseTotalBeCountsLastN1Year { get; set; }
		public List<SupplierPuchaseItem> PurchaseTotalBeCountsLastN2Year { get; set; }
		public List<SupplierPuchaseItem> PurchaseTotalBeCountsLastN3Year { get; set; }
		public List<SupplierPuchaseItem> PurchaseTotalBeCountsLastN4Year { get; set; }
		public List<SupplierPuchaseItem> PurchaseTotalBeAmountsCurrentYear { get; set; }
		public List<SupplierPuchaseItem> PurchaseTotalBeAmountsLastN1Year { get; set; }
		public List<SupplierPuchaseItem> PurchaseTotalBeAmountsLastN2Year { get; set; }
		public List<SupplierPuchaseItem> PurchaseTotalBeAmountsLastN3Year { get; set; }
		public List<SupplierPuchaseItem> PurchaseTotalBeAmountsLastN4Year { get; set; }

		// - BE stats =//=> by Lager
		public List<SupplierLagerItem> BeTotal { get; set; }
		public List<SupplierLagerItem> BeClosed { get; set; }
		public List<SupplierLagerItem> BeDelays { get; set; }
		public List<SupplierLagerItem> BeOpen { get; set; }
		// - Be lists
		public List<SupplierLagerItem> BeUnplaced { get; set; }
		public List<SupplierLagerItem> BeUnconfirmed { get; set; }
		public List<SupplierLagerItem> BeDeliveryOverdue { get; set; }
		public List<SupplierLagerItem> BeNext4KwDelivery { get; set; }

		// - 
		public List<KeyValuePair<int, string>> Lagers { get; set; }

	}
	public class SupplierYearItem
	{
		public int Year { get; set; }
		public long BeCount { get; set; }
		public long BeArticleCount { get; set; }
		public decimal BeAmount { get; set; }
	}
	public class SupplierLagerItem
	{
		public int Lager { get; set; }
		public long BeCount { get; set; }
		public long BeArticleCount { get; set; }
		public decimal BeAmount { get; set; }
	}
	public class SupplierPuchaseItem
	{
		public int Index { get; set; }
		public string Kw { get; set; }
		public decimal KwValue { get; set; }
	}

	public class SupplierHistoryOrderRequestModel: IPaginatedRequestModel
	{
		public int Year { get; set; }
		public int? AddressNr { get; set; }
		public int? LagerId { get; set; }
		public string QueryTerm { get; set; }
		public int? OrderType { get; set; }
	}
	public class SupplierHistoryOrderResponseModel: IPaginatedResponseModel<SupplierHistoryItem>
	{

	}
	public class SupplierHistoryItem
	{
		public decimal? Anzahl { get; set; }
		public int? Bestellung_Nr { get; set; }
		public DateTime? Datum { get; set; }
		public decimal? Einzelpreis { get; set; }
		public decimal? Gesamtpreis { get; set; }
		public int? LagerId { get; set; }
		public string Lieferant { get; set; }
		public string Artikelnummer { get; set; }
		public string Position { get; set; }
		public int? OrderId { get; set; }
		public int? ArticleId { get; set; }
		public int? SupplierId { get; set; }
		public string Lager { get; set; }
		public SupplierHistoryItem(Infrastructure.Data.Access.Joins.ArticleStatisticsAccess.Sales.PrsSupplierOrderHistoryEntity entity)
		{
			if(entity is null)
			{
				return;
			}
			// - 
			Anzahl = entity.Anzahl;
			Bestellung_Nr = entity.Bestellung_Nr;
			Datum = entity.Datum;
			Einzelpreis = entity.Einzelpreis;
			Gesamtpreis = entity.Gesamtpreis;
			LagerId = entity.LagerId;
			Lieferant = entity.Lieferant;
			Artikelnummer = entity.Artikelnummer;
			OrderId = entity.OrderId;
			ArticleId = entity.ArticleId;
			SupplierId = entity.SupplierId;
			Lager = entity.Lager;
			Position = entity.Position;
		}
	}

	public class SupplierArticleRequestModel: IPaginatedRequestModel
	{
		public int? AddressNr { get; set; }
		public bool? IsStandard { get; set; }
		public bool? IsActive { get; set; }
		public string QueryTerm { get; set; }

	}
	public class SupplierArticleResponseModel: IPaginatedResponseModel<SupplierArticleItem>
	{

	}
	public class SupplierArticleItem
	{
		public bool? aktiv { get; set; }
		public int Address_Nr { get; set; }
		public int Artikel_Nr { get; set; }
		public string Artikelnummer { get; set; }
		public string Bestell_Nr { get; set; }
		public string Bezeichnung_1 { get; set; }
		public string Bezeichnung_2 { get; set; }
		public decimal? Einkaufspreis { get; set; }
		public int? Lieferantennummer { get; set; }
		public decimal? Mindestbestellmenge { get; set; }
		public string Name1 { get; set; }
		public string Name2 { get; set; }
		public bool? Standardlieferant { get; set; }
		public int? Wiederbeschaffungszeitraum { get; set; }
		public int? SupplierId { get; set; }
		public SupplierArticleItem(Infrastructure.Data.Entities.Tables.BSD.PrsSupplierArticleEntity entity)
		{
			if(entity is null)
			{
				return;
			}
			// -
			aktiv = entity.aktiv;
			Address_Nr = entity.Address_Nr ?? -1;
			Artikel_Nr = entity.Artikel_Nr;
			Artikelnummer = entity.Artikelnummer;
			Bestell_Nr = entity.Bestell_Nr;
			Bezeichnung_1 = entity.Bezeichnung_1;
			Bezeichnung_2 = entity.Bezeichnung_2;
			Einkaufspreis = entity.Einkaufspreis;
			Lieferantennummer = entity.Lieferantennummer;
			Mindestbestellmenge = entity.Mindestbestellmenge;
			Name1 = entity.Name1;
			Name2 = entity.Name2;
			Standardlieferant = entity.Standardlieferant;
			Wiederbeschaffungszeitraum = entity.Wiederbeschaffungszeitraum;
			SupplierId = entity.SupplierId;
		}
	}
}
