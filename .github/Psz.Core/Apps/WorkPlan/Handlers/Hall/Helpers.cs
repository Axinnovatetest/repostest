namespace Psz.Core.Apps.WorkPlan.Handlers
{
	public partial class Hall
	{
		internal class Helpers
		{
			public static Infrastructure.Data.Entities.Tables.WPL.HallEntity ToDataEntity(Models.Hall.HallModel model)
			{
				return new Infrastructure.Data.Entities.Tables.WPL.HallEntity
				{
					ArchiveTime = model.ArchiveTime,
					ArchiveUserId = model.ArchiveUserId,
					CountryId = model.CountryId,
					CreationTime = model.CreationTime,
					CreationUserId = model.CreationUserId,
					Id = model.Id,
					IsArchived = model.IsArchived,
					LastEditTime = model.LastEditTime,
					LastEditUserId = model.LastEditUserId,
					Name = model.Name,
					Adress = model.Adress,
				};
			}
		}
	}
}
