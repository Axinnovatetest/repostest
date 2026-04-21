namespace Infrastructure.Data.Entities.Tables.Logistics.InventroyStock
{
	public class Wip2QueryReturnEntity
	{
		public string ArtikelNr { get; set; }
		public decimal AssembledPercent { get; set; }
		public decimal CrimpedPercent { get; set; }
		public decimal CutPercent { get; set; }
		public DateTime Due { get; set; }
		public decimal ElectricalInspectedPercent { get; set; }
		public string FA { get; set; }
		public decimal Gesamtkosten { get; set; }
		public int Id { get; set; }
		public int IdFa { get; set; }
		public DateTime? InventoryYear { get; set; }
		public string Item { get; set; }
		public int LagerId { get; set; }
		public decimal Materialkosten { get; set; }
		public int OpenQty { get; set; }
		public decimal OpticalInspectedPercent { get; set; }
		public decimal PickedPercent { get; set; }
		public decimal PrepedPercent { get; set; }

		public Wip2QueryReturnEntity() { }

		public Wip2QueryReturnEntity(DataRow dataRow)
		{
			ArtikelNr = Convert.ToString(dataRow["ArtikelNr"]);
			AssembledPercent = Convert.ToDecimal(dataRow["AssembledPercent"]);
			CrimpedPercent = Convert.ToDecimal(dataRow["CrimpedPercent"]);
			CutPercent = Convert.ToDecimal(dataRow["CutPercent"]);
			Due = Convert.ToDateTime(dataRow["Due"]);
			ElectricalInspectedPercent = Convert.ToDecimal(dataRow["ElectricalInspectedPercent"]);
			FA = Convert.ToString(dataRow["FA"]);
			Gesamtkosten = Convert.ToDecimal(dataRow["Gesamtkosten"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			IdFa = Convert.ToInt32(dataRow["IdFa"]);
			InventoryYear = (dataRow["InventoryYear"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["InventoryYear"]);
			Item = Convert.ToString(dataRow["Item"]);
			LagerId = Convert.ToInt32(dataRow["LagerId"]);
			Materialkosten = Convert.ToDecimal(dataRow["Materialkosten"]);
			OpenQty = Convert.ToInt32(dataRow["OpenQty"]);
			OpticalInspectedPercent = Convert.ToDecimal(dataRow["OpticalInspectedPercent"]);
			PickedPercent = Convert.ToDecimal(dataRow["PickedPercent"]);
			PrepedPercent = Convert.ToDecimal(dataRow["PrepedPercent"]);
		}


		public Wip2QueryReturnEntity ShallowClone()
		{
			return new Wip2QueryReturnEntity
			{
				ArtikelNr = ArtikelNr,
				AssembledPercent = AssembledPercent,
				CrimpedPercent = CrimpedPercent,
				CutPercent = CutPercent,
				Due = Due,
				ElectricalInspectedPercent = ElectricalInspectedPercent,
				FA = FA,
				Gesamtkosten = Gesamtkosten,
				Id = Id,
				IdFa = IdFa,
				InventoryYear = InventoryYear,
				Item = Item,
				LagerId = LagerId,
				Materialkosten = Materialkosten,
				OpenQty = OpenQty,
				OpticalInspectedPercent = OpticalInspectedPercent,
				PickedPercent = PickedPercent,
				PrepedPercent = PrepedPercent
			};
		}
	}
}
