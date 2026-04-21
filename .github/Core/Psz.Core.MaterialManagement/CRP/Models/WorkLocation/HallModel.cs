using Infrastructure.Data.Entities.Tables.WPL;

namespace Psz.Core.MaterialManagement.CRP.Models.WorkLocation
{
	public class HallModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int CountryId { get; set; }
		public string CountryName { get; set; }
		public List<WorkAreaModel> WorkAreas { get; set; } = new List<WorkAreaModel>();

		public HallModel(HallEntity hallEntity,
			CountryEntity countryEntity,
			List<WorkAreaEntity> workAreaEntities,
			List<WorkStationMachineEntity> workStationEntities,
			List<DepartmentEntity> departementEntities)
		{
			this.Id = hallEntity.Id;
			this.Name = hallEntity.Name;
			this.CountryId = hallEntity.CountryId;
			this.CountryName = countryEntity?.Name;

			foreach(var workAreaEntity in workAreaEntities)
			{
				var workAreaStations = workStationEntities.FindAll(e => e.WorkAreaId == workAreaEntity.Id);
				var departmentEntity = departementEntities?.Find(x => x.Id == workAreaEntity.DepartmentId);

				this.WorkAreas.Add(new WorkAreaModel(workAreaEntity, workAreaStations, countryEntity, hallEntity, departmentEntity));
			}
		}
	}
}
