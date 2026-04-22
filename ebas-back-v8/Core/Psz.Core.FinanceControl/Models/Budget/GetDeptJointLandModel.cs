namespace Psz.Core.FinanceControl.Models.Budget
{
	public class GetDeptJointLandModel
	{
		public int ID { get; set; }
		public int? ID_Department { get; set; }
		public int? ID_Land { get; set; }
		public int? ID_user { get; set; }
		public string EmailUser { get; set; }



		public GetDeptJointLandModel() { }

		public GetDeptJointLandModel(Infrastructure.Data.Entities.Tables.FNC.Land_Department_JointEntity Land_Department_JointEntity)
		{
			ID = Land_Department_JointEntity.ID;
			ID_Department = Land_Department_JointEntity.ID_Department;
			ID_Land = Land_Department_JointEntity.ID_Land;
			ID_user = Land_Department_JointEntity.ID_user;
			EmailUser = Land_Department_JointEntity.EmailUser;
		}
		public Infrastructure.Data.Entities.Tables.FNC.Land_Department_JointEntity JointDeptsLand()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.Land_Department_JointEntity
			{
				ID = ID,
				ID_Department = ID_Department,
				ID_Land = ID_Land,
				ID_user = ID_user,
				EmailUser = EmailUser,
			};
		}
	}
}
