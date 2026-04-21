namespace Psz.Core.FinanceControl.Models.Budget
{
	public class GetDeptJointLandResponsableModel
	{
		public int ID { get; set; }
		public int? ID_Department { get; set; }
		public int? ID_Land { get; set; }
		public int? ID_user { get; set; }
		public string Username { get; set; }
		public string Name { get; set; }




		public GetDeptJointLandResponsableModel() { }

		public GetDeptJointLandResponsableModel(Infrastructure.Data.Entities.Tables.FNC.Department_Responsable_JointEntity Land_Department_JointEntity)


		{

			ID = Land_Department_JointEntity.ID;
			ID_Department = Land_Department_JointEntity.ID_Department;
			ID_Land = Land_Department_JointEntity.ID_Land;
			ID_user = Land_Department_JointEntity.ID_user;
			Username = Land_Department_JointEntity.Username;
			Name = Land_Department_JointEntity.Name;


		}
		public Infrastructure.Data.Entities.Tables.FNC.Department_Responsable_JointEntity JointDeptsLand()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.Department_Responsable_JointEntity
			{
				ID = ID,
				ID_Department = ID_Department,
				ID_Land = ID_Land,
				ID_user = ID_user,
				Username = Username,
				Name = Name,
			};
		}
	}
}
