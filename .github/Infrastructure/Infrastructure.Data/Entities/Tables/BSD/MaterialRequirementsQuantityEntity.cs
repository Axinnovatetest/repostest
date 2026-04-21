using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Tables.BSD
{
	public class MaterialRequirementsQuantityEntity
	{
		public int ArtikelNr { get; set; }
		public int CW { get; set; }
		public int Year { get; set; }
		public decimal? Quantity { get; set; }
		public MaterialRequirementsQuantityEntity() { }

		public MaterialRequirementsQuantityEntity(DataRow dataRow)
		{
			ArtikelNr = Convert.ToInt32(dataRow["ArtikelNr"]);
			CW = Convert.ToInt32(dataRow["CW"]);
			Year = Convert.ToInt32(dataRow["Year"]);
			Quantity = (dataRow["Quantity"] == System.DBNull.Value) ? (decimal?)null : Convert.ToDecimal(dataRow["Quantity"]);
		}
	}
}
