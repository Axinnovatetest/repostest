using System;
using System.Data;

namespace Infrastructure.Data.Entities.Joins.CTS
{
	public class RechnungReprintEntity
	{
		public int? RechnungNr { get; set; }
		public int? RechnungForfallNr { get; set; }
		public DateTime? CreationTime { get; set; }
		public string CustomerName { get; set; }
		public string adress { get; set; }
		public string Bezug { get; set; }
		public string KundenType { get; set; }

		public RechnungReprintEntity()
		{

		}

		public RechnungReprintEntity(DataRow dataRow)
		{
			RechnungNr = (dataRow["RechnungNr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["RechnungNr"]);
			RechnungForfallNr = (dataRow["RechnungForfallNr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["RechnungForfallNr"]);
			CreationTime = (dataRow["CreationTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationTime"]);
			CustomerName = (dataRow["CustomerName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerName"]);
			adress = (dataRow["adress"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["adress"]);
			Bezug = (dataRow["Bezug"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezug"]);
			KundenType = (dataRow["CustomerRechnungType"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerRechnungType"]);
		}
	}
}
