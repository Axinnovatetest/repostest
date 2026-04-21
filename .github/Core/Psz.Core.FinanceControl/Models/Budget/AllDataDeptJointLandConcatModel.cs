namespace Psz.Core.FinanceControl.Models.Budget
{
	public class AllDataDeptJointLandConcatModel
	{
		public int ID { get; set; }
		public int? ID_Department { get; set; }
		public string Departement_name { get; set; }
		public int? ID_Land { get; set; }
		public string Land_name { get; set; }
		public int? ID_user { get; set; }
		public string Name { get; set; }
		public string NameFull => Departement_name + " " + Land_name;



		public AllDataDeptJointLandConcatModel() { }

		public AllDataDeptJointLandConcatModel(Infrastructure.Data.Entities.Tables.FNC.AllDataLand_Department_JointEntity allDataLand_Department_JointEntity)


		{

			ID = allDataLand_Department_JointEntity.ID;
			ID_Department = allDataLand_Department_JointEntity.ID_Department;
			Departement_name = allDataLand_Department_JointEntity.Departement_name;
			ID_Land = allDataLand_Department_JointEntity.ID_Land;
			Land_name = allDataLand_Department_JointEntity.Land_name;
			ID_user = allDataLand_Department_JointEntity.ID_user;
			Name = allDataLand_Department_JointEntity.Name;


		}
		public Infrastructure.Data.Entities.Tables.FNC.AllDataLand_Department_JointEntity ToBudgetallDataJintDeptsLand()
		{
			return new Infrastructure.Data.Entities.Tables.FNC.AllDataLand_Department_JointEntity
			{
				ID = ID,
				ID_Department = ID_Department,
				Departement_name = Departement_name,
				ID_Land = ID_Land,
				Land_name = Land_name,
				ID_user = ID_user,
				Name = Name,
			};
		}
	}
}
