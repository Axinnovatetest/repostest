using Infrastructure.Data.Entities.Tables.WPL;

namespace Psz.Core.MaterialManagement.CRP.Models.WorkLocation
{
	public class WorkStationModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int CountryId { get; set; }
		public string CountryName { get; set; }
		public int HallId { get; set; }
		public string HallName { get; set; }

		public WorkStationModel() { }
		public WorkStationModel(WorkStationMachineEntity workStationMachineEntity,
			CountryEntity countryEntity, HallEntity hallEntity)
		{
			this.Id = workStationMachineEntity.Id;
			this.Name = workStationMachineEntity.Name;
			this.CountryId = countryEntity?.Id ?? -1;
			this.CountryName = countryEntity?.Name;
			this.HallId = hallEntity?.Id ?? -1;
			this.HallName = hallEntity?.Name;
		}
	}
}
