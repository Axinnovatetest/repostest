using System;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.Services.Reporting
{
	public class Helpers
	{
		public enum ReportType
		{
			INVOICE,
			ORDER_CONFIRMATION,
			ORDER_FORECAST,
			ORDER_CONTRACT,
			ORDER_KANBAN,
			ORDER_DELIVERY,
			ORDER_CREDIT,
			FNC_ORDER,
			FNC_ORDER_FINAL,
			FNC_INVOICE,
			CTS_DLF,
			BSD_ART_STATS_PSZPRIO1,
			BSD_ART_STATS_PSZPRIO2,
			BSD_ART_STATS_PROJECTMSG,
			BSD_ART_STATS_CARTONS,
			BSD_ART_STATS_CIRCULATIONS,
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
			MTM_Order_Report_1,
			MTM_BestellungohneFA,
			MTM_BestandProWerkohneBedarfcz,
			MTM_BestandProWerkohneBedarfde,
			MTM_BestandProWerkohneBedarfws,
			MTM_BestandProWerkohneBedarfal,
			MTM_BestandProWerkohneBedarftn,
			MTM_BestandProWerkohneBedarfbetn,
			MTM_BestandProWerkohneBedarfgztn,
			MTM_Order_Report_2,
			MTM_Order_Report_3,
			MTM_Wareneingang_ESD_SCHUTZ,
			MTM_Wareneingang_PSZ_MHD_Etikett,
			MTM_Wareneingang_PSZ_MHD_Etikett_WaterMark,
			MTM_Wareneingang_Etikett,
			MTM_Wareneingang_Report
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
					return "";
				case ReportType.ORDER_KANBAN:
					return "";
				case ReportType.ORDER_CONTRACT:
					return "Invoice.frx";
				case ReportType.ORDER_DELIVERY:
					return "DeliveryNote.frx";
				case ReportType.ORDER_CREDIT:
					return "";
				case ReportType.FNC_ORDER:
					return "FNC_Order.frx";
				case ReportType.FNC_ORDER_FINAL:
					return "FNC_OrderFinal.frx";
				case ReportType.FNC_INVOICE:
					return "FNC_Invoice.frx";
				case ReportType.CTS_DLF:
					return "CTS_deliveryPlan.frx";
				case ReportType.BSD_ART_STATS_PSZPRIO1:
					return "BSD_Articles_Statistics_Prio1.frx";
				case ReportType.BSD_ART_STATS_PSZPRIO2:
					return "BSD_Articles_Statistics_Prio2.frx";
				case ReportType.BSD_ART_STATS_PROJECTMSG:
					return "BSD_Articles_Statistics_ProjectMessage.frx";
				case ReportType.BSD_ART_STATS_CARTONS:
					return "BSD_Articles_Statistics_Cartons.frx";
				case ReportType.BSD_ART_STATS_CIRCULATIONS:
					return "BSD_Articles_Statistics_Circulations.frx";
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
				case ReportType.MTM_Order_Report_1:
					return "report.frx";
				case ReportType.MTM_Order_Report_2:
					return "report2.frx";
				case ReportType.MTM_Order_Report_3:
					return "PRS_Kanban.frx";
				case ReportType.MTM_BestellungohneFA:
					return "BestellungohneFA.frx";
				case ReportType.MTM_BestandProWerkohneBedarfgztn:
					return "BestandProWerkohneBedarfgztn.frx";
				case ReportType.MTM_BestandProWerkohneBedarfcz:
					return "BestandProWerkohneBedarfcz.frx";
				case ReportType.MTM_BestandProWerkohneBedarfal:
					return "BestandProWerkohneBedarfal.frx";
				case ReportType.MTM_BestandProWerkohneBedarfde:
					return "BestandProWerkohneBedarfde.frx";
				case ReportType.MTM_BestandProWerkohneBedarftn:
					return "BestandProWerkohneBedarftn.frx";
				case ReportType.MTM_BestandProWerkohneBedarfbetn:
					return "BestandProWerkohneBedarfbetn.frx";
				case ReportType.MTM_BestandProWerkohneBedarfws:
					return "BestandProWerkohneBedarfws.frx";
				case ReportType.MTM_Wareneingang_ESD_SCHUTZ:
					return "MTM_Wareneingang_ESD_SCHUTZ.frx";
				case ReportType.MTM_Wareneingang_Etikett:
					return "MTM_Wareneingang_Etikett.frx";
				case ReportType.MTM_Wareneingang_PSZ_MHD_Etikett:
					return "MTM_Wareneingang_PSZ_MHD_Etikett.frx";
				case ReportType.MTM_Wareneingang_PSZ_MHD_Etikett_WaterMark:
					return "MTM_Wareneingang_PSZ_MHD_Etikett_WaterMark.frx";
				case ReportType.MTM_Wareneingang_Report:
					return "MTM_Wareneingang_Report.frx";
				default:
					return "";
			}
		}

		public static string FormatNumber(decimal number, string groupSeparator = " ")
		{
			// Create a custom NumberFormatInfo
			NumberFormatInfo formatInfo = new NumberFormatInfo
			{
				NumberGroupSeparator = groupSeparator,
				NumberDecimalSeparator = ",",
				NumberGroupSizes = new[] { 3 }
			};

			string formattedNumber = number.ToString("#,0.00", formatInfo);

			return formattedNumber;
		}

	}
	public class RahmenTextItem
	{
		public string Title { get; set; }
		public string Paragraph { get; set; }
		public RahmenTextItem(string title, string paragraph)
		{
			this.Title = title;
			this.Paragraph = paragraph;
		}
	}
}