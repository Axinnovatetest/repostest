using System.ComponentModel;


namespace Psz.Core.CRP.Enums
{
	public class CRPEnums
	{
		public enum ForcastDraftType
		{
			Hoch = 0,
			Quer = 1
		}
		public enum ForcastType
		{
			[Description("Forecast")]
			Forecast = 0,
			[Description("Kanban/Rahmen")]
			Kanban_Rahmen = 1,
			[Description("Planbedarf")]
			Planbedarf = 2,
		}
		public enum FASystemPeriod
		{
			H1 = 0,
			FoorMonths = 1
		}
		public enum FaPlannugUnits
		{
			[Description("AL")]
			AL = 26,
			[Description("CZ")]
			CZ = 6,
			[Description("DE")]
			DE = 15,
			[Description("TN")]
			TN = 7,
			[Description("BETN")]
			BETN = 60,
			[Description("WS")]
			WS = 42,
			[Description("GZTN")]
			GZTN = 102,
		}

		public enum AgentAction
		{
			[Description("FA Plannung")]
			FAPlannung = 1,
			[Description("UBG Plannung")]
			UBGPlannung = 2,
		}

		public enum HistoryFGImportTypes
		{
			[Description("By Excel")]
			byExcel=0,
			[Description("By SQL Agent")]
			bySqlAgent=1,
			[Description("By User Forcing Agent")]
			ByUserForcingAgent=2
		}

		public enum FaPlannungHistorieImportType
		{
			[Description("By Excel")]
			ByExcel = 0,
			[Description("Automatic Agent")]
			AutomaticAgent = 1,
			[Description("Forced Agent")]
			ForcedAgent = 2
		}
	}
}