using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Tables.Statistics.MGO
{
	public class InvoiceAmountEntity
	{
		public DateTime Belegdatum { get; set; }
		public decimal? Amount { get; set; }
		public byte? KW { get; set; }
		public short? Year { get; set; }

		public InvoiceAmountEntity() { }
		public InvoiceAmountEntity(DataRow dataRow)
		{
			Belegdatum = Convert.ToDateTime(dataRow["Belegdatum"]);
			Amount = (dataRow["Amount"] == DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Amount"]);
			KW = (dataRow["KW"] == DBNull.Value) ? (byte?)null : Convert.ToByte(dataRow["KW"]);
			Year = (dataRow["Year"] == DBNull.Value) ? (short?)null : Convert.ToInt16(dataRow["Year"]);
		}
	}
}