namespace Psz.Core.Logistics.Enums
{
	public class ReportingEnums
	{

		public enum ReportType
		{
			VERPACKUNGCHOOSE1,
			AUSCHUSSKOSTEN_TECHNIK_INFO,
			VERPACKUNGCHOOSE2,
			LSDRUCKER,
			BESTANDSPERRLAGER,
			LSEtikettenDRUCKER,
			ScannerRohmaterial,
			Lagerbewegung,
			LagerbewegungPlantBooking,
			EntnahmeWert,
			EntnahmeWertMitBerechnung,
			CCMat_ReportPDFReport,
			CCFG_ReportPDFReport,
			RapportWareneingangLieferant,
			PlantBookingTicket
		}

		public static string GetReportTemplateFileName(ReportType reportType)
		{
			switch(reportType)
			{
				case ReportType.VERPACKUNGCHOOSE1:
					return "PSZ_Packliste.frx";
				case ReportType.VERPACKUNGCHOOSE2:
					return "PSZ_Packliste_SpeditionWarenwert.frx";
				case ReportType.LSDRUCKER:
					return "PSZ_Packliste_LS_von_Versand.frx";
				case ReportType.LSEtikettenDRUCKER:
					return "PSZ_Packliste_LS_Etiketten.frx";
				case ReportType.AUSCHUSSKOSTEN_TECHNIK_INFO:
					return "Auschusskosten_Technik_Info.frx";
				case ReportType.BESTANDSPERRLAGER:
					return "BestandSperrlager.frx";
				case ReportType.ScannerRohmaterial:
					return "ScannerRohmaterial.frx";
				case ReportType.Lagerbewegung:
					return "Lagerbewegung.frx";
				case ReportType.LagerbewegungPlantBooking:
					return "LagerbewegungPlantBooking.frx";
			    case ReportType.EntnahmeWert:
					return "EntnahmeWert.frx";
				case ReportType.EntnahmeWertMitBerechnung:
					return "EntnahmeWertMitBerechnung.frx";
				case ReportType.CCMat_ReportPDFReport:
					return "CCMat_ReportPDFReport.frx";
				case ReportType.CCFG_ReportPDFReport:
					return "CCFG_ReportPDFReport.frx";
				case ReportType.RapportWareneingangLieferant:
					return "RapportWareneingangLieferant.frx";
				case ReportType.PlantBookingTicket:
					return "TicketModelLGT.frx";
				default:
					return "";
			}
		}
	}
}
