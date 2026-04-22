namespace Psz.Core.MaterialManagement.Orders.Models.OrderValidation
{
	public class ClientModel
	{
		public class ClientResponseModel
		{
			public string Id { get; set; }
			public string Name { get; set; }


			public ClientResponseModel(Infrastructure.Data.Entities.Tables.MTM.PSZ_MandantenEntity PSZ_MandantenEntity)
			{
				Id = PSZ_MandantenEntity.ID.ToString();
				Name = PSZ_MandantenEntity.Mandant;
			}
		}
		public class ClientRequestModel
		{
			public string Name { get; set; }
		}
	}
}
