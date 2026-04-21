using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Tables.Logistics
{
    public class GetSupplierCAQEntity
    {
			public int SupplierId { get; set; }
			public string SupplierName { get; set; }
			public GetSupplierCAQEntity()
			{

			}
			public GetSupplierCAQEntity(DataRow dataRow)
			{
			   SupplierId = Convert.ToInt32(dataRow["SupplierId"]);
			   SupplierName = (dataRow["SupplierName"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["SupplierName"]);
			}
		}
	}

