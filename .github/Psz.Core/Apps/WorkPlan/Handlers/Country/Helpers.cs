namespace Psz.Core.Apps.WorkPlan.Handlers.Country
{
	public partial class Country
	{
		internal class Helpers
		{
			public static Infrastructure.Data.Entities.Tables.WPL.CountryEntity ToDataEntity(Models.Country.CountryModel model)
			{
				return new Infrastructure.Data.Entities.Tables.WPL.CountryEntity
				{
					ArchiveTime = model.ArchiveTime,
					ArchiveUserId = model.ArchiveUserId,
					CreationTime = model.CreationTime,
					CreationUserId = model.CreationUserId,
					Designation = model.Designation,
					Id = model.Id,
					IsArchived = model.IsArchived,
					LastEditTime = model.LastEditTime,
					LastEditUserId = model.LastEditUserId,
					Name = model.Name,
				};
			}
		}
	}
}
