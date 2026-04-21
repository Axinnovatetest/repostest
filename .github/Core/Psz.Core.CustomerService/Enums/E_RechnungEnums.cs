using System.ComponentModel;

namespace Psz.Core.CustomerService.Enums
{
	public class E_RechnungEnums
	{
		public enum RechnungTyp
		{
			[Description("Einzelrechnung")]
			Einzelrechnung = 0,
			[Description("Sammelrechnung")]
			Sammelrechnung = 1,
			[Description("Sonderregelung")]
			Sonderregelung = 2,
		}
	}
}
