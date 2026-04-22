using System.Data;

namespace Infrastructure.Data.Entities.Joins.FNC.Accounting;
public class AusfuhrEntity
{
	public string Vorname_NameFirma { get; set; }
	public double? Zolltarif_nr { get; set; }
	public int TotalCount { get; set; }
	public double VK_Nettosumme { get; set; }
	public double Gewicht_in_kg { get; set; }
	public string Ursprungsland { get; set; }
	public AusfuhrEntity(DataRow dataRow)
	{
		//TotalCount = (dataRow["TotalCount"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["TotalCount"].ToString());
		//Zolltarif_nr = ((string.IsNullOrWhiteSpace(dataRow["Zolltarif_nr"].ToString())) ||  (dataRow["Zolltarif_nr"].ToString() == string.Empty) || (dataRow["Zolltarif_nr"] == System.DBNull.Value)) ? 0 : Convert.ToDouble(dataRow["Zolltarif_nr"].ToString());
		//Vorname_NameFirma = (dataRow["Vorname"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Vorname"].ToString());
		//VK_Nettosumme = ((string.IsNullOrEmpty(dataRow["VK-Nettosumme"].ToString())) ||  (string.IsNullOrWhiteSpace(dataRow["VK-Nettosumme"].ToString())) ||  dataRow["VK-Nettosumme"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["VK-Nettosumme"].ToString());
		//Gewicht_in_kg = ((string.IsNullOrEmpty(dataRow["Gewicht in kg"].ToString())) || (string.IsNullOrWhiteSpace(dataRow["Gewicht in kg"].ToString())) ||  dataRow["Gewicht in kg"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Gewicht in kg"].ToString());
		//Ursprungsland = (dataRow["Ursprungsland"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ursprungsland"].ToString());
		TotalCount = ConversionHelpers.ConvertToInt32("TotalCount", ref dataRow);
		//Zolltarif_nr = ConversionHelpers.ConvertToDouble("Zolltarif_nr",ref dataRow);
		Zolltarif_nr = ConversionHelpers.ConvertToDoubleNullable("Zolltarif_nr", ref dataRow);
		VK_Nettosumme = ConversionHelpers.ConvertToDouble("VK-Nettosumme", ref dataRow);
		Gewicht_in_kg = ConversionHelpers.ConvertToDouble("Gewicht in kg", ref dataRow);
		Vorname_NameFirma = ConversionHelpers.ConvertToString("Vorname", ref dataRow);
		Ursprungsland = ConversionHelpers.ConvertToString("Ursprungsland", ref dataRow);
	}
}
