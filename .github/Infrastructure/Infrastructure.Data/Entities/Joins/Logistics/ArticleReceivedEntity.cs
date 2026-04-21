using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Joins.Logistics
{
	public class ArticleReceivedEntity
	{
		public DateTime? Datum { get; set; }
		public decimal? Menge { get; set; }
		public int Verpackungsnr { get; set; }

		public ArticleReceivedEntity() { }
		public ArticleReceivedEntity(DataRow dataRow)
		{
			Datum = (dataRow["Datum"] == System.DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(dataRow["Datum"]);
			Menge = (dataRow["Menge"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Menge"]);
			Verpackungsnr = (dataRow["Verpackungsnr"] == System.DBNull.Value) ? 0 : Convert.ToInt32(dataRow["Verpackungsnr"]);
		}
	}
}
