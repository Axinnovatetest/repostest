using System;

namespace Psz.Core.Logistics.Interfaces
{
	public interface IInventoryStockService
	{
		ResponseModel<Psz.Core.Logistics.Models.InverntoryStockModels.InventoryFaStatsModel> CalculateStartedOpenFaHandler(Core.Identity.Models.UserModel user, int lagerId);
	}
}
