namespace Psz.Core.CustomerService.CsStatistics.Models
{
	public class DepartementEmployeeModel
	{
		public string AV_Mitarbeiter { get; set; }
		public string EMAIL { get; set; }
		public int ID { get; set; }

		public DepartementEmployeeModel()
		{

		}

		public DepartementEmployeeModel(Infrastructure.Data.Entities.Tables.CTS.AV_Abteilung_MitarbeiterEntity entity)
		{
			AV_Mitarbeiter = entity.AV_Mitarbeiter;
			EMAIL = entity.EMAIL;
			ID = entity.ID;
		}

		public Infrastructure.Data.Entities.Tables.CTS.AV_Abteilung_MitarbeiterEntity ToEntity()
		{
			return new Infrastructure.Data.Entities.Tables.CTS.AV_Abteilung_MitarbeiterEntity
			{
				AV_Mitarbeiter = AV_Mitarbeiter,
				EMAIL = EMAIL,
				ID = ID,
			};
		}
	}
}
