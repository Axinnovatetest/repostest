using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.STG
{
	public class Stammdaten_FirmaEntity
	{
		public string Euroformatierung { get; set; }
		public byte[] Logo { get; set; }
		public int Nr { get; set; }
		public string Standard_LKZ { get; set; }
		public double? Standard_USt { get; set; }
		public string Text_fuß { get; set; }
		public string Text_kopf { get; set; }
		public int? Währung { get; set; }

		public Stammdaten_FirmaEntity() { }

		public Stammdaten_FirmaEntity(DataRow dataRow)
		{
			Euroformatierung = (dataRow["Euroformatierung"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Euroformatierung"]);
			Logo = (dataRow["Logo"] == System.DBNull.Value) ? new byte[0] : (byte[])dataRow["Logo"];
			Nr = Convert.ToInt32(dataRow["Nr"]);
			Standard_LKZ = (dataRow["Standard_LKZ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Standard_LKZ"]);
			Standard_USt = (dataRow["Standard_USt"] == System.DBNull.Value) ? (double?)null : Convert.ToDouble(dataRow["Standard_USt"]);
			Text_fuß = (dataRow["Text_fuß"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Text_fuß"]);
			Text_kopf = (dataRow["Text_kopf"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Text_kopf"]);
			Währung = (dataRow["Währung"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Währung"]);
		}
	}
}

