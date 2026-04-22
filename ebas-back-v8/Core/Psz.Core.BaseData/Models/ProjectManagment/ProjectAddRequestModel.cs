using System;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Psz.Core.BaseData.Models.ProjectManagment
{
	public class ProjectAddRequestModel
	{
		public ProjectHeader Header { get; set; }
		public List<ProjectCable> Cables { get; set; }
	}
	public class ProjectHeader
	{
		public int Id { get; set; }
		public int? CustomerNumber { get; set; }
		public int? PMManagerId { get; set; }
		public string PmManagerName { get; set; }
		public int? PMManagerFactoryId { get; set; }
		public string PMManagerFactoryName { get; set; }
		public int? Factory { get; set; }
		public int? Type { get; set; }
		public int? CSManagerId { get; set; }
		public string CSManagerName { get; set; }
		public string Name { get; set; }
		public string OfferNumber { get; set; }
		public decimal? QuantityKS { get; set; }
		public DateTime? DeliveryDate { get; set; }
		public string CustomerRefrence { get; set; }
		public int? StatusId { get; set; }
		public string Status { get; set; }
		public ProjectHeader()
		{

		}
		public ProjectHeader(Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_ProjectsEntity entity)
		{
			Id = entity.Id;
			CustomerNumber = entity.CustomerNumber;
			PMManagerId = entity.PMManagerUserId;
			PmManagerName = entity.PMManagerUsername;
			PMManagerFactoryId = entity.PMManagerFactoryUserId;
			PMManagerFactoryName = entity.PMManagerFactoryUsername;
			Factory = entity.Factory;
			Type = entity.TypeId;
			CSManagerId = entity.CSManagerUserId;
			CSManagerName = entity.CSManagerUsername;
			Name = entity.ProjectName;
			OfferNumber = entity.OfferNumber;
			QuantityKS = entity.QuantityKS;
			DeliveryDate = entity.DeliveryDate;
			CustomerRefrence = entity.CustomerRefrence;
			StatusId = entity.StatusId;
			Status = entity.Status;
		}
	}
	public class ProjectCable
	{
		public int? ProjectId { get; set; }
		public int? ArticleId { get; set; }
		public int? ResponsibleId { get; set; }
	}
}
