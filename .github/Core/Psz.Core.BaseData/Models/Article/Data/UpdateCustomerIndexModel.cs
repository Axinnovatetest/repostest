namespace Psz.Core.BaseData.Models.Article.Data
{
	public class UpdateCustomerIndexResponseModel
	{

		public class StockModel
		{

		}
		public class FAModel
		{

		}
		public class ABModel
		{
			public int Nr { get; set; }
			public int AngebotNr { get; set; }
			public int ProjektNr { get; set; }
			public int PositionNr { get; set; }
			public int PositionNumber { get; set; }
			public int PositionArticleId { get; set; }
			public int PositionArticleNumber { get; set; }
			public decimal PositionOpenQuantity { get; set; }
			public string PositionArticleCustomerIndex { get; set; }
			public ABModel(
				Infrastructure.Data.Entities.Tables.PRS.AngeboteEntity ABEntity,
				Infrastructure.Data.Entities.Tables.PRS.AngeboteneArtikelEntity PositionEntity)
			{


			}
		}
	}
}
