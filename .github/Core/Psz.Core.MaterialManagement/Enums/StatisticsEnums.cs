namespace Psz.Core.MaterialManagement.Enums
{
	public class StatisticsEnums
	{
		public enum Lager
		{
			TN = 7,
			BETN = 60,
			WSTN = 42,
			GZTN = 102,
			AL = 26,
			TEMPAL = 156,
			CZ = 6,
			DE = 15,
		}

		public enum ReportType
		{
			BRUTTO_BEDARF
		}

		public static string GetReportTemplateFileName(ReportType reportType)
		{
			switch(reportType)
			{
				case ReportType.BRUTTO_BEDARF:
					return "BruttoBedarf.frx";
				default:
					return "";
			}
		}
	}
}
