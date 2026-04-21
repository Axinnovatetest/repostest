namespace Psz.Core.CustomerService.Enums
{
	public class ReportingEnums
	{
		public enum ReportType
		{
			INVOICE,
			ORDER_CONFIRMATION,
			ORDER_FORECAST,
			ORDER_KANBAN,
			ORDER_CONTRACT,
			ORDER_DELIVERY,
			ORDER_CREDIT,

			CTS_DLF,
			CTS_WERK_TERMIN,
			CTS_WUNSH_TERMIN_ADMIN,
			CTS_WUNSH_TERMIN_USER,
			CTS_FA_DRUCK,
			CTS_FA_TECHNIC,
			CTS_AUSWERTUNG_ENDKONTROLLE,
			CTS_LAUFKARTE_SCHNEIDEREI,
			CTS_FA_UPDATE,
			CTS_FEHLEMATERIAL,
			CTS_FASTAPLEREPORT,
			CTS_KAPAZITATLANG,
			CTS_EXPORT,
			CTS_BACKLOGFG1,
			CTS_BACKLOGFG2,
			CTS_BACKLOGHW1,
			CTS_BACKLOGHW2,
			CTS_BESTAND,
			CTS_FANPEX,
			CTS_LIEFERPLANNUNG,
			CTS_CONTACTSFA,
			CTS_AUSWERTUNGSCHNEIDEREI,
			CTS_RECHNUNGCZ,
			CTS_RECHNUNGTN,
			CTS_RECHNUNGAL,
			CTS_RECHNUNGROH,
			CTS_RECHNUNGENDKONTROLLE,
			CTS_NACHBERECHNUNGTN,
			CTS_RG,
			CTS_RECHNUNG,
			CTS_RA,
		}

		public static string GetReportTemplateFileName(ReportType reportType)
		{
			switch(reportType)
			{
				case ReportType.INVOICE:
					return "Invoice.frx";
				case ReportType.ORDER_CONFIRMATION:
					return "Invoice.frx";
				case ReportType.ORDER_FORECAST:
					return "Forcast.frx";
				case ReportType.ORDER_KANBAN:
					return "";
				case ReportType.ORDER_CONTRACT:
					return "";
				case ReportType.ORDER_DELIVERY:
					return "DeliveryNote.frx";
				case ReportType.ORDER_CREDIT:
					return "Gutshcrift.frx";
				case ReportType.CTS_DLF:
					return "CTS_deliveryPlan.frx";
				case ReportType.CTS_WERK_TERMIN:
					return "WerkReport.frx";
				case ReportType.CTS_WUNSH_TERMIN_ADMIN:
					return "WunshReportAdmin.frx";
				case ReportType.CTS_WUNSH_TERMIN_USER:
					return "WunshReportUser.frx";
				case ReportType.CTS_FA_DRUCK:
					return "FADruck.frx";
				case ReportType.CTS_FA_TECHNIC:
					return "FATechnic.frx";
				case ReportType.CTS_AUSWERTUNG_ENDKONTROLLE:
					return "AuswertungEndkontrolle.frx";
				case ReportType.CTS_LAUFKARTE_SCHNEIDEREI:
					return "LaufkarteSchneiderei.frx";
				case ReportType.CTS_FA_UPDATE:
					return "FAUpdate.frx";
				case ReportType.CTS_FEHLEMATERIAL:
					return "Fehlematerial.frx";
				case ReportType.CTS_FASTAPLEREPORT:
					return "StappleReport.frx";
				case ReportType.CTS_KAPAZITATLANG:
					return "KapazitatLang.frx";
				case ReportType.CTS_EXPORT:
					return "Export.frx";
				case ReportType.CTS_BACKLOGFG1:
					return "BackLogFG1.frx";
				case ReportType.CTS_BACKLOGFG2:
					return "BackLogFG2.frx";
				case ReportType.CTS_BACKLOGHW1:
					return "BackLogHW1.frx";
				case ReportType.CTS_BACKLOGHW2:
					return "BackLogHW2.frx";
				case ReportType.CTS_BESTAND:
					return "Bestand.frx";
				case ReportType.CTS_FANPEX:
					return "FAnpex.frx";
				case ReportType.CTS_LIEFERPLANNUNG:
					return "LieferPlannung.frx";
				case ReportType.CTS_CONTACTSFA:
					return "ContactsFA.frx";
				case ReportType.CTS_AUSWERTUNGSCHNEIDEREI:
					return "AuswertungSchneiderei.frx";
				case ReportType.CTS_RECHNUNGCZ:
					return "RechnungCZ.frx";
				case ReportType.CTS_RECHNUNGTN:
					return "RechnungTN.frx";
				case ReportType.CTS_RECHNUNGAL:
					return "RechnungAL.frx";
				case ReportType.CTS_RECHNUNGROH:
					return "RechnungROH.frx";
				case ReportType.CTS_RECHNUNGENDKONTROLLE:
					return "RechnungEndkontrolle.frx";
				case ReportType.CTS_NACHBERECHNUNGTN:
					return "NachBerechnungTN.frx";
				case ReportType.CTS_RG:
					return "RG.frx";
				case ReportType.CTS_RECHNUNG:
					return "rechnung.frx";
				case ReportType.CTS_RA:
					return "rahmen.frx";
				default:
					return "";
			}
		}
		internal enum OrderTypes: int
		{
			Order = 0,
			OrderChange = 1,
			OrderResponse = 2,
			Undefined = -1
		}
	}
}
