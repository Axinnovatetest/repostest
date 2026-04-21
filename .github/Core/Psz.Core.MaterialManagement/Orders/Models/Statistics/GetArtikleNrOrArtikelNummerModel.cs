namespace Psz.Core.MaterialManagement.Orders.Models.Statistics
{
	public class GetArtikleNrOrArtikelNummerModel
	{
		public int ArtikelNr { get; set; }
		public string Artiklenummer { get; set; }
		public GetArtikleNrOrArtikelNummerModel(Infrastructure.Data.Entities.Tables.MTM.Orders.ArtikelStatisticsEntity data)
		{
			ArtikelNr = data.ArtikelNr;
			Artiklenummer = data.Artiklenummer ?? "";
		}
	}
	public class GetArtikleNrOrArtikelNummerRequestModel
	{
		public int ArtikelNr { get; set; }
		public string Artiklenummer { get; set; }
	}
}
