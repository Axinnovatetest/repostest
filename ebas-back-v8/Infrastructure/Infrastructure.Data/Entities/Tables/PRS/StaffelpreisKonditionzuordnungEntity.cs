using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.PRS
{
	public class StaffelpreisKonditionzuordnungEntity
	{
		public int? Artikel_Nr { get; set; }
		public decimal? Betrag { get; set; }
		public string Kostenart { get; set; }
		public int Nr_Staffel { get; set; }
		public decimal? ProduKtionzeit { get; set; }
		public string Staffelpreis_Typ { get; set; }
		public decimal? Stundensatz { get; set; }

		public StaffelpreisKonditionzuordnungEntity() { }
		public StaffelpreisKonditionzuordnungEntity(DataRow dataRow)
		{
			Artikel_Nr = (dataRow["Artikel_Nr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["Artikel_Nr"]);
			Betrag = (dataRow["Betrag"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Betrag"]);
			Kostenart = (dataRow["Kostenart"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kostenart"]);
			Nr_Staffel = Convert.ToInt32(dataRow["Nr_Staffel"]);
			ProduKtionzeit = (dataRow["ProduKtionzeit"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["ProduKtionzeit"]);
			Staffelpreis_Typ = (dataRow["Staffelpreis_Typ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Staffelpreis_Typ"]);
			Stundensatz = (dataRow["Stundensatz"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Stundensatz"]);
		}
	}
}

