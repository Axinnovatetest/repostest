using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Psz.Core.Apps.EDI.Enums
{
	public class OrderEnums
	{
		internal enum GlobalOrderChangeStatus: int
		{
			Pending = 0,
			Accepted = 1,
			Ignored = 2,
			Overwritten = 3,
		}

		internal enum OrderChangeItemTypes: int
		{
			New = 1,
			Canceled = 2,
			Changed = 3,
			NotChanged = 4,
		}

		internal enum OrderChangeItemStatus: int
		{
			Pending = 0,
			Accepted = 1,
			Ignored = 2,
			Overwritten = 3,
		}
		internal enum OrderTypes: int
		{
			Order = 0,
			OrderChange = 1,
			OrderResponse = 2,
			Undefined = -1
		}
		public enum ArticleProductionPlace
		{
			[Description("AL")]
			AL = 0,
			[Description("TN")]
			TN = 1,
			[Description("BETN")]
			BETN = 2,
			[Description("WS")]
			WS = 3,
			[Description("DE")]
			DE = 4,
			[Description("CZ")]
			CZ = 5,
			[Description("GZTN")]
			GZTN = 6
		}
		public enum ArticleProductionPlace__
		{
			[Description("AL")]
			AL = 26,
			//[Description("TN")]
			//TN = 1,
			//[Description("BETN")]
			//BETN = 2,
			[Description("WS")]
			WS = 42,
			[Description("DE")]
			DE = 15,
			[Description("CZ")]
			CZ = 6,
			[Description("GZTN")]
			GZTN = 102
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
				case ArticleProductionPlace.BETN:
					_find = Lagerorts.Where(x => x.Lagerort.Contains("/BE_TN")).ToList();
					return (_find != null && _find.Count > 0) ? new KeyValuePair<int, string>(_find[0].LagerortId, _find[0].Lagerort) : new KeyValuePair<int, string>(-1, null);
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
		public static KeyValuePair<int, string> GetArticleHauplager(ArticleProductionPlace__ place)
		{
			var _find = new List<Infrastructure.Data.Entities.Tables.INV.LagerorteEntity>();
			var Lagerorts = Infrastructure.Data.Access.Tables.INV.LagerorteAccess.GetForOrderReception(new List<int> { });
			switch(place)
			{
				case ArticleProductionPlace__.AL:
					_find = Lagerorts.Where(x => x.Lagerort.Contains("/AL")).ToList();
					return (_find != null && _find.Count > 0) ? new KeyValuePair<int, string>(_find[0].LagerortId, _find[0].Lagerort) : new KeyValuePair<int, string>(-1, null);
				//case ArticleProductionPlace__.TN:
				//	_find = Lagerorts.Where(x => x.Lagerort.Contains("/TN")).ToList();
				//	return (_find != null && _find.Count > 0) ? new KeyValuePair<int, string>(_find[0].LagerortId, _find[0].Lagerort) : new KeyValuePair<int, string>(-1, null);
				//case ArticleProductionPlace__.BETN:
				//	_find = Lagerorts.Where(x => x.Lagerort.Contains("/BE_TN")).ToList();
					//return (_find != null && _find.Count > 0) ? new KeyValuePair<int, string>(_find[0].LagerortId, _find[0].Lagerort) : new KeyValuePair<int, string>(-1, null);
				case ArticleProductionPlace__.WS:
					_find = Lagerorts.Where(x => x.Lagerort.Contains("/WS")).ToList();
					return (_find != null && _find.Count > 0) ? new KeyValuePair<int, string>(_find[0].LagerortId, _find[0].Lagerort) : new KeyValuePair<int, string>(-1, null);
				case ArticleProductionPlace__.DE:
					_find = Lagerorts.Where(x => x.Lagerort.Contains("/D")).ToList();
					return (_find != null && _find.Count > 0) ? new KeyValuePair<int, string>(_find[0].LagerortId, _find[0].Lagerort) : new KeyValuePair<int, string>(-1, null);
				case ArticleProductionPlace__.CZ:
					_find = Lagerorts.Where(x => x.Lagerort.Contains("/CZ")).ToList();
					return (_find != null && _find.Count > 0) ? new KeyValuePair<int, string>(_find[0].LagerortId, _find[0].Lagerort) : new KeyValuePair<int, string>(-1, null);
				case ArticleProductionPlace__.GZTN:
					_find = Lagerorts.Where(x => x.Lagerort.Contains("/GZ")).ToList();
					return (_find != null && _find.Count > 0) ? new KeyValuePair<int, string>(_find[0].LagerortId, _find[0].Lagerort) : new KeyValuePair<int, string>(-1, null);
				default:
					return new KeyValuePair<int, string>(-1, null);
			}
		}
	}
}
