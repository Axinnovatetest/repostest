using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.Logistics
{
	public class StatisticsEntity
	{
		public string Sklad { get; set; }
		public DateTime? letzteBewegung { get; set; }
		public string CisloPSZ { get; set; }
		public decimal Mnozstvi { get; set; }
		public string CisloVyrobce { get; set; }
		public string KontrolaOk { get; set; }
		public string WE_VOH_ID { get; set; }
		public int totalNombre { get; set; }
		public StatisticsEntity(DataRow dataRow)
		{

			Sklad = (dataRow["Sklad"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Sklad"]);
			letzteBewegung = (dataRow["Poslední Transakce"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Poslední Transakce"]);
			CisloPSZ = (dataRow["Cislo PSZ"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Cislo PSZ"]);
			Mnozstvi = (dataRow["Množství"] == System.DBNull.Value) ? 0 : Convert.ToDecimal(dataRow["Množství"]);
			CisloVyrobce = ((dataRow["Name1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Name1"])) + "# " + ((dataRow["Cislo Vyrobce"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Cislo Vyrobce"]));
			KontrolaOk = (dataRow["Kontrola ok?"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Kontrola ok?"]);
			WE_VOH_ID = (dataRow["WE_VOH_ID"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["WE_VOH_ID"]);
			totalNombre = (dataRow["totalNombre"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["totalNombre"]);



		}
	}
}
