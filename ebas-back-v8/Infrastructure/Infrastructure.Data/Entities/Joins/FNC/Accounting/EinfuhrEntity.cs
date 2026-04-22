using System.Data;

namespace Infrastructure.Data.Entities.Joins.FNC.Accounting
{
	public class EinfuhrEntity
	{
		public string Lieferantengruppe { get; set; }
		public string Name1 { get; set; }
		public string Zolltarif_nr { get; set; }
		public int TotalCount { get; set; }
		public string Ursprungsland { get; set; }
		public double Nettopreis { get; set; }
		public double Gewicht_in_kg { get; set; }
		public EinfuhrEntity(DataRow dataRow)
		{
			//TotalCount = (dataRow["TotalCount"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["TotalCount"].ToString());
			//Lieferantengruppe = (dataRow["Lieferantengruppe"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lieferantengruppe"].ToString());
			//Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"].ToString());
			//Ursprungsland = (dataRow["Ursprungsland"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ursprungsland"].ToString());
			//Zolltarif_nr = (( string.IsNullOrWhiteSpace(dataRow["Zolltarif_nr"].ToString())) || (dataRow["Zolltarif_nr"].ToString() == string.Empty) || (dataRow["Zolltarif_nr"] == System.DBNull.Value)) ? 0 : Convert.ToInt64(dataRow["Zolltarif_nr"].ToString());
			//Zolltarif_nr = (dataRow["Zolltarif_nr"] == System.DBNull.Value) ? 0 : Convert.ToInt64(dataRow["Zolltarif_nr"].ToString());
			//Nettopreis = ((string.IsNullOrEmpty(dataRow["Nettopreis"].ToString())) || (string.IsNullOrWhiteSpace(dataRow["Nettopreis"].ToString())) || dataRow["Nettopreis"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Nettopreis"].ToString());
			//Gewicht_in_kg = ((string.IsNullOrEmpty(dataRow["Gewicht_in_kg"].ToString())) || (string.IsNullOrWhiteSpace(dataRow["Gewicht_in_kg"].ToString())) || dataRow["Gewicht_in_kg"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Gewicht_in_kg"].ToString());

			Lieferantengruppe = ConversionHelpers.ConvertToString("Lieferantengruppe", ref dataRow);
			Name1 = ConversionHelpers.ConvertToString("Name1", ref dataRow);
			Ursprungsland = ConversionHelpers.ConvertToString("Ursprungsland", ref dataRow);
			TotalCount = ConversionHelpers.ConvertToInt32("TotalCount", ref dataRow);
			Zolltarif_nr = ConversionHelpers.ConvertToString("Zolltarif_nr", ref dataRow);
			Nettopreis = ConversionHelpers.ConvertToDouble("Nettopreis", ref dataRow);
			Gewicht_in_kg = ConversionHelpers.ConvertToDouble("Gewicht_in_kg", ref dataRow);

		}
	}

}
