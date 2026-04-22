using Psz.Core.Logistics.Enums;
using Psz.Core.MaterialManagement.Settings;

namespace Psz.Core.Logistics.Helpers
{
	public class LagerHelperHandler
	{
		public static int GetProductionLager(Lager lager)
		{
			switch(lager)
			{
				case Lager.WSTN:
				case Lager.TN:
					return 420;
				case Lager.GZTN:
					return 103;
				case Lager.AL:
					return 260;
				case Lager.CZ:
					return 66;
				default:
					return 0;
			}
		}
		public static List<int> GetProductionWarehouseIds(int lagerId)
		{
			var prodWarehouseIds = new List<int>
				{
					(int)Psz.Core.Logistics.Helpers.LagerHelperHandler.GetProductionLager((Enums.Lager)(lagerId))
				};
			if(lagerId == (int)Enums.Lager.WSTN)
			{
				prodWarehouseIds.Add((int)Psz.Core.Logistics.Helpers.LagerHelperHandler.GetProductionLager(Enums.Lager.TN));
			}
			return prodWarehouseIds;
		}
		public static List<int> GetWarehouseIds(int lagerId)
		{
			// - 42 must be the FIRST item in the list
			return lagerId == 42 ? new List<int> { 42, 7 } : new List<int> { lagerId };
		}

	}
}
