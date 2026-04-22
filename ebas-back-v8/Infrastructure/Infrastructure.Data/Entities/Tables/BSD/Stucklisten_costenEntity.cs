using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class Stucklisten_costenEntity
	{
		public double? VK_PSZ { get; set; }
		public double? Kalkulatorischekosten { get; set; }
		public double? Somme_Material { get; set; }
		public string Artikel_artikelnummer { get; set; }

		public Stucklisten_costenEntity()
		{

		}
		public Stucklisten_costenEntity(DataRow dataRow)
		{
			VK_PSZ = (dataRow["VK_PSZ"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["VK_PSZ"]);
			Kalkulatorischekosten = (dataRow["Kalkulatorischekosten"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Kalkulatorischekosten"]);
			Somme_Material = (dataRow["Somme_Material"] == System.DBNull.Value) ? 0 : Convert.ToDouble(dataRow["Somme_Material"]);
			Artikel_artikelnummer = (dataRow["Artikel_artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikel_artikelnummer"]);
		}
	}
}
