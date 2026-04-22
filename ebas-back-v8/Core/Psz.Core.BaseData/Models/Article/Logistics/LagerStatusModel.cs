using System;

namespace Psz.Core.BaseData.Models.Article.Logistics
{
	public class LagerStatusModel
	{
		public int LagerId { get; set; }
		public int LagerOrtNumber { get; set; }
		public decimal ArticleQuantity { get; set; }
		public DateTime? LastEdit { get; set; }
		public LagerStatusModel(Infrastructure.Data.Entities.Tables.PRS.LagerEntity entity)
		{
			LagerId = entity.ID;
			LagerOrtNumber = entity.Lagerort_id ?? -1;
			ArticleQuantity = (decimal)(entity.Bestand ?? 0);
			LastEdit = entity.letzte_Bewegung;
		}
	}
}
