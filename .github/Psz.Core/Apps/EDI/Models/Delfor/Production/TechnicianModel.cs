namespace Psz.Core.Apps.EDI.Models.Delfor.Production
{
	public class TechnicianModel
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public TechnicianModel(Infrastructure.Data.Entities.Tables.PRS.TechnikerEntity technikerEntity)
		{
			Id = technikerEntity.ID;
			Name = technikerEntity.Name;
		}
		public Infrastructure.Data.Entities.Tables.PRS.TechnikerEntity ToTechnikerEntity()
		{
			return new Infrastructure.Data.Entities.Tables.PRS.TechnikerEntity
			{
				ID = Id,
				Name = Name
			};
		}
	}
}
