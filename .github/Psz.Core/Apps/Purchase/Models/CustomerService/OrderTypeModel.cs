namespace Psz.Core.Apps.Purchase.Models.CustomerService
{
	public class OrderTypeModel
	{
		public int Id { get; set; }
		public string Type { get; set; }
		public string Description { get; set; }

		public OrderTypeModel(Infrastructure.Data.Entities.Tables.STG.OrderTypesEntity orderTypesEntity)
		{
			if(orderTypesEntity == null)
				return;

			Id = orderTypesEntity.Id;
			Type = orderTypesEntity.Type;
			Description = orderTypesEntity.Description;
		}
	}
}
