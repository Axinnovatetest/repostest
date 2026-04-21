using System;
using System.Data;

namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class __E_rechnung_CreatedEntity
	{
		public DateTime? CreationTime { get; set; }
		public string CustomerName { get; set; }
		public int? CustomerNr { get; set; }
		public string CustomerRechnungType { get; set; }
		public int Id { get; set; }
		public int? LsAngebotNr { get; set; }
		public int? LSNr { get; set; }
		public int? RechnungForfallNr { get; set; }
		public int? RechnungNr { get; set; }
		public int? RechnungProjectNr { get; set; }
		public DateTime? SentTime { get; set; }

		public __E_rechnung_CreatedEntity() { }

		public __E_rechnung_CreatedEntity(DataRow dataRow)
		{
			CreationTime = (dataRow["CreationTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["CreationTime"]);
			CustomerName = (dataRow["CustomerName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerName"]);
			CustomerNr = (dataRow["CustomerNr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CustomerNr"]);
			CustomerRechnungType = (dataRow["CustomerRechnungType"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerRechnungType"]);
			Id = Convert.ToInt32(dataRow["Id"]);
			LsAngebotNr = (dataRow["LsAngebotNr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LsAngebotNr"]);
			LSNr = (dataRow["LSNr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["LSNr"]);
			RechnungForfallNr = (dataRow["RechnungForfallNr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["RechnungForfallNr"]);
			RechnungNr = (dataRow["RechnungNr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["RechnungNr"]);
			RechnungProjectNr = (dataRow["RechnungProjectNr"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["RechnungProjectNr"]);
			SentTime = (dataRow["SentTime"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["SentTime"]);
		}

		public __E_rechnung_CreatedEntity ShallowClone()
		{
			return new __E_rechnung_CreatedEntity
			{
				CreationTime = CreationTime,
				CustomerName = CustomerName,
				CustomerNr = CustomerNr,
				CustomerRechnungType = CustomerRechnungType,
				Id = Id,
				LsAngebotNr = LsAngebotNr,
				LSNr = LSNr,
				RechnungForfallNr = RechnungForfallNr,
				RechnungNr = RechnungNr,
				RechnungProjectNr = RechnungProjectNr,
				SentTime = SentTime
			};
		}
	}
}

