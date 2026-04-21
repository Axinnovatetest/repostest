using System.ComponentModel;

namespace Psz.Core.Apps.Purchase.Enums
{
	public class OrderEnums
	{
		internal enum EdiDocumentTypes: int
		{
			Order = 0,
			OrderChange = 1,
			OrderResponse = 2,
			Undefined = -1
		}

		public enum Types: int
		{
			[Description("Auftragsbestätigung")]
			Confirmation = 0, // Auftragsbestätigung
			[Description("Bedarfsvorschau")]
			Forecast = 1, // Bedarfsvorschau
			[Description("Rahmenauftrag")]
			Contract = 2, // Rahmenauftrag
			[Description("Kanban")]
			Kanban = 3, // Kanban
			[Description("Lieferschein")]
			Delivery = 4, // Lieferschein
			[Description("Rechnung")]
			Invoice = 5, // Rechnung
			[Description("Gutschrift")]
			Credit = 6, // Gutschrift
		}
		internal static string TypeToData(Types type)
		{
			switch(type)
			{
				default:
				case Types.Confirmation:
					return Infrastructure.Data.Access.Tables.PRS.AngeboteAccess.TYP_CONFIRMATION;
				case Types.Forecast:
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
			}
		}
	}
}
