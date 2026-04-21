using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.FNC.Accounting;

public class StammdatenkontrolleWareneingangeEntity
{
	public string Lieferantengruppe { get; set; }
	public DateTime? Datum { get; set; }
	public string Artikelnummer { get; set; }
	public double Anzahl { get; set; }
	public string Warengruppe { get; set; }
	public double Gewicht_in_gr { get; set; }
	public string Zolltarif_nr { get; set; }
	public string Ursprungsland { get; set; }
	public string Name1 { get; set; }
	public double Gesamtpreis { get; set; }
	public int TotalCount { get; set; }
	public StammdatenkontrolleWareneingangeEntity(DataRow dataRow)
	{
		TotalCount = (dataRow["TotalCount"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["TotalCount"].ToString());
		Lieferantengruppe = (dataRow["Lieferantengruppe"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Lieferantengruppe"].ToString());
		Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"].ToString());
		Artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"].ToString());
		Anzahl = ((string.IsNullOrEmpty(dataRow["Anzahl"].ToString())) || (string.IsNullOrWhiteSpace(dataRow["Anzahl"].ToString())) || dataRow["Anzahl"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Anzahl"].ToString());
		Warengruppe = (dataRow["Warengruppe"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Warengruppe"].ToString());
		Gewicht_in_gr = ((string.IsNullOrEmpty(dataRow["Gewicht in gr"].ToString())) || (string.IsNullOrWhiteSpace(dataRow["Gewicht in gr"].ToString())) || dataRow["Gewicht in gr"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Gewicht in gr"].ToString());
		//Zolltarif_nr = ((string.IsNullOrEmpty(dataRow["Zolltarif_nr"].ToString())) || (string.IsNullOrWhiteSpace(dataRow["Zolltarif_nr"].ToString())) || dataRow["Zolltarif_nr"] == System.DBNull.Value) ? 0 : Convert.ToInt64(dataRow["Zolltarif_nr"].ToString());
		Ursprungsland = (dataRow["Ursprungsland"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Ursprungsland"].ToString());
		Zolltarif_nr = (dataRow["Zolltarif_nr"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Zolltarif_nr"].ToString());
		Name1 = (dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"].ToString());
		Gesamtpreis = ((string.IsNullOrEmpty(dataRow["Gesamtpreis"].ToString())) || (string.IsNullOrWhiteSpace(dataRow["Gesamtpreis"].ToString())) || dataRow["Gesamtpreis"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Gesamtpreis"].ToString());
	}
}
public class StammdatenkontrolleWareneingangeCountEntity
{

	public int TotalCount { get; set; }
	public StammdatenkontrolleWareneingangeCountEntity(DataRow dataRow)
	{
		TotalCount = ((string.IsNullOrEmpty(dataRow["TotalCount"].ToString())) || (string.IsNullOrWhiteSpace(dataRow["TotalCount"].ToString())) || dataRow["TotalCount"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["TotalCount"].ToString());
	}

}
