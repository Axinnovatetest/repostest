
using System.ComponentModel;


namespace Psz.Core.Logistics.Enums
{
	public enum LagerEnum
	{
		[Description("TN")]
		TN = 7,
		[Description("BE_TN")]
		BETN = 60,
		[Description("GZ-TN")]
		GZTN = 102,
		[Description("WS")]
		WS = 42,
		[Description("Albanien")]
		Albanien = 26,
		[Description("Eigenfertigung")]
		Eigenfertigung = 6,
		[Description("Fertigung D")]
		FertigungD = 15
	}
	public enum Lager
	{
		TN = 7,
		WSTN = 42,
		GZTN = 102,
		AL = 26,
		CZ = 6,
		DE = 15,
	}
	public enum WarehouseMouvementStatus
	{
		Todo = 0,
		Inprogress = 1,
		Complete = 2
	}
}
