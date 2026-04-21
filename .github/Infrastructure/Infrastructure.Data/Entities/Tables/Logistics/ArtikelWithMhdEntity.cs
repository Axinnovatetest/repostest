using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Entities.Tables.Logistics
{
    public class ArtikelWithMhdEntity
    {

			public int artikelNr { get; set; }
			public string artikelnummer { get; set; }
			public string bezeichnung1 { get; set; }
			public string einheit { get; set; }
			public bool MHD  { get; set; }
	public ArtikelWithMhdEntity() { }
			public ArtikelWithMhdEntity(DataRow dataRow)
			{
				artikelNr = Convert.ToInt32(dataRow["ArtikelNr"]);
				artikelnummer = (dataRow["Artikelnummer"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Artikelnummer"]);
				bezeichnung1 = (dataRow["Bezeichnung1"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Bezeichnung1"]);
				einheit = (dataRow["Einheit"] == System.DBNull.Value) ? "" : Convert.ToString(dataRow["Einheit"]);
			    MHD = ((dataRow["MHD"] == System.DBNull.Value) ? (bool?)null : Convert.ToBoolean(dataRow["MHD"])) ?? false;
		}

		}
	}



