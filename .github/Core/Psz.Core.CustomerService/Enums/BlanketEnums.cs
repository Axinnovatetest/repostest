using System.ComponentModel;

namespace Psz.Core.CustomerService.Enums
{
	public class BlanketEnums
	{

		public enum Types
		{
			[Description("Sale")]
			sale = 0,
			[Description("Purchase")]
			purchase = 1,

		}
		public enum ArticleFileType
		{
			[Description("OverviewImage")]
			OverviewImage = 0,
			[Description("QualityAttachement")]
			QualityAttachement = 1,
			[Description("BOMPosition")]
			BOMPosition = 2,
			[Description("BOMAltPosition")]
			BOMAltPosition = 3,
			[Description("RahmenAuftrag")]
			RahmenAuftrag = 4
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
		public enum ActionStatus
		{
			[Description("SubmitValidate")]
			SubmitValidate = 0,
			[Description("Valider")]
			Valider = 1,
			[Description("Rejeter")]
			Rejeter = 2,
			[Description("Fermer")]
			Fermer = 3,
			[Description("Annuler")]
			Annuler = 4,

		}
		public enum ColorStatus
		{
			Green = 0,
			Orange = 1,
			Yellow = 2,
			Red = 3,
		}
	}
}
