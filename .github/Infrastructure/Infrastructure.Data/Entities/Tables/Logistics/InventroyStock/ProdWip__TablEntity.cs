namespace Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities
{
	public class ProductionWipEntity
	{
		public decimal ArticleAssembled { get; set; }
		public decimal ArticleCrimped { get; set; }
		public decimal ArticleCut { get; set; }
		public decimal ArticleElectricalInspected { get; set; }
		public decimal ArticleGesamtkosten { get; set; }
		public decimal ArticleMaterialkosten { get; set; }
		public decimal ArticleOpticalInspected { get; set; }
		public decimal ArticlePicked { get; set; }
		public decimal ArticlePreped { get; set; }
		public string ArtikelNr { get; set; }
		public DateTime Due { get; set; }
		public int FA { get; set; }
		public decimal FaAssembled { get; set; }
		public decimal FaCrimped { get; set; }
		public decimal FaCut { get; set; }
		public decimal FaElectricalInspected { get; set; }
		public decimal FaOpticalInspected { get; set; }
		public decimal FaPicked { get; set; }
		public decimal FaPreped { get; set; }
		public int Id { get; set; }
		public int IdFa { get; set; }
		public DateTime? InventoryYear { get; set; }
		public string Item { get; set; }
		public int LagerId { get; set; }
		public int OpenQty { get; set; }
		public decimal UserAssembled { get; set; }
		public decimal UserAssembledPercent { get; set; }
		public decimal UserCrimped { get; set; }
		public decimal UserCrimpedPercent { get; set; }
		public decimal UserCut { get; set; }
		public decimal UserCutPercent { get; set; }
		public decimal UserElectricalInspected { get; set; }
		public decimal UserElectricalInspectedPercent { get; set; }
		public decimal UserOpticalInspected { get; set; }
		public decimal UserOpticalInspectedPercent { get; set; }
		public decimal UserPicked { get; set; }
		public decimal UserPickedPercent { get; set; }
		public decimal UserPreped { get; set; }
		public decimal UserPrepedPercent { get; set; }

		public ProductionWipEntity() { }

		public ProductionWipEntity(DataRow dataRow)
		{
			ArticleAssembled = Convert.ToDecimal(dataRow["ArticleAssembled"]);
			ArticleCrimped = Convert.ToDecimal(dataRow["ArticleCrimped"]);
			ArticleCut = Convert.ToDecimal(dataRow["ArticleCut"]);
			ArticleElectricalInspected = Convert.ToDecimal(dataRow["ArticleElectricalInspected"]);
			ArticleGesamtkosten = Convert.ToDecimal(dataRow["ArticleGesamtkosten"]);
			ArticleMaterialkosten = Convert.ToDecimal(dataRow["ArticleMaterialkosten"]);
			ArticleOpticalInspected = Convert.ToDecimal(dataRow["ArticleOpticalInspected"]);
			ArticlePicked = Convert.ToDecimal(dataRow["ArticlePicked"]);
			ArticlePreped = Convert.ToDecimal(dataRow["ArticlePreped"]);
			ArtikelNr = Convert.ToString(dataRow["ArtikelNr"]);
			Due = Convert.ToDateTime(dataRow["Due"]);
			FA = Convert.ToInt32(dataRow["FA"]);
			FaAssembled = Convert.ToDecimal(dataRow["FaAssembled"]);
			FaCrimped = Convert.ToDecimal(dataRow["FaCrimped"]);
			FaCut = Convert.ToDecimal(dataRow["FaCut"]);
			FaElectricalInspected = Convert.ToDecimal(dataRow["FaElectricalInspected"]);
			FaOpticalInspected = Convert.ToDecimal(dataRow["FaOpticalInspected"]);
			FaPicked = Convert.ToDecimal(dataRow["FaPicked"]);
			FaPreped = Convert.ToDecimal(dataRow["FaPreped"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			IdFa = Convert.ToInt32(dataRow["IdFa"]);
			InventoryYear = (dataRow["InventoryYear"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["InventoryYear"]);
			Item = Convert.ToString(dataRow["Item"]);
			LagerId = Convert.ToInt32(dataRow["LagerId"]);
			OpenQty = Convert.ToInt32(dataRow["OpenQty"]);
			UserAssembled = Convert.ToDecimal(dataRow["UserAssembled"]);
			UserAssembledPercent = Convert.ToDecimal(dataRow["UserAssembledPercent"]);
			UserCrimped = Convert.ToDecimal(dataRow["UserCrimped"]);
			UserCrimpedPercent = Convert.ToDecimal(dataRow["UserCrimpedPercent"]);
			UserCut = Convert.ToDecimal(dataRow["UserCut"]);
			UserCutPercent = Convert.ToDecimal(dataRow["UserCutPercent"]);
			UserElectricalInspected = Convert.ToDecimal(dataRow["UserElectricalInspected"]);
			UserElectricalInspectedPercent = Convert.ToDecimal(dataRow["UserElectricalInspectedPercent"]);
			UserOpticalInspected = Convert.ToDecimal(dataRow["UserOpticalInspected"]);
			UserOpticalInspectedPercent = Convert.ToDecimal(dataRow["UserOpticalInspectedPercent"]);
			UserPicked = Convert.ToDecimal(dataRow["UserPicked"]);
			UserPickedPercent = Convert.ToDecimal(dataRow["UserPickedPercent"]);
			UserPreped = Convert.ToDecimal(dataRow["UserPreped"]);
			UserPrepedPercent = Convert.ToDecimal(dataRow["UserPrepedPercent"]);
		}

		public ProductionWipEntity ShallowClone()
		{
			return new ProductionWipEntity
			{
				ArticleAssembled = ArticleAssembled,
				ArticleCrimped = ArticleCrimped,
				ArticleCut = ArticleCut,
				ArticleElectricalInspected = ArticleElectricalInspected,
				ArticleGesamtkosten = ArticleGesamtkosten,
				ArticleMaterialkosten = ArticleMaterialkosten,
				ArticleOpticalInspected = ArticleOpticalInspected,
				ArticlePicked = ArticlePicked,
				ArticlePreped = ArticlePreped,
				ArtikelNr = ArtikelNr,
				Due = Due,
				FA = FA,
				FaAssembled = FaAssembled,
				FaCrimped = FaCrimped,
				FaCut = FaCut,
				FaElectricalInspected = FaElectricalInspected,
				FaOpticalInspected = FaOpticalInspected,
				FaPicked = FaPicked,
				FaPreped = FaPreped,
				Id = Id,
				IdFa = IdFa,
				InventoryYear = InventoryYear,
				Item = Item,
				LagerId = LagerId,
				OpenQty = OpenQty,
				UserAssembled = UserAssembled,
				UserAssembledPercent = UserAssembledPercent,
				UserCrimped = UserCrimped,
				UserCrimpedPercent = UserCrimpedPercent,
				UserCut = UserCut,
				UserCutPercent = UserCutPercent,
				UserElectricalInspected = UserElectricalInspected,
				UserElectricalInspectedPercent = UserElectricalInspectedPercent,
				UserOpticalInspected = UserOpticalInspected,
				UserOpticalInspectedPercent = UserOpticalInspectedPercent,
				UserPicked = UserPicked,
				UserPickedPercent = UserPickedPercent,
				UserPreped = UserPreped,
				UserPrepedPercent = UserPrepedPercent
			};
		}
	}
	public class ProductionWipTotalEntity
	{
		public string ArtikelNr { get; set; }
		public DateTime Due { get; set; }
		public int FA { get; set; }
		public int Id { get; set; }
		public int IdFa { get; set; }
		public DateTime? InventoryYear { get; set; }
		public string Item { get; set; }
		public int LagerId { get; set; }
		public int OpenQty { get; set; }
		public decimal Total { get; set; }

		public ProductionWipTotalEntity() { }

		public ProductionWipTotalEntity(DataRow dataRow)
		{
			ArtikelNr = Convert.ToString(dataRow["ArtikelNr"]);
			Due = Convert.ToDateTime(dataRow["Due"]);
			FA = Convert.ToInt32(dataRow["FA"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			IdFa = Convert.ToInt32(dataRow["IdFa"]);
			InventoryYear = (dataRow["InventoryYear"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["InventoryYear"]);
			Item = Convert.ToString(dataRow["Item"]);
			LagerId = Convert.ToInt32(dataRow["LagerId"]);
			OpenQty = Convert.ToInt32(dataRow["OpenQty"]);
			Total = Convert.ToDecimal(dataRow["Total"]);
		}
	}
}
