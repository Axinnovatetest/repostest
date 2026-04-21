using System;
using System.Collections.Generic;

namespace Psz.Core.BaseData.Models.Article.Data
{
	public class UpgradableHUBGRequestModel
	{
		public int CurrentArticleId { get; set; }
		public int NewArticleId { get; set; }
	}
	public class UpgradableHUBGResponseModel
	{
		public int CurrentArticleId { get; set; }
		public int NewArticleId { get; set; }
		public List<UpgradableHUBGItem> HBGItems { get; set; }
		public List<UpgradableHUBGItem> UBGItems { get; set; }
	}
	public class UpgradableHUBGItem
	{
		public int CurrentArticleId { get; set; }
		public string CurrentArticleNumber { get; set; }
		public string CurrentArticleIndex { get; set; }
		public DateTime? CurrentArticleIndexDate { get; set; }
		public int NewArticleId { get; set; }
		public string NewArticleNumber { get; set; }
		public string NewArticleIndex { get; set; }
		public DateTime? NewArticleIndexDate { get; set; }

		// -
		public bool Upgrade { get; set; } = false;
		public UpgradableHUBGItem(
			Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity currentA,
			Infrastructure.Data.Entities.Tables.PRS.ArtikelEntity newA)
		{
			// -
			CurrentArticleId = currentA?.ArtikelNr ?? -1;
			CurrentArticleNumber = currentA?.ArtikelNummer;
			CurrentArticleIndex = currentA?.Index_Kunde;
			CurrentArticleIndexDate = currentA?.Index_Kunde_Datum;

			// - 
			NewArticleId = newA?.ArtikelNr ?? -1;
			NewArticleNumber = newA?.ArtikelNummer;
			NewArticleIndex = newA?.Index_Kunde;
			NewArticleIndexDate = newA?.Index_Kunde_Datum;
		}
	}
}
