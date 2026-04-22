using System;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data.Access.Tables.BSD
{
	public class ArtikelCurrentQuantityEntity
	{
		public decimal? CurrentQuantity { get; set; }
		public int ArtikelNr { get; set; }
		public ArtikelCurrentQuantityEntity(DataRow dataRow)
		{
			ArtikelNr = Convert.ToInt32(dataRow["Artikel_Nr"]);
			CurrentQuantity = (dataRow["quantity"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["quantity"]);
		}
	}
}
