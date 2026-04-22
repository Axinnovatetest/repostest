

namespace Psz.Core.CRP.Models.FAPlanning
{
	public class FaPlanningLogRequestModel
	{
		public int Id { get; set; }
		public DateTime Date { get; set; }
		public int UserId { get; set; }
		public string UserName { get; set; }
		public FaPlanningLogRequestModel(Infrastructure.Data.Entities.Joins.CRP.FaPlanningComputeLogsEntity entity, string username)
		{
			if(entity is null)
			{
				return;
			}
			Id = entity.Id;
			Date = entity.ExecDate ?? DateTime.MinValue;
			UserId = entity.ExecUserId ?? -1;
			UserName = username;
		}
	}
}
