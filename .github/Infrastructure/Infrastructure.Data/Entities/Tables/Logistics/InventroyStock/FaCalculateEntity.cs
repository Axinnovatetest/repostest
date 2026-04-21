namespace Infrastructure.Data.Entities.Tables.Logistics.InventroyStockEntities
{
	public class FaCalculateEntity
	{
		public string? Site { get; set; }
		public int? CountFa { get; set; }
		public FaCalculateEntity(string site, int countFa)
		{
			Site = site;
			CountFa = countFa;
		}
		public FaCalculateEntity()
		{
		}
		public FaCalculateEntity(DataRow dataRow)
		{
			Site = (dataRow["Site"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Site"]);
			CountFa = Convert.ToInt32(dataRow["CountFa"]);
		}
	}
}
