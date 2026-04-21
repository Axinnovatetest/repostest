using System.Data;

namespace Infrastructure.Data.Entities.Joins.MTM.Order.Statistics
{

	public class GetFaultyArtikelNummerEntity
	{
		public string Artikelnummer { get; set; }
		public GetFaultyArtikelNummerEntity(DataRow dataRow)
		{
			Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : dataRow["Artikelnummer"].ToString();
		}
	}

}
