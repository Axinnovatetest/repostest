namespace Psz.Core.MaterialManagement.Models
{
	public class InfoRahmennummerModel
	{
		public int Nr { get; set; }
		public string AngeboteNr { get; set; }
		public int PositionNr { get; set; }
		public int Position { get; set; }
		public decimal Anzahl { get; set; }
		public DateTime? ExtensionDate { get; set; }
		public DateTime? GultigAb { get; set; }
		public DateTime? GultigBis { get; set; }

		public InfoRahmennummerModel()
		{

		}

		public InfoRahmennummerModel(Infrastructure.Data.Entities.Joins.MTM.Order.InfoRahmennummerEntity entity)
		{
			Nr = entity.Nr;
			AngeboteNr = entity.Bezug;
			PositionNr = entity.PositionNr;
			Position = entity.Position;
			Anzahl = entity.Anzahl;
			ExtensionDate = entity.ExtensionDate;
			GultigAb = entity.GultigAb;
			GultigBis = entity.GultigBis;
		}
	}
}
