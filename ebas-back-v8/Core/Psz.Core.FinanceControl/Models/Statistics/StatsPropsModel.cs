using System.Collections.Generic;


namespace Psz.Core.FinanceControl.Models.Statistics
{
	public class StatsPropsModel
	{
		public List<LandsModel> Lands { get; set; }
		public List<DepartementsModel> Departements { get; set; }
		public List<EmplyoeesModel> Employees { get; set; }
		public bool IsSiteDirector { get; set; }
		public bool IsHeadOfDepartement { get; set; }
	}
	public class LandsModel
	{
		public int Id { get; set; }
		public string LandName { get; set; }
		public LandsModel()
		{

		}
		public LandsModel(Infrastructure.Data.Entities.Tables.STG.CompanyEntity entity)
		{
			Id = entity.Id;
			LandName = entity.Name;
		}
	}
	public class DepartementsModel
	{
		public int Id { get; set; }
		public int LandId { get; set; }
		public string DepartementName { get; set; }
		public DepartementsModel()
		{

		}
		public DepartementsModel(Infrastructure.Data.Entities.Tables.STG.DepartmentEntity entity)
		{
			var land = Infrastructure.Data.Access.Tables.STG.CompanyAccess.Get(entity.CompanyId);
			Id = (int)entity.Id;
			LandId = (int)entity.CompanyId;
			DepartementName = $"{land.Name} | {entity.Name}";
		}
	}
	public class EmplyoeesModel
	{
		public int Id { get; set; }
		public int LandId { get; set; }
		public int DepartementId { get; set; }
		public string EmployeeName { get; set; }
		public EmplyoeesModel()
		{

		}
		public EmplyoeesModel(Infrastructure.Data.Entities.Tables.COR.UserEntity entity)
		{
			Id = entity.Id;
			LandId = entity.CompanyId ?? -1;
			DepartementId = entity.DepartmentId ?? -1;
			EmployeeName = entity.Name;
		}
	}
}