using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Infrastructure.Data.Entities.Tables.CTS
{
	public class EDI_CustomerConcernItemsEntity
	{
		public int? ConcernId { get; set; }
		public int? ConcernNumber { get; set; }
		public string CustomerDUNS { get; set; }
		public int? CustomerNumber { get; set; }
		public int Id { get; set; }

		public EDI_CustomerConcernItemsEntity() { }

		public EDI_CustomerConcernItemsEntity(DataRow dataRow)
		{
			ConcernId = (dataRow["ConcernId"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ConcernId"]);
			ConcernNumber = (dataRow["ConcernNumber"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["ConcernNumber"]);
			CustomerDUNS = (dataRow["CustomerDUNS"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["CustomerDUNS"]);
			CustomerNumber = (dataRow["CustomerNumber"] == System.DBNull.Value) ? (int?)null : Convert.ToInt32(dataRow["CustomerNumber"]);
			Id = Convert.ToInt32(dataRow["Id"]);
		}

		public EDI_CustomerConcernItemsEntity ShallowClone()
		{
			return new EDI_CustomerConcernItemsEntity
			{
				ConcernId = ConcernId,
				ConcernNumber = ConcernNumber,
				CustomerDUNS = CustomerDUNS,
				CustomerNumber = CustomerNumber,
				Id = Id
			};
		}
	}
}

