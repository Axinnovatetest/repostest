using System;

namespace Psz.Core.BaseData.Models.Article.Statistics.Logistics
{
	public class InResponseModel
	{
		public string ProjectNr { get; set; }
		public string Type { get; set; }
		public string ArticleNumber { get; set; }
		public int? ArtikleNr { get; set; }
		public string OpenCurrent { get; set; }
		public string Unit { get; set; }
		public DateTime? Date { get; set; }
		public string Name { get; set; }
		public DateTime? DeliveryDate { get; set; }
		public string SupplierABNr { get; set; }
		public DateTime? ConfirmDate { get; set; }
		public string Comments { get; set; }
		public string OrderNr { get; set; }
		public string CompletePos { get; set; }
		public string Booked { get; set; }
		public string Production { get; set; }
		public string FrameworkOrder { get; set; }
		public InResponseModel(Infrastructure.Data.Entities.Joins.ArticleStatisticsEntities.LogisticsIn logisticsIn)
		{
			if(logisticsIn == null)
				return;

			try
			{
				ArtikleNr = logisticsIn.ArtikleNr;
				ProjectNr = logisticsIn.ProjectNr;
				Type = logisticsIn.Type;
				ArticleNumber = logisticsIn.ArticleNumber;
				OpenCurrent = logisticsIn.OpenCurrent;
				Unit = logisticsIn.Unit;
				Date = logisticsIn.Date;
				Name = logisticsIn.Name;
				DeliveryDate = logisticsIn.DeliveryDate;
				SupplierABNr = logisticsIn.SupplierABNr;
				ConfirmDate = logisticsIn.ConfirmDate;
				Comments = logisticsIn.Comments;
				OrderNr = logisticsIn.OrderNr;
				CompletePos = logisticsIn.CompletePos;
				Booked = logisticsIn.Booked;
				Production = logisticsIn.Production;
				FrameworkOrder = logisticsIn.FrameworkOrder;
			} catch(Exception c)
			{
				throw;
			}
		}
	}
}
