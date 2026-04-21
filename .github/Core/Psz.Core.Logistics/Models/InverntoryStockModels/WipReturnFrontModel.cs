namespace Psz.Core.Logistics.Models.InverntoryStockModels
{
	public class WipQueryReturnModel
	{
		public string Fertigungsnummer { get; set; }
		public string Article { get; set; }
		public int WipStepValue { get; set; }
		public int OpenQty { get; set; }
		public int WipTotalValue { get; set; }
		public string Step { get; set; }
		public int CompletedPercent { get; set; }

		public WipQueryReturnModel() { }
		public WipQueryReturnModel(Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities.WipQueryReturnEntity entity)
		{
			if(entity == null)
				return;

			Fertigungsnummer = entity.Fertigungsnummer;
			Article = entity.Article;
			OpenQty = entity.OpenQty;
			WipStepValue = entity.WipStepValue;
			WipTotalValue = entity.WipTotalValue;
			Step = entity.Step;
			CompletedPercent = entity.CompletedPercent;
		}
	}
}
