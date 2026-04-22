using Newtonsoft.Json;
using System.Collections.Generic;

namespace Psz.Core.Identity.Models
{
	public class FinancialAccessModel
	{
		[JsonProperty("ModuleActivated")]
		public bool ModuleActivated;

		[JsonProperty("CreditManagement")]
		public CreditManagementAccessModel CreditManagement;

		[JsonProperty("CashLiquidityManagement")]
		public CashLiquidityManagementAccessModel CashLiquidityManagement;

		[JsonProperty("Budget")]
		public FNCBudgetAccessProfileModel Budget;

		[JsonProperty("Administration")]
		public Administration Administration;
		public FinancialAccessModel()
		{
			CreditManagement = new CreditManagementAccessModel();
			CashLiquidityManagement = new CashLiquidityManagementAccessModel();
			Budget = null;
			Administration = new Administration();
		}
		public FinancialAccessModel(List<Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity> accessProfileEntities)
		{
			if(accessProfileEntities == null || accessProfileEntities.Count <= 0)
				return;

			var credit = false;
			var cash = false;
			var admin = false;
			foreach(var accessItem in accessProfileEntities)
			{
				credit = credit || (accessItem?.CreditManagement ?? false);
				cash = cash || (accessItem?.CashLiquidity ?? false);
				admin = admin || (accessItem?.Administration ?? false);
				ModuleActivated = ModuleActivated || (accessItem?.ModuleActivated ?? false);
			}

			CreditManagement = new CreditManagementAccessModel { ModuleActivated = credit };
			CashLiquidityManagement = new CashLiquidityManagementAccessModel { ModuleActivated = cash };
			Administration = new Administration { ModuleActivated = admin };
			Budget = new FNCBudgetAccessProfileModel(accessProfileEntities);
		}
	}
	public class FinancialAccessMinimalModel
	{
		[JsonProperty("ModuleActivated")]
		public bool ModuleActivated = true;

		[JsonProperty("CreditManagement")]
		public bool CreditManagement;

		[JsonProperty("CashLiquidityManagement")]
		public bool CashLiquidityManagement;

		[JsonProperty("Budget")]
		public bool Budget;

		[JsonProperty("Administration")]
		public bool Administration;
		public FinancialAccessMinimalModel()
		{
			CreditManagement = false;
			CashLiquidityManagement = false;
			Budget = false;
			Administration = false;
		}
		public FinancialAccessMinimalModel(FinancialAccessModel entity)
		{
			CreditManagement = entity.CreditManagement?.ModuleActivated ?? false;
			CashLiquidityManagement = entity.CashLiquidityManagement?.ModuleActivated ?? false;
			Budget = entity.Budget?.ModuleActivated ?? false;
			Administration = entity.Administration?.ModuleActivated ?? false;
		}
		public FinancialAccessMinimalModel(Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity entity)
		{
			CreditManagement = entity.CreditManagement;
			CashLiquidityManagement = entity.CashLiquidity;
			Budget = entity.Budget;
			Administration = entity.Administration ?? false;
		}
		public Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity ToDbEntity(int id, int mainId)
		{
			return new Infrastructure.Data.Entities.Tables.FNC.AccessProfileEntity
			{
				Id = id,
				//MainAccessProfileId = mainId,
				CreditManagement = ModuleActivated && CreditManagement,
				CashLiquidity = ModuleActivated && CashLiquidityManagement,
				Budget = ModuleActivated && Budget,
				Administration = ModuleActivated && Administration,

				// -
				ModuleActivated = ModuleActivated,
			};
		}
	}
}
