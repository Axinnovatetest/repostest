namespace Psz.Core.Apps.EDI.Handlers.OrderError
{
	public partial class OrderError
	{
		internal class Helpers
		{
			public static Models.OrderError.OrderErrorModel ToOrderError(Infrastructure.Data.Entities.Tables.PRS.OrderErrorEntity dbEntity,
				Infrastructure.Data.Entities.Tables.COR.UserEntity validationUserDb,
				string customerName)
			{
				return new Models.OrderError.OrderErrorModel
				{
					Error = dbEntity.Error,
					FilePath = System.IO.Path.GetFileName(dbEntity.FileName),
					Id = dbEntity.Id,
					ClientName = customerName,
					Validated = dbEntity.Validated,
					ClientId = dbEntity.CustomerId,

					ValidationTime = dbEntity.ValidationTime,
					ValidationUserId = dbEntity.ValidationUserId,
					ValidationUser = validationUserDb?.Name,

					CustomerNumber = dbEntity.CustomerNumber,
					CreationTime = dbEntity.CreationTime,
				};
			}
		}
	}
}
