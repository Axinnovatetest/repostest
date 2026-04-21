namespace Psz.Core.MaterialManagement.Orders.Models.Suppliers
{
	public class GetResponseModel
	{
		public int Id { get; set; }

		public string Name1 { get; set; }
		public int Lieferantennummer { get; set; }
		public string Ort { get; set; }
		public GetResponseModel() { }
		public GetResponseModel(Infrastructure.Data.Entities.Tables.MTM.AdressenEntity entity)
		{
			Id = entity.Nr;
			Name1 = entity.Name1;
			Lieferantennummer = entity.Lieferantennummer.HasValue ? entity.Lieferantennummer.Value : -1;
			Ort = entity.Ort;
		}
	}
	public class GetRequestModel
	{
		public string Filter { get; set; }

	}
}
