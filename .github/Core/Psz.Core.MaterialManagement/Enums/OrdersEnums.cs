using System.ComponentModel;

namespace Psz.Core.MaterialManagement.Enums
{
	public class OrderEnums
	{
		public enum OrderTypes
		{
			[Description("Bestellung")]
			Order = 1,
			[Description("Kanbanabruf")]
			Kanaban = 2,
			[Description("Wareneingang")]
			Wareneingang = 3
		}

		public enum WareneingangReportTypes
		{
			[Description("ESD_SCHUTZ")]
			ESD_SCHUTZ = 1,
			[Description("PSZ_MHD_Etikett")]
			PSZ_MHD_Etikett = 2,
			[Description("Wareneingang_Etikett")]
			Wareneingang_Etikett = 3,
			[Description("Wareneingang_Report")]
			Wareneingang_Report = 4,
		}
		public enum RAStatus
		{
			[Description("Draft")]
			Draft = 0,
			[Description("InProgress")]
			InProgress = 1,
			[Description("Validated")]
			Validated = 2,
			[Description("Canceled")]
			Canceled = 3,
			[Description("Closed")]
			Closed = 4
		}
	}
}
