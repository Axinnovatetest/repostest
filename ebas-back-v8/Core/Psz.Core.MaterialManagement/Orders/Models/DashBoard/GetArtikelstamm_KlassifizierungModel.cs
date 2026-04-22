namespace Psz.Core.MaterialManagement.Orders.Models.DashBoard
{
	public class GetArtikelstamm_KlassifizierungTrimmedReposneModel
	{
		public int ID { get; set; }
		public string Klassifizierung { get; set; }
		public GetArtikelstamm_KlassifizierungTrimmedReposneModel(Infrastructure.Data.Entities.Tables.MTM.Orders.Artikelstamm_KlassifizierungTrimmedEntity data)
		{
			ID = data.ID;
			Klassifizierung = data.Klassifizierung;
		}
		public GetArtikelstamm_KlassifizierungTrimmedReposneModel()
		{

		}
	}
	public class GetArtikelstamm_KlassifizierungTrimmedRequestModel
	{
		public int Filter { get; set; } = 0;
	}
}
