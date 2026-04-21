namespace Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities
{
	public class WipQueryReturnEntity
	{
		public string Fertigungsnummer { get; set; }
		public string Article { get; set; }
		public int WipStepValue { get; set; }
		public int OpenQty { get; set; }
		public int WipTotalValue { get; set; }
		public string Step { get; set; }
		public int CompletedPercent { get; set; }
		public int ArtikelNr { get; set; }
		public int IdFa { get; set; }
		public WipQueryReturnEntity() { }

		public WipQueryReturnEntity(DataRow dataRow)
		{
			Fertigungsnummer = Convert.ToString(dataRow["Fertigungsnummer"]);
			Article = Convert.ToString(dataRow["Article"]);
			WipStepValue = Convert.ToInt32(dataRow["WipStepValue"]);
			OpenQty = Convert.ToInt32(dataRow["OpenQty"]);
			WipTotalValue = Convert.ToInt32(dataRow["WipTotalValue"]);
			Step = Convert.ToString(dataRow["Step"]);
			CompletedPercent = Convert.ToInt32(dataRow["CompletedPercent"]);
			IdFa = Convert.ToInt32(dataRow["IdFa"]);
			ArtikelNr = Convert.ToInt32(dataRow["ArtikelNr"]);

		}

		public WipQueryReturnEntity ShallowClone()
		{
			return new WipQueryReturnEntity
			{
				Fertigungsnummer = Fertigungsnummer,
				Article = Article,
				WipTotalValue = WipTotalValue,
				OpenQty = OpenQty,
				WipStepValue = WipStepValue,
				Step= Step,
				CompletedPercent = CompletedPercent,
				ArtikelNr= ArtikelNr,
				IdFa= IdFa,
			};
		}
	}
}
