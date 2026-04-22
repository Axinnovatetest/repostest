using Infrastructure.Data.Entities.Tables.WPL;

namespace Psz.Core.MaterialManagement.CRP.Models.WorkLocation
{
	public class WorkAreaModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int CountryId { get; set; }
		public string CountryName { get; set; }
		public int HallId { get; set; }
		public string HallName { get; set; }
		public int DepartmentId { get; set; }
		public string DepartmentName { get; set; }
		public List<WorkStationModel> WorkStations { get; set; } = new List<WorkStationModel>();

		public WorkAreaModel(WorkAreaEntity workAreaEntity,
			List<WorkStationMachineEntity> workStationMachineEntities,
			CountryEntity countryEntity, HallEntity hallEntity, DepartmentEntity departementEntity)
		{
			this.Id = workAreaEntity.Id;
			this.Name = workAreaEntity.Name;
			this.CountryId = countryEntity?.Id ?? -1;
			this.CountryName = countryEntity?.Name;
			this.HallId = hallEntity?.Id ?? -1;
			this.HallName = hallEntity?.Name;
			this.DepartmentId = departementEntity?.Id ?? -1;
			this.DepartmentName = departementEntity?.Name;

			foreach(var workStationMachineEntity in workStationMachineEntities)
			{
				this.WorkStations.Add(new WorkStationModel(workStationMachineEntity, countryEntity, hallEntity));
			}
		}
	}
}
