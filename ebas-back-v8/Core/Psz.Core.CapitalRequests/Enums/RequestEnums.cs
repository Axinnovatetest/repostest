using System.ComponentModel;

namespace Psz.Core.CapitalRequests.Enums
{
	public class RequestEnums
	{
		public enum RequestCategories
		{
			[Description("01-Mesure Cable (G1/G2) Court")]
			MesureCourt = 1,
			[Description("02-Mesure Cable (G1/G2) Long")]
			MesureLong = 2,
			[Description("03-Mesure Tube (G3) Court")]
			TubeCourt = 3,
			[Description("04-Mesure Tube (G3) Long")]
			TubeLong = 4,
			[Description("05-Article Manque")]
			ArticleManque = 5,
			[Description("06-article Plus")]
			ArticlePlus = 6,
			[Description("07-Article Faux")]
			ArticleFaux = 7,
			[Description("08-Formboard Orientation Branche")]
			FormboardOrientationBranche = 8,
			[Description("09-Formboard Ordre des Branches")]
			FormboardOrdredesBranches = 9,
			[Description("10-Formboard Mesure Faux")]
			FormboardMesureFaux = 10,
			[Description("11-Formboard encombré")]
			Formboardencombré = 11,
			[Description("12-Documentation Manque Information")]
			DocumentationManqueInformation = 12,
			[Description("13-Specification Client non Respecté")]
			SpecificationClientnonRespecté = 13,
			[Description("14-Autre")]
			Autres = 14,
		}
		public enum RequestStatus
		{
			[Description("Open")]
			Open = 0,
			[Description("In Progress")]
			InProgress = 1,
			[Description("Closed")]
			Closed = 2
		}
		public enum Plants
		{
			TN = 7,
			BETN = 60,
			WSTN = 42,
			GZTN = 102,
			AL = 26,
			CZ = 6,
			DE = 15
		}
	}
}
