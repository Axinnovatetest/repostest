namespace Psz.Core.Apps.WorkPlan.Handlers
{
	public partial class StandardOperation
	{
		internal class Helpers
		{
			public static Infrastructure.Data.Entities.Tables.WPL.StandardOperationEntity ToDataEntity(Models.StandardOperation.StandardOperationModel model)
			{
				return new Infrastructure.Data.Entities.Tables.WPL.StandardOperationEntity
				{
					ArchiveTime = model.ArchiveTime,
					ArchiveUserId = model.ArchiveUserId,
					CreationTime = model.CreationTime,
					CreationUserId = model.CreationUserId,
					Id = model.Id,
					IsArchived = model.IsArchived,
					LastEditTime = model.LastEditTime,
					LastEditUserId = model.LastEditUserId,
					Name = model.Name,
					OperationValueAdding = model.OperationValueAdding,
				};
			}
		}
	}
}
