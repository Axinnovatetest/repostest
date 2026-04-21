using System;

namespace Psz.Core.BaseData.Models.Article.Statistics.Logistics
{
	public class FaStatusResponseModel
	{
		public string ArticleNumber { get; set; }
		public int? ArtikelNr { get; set; }
		public string Designation { get; set; }
		public string FaNumber { get; set; }
		public string FaQuantity { get; set; }
		public string Completed { get; set; }
		public string Open { get; set; }
		public DateTime? DesiredDate { get; set; }
		public DateTime? ScheduledDate { get; set; }
		public string Label { get; set; }
		public string Comments { get; set; }
		public string CustomerIndex { get; set; }
		public DateTime? CustomerIndexDatum { get; set; }
		public string BomVersion { get; set; }
		public string CpVersion { get; set; }
		public string WorkDate { get; set; }
		public string Kommisioniert_komplett { get; set; }
		public string Kommisioniert_teilweise { get; set; }
		public string FA_Gestartet { get; set; }
		// - 2022-06-20
		public int FaId { get; set; }

		public FaStatusResponseModel(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.LogisticsFaStatus logisticsFaStatus)
		{
			if(logisticsFaStatus == null)
				return;
			ArtikelNr = logisticsFaStatus.ArtikelNr;
			Designation = logisticsFaStatus.Designation;
			FaNumber = logisticsFaStatus.FaNumber;
			ArticleNumber = logisticsFaStatus.ArticleNumber;
			FaQuantity = logisticsFaStatus.FaQuantity;
			Completed = logisticsFaStatus.Completed;
			Open = logisticsFaStatus.Open;
			DesiredDate = DateTime.TryParse(logisticsFaStatus.DesiredDate, out var dd) ? dd : (DateTime?)null;
			ScheduledDate = DateTime.TryParse(logisticsFaStatus.ScheduledDate, out var sd) ? sd : (DateTime?)null;
			Label = logisticsFaStatus.Label;
			Comments = logisticsFaStatus.Comments;
			CustomerIndex = logisticsFaStatus.CustomerIndex;
			CustomerIndexDatum = logisticsFaStatus.CustomerIndexDatum;
			WorkDate = logisticsFaStatus.WorkDate;
			BomVersion = logisticsFaStatus.BomVersion;
			CpVersion = logisticsFaStatus.CpVersion;
			Kommisioniert_komplett = logisticsFaStatus.Kommisioniert_komplett;
			Kommisioniert_teilweise = logisticsFaStatus.Kommisioniert_teilweise;
			FA_Gestartet = logisticsFaStatus.FA_Gestartet;
			FaId = logisticsFaStatus.FaID ?? -1;
		}
	}
}
