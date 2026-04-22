using static Psz.Core.Logistics.Models.InverntoryStockModels.GetLogRequestModel;

namespace Psz.Core.Logistics.Models.InverntoryStockModels
{
	public class GetLogRequestModel: IPaginatedRequestModel
	{
		public string? SearchValue { get; set; }
		public int? LagerId { get; set; }

		public class GetLogItemResponseModel
		{
			public int Id { get; set; }
			public string LogDescription { get; set; }
			public int? LogsType { get; set; }
			public DateTime? LogTime { get; set; }
			public int? LogUserId { get; set; }
			public int? ObjectId { get; set; }
			public string ObjectName { get; set; }
			public string LogUserName { get; set; }

			public GetLogItemResponseModel(Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.LogsEntity _logsEntity)
			{
				if(_logsEntity == null)
					return;
				Id = _logsEntity.Id;
				LogDescription = _logsEntity.LogDescription;
				LogTime = _logsEntity.LogTime;
				LogUserId = _logsEntity.LogUserId;
				ObjectId = _logsEntity.ObjectId;
				ObjectName = _logsEntity.ObjectName;
				LogsType = _logsEntity.LogsType;
				LogUserName= _logsEntity.LogUserName;
			}
		}
	}
}
public class GetLogResponseModel: IPaginatedResponseModel<GetLogItemResponseModel>
{
}