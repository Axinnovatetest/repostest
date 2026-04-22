using Infrastructure.Data.Entities.Tables.WPL;

namespace Psz.Core.MaterialManagement.CRP.Models.WorkLocation
{
	public class DepartementModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public List<WorkAreaModel> WorkAreas { get; set; } = new List<WorkAreaModel>();

		public DepartementModel() { }
		public DepartementModel(DepartmentEntity departementEntity,
		   List<WorkAreaEntity> workAreaEntities,
		   List<WorkStationMachineEntity> workStationEntities,
		   List<CountryEntity> countryEntities,
		   List<HallEntity> hallEntities,
		   List<DepartmentEntity> departementEntities)
		{
			this.Id = departementEntity.Id;
			this.Name = departementEntity.Name;

			foreach(var workAreaEntity in workAreaEntities)
			{
				var workAreaStations = workStationEntities?.FindAll(e => e.WorkAreaId == workAreaEntity.Id);
				var country = countryEntities?.Find(x => x.Id == workAreaEntity.CountryId);
				var hallEntity = hallEntities?.Find(x => x.Id == workAreaEntity.HallId);
				var departmentEntity = departementEntities?.Find(x => x.Id == workAreaEntity.DepartmentId);

				this.WorkAreas.Add(new WorkAreaModel(workAreaEntity, workAreaStations, country, hallEntity, departmentEntity));
			}
		}
	}
}
