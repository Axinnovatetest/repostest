using System;

namespace Psz.Core.BaseData.Models.ObjectLog
{
	public class ObjectLogModel
	{
		public int Id { get; set; }
		public int LastUpdateUserId { get; set; }
		public DateTime LastUpdateTime { get; set; }
		public string LastUpdateUsername { get; set; }
		public string LastUpdateUserFullName { get; set; }
		public string LogObject { get; set; }
		public string LogDescription { get; set; }
		public ObjectLogModel()
		{

		}
		public ObjectLogModel(Infrastructure.Data.Entities.Tables.PRS.ObjectLogEntity objectLogEntity)
		{
			if(objectLogEntity == null)
				return;

			Id = objectLogEntity.Id;
			LastUpdateUserId = objectLogEntity.LastUpdateUserId.HasValue ? Convert.ToInt32(objectLogEntity.LastUpdateUserId) : -1;
			LastUpdateTime = objectLogEntity.LastUpdateTime.HasValue ? Convert.ToDateTime(objectLogEntity.LastUpdateTime) : DateTime.MaxValue;
			LastUpdateUsername = objectLogEntity.LastUpdateUsername;
			LastUpdateUserFullName = objectLogEntity.LastUpdateUserFullName;
			LogObject = objectLogEntity.LogObject;
			LogDescription = objectLogEntity.LogDescription;
		}

	}
}
