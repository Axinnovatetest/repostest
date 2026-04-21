namespace Psz.Core.MaterialManagement.Orders.Models.Orders
{
	public class LogResponseModel
	{
		public int Id { get; set; }
		public string LogDescription { get; set; }
		public string LastUpdateUserFullName { get; set; }
		public DateTime LastUpdateTime { get; set; }
		public LogResponseModel(Infrastructure.Data.Entities.Tables.MTM.BestellungenProcessing_LogEntity entity)
		{
			if(entity is null)
			{
				return;
			}

			// - 
			Id = entity.Id;
			LogDescription = entity.LogText;
			LastUpdateTime = entity.DateTime ?? DateTime.MinValue;
			LastUpdateUserFullName = entity.Username;

		}
	}
}
