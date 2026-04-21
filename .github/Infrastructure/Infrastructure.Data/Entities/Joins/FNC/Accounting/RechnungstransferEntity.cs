using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.FNC.Accounting
{
	public class RechnungstransferEntity
	{
		public DateTime? Belegdatum { get; set; }
		public int Periode { get; set; }
		public int Belegnummer { get; set; }
		public string Buchungstext { get; set; }
		public double Betrag { get; set; }
		public string Whrg { get; set; }
		public int Sollkto { get; set; }
		public int Habenkto { get; set; }
		public string Bezug { get; set; }
		public int TotalCount { get; set; }
		public RechnungstransferEntity(DataRow dataRow)
		{
			Periode = ((string.IsNullOrWhiteSpace(dataRow["Periode"].ToString())) || (dataRow["Periode"].ToString() == string.Empty) || (dataRow["Periode"] == System.DBNull.Value)) ? 0 : Convert.ToInt32(dataRow["Periode"].ToString());
			TotalCount = (dataRow["TotalCount"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["TotalCount"].ToString());
			Belegdatum = (dataRow["Belegdatum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Belegdatum"].ToString());
			Belegnummer = ((string.IsNullOrWhiteSpace(dataRow["Belegnummer"].ToString())) || (dataRow["Belegnummer"].ToString() == string.Empty) || (dataRow["Belegnummer"] == System.DBNull.Value)) ? 0 : Convert.ToInt32(dataRow["Belegnummer"].ToString());
			Buchungstext = (dataRow["Buchungstext"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Buchungstext"].ToString());
			Betrag = ((string.IsNullOrWhiteSpace(dataRow["Betrag"].ToString())) || (dataRow["Betrag"].ToString() == string.Empty) || (dataRow["Betrag"] == System.DBNull.Value)) ? 0 : Convert.ToDouble(dataRow["Betrag"].ToString());
			Whrg = (dataRow["Whrg"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Whrg"].ToString());
			Bezug = (dataRow["Bezug"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezug"].ToString());
			Sollkto = ((string.IsNullOrWhiteSpace(dataRow["Sollkto"].ToString())) || (dataRow["Sollkto"].ToString() == string.Empty) || (dataRow["Sollkto"] == System.DBNull.Value)) ? 0 : Convert.ToInt32(dataRow["Sollkto"].ToString());
			Habenkto = ((string.IsNullOrWhiteSpace(dataRow["Habenkto"].ToString())) || (dataRow["Habenkto"].ToString() == string.Empty) || (dataRow["Habenkto"] == System.DBNull.Value)) ? 0 : Convert.ToInt32(dataRow["Habenkto"].ToString());
		}
	}
}
