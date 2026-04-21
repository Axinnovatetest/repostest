using System;

namespace Psz.Core.BaseData.Models.ProjectManagment
{
	public class ProjectsMinimalModel
	{
		public int Id { get; set; }
		public int? CustomerNumber { get; set; }
		public string Customer { get; set; }
		public int Open { get; set; }
		public int Completed { get; set; }
		public string Status { get; set; }
		public string Manager { get; set; }
		public DateTime? DateCreation { get; set; }
		public ProjectsMinimalModel()
		{

		}
		public ProjectsMinimalModel(Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_ProjectsEntity entity)
		{
			Id = entity.Id;
			CustomerNumber = entity.CustomerNumber;
			Customer = entity.CustomerName;
			Status = entity.Status;
			Manager = entity.PMManagerUsername;
			DateCreation = entity.CreationTime;
		}
	}

	public class ProjectModel
	{
		public string Id { get; set; }
		public string OfferNumber { get; set; }
		public string Customer { get; set; }
		public string ProjectName { get; set; }
		public decimal? QuantityKS { get; set; }
		public string Status { get; set; }
		public string Manager { get; set; }
		public DateTime? DateCreation { get; set; }

		public ProjectModel()
		{

		}
		public ProjectModel(Infrastructure.Data.Entities.Tables.BSD.__bsd_pm_ProjectsEntity entity)
		{
			var cables = Infrastructure.Data.Access.Tables.BSD.__bsd_pm_CablesAccess.GetByByProjectAndStatus(entity.Id, null);
			Id = entity.Id.ToString("D3");
			OfferNumber = entity.OfferNumber;
			Customer = entity.CustomerName;
			ProjectName = entity.ProjectName;
			QuantityKS = cables.Count;
			Status = entity.Status;
			Manager = entity.PMManagerUsername;
			DateCreation = entity.CreationTime;
		}
	}
}