using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using static Psz.Core.Common.Enums.ArticleEnums;

namespace Psz.Core.CustomerService.Enums
{
	public class OrderEnums
	{
		public enum Types
		{
			[Description(Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_CONFIRMATION)]
			Confirmation = 0, // Auftragsbestätigung
			[Description(Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_FORECAST)]
			forecast = 1, // bedarfsvorschau
			[Description(Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_CONTRACT)]
			Contract = 2, // Rahmenauftrag
			[Description(Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_KANBAN)]
			Kanban = 3, // Kanban
			[Description(Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_DELIVERY)]
			Delivery = 4, // Lieferschein
			[Description(Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_INVOICE)]
			Invoice = 5, // Rechnung
			[Description(Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_CREDIT)]
			Credit = 6, // Gutschrift
			[Description("Forecast")]
			CRPForecast = 7
		}
		internal static string TypeToData(Types type)
		{
			switch(type)
			{
				default:
				case Types.Confirmation:
					return Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_CONFIRMATION;
				case Types.forecast:
					return Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_FORECAST;
				case Types.Contract:
					return Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_CONTRACT;
				case Types.Kanban:
					return Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_KANBAN;
				case Types.Delivery:
					return Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_DELIVERY;
				case Types.Invoice:
					return Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_INVOICE;
				case Types.Credit:
					return Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_CREDIT;
				case Types.CRPForecast:
					return Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_CRPFORECAST;
			}
		}
		internal enum OrderElementStatus: int
		{
			Original = 0,
			Changed = 1,
			Deleted = 2,
		}


		public enum KapazitatLager
		{
			[Description("CZ")]
			CZ = 1,
			[Description("TN")]
			TN = 2,
			[Description("AL")]
			AL = 3,
			[Description("BETN")]
			BETN = 4,
			[Description("WS")]
			WS = 5,
			[Description("GZTN")]
			GZTN = 6,
		}
		public static int GetLagerNumber(KapazitatLager lager)
		{
			switch(lager)
			{
				case KapazitatLager.CZ:
					return 6;
				case KapazitatLager.TN:
					return 7;
				case KapazitatLager.AL:
					return 26;
				case KapazitatLager.BETN:
					return 60;
				case KapazitatLager.WS:
					return 42;
				case KapazitatLager.GZTN:
					return 102;
				default:
					return 0;
			}
		}
		public enum ItemType
		{
			[Description("Prototyp")]
			Prototyp = 1,
			[Description("Erstmuster")]
			Erstmuster = 2,
			[Description("Nullserie")]
			Nullserie = 3,
			[Description("Serie")]
			Serie = 4,
		}

		// - 2023-10-23 - convert MTD values (english) to CTS values (german)
		public static ItemType GetItemType(string type)
		{
			type = (type ?? "").ToLower();
			return type switch
			{
				"prototype" or "prototyp" => ItemType.Prototyp,
				"first sample" or "erstmuster" => ItemType.Erstmuster,
				"null serie" or "nullserie" => ItemType.Nullserie,
				"serie" => ItemType.Serie,
				_ => ItemType.Serie
			};
		}
		public static Psz.Core.Common.Enums.ArticleEnums.SalesItemType ConvertToMTDSalesItemType(string type)
		{
			type = (type ?? "").ToLower();
			return type switch
			{
				"prototype" or "prototyp" => SalesItemType.Prototype,
				"first sample" or "erstmuster" => SalesItemType.FirstSample,
				"null serie" or "nullserie" => SalesItemType.NullSerie,
				"serie" => SalesItemType.Serie,
				_ => SalesItemType.Serie
			};
		}
		public static Psz.Core.Common.Enums.ArticleEnums.SalesItemType ConvertToMTDSalesItemType(int type)
		{
			return type switch
			{
				1 => SalesItemType.Prototype,
				2 => SalesItemType.FirstSample,
				3 => SalesItemType.NullSerie,
				4 => SalesItemType.Serie,
				_ => SalesItemType.Serie
			};
		}
		public static KeyValuePair<int, string> GetArticleHauplager(ArticleProductionPlace place)
		{
			var _find = new List<Infrastructure.Data.Entities.Tables.INV.LagerorteEntity>();
			var Lagerorts = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.GetForOrderReception(new List<int> { });
			switch(place)
			{
				case ArticleProductionPlace.AL:
					_find = Lagerorts.Where(x => x.Lagerort.Contains("/AL")).ToList();
					return (_find != null && _find.Count > 0) ? new KeyValuePair<int, string>(_find[0].LagerortId, _find[0].Lagerort) : new KeyValuePair<int, string>(-1, null);
				case ArticleProductionPlace.TN:
					_find = Lagerorts.Where(x => x.Lagerort.Contains("/TN")).ToList();
					return (_find != null && _find.Count > 0) ? new KeyValuePair<int, string>(_find[0].LagerortId, _find[0].Lagerort) : new KeyValuePair<int, string>(-1, null);
				//case ArticleProductionPlace.BETN:
				//	_find = Lagerorts.Where(x => x.Lagerort.Contains("/TN")).ToList();
				//	return (_find != null && _find.Count > 0) ? new KeyValuePair<int, string>(_find[0].LagerortId, _find[0].Lagerort) : new KeyValuePair<int, string>(-1, null);
				case ArticleProductionPlace.WS:
					_find = Lagerorts.Where(x => x.Lagerort.Contains("/WS")).ToList();
					return (_find != null && _find.Count > 0) ? new KeyValuePair<int, string>(_find[0].LagerortId, _find[0].Lagerort) : new KeyValuePair<int, string>(-1, null);
				case ArticleProductionPlace.DE:
					_find = Lagerorts.Where(x => x.Lagerort.Contains("/D")).ToList();
					return (_find != null && _find.Count > 0) ? new KeyValuePair<int, string>(_find[0].LagerortId, _find[0].Lagerort) : new KeyValuePair<int, string>(-1, null);
				case ArticleProductionPlace.CZ:
					_find = Lagerorts.Where(x => x.Lagerort.Contains("/CZ")).ToList();
					return (_find != null && _find.Count > 0) ? new KeyValuePair<int, string>(_find[0].LagerortId, _find[0].Lagerort) : new KeyValuePair<int, string>(-1, null);
				case ArticleProductionPlace.GZTN:
					_find = Lagerorts.Where(x => x.Lagerort.Contains("/GZ")).ToList();
					return (_find != null && _find.Count > 0) ? new KeyValuePair<int, string>(_find[0].LagerortId, _find[0].Lagerort) : new KeyValuePair<int, string>(-1, null);
				default:
					return new KeyValuePair<int, string>(-1, null);
			}
		}
	}
}
