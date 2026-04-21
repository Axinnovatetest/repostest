using System.Data;

namespace Infrastructure.Data.Entities.Joins.MTM.Order
{
	public class OrderValidationEntity
	{
		public float Bestellt { get; set; }

		public OrderValidationEntity() { }
		public OrderValidationEntity(DataRow dataRow)
		{
			float bestellt = 0;
			float.TryParse(dataRow["Bestellt"].ToString(), out bestellt);
			this.Bestellt = bestellt;
		}

	}
}
