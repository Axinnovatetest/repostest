using System.ComponentModel;

namespace Infrastructure.Services.Email
{
	public class Enums
	{
		public enum Types: int
		{
			General = 0,

			PosNewRecipeReceived = 100,
			PosNewSupplyRequestReceived = 101,

			MobileAppNewOrder = 200,
			MobileAppOrderProcessed = 201,
		}

		public enum MailPriority: int
		{
			NonUrgent = 0,
			Normal = 1,
			Urgent = 2
		}

		public enum RahmenAction
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

		public enum RahmenRedirect
		{
			INS = 0,
			MTM = 1,
		}
	}
}
