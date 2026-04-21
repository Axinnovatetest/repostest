namespace Psz.Core.Logistics.Models.InverntoryStockModels
{
	public class UpdateTaskRequestModel
	{
		public int? Id { get; set; }
		public string? phase { get; set; }
		public string? role { get; set; }
		public int? status { get; set; }
		public string? title { get; set; }
		public int? lagerId { get; set; }

		public UpdateTaskRequestModel() { }
		public UpdateTaskRequestModel(Infrastructure.Data.Entities.Tables.Logistics.InventroyStock.TaskByRoleEntity entity)
		{
			if(entity == null)
				return;
			phase = entity.phase;
			role = entity.role;
			title = entity.title;
			status = entity.status;
			Id = entity.Id;
			lagerId = entity.lagerId;
		}
	}
}
