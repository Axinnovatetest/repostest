namespace Psz.Core.Apps.WorkPlan.Handlers
{
	public partial class WorkStationMachine
	{
		internal class Helpers
		{
			public static Infrastructure.Data.Entities.Tables.WPL.WorkStationMachineEntity ToDataEntity(Models.WorkStationMachine.WorkStationMachineModel model)
			{
				return new Infrastructure.Data.Entities.Tables.WPL.WorkStationMachineEntity
				{
					CountryId = model.CountryId,
					CreationTime = model.CreationTime,
					CreationUserId = model.CreationUserId,
					HallId = model.HallId,
					Id = model.Id,
					IsArchived = model.IsArchived,
					LastEditTime = model.LastEditTime,
					LastEditUserId = model.LastEditUserId,
					Name = model.Name,
					Type = model.Type,
					DeleteTime = model.ArchiveTime,
					DeleteUserId = model.ArchivedUserId,
				};
			}
		}
	}
}
