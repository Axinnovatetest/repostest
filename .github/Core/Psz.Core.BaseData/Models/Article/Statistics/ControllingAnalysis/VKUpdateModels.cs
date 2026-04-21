using Microsoft.AspNetCore.Http;
using Psz.Core.Common.Models;
using System;

namespace Psz.Core.BaseData.Models.Article.Statistics.ControllingAnalysis
{
	public class VKUpdateHistoryRequestModel: IPaginatedRequestModel
	{
		public string ArticleNumber { get; set; }
		public DateTime? From { get; set; }
		public DateTime? To { get; set; }
		public bool isXLS { get; set; } = false;

	}
	public class VKUpdateHistoryResponseModel: IPaginatedResponseModel<VKUpdateHistory>
	{
	}
	public class VKUpdateHistory
	{
		public decimal? Alte_Preis { get; set; }
		public string Artikelnummer { get; set; }
		public DateTime? Datum { get; set; }
		public int id { get; set; }
		public decimal? Neue_Preis { get; set; }
		public string User { get; set; }
		public VKUpdateHistory(Infrastructure.Data.Entities.Tables.BSD.Tbl_Historie_VK_UpdateEntity entity)
		{
			if(entity == null)
				return;

			Alte_Preis = entity.Alte_Preis;
			Artikelnummer = entity.Artikelnummer;
			Datum = entity.Datum;
			id = entity.id;
			Neue_Preis = entity.Neue_Preis;
			User = entity.User;
		}
	}
	public class VKUpdateRequestModel: IAttachmentRequestModel
	{

	}

	public class MinStockUpdateRequestModel
	{
		public IFormFile AttachmentFile { get; set; }
		public string AttachmentFileExtension { get; set; }

	}
	public class VKUpdateResponseModel
	{
		public int ArticleNr { get; set; }
		public string ArticleNumber { get; set; }
		public decimal OldVKPrice { get; set; }
		public decimal NewVKPrice { get; set; }
	}

	public class MinStockUpdateResponseModel
	{
		public string Articlenumber { get; set; }
		public decimal OldMinStock { get; set; }
		public decimal NewMinStock { get; set; }
		public int LagerId { get; set; }
		public DateTime Updatedate { get; set; }
		public int ArticleId { get; set; }

	}

	public class MinStockUpdateHistory
	{
		public DateTime UpdateDate { get; set; }

		public string ArticleNumber { get; set; }

		public int OldMinStock { get; set; }

		public int NewMinStock { get; set; }
		public string User { get; set; }

		public int Lagerort { get; set; }


		public MinStockUpdateHistory(Infrastructure.Data.Entities.Tables.BSD.ArticleMinStockUpdateHistoryEntity entity)
		{
			if(entity == null)
				return;

			UpdateDate = (DateTime)entity.UpdateDate;
			ArticleNumber = entity.ArticleNumber;
			OldMinStock = (int)entity.OldMinStock;
			NewMinStock = (int)entity.NewMinStock;
			User = entity.UpdateUserName;
			Lagerort = (int)entity.LagerId;
		}

	}


	public class MinStockUpdateHistoryRequestModel: IPaginatedRequestModel
	{
		public string ArticleNumber { get; set; }
		public DateTime? From { get; set; }
		public DateTime? To { get; set; }
		public bool isXLS { get; set; } = false;

	}

	public class MinStockUpdateHistoryResponseModel: IPaginatedResponseModel<MinStockUpdateHistory>
	{
	}
}
