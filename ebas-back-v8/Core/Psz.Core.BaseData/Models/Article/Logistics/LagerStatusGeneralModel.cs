using System.Collections.Generic;

namespace Psz.Core.BaseData.Models.Article.Logistics
{
	public class LagerRequestModel
	{
		public int ArticleId { get; set; }
		public bool? IncludeMinStock { get; set; } = false;
	}
	public class LagerStatusGeneralModel
	{
		public int? Warentyp { get; set; }
		public int ArticleID { get; set; }
		public List<LagerStatusModel_2> LagerStatus { get; set; }

		public LagerStatusGeneralModel()
		{

		}
		public LagerStatusGeneralModel(Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity artikelEntity, List<Models.Article.Logistics.LagerExtensionModel> lagerEntity)
		{
			Warentyp = artikelEntity.Warentyp;
			ArticleID = artikelEntity.ArtikelNr;
			if(lagerEntity != null && lagerEntity.Count > 0)
			{
				this.LagerStatus = new List<LagerStatusModel_2>();
				for(var i = 0; i < lagerEntity.Count; i++)
				{
					var item = lagerEntity[i];
					this.LagerStatus.Add(new LagerStatusModel_2(item.LagerEntity, item.KundenIndex));
				}
			}
		}
	}
}
