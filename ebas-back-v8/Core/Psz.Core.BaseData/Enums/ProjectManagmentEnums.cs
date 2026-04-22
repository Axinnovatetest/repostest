using System.ComponentModel;

namespace Psz.Core.BaseData.Enums
{
	public class ProjectManagmentEnums
	{
		public enum TaskStatus
		{
			[Description("Started")]
			Started = 0,
			[Description("Not Started")]
			NotStarted = 1,
			[Description("Finished")]
			Finiched = 2,
		}
		public enum ProjectTypes
		{
			[Description("A")]
			A = 1,
			[Description("B")]
			B = 2,
			[Description("C")]
			C = 3,
			[Description("Hybrid")]
			Hybrid = 4,
		}
		public enum Factories
		{
			[Description("PSZ Tunisia")]
			PszTunisia = 7,
			[Description("WS Tunisie")]
			WsTunisie = 42,
			[Description("PSZ Tunisia Ghazela")]
			PszTunisiaGhzela = 102,
			[Description("Albania")]
			Albania = 26,
			[Description("Czech")]
			Czech = 6,
			[Description("Germany")]
			Germany = 15
		}

		public enum ProductionStatuses
		{
			[Description("Green")]
			Green = 1,
			[Description("Red")]
			Red = 2,
			[Description("Orange")]
			Orange = 3,
		}
		public enum ProjectStatuses
		{
			[Description("Offer")]
			Offer = 1,
			[Description("Sampling")]
			Sampling = 2,
			[Description("Serie")]
			Serie = 3,
		}
	}
}
