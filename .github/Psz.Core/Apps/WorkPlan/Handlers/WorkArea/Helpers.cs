namespace Psz.Core.Apps.WorkPlan.Handlers
{
	public partial class WorkArea
	{
		internal class Helpers
		{
			public static Infrastructure.Data.Entities.Tables.WPL.WorkAreaEntity ToDataEntity(Models.WorkArea.WorkAreaModel workArea)
			{
				return new Infrastructure.Data.Entities.Tables.WPL.WorkAreaEntity
				{
					Id = workArea.Id,
					Name = workArea.Name,
					HallId = workArea.HallId,
					CountryId = workArea.CountryId,
					IsArchived = workArea.IsArchived,
					CreationTime = workArea.CreationTime,
					CreationUserId = workArea.CreationUserId,
					ArchiveTime = workArea.ArchiveTime,
					ArchiveUserId = workArea.ArchiveUserId,
					LastEditTime = workArea.LastEditTime,
					LastEditUserId = workArea.LastEditUserId,
				};
			}
		}
	}
}